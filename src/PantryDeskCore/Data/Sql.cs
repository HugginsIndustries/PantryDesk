namespace PantryDeskCore.Data;

/// <summary>
/// Centralized SQL statements for database operations.
/// All queries use parameterized syntax to prevent SQL injection.
/// </summary>
public static class Sql
{
    // Schema creation
    public const string CreateConfigTable = @"
        CREATE TABLE IF NOT EXISTS config (
            key TEXT PRIMARY KEY,
            value TEXT NOT NULL
        )";

    public const string CreateHouseholdsTable = @"
        CREATE TABLE IF NOT EXISTS households (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            primary_name TEXT NOT NULL,
            address1 TEXT,
            city TEXT,
            state TEXT,
            zip TEXT,
            phone TEXT,
            email TEXT,
            children_count INTEGER NOT NULL DEFAULT 0,
            adults_count INTEGER NOT NULL DEFAULT 0,
            seniors_count INTEGER NOT NULL DEFAULT 0,
            notes TEXT,
            is_active INTEGER NOT NULL DEFAULT 1,
            created_at TEXT NOT NULL,
            updated_at TEXT NOT NULL
        )";

    public const string CreateServiceEventsTable = @"
        CREATE TABLE IF NOT EXISTS service_events (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            household_id INTEGER NOT NULL REFERENCES households(id),
            event_type TEXT NOT NULL,
            event_status TEXT NOT NULL,
            event_date TEXT NOT NULL,
            scheduled_text TEXT,
            override_reason TEXT,
            notes TEXT,
            created_at TEXT NOT NULL
        )";

    public const string CreatePantryDaysTable = @"
        CREATE TABLE IF NOT EXISTS pantry_days (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            pantry_date TEXT NOT NULL UNIQUE,
            is_active INTEGER NOT NULL DEFAULT 1,
            notes TEXT,
            created_at TEXT NOT NULL
        )";

    public const string CreateAuthRolesTable = @"
        CREATE TABLE IF NOT EXISTS auth_roles (
            role_name TEXT PRIMARY KEY,
            password_hash TEXT NOT NULL,
            salt TEXT NOT NULL,
            updated_at TEXT NOT NULL
        )";

    // Config queries
    public const string ConfigGetValue = @"
        SELECT value FROM config WHERE key = @key";

    public const string ConfigSetValue = @"
        INSERT INTO config (key, value)
        VALUES (@key, @value)
        ON CONFLICT(key) DO UPDATE SET value = @value";

    // Household queries
    public const string HouseholdInsert = @"
        INSERT INTO households (
            primary_name, address1, city, state, zip, phone, email,
            children_count, adults_count, seniors_count, notes,
            is_active, created_at, updated_at
        )
        VALUES (
            @primary_name, @address1, @city, @state, @zip, @phone, @email,
            @children_count, @adults_count, @seniors_count, @notes,
            @is_active, @created_at, @updated_at
        )";

    public const string HouseholdSelectById = @"
        SELECT id, primary_name, address1, city, state, zip, phone, email,
               children_count, adults_count, seniors_count, notes,
               is_active, created_at, updated_at
        FROM households
        WHERE id = @id";

    public const string HouseholdSelectAll = @"
        SELECT id, primary_name, address1, city, state, zip, phone, email,
               children_count, adults_count, seniors_count, notes,
               is_active, created_at, updated_at
        FROM households
        ORDER BY primary_name";

    public const string HouseholdUpdate = @"
        UPDATE households
        SET primary_name = @primary_name,
            address1 = @address1,
            city = @city,
            state = @state,
            zip = @zip,
            phone = @phone,
            email = @email,
            children_count = @children_count,
            adults_count = @adults_count,
            seniors_count = @seniors_count,
            notes = @notes,
            is_active = @is_active,
            updated_at = @updated_at
        WHERE id = @id";

    public const string HouseholdSoftDelete = @"
        UPDATE households
        SET is_active = 0, updated_at = @updated_at
        WHERE id = @id";

    // ServiceEvent queries
    public const string ServiceEventInsert = @"
        INSERT INTO service_events (
            household_id, event_type, event_status, event_date,
            scheduled_text, override_reason, notes, created_at
        )
        VALUES (
            @household_id, @event_type, @event_status, @event_date,
            @scheduled_text, @override_reason, @notes, @created_at
        )";

    public const string ServiceEventSelectById = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, created_at
        FROM service_events
        WHERE id = @id";

    public const string ServiceEventSelectByHouseholdId = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, created_at
        FROM service_events
        WHERE household_id = @household_id
        ORDER BY event_date DESC, created_at DESC";

    public const string ServiceEventSelectByDateRange = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, created_at
        FROM service_events
        WHERE event_date >= @start_date AND event_date <= @end_date
        ORDER BY event_date DESC, created_at DESC";

    // PantryDay queries
    public const string PantryDayInsert = @"
        INSERT INTO pantry_days (pantry_date, is_active, notes, created_at)
        VALUES (@pantry_date, @is_active, @notes, @created_at)";

    public const string PantryDaySelectById = @"
        SELECT id, pantry_date, is_active, notes, created_at
        FROM pantry_days
        WHERE id = @id";

    public const string PantryDaySelectByDate = @"
        SELECT id, pantry_date, is_active, notes, created_at
        FROM pantry_days
        WHERE pantry_date = @pantry_date";

    public const string PantryDaySelectAll = @"
        SELECT id, pantry_date, is_active, notes, created_at
        FROM pantry_days
        ORDER BY pantry_date";

    public const string PantryDayUpdate = @"
        UPDATE pantry_days
        SET pantry_date = @pantry_date,
            is_active = @is_active,
            notes = @notes
        WHERE id = @id";

    public const string PantryDayDelete = @"
        DELETE FROM pantry_days WHERE id = @id";

    // AuthRole queries
    public const string AuthRoleSelectByRoleName = @"
        SELECT role_name, password_hash, salt, updated_at
        FROM auth_roles
        WHERE role_name = @role_name";

    public const string AuthRoleUpdatePassword = @"
        INSERT INTO auth_roles (role_name, password_hash, salt, updated_at)
        VALUES (@role_name, @password_hash, @salt, @updated_at)
        ON CONFLICT(role_name) DO UPDATE SET
            password_hash = @password_hash,
            salt = @salt,
            updated_at = @updated_at";
}
