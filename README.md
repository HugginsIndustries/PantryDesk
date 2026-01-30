# PantryDesk

**PantryDesk** is an offline-first Windows desktop application for food pantry client intake and tracking. It helps manage households, pantry day appointments, service eligibility, and monthly reporting—all without requiring an internet connection.

## Features

- **Household Management**: Search, create, and manage household profiles with composition tracking (Children/Adults/Seniors)
- **Pantry Days & Appointments**: Calendar-based pantry day scheduling with appointment support
- **Monthly Eligibility**: Automatic eligibility tracking with override support (requires reason)
- **Statistics Dashboard**: Real-time statistics showing active households, total people served, completed services, unique households served, PantryDay vs Appointment breakdown, override counts by reason, and city-level breakdown (Winlock/Vader/Ryderwood)
- **Monthly Summary Reports**: Comprehensive monthly reports with totals, pantry day breakdown tables, household composition served (Children/Adults/Seniors), and area breakdown. Export to PDF or print directly (print uses PDF format for consistent output)
- **Backup & Restore**: Automatic daily encrypted backups (AES-256), manual backup to default location or USB, one-click restore with safety copy (Admin-only)
- **Data Export**: CSV (Excel-compatible) and JSON exports for external analysis (Admin-only)
- **Demo Data Seeder**: Separate console tool (`PantryDeskSeeder`) to generate realistic demo databases with configurable parameters (household count, date range, RNG seed for deterministic generation). Enforces all data constraints (service area addresses, phone format, no PO boxes, no child-only households) and includes demo moments (ineligible households, overrides, scheduled appointments)

## Privacy & Security

- **Offline-first**: No internet required at runtime. No web calls or SaaS dependencies.
- **PII Protection**: All client data is treated as sensitive PII. No PII is logged or exposed in diagnostics.
- **Role-based Access**: Two shared role logins (Entry/Admin) with salted password hashing.
- **Least Privilege**: Admin-only features (backups, exports, calendar edits) are enforced.

## Quick Start

### Prerequisites

- .NET SDK (latest LTS version)
- Windows 10 or later

### Build

```bash
dotnet restore
dotnet build
```

### Run

```bash
# Run the desktop app
dotnet run --project src/PantryDeskApp

# Run the seeder tool (generates demo_pantrydesk.db in current directory)
dotnet run --project src/PantryDeskSeeder

# Seeder options:
# --households <count>     Number of households (default: 150)
# --months-back <months>   How many months back (default: 6)
# --seed <number>          RNG seed for deterministic generation
# --output <path>           Output database path (default: demo_pantrydesk.db)
# --help                    Show usage information
```

## Repository Structure

- `PantryDeskApp` — WinForms desktop application
- `PantryDeskSeeder` — Console app to generate demo databases
- `PantryDeskCore` — Shared models/data/services used by both app and seeder

## Third-Party Libraries

- **QuestPDF** (MIT License) - Used for PDF generation in monthly summary reports. See <https://www.questpdf.com/> for details.

## License

Licensed under **GNU GPL v3.0 or later**. See [LICENSE.md](LICENSE.md) for full license text.

## Repository

GitHub: <https://github.com/HugginsIndustries/PantryDesk>
