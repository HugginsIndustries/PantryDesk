# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Initial repository scaffolding
- .NET solution skeleton with PantryDeskApp, PantryDeskCore, and PantryDeskSeeder projects
- Repository documentation (README.md, TODO.md, CHANGELOG.md, LICENSE.md)
- .gitignore for .NET/Visual Studio
- `.editorconfig` for consistent C# code formatting
- `AppConfig` class in PantryDeskCore for data folder configuration (defaults to `C:\ProgramData\PantryDesk\`)
- **Phase 1: Database schema and data access layer**
  - SQLite database schema with 5 tables: `config`, `households`, `service_events`, `pantry_days`, `auth_roles`
  - Database migration system with schema version tracking (currently version 1)
  - `DatabaseManager` for connection management and database initialization
  - `DatabaseMigrator` for schema creation and version management
  - Data models: `Household`, `ServiceEvent`, `PantryDay`, `AuthRole`
  - Repository classes with full CRUD operations:
    - `HouseholdRepository` - Create, read, update, soft delete
    - `ServiceEventRepository` - Create and query methods
    - `PantryDayRepository` - Full CRUD operations
    - `AuthRoleRepository` - Role authentication with password hashing
    - `ConfigRepository` - Schema version and app version management
  - `PasswordHasher` using PBKDF2 with 100,000 iterations for secure password storage
  - All SQL queries use parameterized statements (SQL injection protection)
  - Centralized SQL statements in `Sql.cs` class
  - Phase 1 verification test button in PantryDeskApp
- **Phase 2: Authentication (Entry/Admin)**
  - `SessionManager` class for tracking current logged-in role
  - `PermissionChecker` helper class for Admin-only feature enforcement
  - `AuthRoleRepository.HasAnyRoles()` method to check if roles exist in database
  - `InitialSetupForm` for first-time password configuration (Entry and Admin roles)
    - Password validation (minimum 8 characters, confirmation required)
    - Warning if Entry and Admin passwords are identical
    - Stores passwords as salted hashes (no default passwords)
  - `LoginForm` for user authentication with role selection
    - Role dropdown (Entry/Admin)
    - Password verification using existing `AuthRoleRepository`
    - Error handling for invalid credentials
  - `ChangePasswordForm` for Admin to change role passwords
    - Requires current password verification
    - Validates new password (8+ characters, confirmation match)
    - Prevents reusing current password
    - Success/error feedback
  - Application startup flow: Initial Setup → Login → Main Form
  - Form1 updated with Admin menu (visible only to Admin users)
  - Form1 title displays current logged-in role
  - Logout functionality
- **Phase 3: Core workflow - Search + Check-in + Create Household**
  - `EligibilityService` class for checking monthly service eligibility rules
  - `HouseholdRepository.SearchByName()` method for partial, case-insensitive name search
  - `ServiceEventRepository.GetLastCompletedByHouseholdId()` method to retrieve most recent completed service
  - `CheckInForm` - Main check-in screen replacing Form1
    - Large search textbox with real-time results
    - DataGridView displaying household details: Name, City/Zip, Size breakdown, Last service, Eligibility status, Active/Inactive status
    - Complete Service button with eligibility checking and pantry day detection
    - New Household button
    - Open Profile button (placeholder for Phase 4)
  - `OverrideReasonForm` modal dialog for ineligible households
    - Required reason dropdown (Special Circumstance, Emergency Need, Admin Override, Other)
    - Optional notes field
  - `NewHouseholdForm` for creating new households
    - Required fields: PrimaryName, at least one person (Children/Adults/Seniors)
    - Optional fields: Address, City, State, Zip, Phone, Email, Notes
    - Validation with clear error messages
  - Monthly eligibility checking: households can only be served once per calendar month
  - Automatic event type classification: PantryDay if today matches active pantry day, else Appointment
  - Search results update immediately after service completion or household creation
- **Phase 4: Household Profile + Service History + Appointments**
  - `ServiceEventRepository.Update()` method for updating service event status and details
  - `HouseholdProfileForm` with tabbed interface (Profile, Service History, Appointments)
  - Profile tab: Full household CRUD with all fields including Active toggle and auto-calculated total size
  - Service History tab: DataGridView displaying all service events with Status and Type filters
  - Appointments tab: Schedule new appointments with required date and text, optional notes
  - Context menu actions on Service History: Mark Scheduled appointments as Completed/Cancelled/NoShow
  - Completed appointments trigger eligibility check and override modal (same as direct check-in)
  - Cancelled/NoShow appointments do NOT affect monthly eligibility
  - CheckInForm "Open Profile" button now opens HouseholdProfileForm and refreshes results on save
- **Phase 5: Pantry day calendar generator + editor**
  - `PantryDaysForm` - Admin-only form for managing pantry day calendar
  - Year generation feature with configurable year input (defaults to current year)
  - Automatic pantry day generation following business rules:
    - January-October: 2nd, 3rd, and 4th Wednesday of each month
    - November-December: 1st, 2nd, and 3rd Wednesday of each month
  - `GetNthWeekdayOfMonth()` helper method for calculating nth occurrence of weekday in month
  - Duplicate prevention: skips dates that already have pantry days
  - DataGridView listing all pantry days with Date, Active status, and Notes columns
  - Edit functionality: change date, toggle active/inactive status, edit notes
  - Date validation prevents duplicate pantry days when editing
  - "Pantry Days" menu item added to Admin menu in CheckInForm and Form1
  - Check-in integration already implemented: CheckInForm uses pantry day matching by date

### Changed

- PantryDeskSeeder now prints "Hello seed" message on run
- Application now requires authentication on every launch
- Form1 test button updated to handle existing pantry days (prevents UNIQUE constraint violations)
- Main application now shows CheckInForm instead of Form1 after login
- Program.cs updated to launch CheckInForm as the main screen
- CheckInForm "Open Profile" button now opens functional HouseholdProfileForm (replaces placeholder)

### Fixed

- Solution verified to build and run from clean clone
- PantryDeskApp opens blank window as expected
- Fixed transaction handling in `DatabaseMigrator.TableExists` method
- Fixed Phase 1 test button to check for existing pantry days before creating (prevents duplicate date errors)