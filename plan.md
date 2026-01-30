# Phased implementation plan (Cursor-friendly, demo-first)

This plan assumes: **C# .NET WinForms + SQLite + separate Seeder console app**, demo target **2026-02-06**. Email stays **optional** (and can be hidden by default in the UI if you want later).

---

# Phase 0 — Repo setup and guardrails (½ day)

## Deliverables

* Git repo with two projects:

  * `PantryDeskApp` (WinForms)
  * `PantryDeskSeeder` (Console)
* Shared library (optional but recommended): `PantryDeskCore` for models/db/helpers used by both app and seeder.

## Suggested structure

```text
/src
  /PantryDeskCore
    Models/
    Data/
    Services/
    Reports/
    Security/
  /PantryDeskApp
    Forms/
    Controls/
    Assets/
  /PantryDeskSeeder
/tests (optional)
README.md
```

## Cursor goals

* Get the solution building and running from a clean clone.
* Add `.editorconfig` and consistent formatting.
* Decide data folder location: `C:\ProgramData\PantryDeskApp\` (configurable).

### Done when

* `PantryDeskApp` opens a blank window.
* `PantryDeskSeeder` runs and prints "Hello seed".

---

# Phase 1 — Database schema + data access layer (Day 1)

## Deliverables

* SQLite schema creation + migrations (simple “schema version” is enough for MVP).
* Data access layer that can:

  * Create/open DB
  * CRUD households
  * Insert/query service events
  * Pantry day CRUD
  * AuthRoles read/write (hashed passwords)
* Central config table: schema version, app version, etc.

## Design choices (MVP-simple)

* Use parameterized queries (avoid ORM if it slows you down).
* Put all SQL in one place (e.g., `PantryDeskCore/Data/Sql.cs`).

### Done when

* App can create a database file and insert/read:

  * 1 household
  * 1 pantry day
  * 1 completed service event

---

# Phase 2 — Authentication (Entry/Admin) (½ day)

## Deliverables

* Login form:

  * select role (Entry/Admin)
  * password
* Role passwords stored salted+hashed.
* Admin screen: “Change role passwords” (minimal UI ok).

## Rules

* Two shared accounts only.
* App should lock out exports/restore/calendar edits for Entry role.

### Done when

* Can log in as Entry/Admin.
* Attempting Admin-only actions as Entry is blocked.

---

# Phase 3 — Core workflow: Search + Check-in + Create Household (Day 2)

This is the "demo killer feature".

## Deliverables

* Main Check-In screen:

  * Big search box (PrimaryName)
  * Results list with:

    * PrimaryName
    * City/Zip
    * HouseholdSize + breakdown (C/A/S)
    * Last completed service date + type
    * Eligibility badge (Eligible / Already served this month)
    * Inactive badge
  * Buttons:

    * Complete Service
    * New Household
    * Open Profile

## Key behaviors

* **Name search**: partial matching, case-insensitive.
* **Complete Service**:

  * If **today is pantry day** → create completed `PantryDay` event
  * Else → create completed `Appointment` event (per your rule)
* **Eligibility**:

  * If already completed this month → allow Entry to proceed but require override modal:

    * dropdown reason (required)
    * optional notes

## Household creation

* “New Household” from main screen:

  * Required: PrimaryName + composition (Children/Adults/Seniors)
  * Address fields recommended (so “service area” is meaningful)
  * Email field **optional** (can be blank; don’t nag)

### Done when

* In <30 seconds, you can:

  * search
  * create household
  * check them in
  * see last-served update immediately
* Override prompt triggers correctly.

---

# Phase 4 — Household Profile + Service History + Appointments (Day 3)

## Deliverables

* Household Profile form:

  * PrimaryName (required)
  * Address1, City, State, Zip
  * Phone
  * Email **optional**
  * Children/Adults/Seniors (auto updates total)
  * Notes
  * Active toggle
* Service History view:

  * list of events (type/status/date)
  * filter by status/type
* Appointment scheduler:

  * ScheduledDate (required)
  * ScheduledText (required; freeform window)
  * Notes optional
  * Creates `ServiceEvents` row: `Appointment + Scheduled`

## Actions

* From history: mark Scheduled appointment as Completed / Cancelled / NoShow.
* Completion should apply the monthly eligibility rule + overrides the same way.

### Done when

* You can schedule an appointment and later mark it completed.
* NoShow/Cancelled does not affect eligibility.

---

# Phase 5 — Pantry day calendar generator + editor (½–1 day)

## Deliverables

* Admin-only “Pantry Days” screen:

  * “Generate for Year” button
  * rule logic:

    * Jan–Oct: 2nd/3rd/4th Wednesday
    * Nov–Dec: 1st/2nd/3rd Wednesday
  * list with edit:

    * change date
    * deactivate/reactivate
    * notes

## App behavior

* Main check-in uses PantryDay match by date.
* If pantry day is deactivated, treat the day as non-pantry (→ appointment classification) unless admin decides otherwise.

### Done when

* Generate 2026 pantry days.
* Edit one day and see check-in behavior reflect it.

---

# Phase 6 — Stats dashboard + Monthly Summary Report (PDF + Print) (Day 4)

## Deliverables

* Stats screen (fast queries):

  * Total active households
  * Total people (sum of sizes of active households)
  * Completed services this month
  * Unique households served this month
  * PantryDay vs Appointment completions this month
  * Overrides count + breakdown by reason
  * By city (Winlock/Vader/Ryderwood)
* Monthly Summary view:

  * month picker
  * “Export PDF”
  * “Print”
* PDF contents:

  * totals
  * pantry day breakdown table
  * household composition served (Children/Adults/Seniors totals)
  * area breakdown

## Recommendation

* For “composition served”, choose:

  * unique households served in the month (simpler)
    Label it clearly: “Totals across unique households served this month”.

### Done when

* You can export a PDF for any month with seeded data and it looks clean.
* Print opens Windows print dialog.

---

# Phase 7 — Backup / Export / Restore (Day 5)

## Deliverables

* Automatic daily backup:

  * on first app run each day, create encrypted zip
* Manual backup now
* Optional “Backup to USB” (choose folder)
* One-click restore (Admin-only):

  * select backup zip
  * validate
  * safety-copy current DB
  * restore DB
  * prompt restart
* Export (Admin-only):

  * CSV: households, service_events, pantry_days
  * JSON: structured export

### Done when

* You can restore from backup and confirm data reverts.
* Exports open fine in Excel (CSV).

---

# Phase 8 — Seeder tool (parallel, but finish by Day 4–5)

You can implement this in parallel once schema is stable.

## Deliverables

* `PantryDeskSeeder` generates `demo_pantrydesk.db` + config metadata.
* Configurable:

  * households count
  * monthsBack
  * city weights (Winlock/Vader/Ryderwood)
  * age weights (Child/Adult/Senior)
  * household size distribution
  * events per pantry day range (25–50)
  * appointments per week range
  * RNG seed
* Outputs match constraints:

  * **No PO boxes**
  * 360-555-xxxx phones
  * Address city/zip in service area
* Realism guardrails for household composition:

  * no child-only households
  * (optional) big-household child/adult boost (configurable)
* Inject “demo moments”:

  * some ineligible households this month
  * some overrides with reasons
  * scheduled appointments upcoming

### Done when

* Running seeder produces DB that instantly makes:

  * eligibility warnings visible
  * stats non-zero
  * monthly PDF interesting

---

# Phase 9 — Demo polish + hardening (final day before demo)

## Deliverables

* UI polish (not fancy, just humane):

  * tab order
  * keyboard shortcuts (Enter to search, etc.)
  * clear labels and error messages
* Edge-case handling:

  * duplicates warning (optional: basic “possible match” notice)
  * inactive household warning
  * prevent empty names / negative counts
* “Demo mode” configuration:

  * app points to `demo_pantrydesk.db` easily (config file or CLI arg)

### Done when

* You can run a scripted demo in 5–7 minutes without surprises.

---

## Cursor workflow tips (practical)

* Work in **small vertical slices**: DB → UI → behavior → commit.
* After each phase, run a “smoke demo” using seeded data.
* Keep SQL queries deterministic and test them with a few known seeded households.

---

## Quick demo script (for Feb 6)

1. Login Entry
2. Search "Smith" → show list + last served
3. Complete service on pantry day → show success
4. Complete again → override modal required → pick "Special Circumstance"
5. Open Stats → show month totals + overrides
6. Export Monthly Summary PDF
7. Admin login → show pantry day calendar generator + backup/restore
