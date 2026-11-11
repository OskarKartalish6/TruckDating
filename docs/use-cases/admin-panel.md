# TruckDating — WPF Admin Panel Use Cases

## 1. Overview

**Application:** WPF Admin Panel  
**Platform:** Windows  
**Technology:** C# / WPF / MVVM  
**Main Actor:** Administrator

The Admin Panel allows administrators to manage users, moderate profile photos, review reports and maintain platform safety.

## 2. Actors

| Actor | Description |
|---|---|
| Administrator | Manages users and platform moderation |
| Backend API | Processes administrator requests and returns data |

## 3. Use Cases

### UC-01 — Administrator Login

**Actor:** Administrator

**Preconditions:**
- Administrator has an existing account.
- Backend API is available.

**Main Flow:**
1. Administrator opens the Admin Panel.
2. System displays the login window.
3. Administrator enters email and password.
4. System sends authentication data to the Backend API.
5. Backend validates credentials.
6. System opens the Admin Dashboard.

**Alternative Flows:**
- Invalid credentials: system displays an error message.
- Account is inactive: access is denied.
- Backend unavailable: system displays a connection error.

### UC-02 — View Dashboard

**Actor:** Administrator

**Preconditions:**
- Administrator is authenticated.

**Main Flow:**
1. Administrator opens the Dashboard.
2. System requests statistics from the Backend API.
3. Backend returns platform statistics.
4. System displays:
   - Total users
   - Active users
   - Suspended users
   - Pending reports
   - Photos awaiting moderation

### UC-03 — Manage Users

**Actor:** Administrator

**Main Flow:**
1. Administrator opens the Users section.
2. System requests the user list.
3. Administrator searches or filters users.
4. Administrator selects a user.
5. System displays the user's profile information.
6. Administrator can suspend or restore the account.
7. System sends the action to the Backend API.
8. Backend updates the account status.
9. System displays the result.

### UC-04 — Moderate Profile Photos

**Actor:** Administrator

**Main Flow:**
1. Administrator opens the Photo Moderation section.
2. System displays photos awaiting review.
3. Administrator selects a photo.
4. System displays the photo and its information.
5. Administrator approves or rejects the photo.
6. Backend updates the photo moderation status.
7. System displays the updated result.

### UC-05 — Review User Reports

**Actor:** Administrator

**Main Flow:**
1. Administrator opens the Reports section.
2. System displays submitted reports.
3. Administrator selects a report.
4. System displays the report reason, description and reported user.
5. Administrator reviews the information.
6. Administrator decides how to handle the report.
7. System sends the decision to the Backend API.
8. Backend updates the report status.

### UC-06 — Perform Moderation Action

**Actor:** Administrator

**Main Flow:**
1. Administrator opens a user profile or report.
2. Administrator selects a moderation action.
3. System requests confirmation.
4. Administrator confirms the action.
5. Backend processes the action.
6. Backend saves the moderation history.
7. System displays the result.

**Possible Actions:**
- Send warning
- Reject a photo
- Suspend an account
- Restore an account
- Ban an account

### UC-07 — View Moderation History

**Actor:** Administrator

**Main Flow:**
1. Administrator opens the Moderation History section.
2. System requests moderation records.
3. Backend returns the records.
4. System displays the administrator, action, affected user and date.

### UC-08 — Administrator Logout

**Actor:** Administrator

**Main Flow:**
1. Administrator selects Logout.
2. System clears the current authentication session.
3. System returns to the login window.

## 4. System Boundary

The WPF Admin Panel communicates with the ASP.NET Core Backend API.

The Admin Panel does not connect directly to PostgreSQL.

