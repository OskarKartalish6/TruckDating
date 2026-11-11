
-- =========================================================
-- TruckDating Database
-- PostgreSQL + PostGIS
-- =========================================================

CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS postgis;

-- =========================================================
-- ENUM TYPES
-- =========================================================

CREATE TYPE account_status AS ENUM (
    'active',
    'suspended',
    'deleted'
);

CREATE TYPE photo_status AS ENUM (
    'pending',
    'approved',
    'rejected'
);

CREATE TYPE report_reason AS ENUM (
    'fake_profile',
    'harassment',
    'spam',
    'inappropriate_content',
    'other'
);

CREATE TYPE report_status AS ENUM (
    'pending',
    'reviewing',
    'resolved',
    'rejected'
);

CREATE TYPE moderation_action_type AS ENUM (
    'warning',
    'photo_rejected',
    'user_suspended',
    'user_unsuspended',
    'user_banned'
);

-- =========================================================
-- USERS
-- Main account for every application user
-- =========================================================

CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    email VARCHAR(255) NOT NULL,
    password_hash TEXT NOT NULL,

    status account_status NOT NULL DEFAULT 'active',

    email_verified BOOLEAN NOT NULL DEFAULT FALSE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX ux_users_email_lower
    ON users (LOWER(email));

-- =========================================================
-- USER PROFILES
-- General profile information
-- =========================================================

CREATE TABLE user_profiles (
    user_id UUID PRIMARY KEY
        REFERENCES users(id) ON DELETE CASCADE,

    first_name VARCHAR(100) NOT NULL,
    date_of_birth DATE NOT NULL,

    bio TEXT,
    gender VARCHAR(50),
    country VARCHAR(100),
    city VARCHAR(100),

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- =========================================================
-- DRIVER PROFILES
-- Optional information for truck drivers
-- A driver is still a regular User
-- =========================================================

CREATE TABLE driver_profiles (
    user_id UUID PRIMARY KEY
        REFERENCES users(id) ON DELETE CASCADE,

    truck_description TEXT,
    experience_years INTEGER,
    preferred_routes TEXT,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_driver_experience
        CHECK (
            experience_years IS NULL
            OR experience_years >= 0
        )
);

-- =========================================================
-- USER PREFERENCES
-- Discovery and matching preferences
-- =========================================================

CREATE TABLE user_preferences (
    user_id UUID PRIMARY KEY
        REFERENCES users(id) ON DELETE CASCADE,

    min_age INTEGER NOT NULL DEFAULT 18,
    max_age INTEGER NOT NULL DEFAULT 99,

    max_distance_km INTEGER DEFAULT 100,

    show_only_drivers BOOLEAN NOT NULL DEFAULT FALSE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_preferences_age
        CHECK (
            min_age >= 18
            AND max_age >= min_age
            AND max_age <= 120
        ),

    CONSTRAINT chk_preferences_distance
        CHECK (
            max_distance_km IS NULL
            OR max_distance_km > 0
        )
);

-- =========================================================
-- ADMIN ACCOUNTS
-- Separate accounts for WPF Admin Panel
-- =========================================================

CREATE TABLE admin_accounts (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,

    is_active BOOLEAN NOT NULL DEFAULT TRUE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- =========================================================
-- USER PHOTOS
-- Actual files are stored outside the database
-- storage_key contains the file identifier/path
-- =========================================================

CREATE TABLE user_photos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    user_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    storage_key TEXT NOT NULL,

    status photo_status NOT NULL DEFAULT 'pending',
    is_primary BOOLEAN NOT NULL DEFAULT FALSE,

    reviewed_by_admin_id UUID
        REFERENCES admin_accounts(id) ON DELETE SET NULL,

    reviewed_at TIMESTAMPTZ,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX ix_user_photos_user_id
    ON user_photos(user_id);

CREATE UNIQUE INDEX ux_user_photos_primary
    ON user_photos(user_id)
    WHERE is_primary = TRUE;

-- =========================================================
-- LIKES
-- User likes another user
-- =========================================================

CREATE TABLE likes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    sender_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    receiver_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT uq_like_pair
        UNIQUE (sender_id, receiver_id),

    CONSTRAINT chk_like_not_self
        CHECK (sender_id <> receiver_id)
);

CREATE INDEX ix_likes_receiver_id
    ON likes(receiver_id);

-- =========================================================
-- MATCHES
-- Created when two users like each other
-- UUIDs are stored in a consistent order
-- =========================================================

CREATE TABLE matches (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    user_one_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    user_two_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_match_users
        CHECK (user_one_id < user_two_id),

    CONSTRAINT uq_match_pair
        UNIQUE (user_one_id, user_two_id)
);

CREATE INDEX ix_matches_user_two
    ON matches(user_two_id);

-- =========================================================
-- CHATS
-- One chat per match
-- =========================================================

CREATE TABLE chats (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    match_id UUID NOT NULL UNIQUE
        REFERENCES matches(id) ON DELETE CASCADE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- =========================================================
-- CHAT PARTICIPANTS
-- Defines who can send messages in a chat
-- =========================================================

CREATE TABLE chat_participants (
    chat_id UUID NOT NULL
        REFERENCES chats(id) ON DELETE CASCADE,

    user_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    joined_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    PRIMARY KEY (chat_id, user_id)
);

-- =========================================================
-- MESSAGES
-- =========================================================

CREATE TABLE messages (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    chat_id UUID NOT NULL,
    sender_id UUID NOT NULL,

    content TEXT NOT NULL,

    is_read BOOLEAN NOT NULL DEFAULT FALSE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_message_participant
        FOREIGN KEY (chat_id, sender_id)
        REFERENCES chat_participants(chat_id, user_id)
        ON DELETE CASCADE,

    CONSTRAINT chk_message_content
        CHECK (LENGTH(TRIM(content)) > 0)
);

CREATE INDEX ix_messages_chat_created
    ON messages(chat_id, created_at);

-- =========================================================
-- USER LOCATIONS
-- Current location shared by the user
-- =========================================================

CREATE TABLE user_locations (
    user_id UUID PRIMARY KEY
        REFERENCES users(id) ON DELETE CASCADE,

    location GEOGRAPHY(POINT, 4326) NOT NULL,

    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX ix_user_locations_geo
    ON user_locations USING GIST(location);

-- =========================================================
-- ROUTE STOPS
-- Optional planned stops shared by a user
-- =========================================================

CREATE TABLE route_stops (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    user_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    name VARCHAR(255) NOT NULL,
    location GEOGRAPHY(POINT, 4326) NOT NULL,

    planned_arrival TIMESTAMPTZ,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX ix_route_stops_user_id
    ON route_stops(user_id);

CREATE INDEX ix_route_stops_geo
    ON route_stops USING GIST(location);

-- =========================================================
-- BLOCKS
-- Users can block other users
-- =========================================================

CREATE TABLE blocks (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    blocker_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    blocked_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT uq_block_pair
        UNIQUE (blocker_id, blocked_id),

    CONSTRAINT chk_block_not_self
        CHECK (blocker_id <> blocked_id)
);

-- =========================================================
-- REPORTS
-- User reports another user
-- =========================================================

CREATE TABLE reports (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    reporter_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    reported_user_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    reason report_reason NOT NULL,
    description TEXT,

    status report_status NOT NULL DEFAULT 'pending',

    assigned_admin_id UUID
        REFERENCES admin_accounts(id) ON DELETE SET NULL,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    resolved_at TIMESTAMPTZ,

    CONSTRAINT chk_report_not_self
        CHECK (reporter_id <> reported_user_id)
);

CREATE INDEX ix_reports_status
    ON reports(status);

CREATE INDEX ix_reports_reported_user
    ON reports(reported_user_id);

-- =========================================================
-- MODERATION ACTIONS
-- Admin activity history
-- =========================================================

CREATE TABLE moderation_actions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    admin_id UUID NOT NULL
        REFERENCES admin_accounts(id) ON DELETE RESTRICT,

    target_user_id UUID
        REFERENCES users(id) ON DELETE SET NULL,

    report_id UUID
        REFERENCES reports(id) ON DELETE SET NULL,

    action moderation_action_type NOT NULL,

    description TEXT,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX ix_moderation_actions_admin
    ON moderation_actions(admin_id);

-- =========================================================
-- REFRESH TOKENS
-- Refresh tokens for secure authentication
-- Store token hashes, not raw tokens
-- =========================================================

CREATE TABLE refresh_tokens (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    user_id UUID NOT NULL
        REFERENCES users(id) ON DELETE CASCADE,

    token_hash TEXT NOT NULL UNIQUE,

    expires_at TIMESTAMPTZ NOT NULL,
    revoked_at TIMESTAMPTZ,

    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX ix_refresh_tokens_user
    ON refresh_tokens(user_id);

-- =========================================================
-- UPDATED_AT TRIGGER
-- Automatically updates updated_at on row changes
-- =========================================================

CREATE OR REPLACE FUNCTION set_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_users_updated_at
BEFORE UPDATE ON users
FOR EACH ROW EXECUTE FUNCTION set_updated_at();

CREATE TRIGGER trg_user_profiles_updated_at
BEFORE UPDATE ON user_profiles
FOR EACH ROW EXECUTE FUNCTION set_updated_at();

CREATE TRIGGER trg_user_preferences_updated_at
BEFORE UPDATE ON user_preferences
FOR EACH ROW EXECUTE FUNCTION set_updated_at();

CREATE TRIGGER trg_admin_accounts_updated_at
BEFORE UPDATE ON admin_accounts
FOR EACH ROW EXECUTE FUNCTION set_updated_at();

CREATE TRIGGER trg_user_locations_updated_at
BEFORE UPDATE ON user_locations
FOR EACH ROW EXECUTE FUNCTION set_updated_at();

-- =========================================================
-- END OF TRUCKDATING DATABASE SCHEMA
-- =========================================================
