# TruckDating — Flutter Mobile App Use Cases

## 1. Overview

**Application:** TruckDating Mobile App  
**Platform:** Android / iOS  
**Technology:** Flutter / Dart  
**Main Actor:** User

The mobile application allows users to create profiles, discover other people, exchange likes, create matches and communicate through private chats.

## 2. Actors

| Actor | Description |
|---|---|
| User | Uses the application to discover and communicate with other users |
| Backend API | Processes application requests and manages business logic |
| Push Notification Service | Sends notifications to users |

## 3. Use Cases

### UC-01 — Register Account

**Actor:** User

**Preconditions:**
- User does not have an existing account.
- User meets the minimum age requirement.

**Main Flow:**
1. User opens the application.
2. User selects Register.
3. System displays the registration form.
4. User enters email and password.
5. User accepts the required terms.
6. Application sends registration data to the Backend API.
7. Backend validates the data and creates an account.
8. System confirms successful registration.

**Alternative Flows:**
- Email already exists: system displays an error.
- Invalid registration data: system displays validation errors.
- User does not meet the age requirement: registration is rejected.

### UC-02 — Login

**Actor:** User

**Main Flow:**
1. User opens the application.
2. User enters email and password.
3. Application sends credentials to the Backend API.
4. Backend validates the credentials.
5. Backend returns authentication tokens.
6. Application opens the main screen.

**Alternative Flow:**
- Invalid credentials: system displays an error message.

### UC-03 — Create and Edit Profile

**Actor:** User

**Preconditions:**
- User is authenticated.

**Main Flow:**
1. User opens the Profile section.
2. User enters personal information.
3. User adds a profile description.
4. User optionally adds information about truck driving.
5. User uploads profile photos.
6. Application sends profile data to the Backend API.
7. Backend saves the profile.
8. System displays the updated profile.

### UC-04 — Set Discovery Preferences

**Actor:** User

**Main Flow:**
1. User opens Discovery Settings.
2. User selects preferred age range.
3. User selects maximum search distance.
4. User chooses whether to see only truck drivers.
5. Application sends preferences to the Backend API.
6. Backend saves the preferences.

### UC-05 — Discover Users

**Actor:** User

**Preconditions:**
- User is authenticated.
- User has completed the required profile information.

**Main Flow:**
1. User opens the Discovery section.
2. Application requests recommended profiles.
3. Backend searches for suitable users based on preferences.
4. Backend returns available profiles.
5. Application displays user cards.
6. User can view profile information and photos.

### UC-06 — Like a User

**Actor:** User

**Main Flow:**
1. User views another user's profile.
2. User selects Like.
3. Application sends the like request to the Backend API.
4. Backend saves the like.
5. Backend checks whether the other user has already liked the current user.
6. If both users liked each other, Backend creates a Match.
7. Application displays a Match notification.

**Alternative Flow:**
- If there is no mutual like, the like is saved without creating a match.

### UC-07 — View Matches

**Actor:** User

**Main Flow:**
1. User opens the Matches section.
2. Application requests the user's matches.
3. Backend returns the match list.
4. Application displays available matches.
5. User selects a match to open the conversation.

### UC-08 — Send and Receive Messages

**Actor:** User

**Preconditions:**
- Users have an existing match.
- A chat exists for the match.

**Main Flow:**
1. User opens a conversation.
2. Application loads the chat history.
3. User enters a message.
4. Application sends the message through the chat connection.
5. Backend validates and saves the message.
6. The other user receives the message.
7. Application displays the new message.

**Alternative Flows:**
- Connection unavailable: application displays a connection error.
- User is not a participant in the chat: message sending is rejected.

### UC-09 — Share Current Location

**Actor:** User

**Preconditions:**
- User is authenticated.
- Location permission is granted.

**Main Flow:**
1. User opens Location Settings.
2. User enables location sharing.
3. Application obtains the current location.
4. Application sends location data to the Backend API.
5. Backend updates the user's location.
6. Location can be used for nearby discovery.

**Alternative Flows:**
- Permission denied: location sharing remains disabled.
- User disables location sharing: application requests deletion of the shared location.

### UC-10 — Manage Route Stops

**Actor:** User

**Main Flow:**
1. User opens the Route section.
2. User selects Add Stop.
3. User selects a location on the map.
4. User enters the stop name and optional arrival time.
5. Application sends the stop information to the Backend API.
6. Backend saves the route stop.
7. Application displays the saved stop.

**Alternative Flows:**
- User edits an existing stop.
- User deletes an existing stop.

### UC-11 — Block a User

**Actor:** User

**Main Flow:**
1. User opens another user's profile.
2. User selects Block.
3. Application requests confirmation.
4. User confirms the action.
5. Backend saves the block.
6. The blocked user is excluded from discovery and communication.

### UC-12 — Report a User

**Actor:** User

**Main Flow:**
1. User opens another user's profile.
2. User selects Report.
3. Application displays available report reasons.
4. User selects a reason.
5. User optionally enters a description.
6. Application sends the report to the Backend API.
7. Backend saves the report.
8. System confirms successful submission.

### UC-13 — Manage Account

**Actor:** User

**Main Flow:**
1. User opens Account Settings.
2. User can update account information.
3. User can change the password.
4. User can log out.
5. User can request account deletion.
6. Backend processes the requested action.

## 4. System Boundary

The Flutter application communicates with the ASP.NET Core Backend API using REST API and SignalR.

The mobile application does not connect directly to PostgreSQL.

