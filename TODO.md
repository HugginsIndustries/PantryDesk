# TODO

Implementation checklist based on phased plan.

## Formatting Rules

- **Completed vs Open:** Move completed items (checked `[x]`) into the **Completed** section at the top. Keep only open items (unchecked `[ ]`) in the **Open** section.
- **Duplicate headings:** Append a space and `(Complete)` to section titles in the Completed section when the same title exists in Open (e.g., `### Client Requirements (Complete)`). This satisfies markdownlint MD024 no-duplicate-heading.
- **Retain structure and details:** When moving an item to Completed, keep the full formatting:
  - Section headers: use `####` for item titles (e.g., `#### Main Check-In Layout Tweak`), `###` for group titles (e.g., `### Client Requirements`).
  - Full details: Impact, Complexity, Acceptance Criteria, Likely files, Rationale — retain all of these.
- **Organization:** Use `#`, `##`, `###`, `####` for clear hierarchy. Match the structure of Open items so Completed mirrors the same organization.

---

## Completed

### Phase 0 — Repo setup and guardrails (Complete)

- [x] Update project naming (FoodBank\* -> PantryDesk\*)
- [x] Create repo scaffolding files (.gitignore, README, LICENSE, etc.)
- [x] Create .NET solution skeleton
- [x] Get solution building and running from clean clone
- [x] Add `.editorconfig` and consistent formatting
- [x] Decide data folder location: `C:\ProgramData\PantryDesk\` (configurable via `AppConfig.GetDataRoot()`)
- [x] `PantryDeskApp` opens a blank window
- [x] `PantryDeskSeeder` runs and prints message

### Phase 1 — Database schema + data access layer (Complete)

- [x] SQLite schema creation + migrations (schema version tracking)
- [x] Data access layer: Create/open DB
- [x] Data access layer: CRUD households
- [x] Data access layer: Insert/query service events
- [x] Data access layer: Pantry day CRUD
- [x] Data access layer: AuthRoles read/write (hashed passwords)
- [x] Central config table: schema version, app version
- [x] Verify: App can create DB and insert/read 1 household, 1 pantry day, 1 service event

### Phase 2 — Authentication (Entry/Admin) (Complete)

- [x] Login form: role selection (Entry/Admin) + password
- [x] Role passwords stored salted+hashed
- [x] Admin screen: "Change role passwords"
- [x] Enforce Admin-only features (exports/restore/calendar edits blocked for Entry)
- [x] Verify: Can log in as Entry/Admin; Admin-only actions blocked for Entry

### Phase 3 — Core workflow: Search + Check-in + Create Household (Complete)

- [x] Main Check-In screen: big search box (PrimaryName)
- [x] Results list with: PrimaryName, City/Zip, HouseholdSize + breakdown, Last service date + type, Eligibility badge, Inactive badge
- [x] Buttons: Complete Service, New Household, Open Profile
- [x] Name search: partial matching, case-insensitive
- [x] Complete Service: pantry day vs appointment classification
- [x] Eligibility check: monthly rule + override modal (reason required)
- [x] Household creation: PrimaryName + composition (required), address fields, optional email
- [x] Verify: Search → create household → check in → see last-served update in <30 seconds; override prompt triggers

### Phase 4 — Household Profile + Service History + Appointments (Complete)

- [x] Household Profile form: all fields + Active toggle
- [x] Service History view: list of events with filters
- [x] Appointment scheduler: ScheduledDate + ScheduledText (required), optional notes
- [x] Actions: mark Scheduled appointment as Completed / Cancelled / NoShow
- [x] Verify: Schedule appointment → mark completed; NoShow/Cancelled doesn't affect eligibility

### Phase 5 — Pantry day calendar generator + editor (Complete)

- [x] Admin-only "Pantry Days" screen
- [x] "Generate for Year" button with rule logic (Jan–Oct: 2nd/3rd/4th Wed; Nov–Dec: 1st/2nd/3rd Wed)
- [x] List with edit: change date, deactivate/reactivate, notes
- [x] Main check-in uses PantryDay match by date
- [x] Verify: Generate 2026 pantry days; edit one day and see check-in behavior reflect it

### Phase 6 — Stats dashboard + Monthly Summary Report (PDF + Print) (Complete)

- [x] Stats screen: totals, completed services, unique households served, PantryDay vs Appointment, overrides breakdown, by city
- [x] Monthly Summary view: month picker, Export PDF, Print
- [x] PDF contents: totals, pantry day breakdown, household composition served, area breakdown
- [x] Verify: Export PDF for any month with seeded data looks clean; Print opens Windows print dialog

### Phase 7 — Backup / Export / Restore (Complete)

- [x] Automatic daily backup: encrypted zip on first app run each day
- [x] Manual backup now
- [x] Optional "Backup to USB" (choose folder)
- [x] One-click restore (Admin-only): select backup zip, validate, safety-copy current DB, restore, prompt restart
- [x] Export (Admin-only): CSV (households, service_events, pantry_days), JSON structured export
- [x] Verify: Restore from backup confirms data reverts; Exports open fine in Excel (CSV)

### Phase 8 — Seeder tool (Complete)

- [x] `PantryDeskSeeder` generates `demo_pantrydesk.db` + config metadata
- [x] Configurable: households count, monthsBack, city weights, age weights, household size distribution, events per pantry day range, appointments per week range, RNG seed
- [x] Outputs match constraints: No PO boxes, 360-555-xxxx phones, address city/zip in service area
- [x] Realism guardrails: no child-only households
- [x] Inject "demo moments": ineligible households, overrides with reasons, scheduled appointments upcoming
- [x] Verify: Running seeder produces DB that makes eligibility warnings visible, stats non-zero, monthly PDF interesting

### Phase 9 — Demo polish + hardening (Complete)

- [x] UI polish: tab order, keyboard shortcuts (Enter to search), clear labels and error messages
- [x] Edge-case handling: duplicates warning, inactive household warning, prevent empty names / negative counts
- [x] "Demo mode" configuration: app points to `demo_pantrydesk.db` easily (config file or CLI arg)
- [x] Application icon support: executable icon and form icons for all windows
- [x] Publishing script: `publish.ps1` for creating standalone executables with demo files
- [x] Verify: Run scripted demo in 5–7 minutes without surprises

### Phase 10 — Statistics Dashboard Redesign (Complete)

- [x] Redesign unified Statistics Dashboard with date range selector and charts
  - Impact: High
  - Complexity: Medium
  - Acceptance Criteria:
    - Merge Monthly Summary functionality into Statistics Dashboard
    - Add date range selector with quick options: This Month, Last Month, Past 3 Months, Past 6 Months, Past Year, This Year, Last Year, Custom Range
    - Default to "This Month" to match current behavior
    - Top row: Summary cards with big numbers (Total Active Households, Total People, Completed Services, Unique Households Served)
    - Charts using OxyPlot.WindowsForms:
      - City Distribution (Pie chart) - households served by city
      - Age Group Distribution (Pie chart) - Children/Adults/Seniors from composition data
      - Monthly Visits Trend (Line chart) - completed services by month for selected range
      - Pantry Day Volume by Event (Bar chart) - completed services per pantry day in range
    - Use colorblind-friendly palette
    - All charts include labels/legends and hover tooltips for exact counts
    - Export PDF and Print buttons export/print the currently selected date range
    - PDF format includes all charts as embedded images and maintains similar structure to current monthly summary
  - Likely files:
    - `src/PantryDeskApp/Forms/StatsForm.cs`
    - `src/PantryDeskApp/Forms/StatsForm.Designer.cs`
    - `src/PantryDeskCore/Services/StatisticsService.cs` (extend for date ranges)
    - `src/PantryDeskCore/Services/ReportService.cs` (update PDF generation to include charts)
    - `src/PantryDeskApp/PantryDeskApp.csproj` (add OxyPlot.WindowsForms and OxyPlot.ImageSharp NuGet packages)
  - Rationale: Current dashboard is text/grids only, harder to scan quickly; unified view with charts improves readability and decision-making

### Client Requirements (Complete)

#### Main Check-In Layout Tweak

- [x] Move City/Zip field to far-right column on main check-in page
  - Impact: Low
  - Complexity: Small
  - Acceptance Criteria:
    - City/Zip column moved to far-right
    - Other columns remain in same order
  - Likely files:
    - `CheckInForm.cs`, `CheckInForm.Designer.cs`
  - Rationale: Client layout preference for readability

#### Main Page Font Size

- [x] Increase font size on main check-in page (household table/list view)
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Larger font on household table/list for improved readability
  - Likely files:
    - `CheckInForm.Designer.cs`
  - Rationale: Client feedback on readability

#### Annual Active-Status Reset

- [x] Derive and sync active status from last qualifying service date
  - Impact: High
  - Complexity: Medium
  - Acceptance Criteria:
    - Keep `IsActive` column; it is stored and displayed in the household profile and main check-in table
    - **Not manually editable** — remove the Active toggle from the household profile; status is system-managed only
    - On every app launch, sync `IsActive` for all households: set false where last qualifying service date is before the reset date (or never served)
    - When completing a qualifying service, set `IsActive` = true for that household (first service in the reporting year reactivates them)
    - Default reset date: Jan 1 each year (reporting year Jan–Dec); Admin setting to change reset date
    - Seeder-generated demo data produces inactive households naturally via service event dates
  - Likely files:
    - Household schema (keep `IsActive`)
    - `HouseholdProfileForm` (remove Active toggle, keep read-only display)
    - Startup logic (sync on every app launch)
    - Complete Service flow (set `IsActive` = true on service completion)
    - Config/settings for reset date
  - Rationale: Annual reporting cycle; automatic status from service history simplifies workflow; demo data stays realistic without manual seeding

#### Per-Household Member Tracking (with Age-Group Calculation) (Complete)

- [x] Add per-household member tracking and age-group calculation (replace single household name with individual members; birthdays drive age groups)
  - Impact: High
  - Complexity: High
  - Acceptance Criteria:
    - Each household has one or more members; each member requires:
      - First name (required)
      - Last name (required)
      - Birthday (required)
    - Optional member fields:
      - Race: White, Black, Hispanic, Native American, Not Specified (used when client refuses/declines; preferred to collect when possible for grants)
      - Veteran status: None, Active Duty, Reserve, Veteran, Unknown, Prefer Not To Answer (default: Prefer Not To Answer; self-reported)
      - Disabled status: Not Disabled, Disabled, Unknown, Prefer Not To Answer (default: Prefer Not To Answer; self-reported)
    - No edit history required for member changes
    - "Disabled Veteran" derived field (Veteran + Disabled) optionally included as reportable
    - **Age groups:** Infant (0–2), Child (2–18), Adult (18–55), Senior (55+). Derived from birthday; no unknown age group. Age group updates automatically on birthdays (e.g., 18th birthday moves member into Adult). Used for reporting and deck-only form.
    - **Main check-in table:** Rename "Name" to "Name (Primary)"; add "Members" column to the right of Name (Primary) with comma-separated list of non-primary members (alphabetical). Each household remains one row. Column widths: AllCells for content-fit columns; Members uses Fill for remaining space. Truncated Members cells show full list via built-in DataGridView tooltip on hover.
    - **Search:** Works on any household member name (primary or other members) — staff can find households when different members come to shop.
  - Update **PantryDeskSeeder** to generate member data (Lewis County, WA defaults; configurable weights; RNG seed for reproducibility):
    - **Birthdays:** Generated from age-group distribution. Household composition defines counts per age band (Infant, Child, Adult, Senior); for each member, assign an age band, then generate a random birthday within that band so age group and birthday align.
    - **Age groups:** Infant (0–2) 2%, Child (2–18) 19%, Adult (18–55) 45%, Senior (55+) 34%
    - **Race:** White 81%, Hispanic 12%, Native American 2%, Black 1%, Not Specified 4%
    - **Veteran status:** None 85.8%, Veteran 11%, Active Duty 0.4%, Reserve 0.3%, Unknown 1.5%, Prefer Not To Answer 1%
    - **Disabled status:** Not Disabled 82%, Disabled 14%, Unknown 2%, Prefer Not To Answer 2%
  - Likely files:
    - Schema migration (household_members table or equivalent)
    - `PantryDeskCore` models, repositories, age-group helper
    - `NewHouseholdForm`, `HouseholdProfileForm`, composition UI
    - `CheckInForm.cs`, `CheckInForm.Designer.cs` (main table columns, search by any member)
    - `PantryDeskSeeder` (member generation, birthday-from-age-group, demographic weights)
    - Reporting queries, deck-only form
  - Rationale: Client needs individual member tracking for grants; demo data should reflect Lewis County; age groups and birthdays must align; staff find households when any member shops

#### Complete Service Dialog Enhancements

- [x] Enhance Complete Service dialog with Visit Type and eligibility rules
  - Impact: High
  - Complexity: Medium
  - Acceptance Criteria:
    - When clicking Complete Service, prompt for:
      - Visit Type (required): Shop with TEFAP, Shop, TEFAP Only, Deck Only
      - Notes (optional)
    - Monthly visit limit rule:
      - ONLY "Shop with TEFAP" and "Shop" count toward 1 visit/month limit
      - "TEFAP Only" and "Deck Only" do NOT count; can occur multiple times as needed
    - Staff chooses visit type; typically "Shop with TEFAP"
    - TEFAP eligibility tracked only via visit type (no separate TEFAP eligibility fields)
  - Likely files:
    - `CheckInForm.cs` (Complete Service flow)
    - Service event schema (visit type field)
    - `EligibilityService` or equivalent
  - Rationale: Different visit types have different eligibility implications for reporting

### Phase 10 — UX Improvements & Workflow Enhancements (Complete)

#### Search & Check-In Improvements

- [x] Add eligibility status icons with text badges
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Eligibility column shows "✅ Eligible" for eligible households
    - Eligibility column shows "❌ Already Served" for ineligible households
    - Color coding remains but is supplemented by icons/text
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
    - `src/PantryDeskApp/Forms/CheckInForm.Designer.cs`
  - Rationale: Improves accessibility for colorblind users and provides clearer visual cues

- [x] Add search debounce (250ms fixed delay)
  - Impact: Low
  - Complexity: Small
  - Acceptance Criteria:
    - Search triggers 250ms after user stops typing (debounced)
    - Enter key still triggers immediate search
    - Search remains responsive but reduces unnecessary queries during typing
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
  - Rationale: Reduces database load during rapid typing while maintaining responsiveness (low priority - user prefers immediate search but acknowledges benefit)

---

## Open

### Phase 10 — UX Improvements & Workflow Enhancements

#### Appointment Visibility & Management

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

#### Statistics Dashboard Redesign

- [ ] Improve chart tooltip formatting and behavior
  - Impact: Medium
  - Complexity: Medium
  - Acceptance Criteria:
    - Custom tooltip implementation with improved formatting:
      - Date formatting: "Pantry Day: 2025-02-25" instead of including time ("2025-02-25 18:00:00 2025-02-26 06:00:00")
      - Count formatting: "Count: 39" instead of "Count: 0 39" (remove zero values)
      - Better overall formatting and readability
    - Improved hover behavior (smoother, less flicker)
    - Disable built-in OxyPlot tooltips in favor of custom implementation
    - Tooltips appear consistently on hover across all chart types
  - Likely files:
    - `src/PantryDeskApp/Forms/StatsForm.cs` (custom tooltip implementation)
    - May require custom WinForms ToolTip control or custom rendering
  - Rationale: Current OxyPlot tooltips show raw data with timestamps and multiple values, making them hard to read. Custom implementation would provide cleaner, more user-friendly tooltips with better hover behavior.

#### Role Management, Check-In Layout & Backup UX

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

- [ ] Move action buttons to same row as search; remove search label
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Search box and Complete Service, New Household, Open Profile buttons on one row
    - Search box fills remaining space on left; buttons on right
    - Remove "Search Name:" label; use "Search by name..." placeholder only (saves space on smaller displays)
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
    - `src/PantryDeskApp/Forms/CheckInForm.Designer.cs`
  - Rationale: More compact top area and clearer layout

- [ ] Add Check-In status bar with role and backup dates
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Status bar at bottom of Check-In window
    - Same font and size as main table (Segoe UI 12pt), bold
    - Left: current role (Entry/Admin)
    - Right: backup status in format "Last Auto Backup: YYYY-MM-DD  Last Manual Backup: YYYY-MM-DD" (use "No backup yet" if never run); consistent date format throughout
    - Updates when role switches or backup completes
  - Likely files:
    - `src/PantryDeskApp/Forms/CheckInForm.cs`
    - `src/PantryDeskApp/Forms/CheckInForm.Designer.cs`
    - `src/PantryDeskCore/Services/BackupService.cs` (methods to get last backup dates)
  - Rationale: Clear visibility of role and backup status at a glance

- [ ] Show data path and last backup dates on BackupRestoreForm
  - Impact: Medium
  - Complexity: Small
  - Acceptance Criteria:
    - Read-only panel at top of BackupRestoreForm displays:
      - Current database path (full path)
      - Last Auto Backup: YYYY-MM-DD (or "No backup yet")
      - Last Manual Backup: YYYY-MM-DD (or "No backup yet")
    - Information helps users verify correct database before restore
  - Likely files:
    - `src/PantryDeskApp/Forms/BackupRestoreForm.cs`
    - `src/PantryDeskApp/Forms/BackupRestoreForm.Designer.cs`
  - Rationale: Provides confidence and context before performing restore operations

- [ ] Fix Backup to USB; add rotation, separate tracking, and weekly reminder
  - Impact: High
  - Complexity: Medium
  - Acceptance Criteria:
    - **Fix:** Use SQLite backup API (or equivalent) so Backup to USB works while database is open instead of copying locked file
    - **Separate tracking:** Store last automatic backup date and last manual backup date separately
    - **Backup rotation:** When Backup to USB targets same folder, keep max 8 backups; delete oldest when creating 9th (per-folder)
    - **Weekly reminder (7 days):** On app launch, if no manual backup in 7+ days, show popup with:
      - Snooze — hides popup until next app launch
      - Backup Now — admin-only, opens Backup to USB dialog; for Entry users, show button disabled with note "Login as Admin to complete manual backup ASAP."
    - Status bar shows both last auto and last manual backup (see status bar item above)
  - Likely files:
    - `src/PantryDeskCore/Services/BackupService.cs`
    - `src/PantryDeskApp/Forms/CheckInForm.cs` (launch reminder)
    - Config/metadata for last manual backup date
  - Rationale: Backup to USB fails with locked DB; separate tracking and weekly reminder encourage regular manual backups; rotation prevents folder clutter

#### Household Form Improvements

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

#### Pantry Day Calendar Improvements

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

#### Seeder CLI Improvements

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

---

### Client Requirements

#### Member Demographics Aggregate Reporting

- [ ] Add aggregate reporting for member demographics
  - Impact: Medium
  - Complexity: Medium
  - Acceptance Criteria:
    - Report totals for: Race, Veteran status, Disabled status (across served households/members)
    - Optionally include derived "Disabled Veteran" totals (Veteran + Disabled)
    - Report accessible from Reports menu or Statistics Dashboard
  - Likely files:
    - `StatisticsService`, `ReportService`
    - New or extended report form
  - Rationale: Grant reporting requires demographic breakdowns

#### Deck-Only Monthly Bulk Entry

- [ ] Add deck-only monthly bulk entry with averaged totals
  - Impact: High
  - Complexity: High
  - Acceptance Criteria:
    - Form for deck-only shopping: Name, Household total, counts per age group (0–2, 2–18, 18–55, 55+)
    - Staff totals all pages over a month (Household total + each age-group total)
    - Divide totals by number of pages (averaging for repeated samples as form is swapped out)
    - Averaged totals added in bulk to monthly statistics for reporting
    - Add deck-only averaged totals ONLY to duplicated individual counts (overall + by age group) for Monthly Activity Report
    - Do not derive unduplicated deck-only counts
  - Likely files:
    - New deck-only entry form
    - `StatisticsService`, Monthly Activity Report logic
    - Config/metadata for storing deck-only monthly data
  - Rationale: Deck-only visitors fill paper form; staff need to incorporate averaged totals into reports

#### Monthly Activity Report (Letter-Size Landscape)

- [ ] Add formatted "Monthly Activity Report" (one Letter-size page, landscape)
  - Impact: High
  - Complexity: High
  - Acceptance Criteria:
    - Accessible from Reports menu; opens dialog
    - Default month: last month; allow selecting month
    - Required header fields: Food bank name, county, prepared by, month/year, phone number
    - Total days open for food distribution: pantry days only (usually 3 unless schedule changes)
    - Household counting: count each household at most once per month; include if at least one Completed qualifying service in selected month
    - Qualifying service types: "Shop with TEFAP", "TEFAP Only", "Shop" only (excludes "Deck Only")
    - Total pounds distributed: (Households Served Total * 65 lbs/household) from all sources
    - Households served:
      - Total = unique households with at least one qualifying service in month
      - Unduplicated = first qualifying service this reporting year occurs in selected month
      - Duplicated = Total - Unduplicated
    - Individuals served:
      - Based on served households; count each household's members once
      - Breakdown by age group (0–2, 2–18, 18–55, 55+) with totals and grand total
      - Duplicated/unduplicated splits match household classification
      - Add deck-only averaged monthly totals to duplicated individual counts (overall + by age group)
    - Fit on one Letter-size sheet, landscape orientation
    - Export PDF and Print options (same as statistics dashboard)
  - Likely files:
    - New `MonthlyActivityReportForm.cs`
    - `ReportService` (PDF generation)
    - `StatisticsService` (report queries)
  - Rationale: Grant reporting requires specific format and metrics
