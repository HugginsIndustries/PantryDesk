# PantryDesk

**PantryDesk** is an offline-first Windows desktop application for food pantry client intake and tracking. It helps manage households, pantry day appointments, service eligibility, and monthly reporting—all without requiring an internet connection.

## Features

- **Household Management**: Search, create, and manage household profiles with per-member tracking and age groups (Infant, Child, Adult, Senior). Active status is system-managed from last qualifying service date (annual reset, default Jan 1; Admin can change reset date)
- **Check-in**: Search and action buttons on one row; search by name (250ms debounce; Enter for immediate search). Eligibility column shows ✅ Eligible / ❌ Already Served with color coding for accessibility. Status bar shows current role and last auto/manual backup dates. "Switch Role" (replaces Logout) returns to Login without exiting.
- **Pantry Days & Appointments**: Calendar-based pantry day scheduling with appointment support
- **Monthly Eligibility**: Visit types (Shop with TEFAP, Shop, TEFAP Only, Deck Only); only Shop/Shop with TEFAP count toward 1 visit/month limit. Override support (requires reason)
- **Statistics Dashboard**: Unified dashboard with date range selection (This Month, Last Month, Past 3/6/12 Months, This/Last Year, Custom Range) and two-page layout:
  - Summary cards: Total Active Households, Total People, Completed Services, Unique Households Served (date-range aware)
  - **Demographics page:** Five pie charts — City, Age Group, Race, Veteran Status, Disability Status (Age Group omits zero-count slices)
  - **Services page:** Visit Type and Event Type pie charts, Monthly Visits Trend (line), Pantry Day Volume by Event (bar); Pantry Day chart fills remaining space
  - Colorblind-friendly color palette (ColorBrewer Set2)
  - Export to PDF or print with embedded charts
- **Backup & Restore**: Automatic daily encrypted backup on first launch each day (AES-GCM with DPAPI or passphrase). Admin-only "Backup to USB…" for manual backup to a chosen folder (max 8 backups per folder; rotation removes oldest zip and its `.meta.json`). Separate tracking for last auto vs last manual backup; status bar and Restore form show both. Weekly reminder if no manual backup in 7+ days (Snooze or Backup Now). Restore form displays database path and last backup dates. One-click restore with safety copy (Admin-only). Backups respect demo mode configuration.
- **Data Export**: CSV (Excel-compatible) and JSON exports for external analysis (Admin-only)
- **Demo Data Seeder**: Separate console tool (`PantryDeskSeeder`) to generate realistic demo databases with configurable parameters (household count, date range, RNG seed for deterministic generation). Enforces all data constraints (service area addresses, phone format, no PO boxes, no child-only households) and includes demo moments (ineligible households, overrides, scheduled appointments). Unknown or invalid arguments produce clear errors; a one-line effective configuration summary is printed before generation.

## Privacy & Security

- **Offline-first**: No internet required at runtime. No web calls or SaaS dependencies.
- **PII Protection**: All client data is treated as sensitive PII. No PII is logged or exposed in diagnostics.
- **Role-based Access**: Two shared role logins (Entry/Admin) with salted password hashing.
- **Least Privilege**: Admin-only features (backups, exports, calendar edits, active status reset date) are enforced.

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
# --households <count>              Number of households (default: 300)
# --months-back <months>            How many months back (default: 6)
# --seed <number>                   RNG seed for deterministic generation
# --output <path>                   Output database path (default: demo_pantrydesk.db)
# --city-weights <pairs>            City weights (e.g., "Winlock=50,Vader=30,Ryderwood=20")
# --age-weights <pairs>             Age weights (e.g., "Child=30,Adult=50,Senior=20")
# --household-size-dist <pairs>     Household size distribution (e.g., "1=20,2=30,3=25,4=15,5=7,6=3")
# --events-per-pantry-day <range>   Range of events per pantry day (e.g., "25-50")
# --appointments-per-week <range>   Range of appointments per week (e.g., "2-8")
# --help                            Show usage information
```

### Publishing

To create standalone executables for distribution:

```powershell
.\publish.ps1
```

This script:

- Creates single-file, self-contained executables for both `PantryDeskApp` and `PantryDeskSeeder`
- Outputs everything to the `dist/` folder at the repo root
- Copies `demo_pantrydesk.db` and creates `PantryDesk.demo.config` (relative path) in `PantryDeskApp/` for portable demo
- Automatically overwrites existing `dist/` contents

The `dist/` folder will contain:

- `PantryDeskApp/PantryDeskApp.exe` - Standalone desktop application
- `PantryDeskApp/PantryDesk.demo.config` - Demo mode config (relative path)
- `PantryDeskApp/demo_pantrydesk.db` - Demo database
- `PantryDeskSeeder/PantryDeskSeeder.exe` - Standalone seeder tool

For portable demos, copy only the `PantryDeskApp/` folder (exe, config, and demo DB together).

### Configuration

#### Data Root Location

By default, PantryDesk stores its database and backups in `C:\ProgramData\PantryDesk\`. You can override this location by setting the `PANTRYDESK_DATA_ROOT` environment variable:

```powershell
# Set environment variable (current session)
$env:PANTRYDESK_DATA_ROOT = "D:\PantryDeskData"

# Or set permanently (requires admin)
[System.Environment]::SetEnvironmentVariable("PANTRYDESK_DATA_ROOT", "D:\PantryDeskData", "Machine")
```

The application will create the directory if it doesn't exist.

#### Demo mode

For demo scenarios, you can point the app at a pre-generated `demo_pantrydesk.db` without changing the normal data location:

1. Place `demo_pantrydesk.db` in the same folder as `PantryDeskApp.exe`, or anywhere on disk.
2. In the folder where `PantryDeskApp.exe` runs, create a text file named `PantryDesk.demo.config` with a single line:

   ```text
   DemoDatabasePath = demo_pantrydesk.db
   ```

   Use a relative path (e.g., `demo_pantrydesk.db`) for portable demos — the path is resolved from the config file location. Absolute paths are also supported.

3. Start the app normally. When this config file is present and the path exists, PantryDesk uses that database instead of the default `C:\ProgramData\PantryDesk\pantrydesk.db`.
4. Delete or rename `PantryDesk.demo.config` to return to normal behavior.

**Note:** When using demo mode, backups and restores will target the demo database path, not the default data root location. This allows you to safely test backup/restore functionality without affecting production data.

**Default Demo Passwords (Demo Databases Only):**

These passwords are **only** for demo databases generated by the seeder tool. Production databases require you to set passwords during initial setup.

- **Entry role**: `entry`
- **Admin role**: `admin`

## Repository Structure

- `PantryDeskApp` — WinForms desktop application
- `PantryDeskSeeder` — Console app to generate demo databases
- `PantryDeskCore` — Shared models/data/services used by both app and seeder

## Third-Party Libraries

- **QuestPDF** (MIT License) - Used for PDF generation in statistics reports. See <https://www.questpdf.com/> for details.
- **OxyPlot.WindowsForms** (MIT License) - Used for charting in the Statistics Dashboard. See <https://oxyplot.github.io/> for details.
- **OxyPlot.ImageSharp** (MIT License) - Used for exporting charts to images for PDF embedding. See <https://oxyplot.github.io/> for details.

## License

Licensed under **GNU GPL v3.0 or later**. See [LICENSE.md](LICENSE.md) for full license text.

## Repository

GitHub: <https://github.com/HugginsIndustries/PantryDesk>
