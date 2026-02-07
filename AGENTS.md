# AGENTS.md — Working rules for AI agents and contributors

This repo builds an **offline-first** Windows desktop app for food bank client intake/tracking plus a **separate seeder** tool for demo data. Keep it boring, safe, and maintainable.

---

## Core principles (non-negotiable)

1. **Offline-first:** No internet required at runtime. No web calls, no SaaS dependencies.
2. **Data safety:** Treat all client data as sensitive PII. Never log or print real PII in debug output.
3. **Simplicity wins:** Prefer fewer screens, fewer clicks, and obvious workflows.
4. **Minimal diffs:** Small, targeted changes. Avoid refactors unless explicitly requested.
5. **Deterministic behavior:** Database queries, reports, and seed generation must be reproducible.

---

## Repo overview

- `PantryDeskApp` — WinForms desktop application
- `PantryDeskSeeder` — Console app to generate `demo_pantrydesk.db` (or equivalent)
- `PantryDeskCore` — Shared models/data/services used by both app and seeder (preferred)

> Note: If project names/folders differ from the above, update this file rather than “guessing” paths.

---

## Build & verification requirements

Before marking work "done", agents must:

- Run:
  - `dotnet build`
  - `dotnet test` (if tests exist)
- If you add/modify any report query or DB schema, verify using a seeded demo DB.

If you can’t run commands (environment limitation), state clearly what you didn’t verify and why.

---

## Database rules

- SQLite is the source of truth (single `.db` file).
- **All SQL must be parameterized** (no string concatenation).
- Schema changes must:
  - bump a `schema_version`
  - include a migration path (even if simple)
  - update the seeder to match
  - update exports (CSV/JSON) if impacted
- Avoid hard deletes for core records; prefer `IsActive`/soft delete patterns.

---

## Authentication & permissions

- Only two shared role logins: **Entry** and **Admin**.
- Store role passwords as **salted hashes** (never plaintext).
- Enforce Admin-only features:
  - backup/restore
  - exports
  - pantry day calendar edits
  - role password changes
- Entry users can still record overrides, but overrides must require a reason.

---

## Demo data / Seeder requirements

Seeder must generate **fake data only**:

- Service area addresses limited to **Winlock, Vader, Ryderwood (WA)**.
- **No PO Boxes** (physical addresses only).
- Phone numbers must use **360-555-XXXX**.
- Household composition must support **Children / Adults / Seniors** with:
  - configurable weights
  - configurable household-size distribution
  - realism guardrails (e.g., no child-only households)
- Seeder should support a fixed RNG seed for repeatable results.

Never commit real addresses, names, or phone numbers from actual clients.

---

## Reporting / PDF

- Monthly summary must support:
  - Export to PDF
  - Print
- Prefer libraries with **permissive licensing**. If introducing a new library:
  - confirm license is acceptable
  - document it in `README.md` or `THIRD_PARTY_NOTICES.md` (if present)

---

## UI/UX constraints

- Optimize for **search by name** + quick check-in.
- Ensure keyboard-friendly workflows:
  - sensible tab order
  - Enter triggers primary action where reasonable
- Error messages must be plain-language and actionable.

---

## Logging & diagnostics

- Do not log PII (names, addresses, phone numbers).
- Logs should focus on technical info (errors, schema version, file paths, operation outcomes).
- If a “Diagnostics” screen exists, it must not expose sensitive data.

---

## Coding conventions

- Keep code readable and straightforward.
- Prefer:
  - clear naming over clever abstractions
  - single-responsibility helpers in `PantryDeskCore`
  - explicit types when it improves clarity
- No large framework changes without explicit instruction.

---

## Documentation & markdown

When editing `.md` files (AGENTS.md, TODO.md, README.md, etc.):

- Follow markdownlint rules (e.g., no spaces inside code spans, no duplicate headings).
- Use proper hierarchy (`#`, `##`, `###`, `####`).
- Prefer clear structure over brevity.

---

## Change discipline (how agents should work)

When implementing a feature:

1. Briefly state what you're changing and where.
2. Implement with minimal diff.
3. Run build/tests (or state what you could not run).
4. Ensure seeder + stats/reporting still work if relevant.
5. Update docs if behavior changes.

Avoid “drive-by” formatting or refactoring across unrelated files.

---

## TODO.md maintenance

When marking items complete or updating the TODO list:

- **Move completed items:** When an item is done (checked `[x]`), move it into the **Completed** section at the top. Remove it from the **Open** section.
- **Retain structure and details:** Keep the full formatting when moving to Completed:
  - Section headers: `####` for item titles, `###` for group titles.
  - Full details: Impact, Complexity, Acceptance Criteria, Likely files, Rationale.
- **Avoid duplicate headings:** Append a space and `(Complete)` to section titles in Completed when the same title exists in Open (e.g., `### Client Requirements (Complete)`). Satisfies markdownlint MD024.
- **Organization:** Use `#`, `##`, `###`, `####` for clear hierarchy. Match the structure of Open items so Completed mirrors the same organization.

---

## Security sanity checks (quick checklist)

- [ ] No web calls / telemetry added
- [ ] No real PII committed
- [ ] Passwords hashed + salted
- [ ] SQL parameterized
- [ ] Backups/encrypted zips (if implemented) don't leak keys/passwords into source

---
