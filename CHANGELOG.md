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

### Changed

- PantryDeskSeeder now prints "Hello seed" message on run
- Application now requires authentication on every launch
- Form1 test button updated to handle existing pantry days (prevents UNIQUE constraint violations)

### Fixed

- Solution verified to build and run from clean clone
- PantryDeskApp opens blank window as expected
- Fixed transaction handling in `DatabaseMigrator.TableExists` method
- Fixed Phase 1 test button to check for existing pantry days before creating (prevents duplicate date errors)