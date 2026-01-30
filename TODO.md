# TODO

Implementation checklist based on phased plan.

## Phase 0 — Repo setup and guardrails

- [x] Update project naming (FoodBank* -> PantryDesk*)
- [x] Create repo scaffolding files (.gitignore, README, LICENSE, etc.)
- [x] Create .NET solution skeleton
- [ ] Get solution building and running from clean clone
- [ ] Add `.editorconfig` and consistent formatting
- [ ] Decide data folder location: `C:\ProgramData\PantryDeskApp\` (configurable)
- [ ] `PantryDeskApp` opens a blank window
- [ ] `PantryDeskSeeder` runs and prints message

## Phase 1 — Database schema + data access layer

- [ ] SQLite schema creation + migrations (schema version tracking)
- [ ] Data access layer: Create/open DB
- [ ] Data access layer: CRUD households
- [ ] Data access layer: Insert/query service events
- [ ] Data access layer: Pantry day CRUD
- [ ] Data access layer: AuthRoles read/write (hashed passwords)
- [ ] Central config table: schema version, app version
- [ ] Verify: App can create DB and insert/read 1 household, 1 pantry day, 1 service event

## Phase 2 — Authentication (Entry/Admin)

- [ ] Login form: role selection (Entry/Admin) + password
- [ ] Role passwords stored salted+hashed
- [ ] Admin screen: "Change role passwords"
- [ ] Enforce Admin-only features (exports/restore/calendar edits blocked for Entry)
- [ ] Verify: Can log in as Entry/Admin; Admin-only actions blocked for Entry

## Phase 3 — Core workflow: Search + Check-in + Create Household

- [ ] Main Check-In screen: big search box (PrimaryName)
- [ ] Results list with: PrimaryName, City/Zip, HouseholdSize + breakdown, Last service date + type, Eligibility badge, Inactive badge
- [ ] Buttons: Complete Service, New Household, Open Profile
- [ ] Name search: partial matching, case-insensitive
- [ ] Complete Service: pantry day vs appointment classification
- [ ] Eligibility check: monthly rule + override modal (reason required)
- [ ] Household creation: PrimaryName + composition (required), address fields, optional email
- [ ] Verify: Search → create household → check in → see last-served update in <30 seconds; override prompt triggers

## Phase 4 — Household Profile + Service History + Appointments

- [ ] Household Profile form: all fields + Active toggle
- [ ] Service History view: list of events with filters
- [ ] Appointment scheduler: ScheduledDate + ScheduledText (required), optional notes
- [ ] Actions: mark Scheduled appointment as Completed / Cancelled / NoShow
- [ ] Verify: Schedule appointment → mark completed; NoShow/Cancelled doesn't affect eligibility

## Phase 5 — Pantry day calendar generator + editor

- [ ] Admin-only "Pantry Days" screen
- [ ] "Generate for Year" button with rule logic (Jan–Oct: 2nd/3rd/4th Wed; Nov–Dec: 1st/2nd/3rd Wed)
- [ ] List with edit: change date, deactivate/reactivate, notes
- [ ] Main check-in uses PantryDay match by date
- [ ] Verify: Generate 2026 pantry days; edit one day and see check-in behavior reflect it

## Phase 6 — Stats dashboard + Monthly Summary Report (PDF + Print)

- [ ] Stats screen: totals, completed services, unique households served, PantryDay vs Appointment, overrides breakdown, by city
- [ ] Monthly Summary view: month picker, Export PDF, Print
- [ ] PDF contents: totals, pantry day breakdown, household composition served, area breakdown
- [ ] Verify: Export PDF for any month with seeded data looks clean; Print opens Windows print dialog

## Phase 7 — Backup / Export / Restore

- [ ] Automatic daily backup: encrypted zip on first app run each day
- [ ] Manual backup now
- [ ] Optional "Backup to USB" (choose folder)
- [ ] One-click restore (Admin-only): select backup zip, validate, safety-copy current DB, restore, prompt restart
- [ ] Export (Admin-only): CSV (households, service_events, pantry_days), JSON structured export
- [ ] Verify: Restore from backup confirms data reverts; Exports open fine in Excel (CSV)

## Phase 8 — Seeder tool

- [ ] `PantryDeskSeeder` generates `demo_pantrydesk.db` + config metadata
- [ ] Configurable: households count, monthsBack, city weights, age weights, household size distribution, events per pantry day range, appointments per week range, RNG seed
- [ ] Outputs match constraints: No PO boxes, 360-555-xxxx phones, address city/zip in service area
- [ ] Realism guardrails: no child-only households
- [ ] Inject "demo moments": ineligible households, overrides with reasons, scheduled appointments upcoming
- [ ] Verify: Running seeder produces DB that makes eligibility warnings visible, stats non-zero, monthly PDF interesting

## Phase 9 — Demo polish + hardening

- [ ] UI polish: tab order, keyboard shortcuts (Enter to search), clear labels and error messages
- [ ] Edge-case handling: duplicates warning, inactive household warning, prevent empty names / negative counts
- [ ] "Demo mode" configuration: app points to `demo_pantrydesk.db` easily (config file or CLI arg)
- [ ] Verify: Run scripted demo in 5–7 minutes without surprises
