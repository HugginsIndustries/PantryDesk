# TODO

Implementation checklist based on phased plan.

## Phase 0 — Repo setup and guardrails

- [x] Update project naming (FoodBank\* -> PantryDesk\*)
- [x] Create repo scaffolding files (.gitignore, README, LICENSE, etc.)
- [x] Create .NET solution skeleton
- [x] Get solution building and running from clean clone
- [x] Add `.editorconfig` and consistent formatting
- [x] Decide data folder location: `C:\ProgramData\PantryDesk\` (configurable via `AppConfig.GetDataRoot()`)
- [x] `PantryDeskApp` opens a blank window
- [x] `PantryDeskSeeder` runs and prints message

## Phase 1 — Database schema + data access layer

- [x] SQLite schema creation + migrations (schema version tracking)
- [x] Data access layer: Create/open DB
- [x] Data access layer: CRUD households
- [x] Data access layer: Insert/query service events
- [x] Data access layer: Pantry day CRUD
- [x] Data access layer: AuthRoles read/write (hashed passwords)
- [x] Central config table: schema version, app version
- [x] Verify: App can create DB and insert/read 1 household, 1 pantry day, 1 service event

## Phase 2 — Authentication (Entry/Admin)

- [x] Login form: role selection (Entry/Admin) + password
- [x] Role passwords stored salted+hashed
- [x] Admin screen: "Change role passwords"
- [x] Enforce Admin-only features (exports/restore/calendar edits blocked for Entry)
- [x] Verify: Can log in as Entry/Admin; Admin-only actions blocked for Entry

## Phase 3 — Core workflow: Search + Check-in + Create Household

- [x] Main Check-In screen: big search box (PrimaryName)
- [x] Results list with: PrimaryName, City/Zip, HouseholdSize + breakdown, Last service date + type, Eligibility badge, Inactive badge
- [x] Buttons: Complete Service, New Household, Open Profile
- [x] Name search: partial matching, case-insensitive
- [x] Complete Service: pantry day vs appointment classification
- [x] Eligibility check: monthly rule + override modal (reason required)
- [x] Household creation: PrimaryName + composition (required), address fields, optional email
- [x] Verify: Search → create household → check in → see last-served update in <30 seconds; override prompt triggers

## Phase 4 — Household Profile + Service History + Appointments

- [x] Household Profile form: all fields + Active toggle
- [x] Service History view: list of events with filters
- [x] Appointment scheduler: ScheduledDate + ScheduledText (required), optional notes
- [x] Actions: mark Scheduled appointment as Completed / Cancelled / NoShow
- [x] Verify: Schedule appointment → mark completed; NoShow/Cancelled doesn't affect eligibility

## Phase 5 — Pantry day calendar generator + editor

- [x] Admin-only "Pantry Days" screen
- [x] "Generate for Year" button with rule logic (Jan–Oct: 2nd/3rd/4th Wed; Nov–Dec: 1st/2nd/3rd Wed)
- [x] List with edit: change date, deactivate/reactivate, notes
- [x] Main check-in uses PantryDay match by date
- [x] Verify: Generate 2026 pantry days; edit one day and see check-in behavior reflect it

## Phase 6 — Stats dashboard + Monthly Summary Report (PDF + Print)

- [x] Stats screen: totals, completed services, unique households served, PantryDay vs Appointment, overrides breakdown, by city
- [x] Monthly Summary view: month picker, Export PDF, Print
- [x] PDF contents: totals, pantry day breakdown, household composition served, area breakdown
- [x] Verify: Export PDF for any month with seeded data looks clean; Print opens Windows print dialog

## Phase 7 — Backup / Export / Restore

- [x] Automatic daily backup: encrypted zip on first app run each day
- [x] Manual backup now
- [x] Optional "Backup to USB" (choose folder)
- [x] One-click restore (Admin-only): select backup zip, validate, safety-copy current DB, restore, prompt restart
- [x] Export (Admin-only): CSV (households, service_events, pantry_days), JSON structured export
- [x] Verify: Restore from backup confirms data reverts; Exports open fine in Excel (CSV)

## Phase 8 — Seeder tool

- [x] `PantryDeskSeeder` generates `demo_pantrydesk.db` + config metadata
- [x] Configurable: households count, monthsBack, city weights, age weights, household size distribution, events per pantry day range, appointments per week range, RNG seed
- [x] Outputs match constraints: No PO boxes, 360-555-xxxx phones, address city/zip in service area
- [x] Realism guardrails: no child-only households
- [x] Inject "demo moments": ineligible households, overrides with reasons, scheduled appointments upcoming
- [x] Verify: Running seeder produces DB that makes eligibility warnings visible, stats non-zero, monthly PDF interesting

## Phase 9 — Demo polish + hardening

- [x] UI polish: tab order, keyboard shortcuts (Enter to search), clear labels and error messages
- [x] Edge-case handling: duplicates warning, inactive household warning, prevent empty names / negative counts
- [x] "Demo mode" configuration: app points to `demo_pantrydesk.db` easily (config file or CLI arg)
- [x] Application icon support: executable icon and form icons for all windows
- [x] Publishing script: `publish.ps1` for creating standalone executables with demo files
- [x] Verify: Run scripted demo in 5–7 minutes without surprises

## Phase 10 — UX Improvements & Workflow Enhancements

### Appointment Visibility & Management

- [ ] Add dedicated "Appointments" screen with filters and search
  - Impact: High
  - Complexity: Medium
  - Acceptance Criteria:
    - New "Appointments" form accessible from Reports menu or top-level menu
    - Filters: date range (Today, Next 7 Days, Next 30 Days, Custom), status (Scheduled/Completed/Cancelled/NoShow), type (Appointment/PantryDay), search by PrimaryName/City
    - Columns: Date, Household Name, City/Zip, Status, Scheduled Text, Notes indicator
    - Default sort by Scheduled Date ascending, secondary sort by Name
    - Actions: Open Profile, Mark Completed/Cancelled/NoShow from selected row
  - Likely files:
    - `src/PantryDeskApp/Forms/AppointmentsForm.cs` (new)
    - `src/PantryDeskApp/Forms/AppointmentsForm.Designer.cs` (new)
    - `src/PantryDeskApp/Forms/CheckInForm.cs` (menu entry)
  - Rationale: Appointments currently only visible within individual household profiles, limiting scheduling oversight

- [ ] Add "Upcoming Appointments" panel to Check-In screen
  - Impact: High
  - Complexity: Medium
  - Acceptance Criteria:
    - Compact panel on Check-In showing next 7/14/30 days (configurable range)
    - Displays: Date, Household Name, Scheduled Text, Status
    - Selecting a row enables "Open Profile" and "Mark Completed" buttons
    - Default sort by earliest scheduled date
    - Panel updates when appointments are created/completed
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
    - `src/PantryDeskApp/Forms/CheckInForm.Designer.cs`
  - Rationale: Provides quick visibility of upcoming appointments in the main workflow hub

- [ ] Surface appointment completion actions without right-click dependency
  - Impact: High
  - Complexity: Small
  - Acceptance Criteria:
    - "Mark Completed", "Mark Cancelled", "Mark NoShow" buttons appear above Service History grid when a Scheduled appointment is selected
    - Buttons are disabled when no Scheduled appointment is selected or when non-Scheduled event is selected
    - Right-click context menu remains available as alternative
  - Likely files:
    - `src/PantryDeskApp/Forms/HouseholdProfileForm.cs`
    - `src/PantryDeskApp/Forms/HouseholdProfileForm.Designer.cs`
  - Rationale: Context menu actions are not discoverable; explicit buttons improve usability

### Statistics Dashboard Redesign

- [ ] Redesign unified Statistics Dashboard with date range selector and charts
  - Impact: High
  - Complexity: Medium
  - Acceptance Criteria:
    - Merge Monthly Summary functionality into Statistics Dashboard
    - Add date range selector with quick options: This Month, Last Month, Past 3 Months, Past 6 Months, Past Year, This Year, Last Year, Custom Range
    - Default to "This Month" to match current behavior
    - Top row: Summary cards with big numbers (Total Active Households, Total People, Completed Services, Unique Households Served)
    - Middle row: Charts using System.Windows.Forms.DataVisualization.Charting:
      - City Distribution (Pie/Donut chart) - households served by city
      - Age Group Distribution (Pie/Donut chart) - Children/Adults/Seniors from composition data
      - Monthly Visits Trend (Line chart) - completed services by month for selected range
    - Bottom row: Bar charts:
      - Pantry Day Volume by Event (Bar chart) - completed services per pantry day in range
      - Overrides by Reason (Bar/Stacked chart) - override counts by reason
    - Use ColorBrewer or standard colorblind-friendly palette
    - All charts include labels/legends and tooltip values for exact counts
    - Export PDF and Print buttons export/print the currently selected date range
    - PDF format includes all charts and maintains similar structure to current monthly summary
  - Likely files:
    - `src/PantryDeskApp/Forms/StatsForm.cs`
    - `src/PantryDeskApp/Forms/StatsForm.Designer.cs`
    - `src/PantryDeskCore/Services/StatisticsService.cs` (extend for date ranges)
    - `src/PantryDeskCore/Services/ReportService.cs` (update PDF generation to include charts)
    - `src/PantryDeskApp/PantryDeskApp.csproj` (add System.Windows.Forms.DataVisualization NuGet package)
  - Rationale: Current dashboard is text/grids only, harder to scan quickly; unified view with charts improves readability and decision-making

### Search & Check-In Improvements

- [ ] Add eligibility status icons with text badges
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Eligibility column shows "✅ Eligible" for eligible households
    - Eligibility column shows "❌ Already Served" for ineligible households
    - Color coding remains but is supplemented by icons/text
    - Add legend near results grid explaining eligibility indicators
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
    - `src/PantryDeskApp/Forms/CheckInForm.Designer.cs`
  - Rationale: Improves accessibility for colorblind users and provides clearer visual cues

- [ ] Add search debounce (250ms fixed delay)
  - Impact: Low
  - Complexity: Small
  - Acceptance Criteria:
    - Search triggers 250ms after user stops typing (debounced)
    - Enter key still triggers immediate search
    - Search remains responsive but reduces unnecessary queries during typing
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
  - Rationale: Reduces database load during rapid typing while maintaining responsiveness (low priority - user prefers immediate search but acknowledges benefit)

### Role Management & Navigation

- [ ] Replace "Logout" with "Switch Role" functionality
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Remove "Logout" menu item that exits application
    - Add "Switch Role" menu item that returns to Login dialog without exiting app
    - Current role is preserved in session until switch completes
    - After successful login with different role, Check-In form reopens with new role context
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
    - `src/PantryDeskApp/Forms/CheckInForm.Designer.cs`
    - `src/PantryDeskApp/Program.cs` (may need to adjust flow)
  - Rationale: Enables quick role switching during busy periods without full app restart

- [ ] Show current role and last backup date on Check-In status bar
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Status bar (or status strip) displays current role (Entry/Admin)
    - Status bar displays last backup date (formatted as "Last backup: MM/DD/YYYY" or "No backup yet")
    - Information updates when role switches or backup completes
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
    - `src/PantryDeskApp/Forms/CheckInForm.Designer.cs`
    - `src/PantryDeskCore/Services/BackupService.cs` (may need method to get last backup date)
  - Rationale: Provides context for volunteers about current permissions and backup status

### Household Form Improvements

- [ ] Improve New Household form layout with group boxes and auto-sizing
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Form auto-sizes with vertical scrolling if content exceeds screen
    - Group boxes organize fields: "Contact Information" (address, phone, email) and "Household Size" (children/adults/seniors)
    - Form remains functional on small screens without feeling cramped
  - Likely files:
    - `src/PantryDeskApp/Forms/NewHouseholdForm.cs`
    - `src/PantryDeskApp/Forms/NewHouseholdForm.Designer.cs`
  - Rationale: Current form is long and fixed-size, causing usability issues on smaller screens

- [ ] Add "Possible duplicates" hint on name field focus loss
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - When PrimaryName field loses focus, check for similar household names
    - Display hint message below name field if potential duplicates found (e.g., "Possible duplicate: Smith, John")
    - Hint appears immediately on focus loss (no debounce needed)
    - Hint does not block form submission but provides early warning
  - Likely files:
    - `src/PantryDeskApp/Forms/NewHouseholdForm.cs`
    - `src/PantryDeskApp/Forms/NewHouseholdForm.Designer.cs`
    - `src/PantryDeskCore/Data/HouseholdRepository.cs` (may need similarity search method)
  - Rationale: Current duplicate warning only appears at save time; early hint reduces data entry waste

### Backup/Restore UX

- [ ] Show data path and last backup date on Backup/Restore form
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Read-only panel at top of BackupRestoreForm displays:
      - Current database path (full path)
      - Last backup date (formatted, or "No backup yet")
    - Information helps users verify they're working with correct database before restore
  - Likely files:
    - `src/PantryDeskApp/Forms/BackupRestoreForm.cs`
    - `src/PantryDeskApp/Forms/BackupRestoreForm.Designer.cs`
  - Rationale: Provides confidence and context before performing restore operations

### Pantry Day Calendar Improvements

- [ ] Add month/year filter to Pantry Days admin view
  - Impact: Low
  - Complexity: Small
  - Acceptance Criteria:
    - Dropdown filters for month and year
    - Grid shows only pantry days matching selected month/year
    - "All" option available to show all pantry days
    - Inactive days highlighted (grayed out) in grid for visibility
  - Likely files:
    - `src/PantryDeskApp/Forms/PantryDaysForm.cs`
    - `src/PantryDeskApp/Forms/PantryDaysForm.Designer.cs`
  - Rationale: Reduces scrolling when managing large numbers of pantry days

### Seeder CLI Improvements

- [ ] Add validation for unknown command-line arguments
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Unknown flags produce clear error message: "Unknown argument: --flag. Use --help for usage information."
    - Error message includes hint to use --help
    - Seeder exits with non-zero code on unknown argument
  - Likely files:
    - `src/PantryDeskSeeder/SeederConfig.cs`
    - `src/PantryDeskSeeder/Program.cs`
  - Rationale: Current CLI silently ignores unknown args, which can cause confusion

- [ ] Print effective configuration summary before seeding begins
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Before database generation starts, print one-line summary: "Generating database with X households, Y months back, output: [path], seed: Z"
    - Summary reflects all effective settings (defaults + overrides)
    - Helps users confirm their inputs before generation
  - Likely files:
    - `src/PantryDeskSeeder/Program.cs`
    - `src/PantryDeskSeeder/SeederConfig.cs`
  - Rationale: Provides confirmation of effective configuration before potentially long-running operation
  