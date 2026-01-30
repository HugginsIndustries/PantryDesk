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

### Changed

- PantryDeskSeeder now prints "Hello seed" message on run

### Fixed

- Solution verified to build and run from clean clone
- PantryDeskApp opens blank window as expected
