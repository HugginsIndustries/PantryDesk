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
            visit_type TEXT,
            created_at TEXT NOT NULL
        )";

    public const string ServiceEventAlterAddVisitType = @"
        ALTER TABLE service_events ADD COLUMN visit_type TEXT";

    public const string ServiceEventBackfillVisitType = @"
        UPDATE service_events SET visit_type = 'Shop with TEFAP'
        WHERE event_status = 'Completed' AND (visit_type IS NULL OR visit_type = '')";

    public const string HouseholdMembersMigrateVeteranStatus = @"
        UPDATE household_members SET veteran_status = 'Not Specified'
        WHERE veteran_status IN ('Unknown', 'Prefer Not To Answer')";

    public const string HouseholdMembersMigrateDisabledStatus = @"
        UPDATE household_members SET disabled_status = 'Not Specified'
        WHERE disabled_status IN ('Unknown', 'Prefer Not To Answer')";

    public const string HouseholdMembersMigrateVeteranStatusToThreeOptions = @"
        UPDATE household_members SET veteran_status = 'Not Veteran'
        WHERE veteran_status IN ('None', 'Active Duty', 'Reserve')";

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

    public const string CreateDeckStatsMonthlyTable = @"
        CREATE TABLE IF NOT EXISTS deck_stats_monthly (
            year INTEGER NOT NULL,
            month INTEGER NOT NULL,
            household_total_avg REAL NOT NULL,
            infant_avg REAL NOT NULL,
            child_avg REAL NOT NULL,
            adult_avg REAL NOT NULL,
            senior_avg REAL NOT NULL,
            page_count INTEGER,
            updated_at TEXT NOT NULL,
            PRIMARY KEY (year, month)
        )";

    public const string CreateHouseholdMembersTable = @"
        CREATE TABLE IF NOT EXISTS household_members (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            household_id INTEGER NOT NULL REFERENCES households(id),
            first_name TEXT NOT NULL,
            last_name TEXT NOT NULL,
            birthday TEXT NOT NULL,
            is_primary INTEGER NOT NULL DEFAULT 0,
            race TEXT,
            veteran_status TEXT,
            disabled_status TEXT
        )";

    public const string CreateHouseholdMembersIndexHouseholdId = @"
        CREATE INDEX IF NOT EXISTS idx_household_members_household_id ON household_members(household_id)";

    public const string CreateHouseholdMembersIndexName = @"
        CREATE INDEX IF NOT EXISTS idx_household_members_name ON household_members(first_name, last_name)";

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

    public const string HouseholdBulkSyncIsActive = @"
        UPDATE households
        SET is_active = CASE
            WHEN COALESCE(
                (SELECT MAX(event_date) FROM service_events
                 WHERE household_id = households.id AND event_status = 'Completed'),
                '0001-01-01') >= @reset_date
            THEN 1
            ELSE 0
        END,
        updated_at = @updated_at";

    public const string HouseholdUpdateIsActive = @"
        UPDATE households
        SET is_active = @is_active, updated_at = @updated_at
        WHERE id = @id";

    public const string HouseholdSearchByName = @"
        SELECT id, primary_name, address1, city, state, zip, phone, email,
               children_count, adults_count, seniors_count, notes,
               is_active, created_at, updated_at
        FROM households
        WHERE primary_name LIKE @search_term COLLATE NOCASE
        ORDER BY primary_name";

    public const string HouseholdSearchByMemberName = @"
        SELECT DISTINCT h.id, h.primary_name, h.address1, h.city, h.state, h.zip, h.phone, h.email,
               h.children_count, h.adults_count, h.seniors_count, h.notes,
               h.is_active, h.created_at, h.updated_at
        FROM households h
        INNER JOIN household_members m ON m.household_id = h.id
        WHERE (m.first_name || ' ' || m.last_name) LIKE @search_term COLLATE NOCASE
           OR m.first_name LIKE @search_term COLLATE NOCASE
           OR m.last_name LIKE @search_term COLLATE NOCASE
        ORDER BY h.primary_name";

    public const string HouseholdFindPotentialDuplicates = @"
        SELECT id, primary_name, address1, city, state, zip, phone, email,
               children_count, adults_count, seniors_count, notes,
               is_active, created_at, updated_at
        FROM households
        WHERE primary_name LIKE @primary_name COLLATE NOCASE
          AND (@city IS NULL OR city = @city)
          AND (@phone IS NULL OR phone = @phone)
        ORDER BY primary_name
        LIMIT 10";

    // HouseholdMember queries
    public const string HouseholdMemberInsert = @"
        INSERT INTO household_members (
            household_id, first_name, last_name, birthday, is_primary,
            race, veteran_status, disabled_status
        )
        VALUES (
            @household_id, @first_name, @last_name, @birthday, @is_primary,
            @race, @veteran_status, @disabled_status
        )";

    public const string HouseholdMemberUpdate = @"
        UPDATE household_members
        SET first_name = @first_name,
            last_name = @last_name,
            birthday = @birthday,
            is_primary = @is_primary,
            race = @race,
            veteran_status = @veteran_status,
            disabled_status = @disabled_status
        WHERE id = @id";

    public const string HouseholdMemberDelete = @"
        DELETE FROM household_members WHERE id = @id";

    public const string HouseholdMemberDeleteByHouseholdId = @"
        DELETE FROM household_members WHERE household_id = @household_id";

    public const string HouseholdMemberSelectById = @"
        SELECT id, household_id, first_name, last_name, birthday, is_primary,
               race, veteran_status, disabled_status
        FROM household_members
        WHERE id = @id";

    public const string HouseholdMemberSelectByHouseholdId = @"
        SELECT id, household_id, first_name, last_name, birthday, is_primary,
               race, veteran_status, disabled_status
        FROM household_members
        WHERE household_id = @household_id
        ORDER BY is_primary DESC, last_name, first_name";

    public const string HouseholdMemberSelectPrimaryByHouseholdId = @"
        SELECT id, household_id, first_name, last_name, birthday, is_primary,
               race, veteran_status, disabled_status
        FROM household_members
        WHERE household_id = @household_id AND is_primary = 1
        LIMIT 1";

    public const string HouseholdMemberSelectAll = @"
        SELECT id, household_id, first_name, last_name, birthday, is_primary,
               race, veteran_status, disabled_status
        FROM household_members
        ORDER BY household_id, is_primary DESC, last_name, first_name";

    // ServiceEvent queries
    public const string ServiceEventInsert = @"
        INSERT INTO service_events (
            household_id, event_type, event_status, event_date,
            scheduled_text, override_reason, notes, visit_type, created_at
        )
        VALUES (
            @household_id, @event_type, @event_status, @event_date,
            @scheduled_text, @override_reason, @notes, @visit_type, @created_at
        )";

    public const string ServiceEventSelectById = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, visit_type, created_at
        FROM service_events
        WHERE id = @id";

    public const string ServiceEventSelectByHouseholdId = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, visit_type, created_at
        FROM service_events
        WHERE household_id = @household_id
        ORDER BY event_date DESC, created_at DESC";

    public const string ServiceEventSelectByDateRange = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, visit_type, created_at
        FROM service_events
        WHERE event_date >= @start_date AND event_date <= @end_date
        ORDER BY event_date DESC, created_at DESC";

    public const string ServiceEventSelectAll = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, visit_type, created_at
        FROM service_events
        ORDER BY event_date DESC, created_at DESC";

    public const string ServiceEventSelectLastCompletedByHouseholdId = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, visit_type, created_at
        FROM service_events
        WHERE household_id = @household_id AND event_status = 'Completed'
        ORDER BY event_date DESC, created_at DESC
        LIMIT 1";

    public const string ServiceEventSelectCompletedByHouseholdAndDateRange = @"
        SELECT id, household_id, event_type, event_status, event_date,
               scheduled_text, override_reason, notes, visit_type, created_at
        FROM service_events
        WHERE household_id = @household_id 
          AND event_status = 'Completed'
          AND event_date >= @start_date 
          AND event_date <= @end_date
        LIMIT 1";

    public const string ServiceEventSelectCompletedQualifyingByHouseholdAndDateRange = @"
        SELECT 1
        FROM service_events
        WHERE household_id = @household_id 
          AND event_status = 'Completed'
          AND event_date >= @start_date 
          AND event_date <= @end_date
          AND (visit_type IN ('Shop with TEFAP', 'Shop') OR visit_type IS NULL)
        LIMIT 1";

    public const string ServiceEventUpdate = @"
        UPDATE service_events
        SET event_type = @event_type,
            event_status = @event_status,
            event_date = @event_date,
            scheduled_text = @scheduled_text,
            override_reason = @override_reason,
            notes = @notes,
            visit_type = @visit_type
        WHERE id = @id";

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

    // DeckStatsMonthly queries
    public const string DeckStatsMonthlyUpsert = @"
        INSERT INTO deck_stats_monthly (year, month, household_total_avg, infant_avg, child_avg, adult_avg, senior_avg, page_count, updated_at)
        VALUES (@year, @month, @household_total_avg, @infant_avg, @child_avg, @adult_avg, @senior_avg, @page_count, @updated_at)
        ON CONFLICT(year, month) DO UPDATE SET
            household_total_avg = @household_total_avg,
            infant_avg = @infant_avg,
            child_avg = @child_avg,
            adult_avg = @adult_avg,
            senior_avg = @senior_avg,
            page_count = @page_count,
            updated_at = @updated_at";

    public const string DeckStatsMonthlySelectByYearMonth = @"
        SELECT year, month, household_total_avg, infant_avg, child_avg, adult_avg, senior_avg, page_count, updated_at
        FROM deck_stats_monthly
        WHERE year = @year AND month = @month";

    public const string DeckStatsMonthlyExists = @"
        SELECT 1 FROM deck_stats_monthly WHERE year = @year AND month = @month LIMIT 1";

    public const string DeckStatsMonthlySelectAll = @"
        SELECT year, month, household_total_avg, infant_avg, child_avg, adult_avg, senior_avg, page_count, updated_at
        FROM deck_stats_monthly
        ORDER BY year DESC, month DESC";

    // Statistics queries
    public const string StatisticsCountActiveHouseholds = @"
        SELECT COUNT(*) FROM households WHERE is_active = 1";

    public const string StatisticsSumTotalPeople = @"
        SELECT COALESCE(
            (SELECT COUNT(*) FROM household_members m
             INNER JOIN households h ON m.household_id = h.id
             WHERE h.is_active = 1), 0)";

    public const string StatisticsCountCompletedServicesInRange = @"
        SELECT COUNT(*)
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @start_date AND event_date <= @end_date";

    public const string StatisticsCountUniqueHouseholdsServedInRange = @"
        SELECT COUNT(DISTINCT household_id)
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @start_date AND event_date <= @end_date";

    public const string StatisticsCountCompletedServicesByTypeInRange = @"
        SELECT event_type, COUNT(*) as count
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @start_date AND event_date <= @end_date
        GROUP BY event_type";

    public const string StatisticsCountOverridesInRange = @"
        SELECT COUNT(*)
        FROM service_events
        WHERE event_status = 'Completed'
        AND override_reason IS NOT NULL
        AND event_date >= @start_date AND event_date <= @end_date";

    public const string StatisticsBreakdownByCityInRange = @"
        SELECT 
            COALESCE(h.city, 'Other') as city,
            COUNT(DISTINCT se.household_id) as households_served,
            COUNT(*) as services_completed
        FROM service_events se
        INNER JOIN households h ON se.household_id = h.id
        WHERE se.event_status = 'Completed'
        AND se.event_date >= @start_date AND se.event_date <= @end_date
        GROUP BY COALESCE(h.city, 'Other')
        ORDER BY city";

    public const string StatisticsBreakdownByOverrideReasonInRange = @"
        SELECT 
            override_reason as reason,
            COUNT(*) as count
        FROM service_events
        WHERE event_status = 'Completed'
        AND override_reason IS NOT NULL
        AND event_date >= @start_date AND event_date <= @end_date
        GROUP BY override_reason
        ORDER BY count DESC";

    public const string StatisticsPantryDayBreakdownInRange = @"
        SELECT 
            se.event_date as pantry_date,
            COUNT(*) as completed_services
        FROM service_events se
        WHERE se.event_status = 'Completed'
        AND se.event_type = 'PantryDay'
        AND se.event_date >= @start_date AND se.event_date <= @end_date
        GROUP BY se.event_date
        ORDER BY se.event_date";

    public const string StatisticsSelectServedHouseholdIdsInRange = @"
        SELECT DISTINCT household_id
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @start_date AND event_date <= @end_date";

    /// <summary>
    /// First completed (any type) service date in reporting year per household. Used for unduplicated/duplicated split in reports.
    /// </summary>
    public const string StatisticsFirstCompletedDateInReportingYearPerHousehold = @"
        SELECT household_id, MIN(event_date) as first_date
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @reporting_year_start AND event_date <= @reporting_year_end
        GROUP BY household_id";

    public const string StatisticsMonthlyVisitsTrend = @"
        SELECT 
            strftime('%Y-%m', event_date) as month,
            COUNT(*) as count
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @start_date AND event_date <= @end_date
        GROUP BY strftime('%Y-%m', event_date)
        ORDER BY month";

    /// <summary>
    /// Total individuals (member count) in households with at least one completed service in range.
    /// Each member counted once; households with multiple services counted once per member.
    /// </summary>
    public const string StatisticsTotalPeopleServedInRange = @"
        SELECT COALESCE(SUM(member_count), 0) FROM (
            SELECT COUNT(m.id) as member_count
            FROM household_members m
            INNER JOIN (
                SELECT DISTINCT household_id
                FROM service_events
                WHERE event_status = 'Completed'
                AND event_date >= @start_date AND event_date <= @end_date
            ) served ON served.household_id = m.household_id
            GROUP BY m.household_id
        ) counts";

    public const string StatisticsBreakdownByVisitTypeInRange = @"
        SELECT 
            COALESCE(NULLIF(TRIM(visit_type), ''), 'Not Specified') as label,
            COUNT(*) as count
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @start_date AND event_date <= @end_date
        GROUP BY COALESCE(NULLIF(TRIM(visit_type), ''), 'Not Specified')
        ORDER BY count DESC";

    public const string StatisticsDemographicsByRaceInRange = @"
        SELECT 
            COALESCE(NULLIF(TRIM(m.race), ''), 'Not Specified') as label,
            COUNT(*) as count
        FROM household_members m
        INNER JOIN (
            SELECT DISTINCT household_id
            FROM service_events
            WHERE event_status = 'Completed'
            AND event_date >= @start_date AND event_date <= @end_date
        ) served ON served.household_id = m.household_id
        GROUP BY COALESCE(NULLIF(TRIM(m.race), ''), 'Not Specified')
        ORDER BY count DESC";

    public const string StatisticsDemographicsByVeteranStatusInRange = @"
        SELECT 
            COALESCE(NULLIF(TRIM(m.veteran_status), ''), 'Not Specified') as label,
            COUNT(*) as count
        FROM household_members m
        INNER JOIN (
            SELECT DISTINCT household_id
            FROM service_events
            WHERE event_status = 'Completed'
            AND event_date >= @start_date AND event_date <= @end_date
        ) served ON served.household_id = m.household_id
        GROUP BY COALESCE(NULLIF(TRIM(m.veteran_status), ''), 'Not Specified')
        ORDER BY count DESC";

    public const string StatisticsDemographicsByDisabledStatusInRange = @"
        SELECT 
            COALESCE(NULLIF(TRIM(m.disabled_status), ''), 'Not Specified') as label,
            COUNT(*) as count
        FROM household_members m
        INNER JOIN (
            SELECT DISTINCT household_id
            FROM service_events
            WHERE event_status = 'Completed'
            AND event_date >= @start_date AND event_date <= @end_date
        ) served ON served.household_id = m.household_id
        GROUP BY COALESCE(NULLIF(TRIM(m.disabled_status), ''), 'Not Specified')
        ORDER BY count DESC";

    /// <summary>
    /// Veteran status for served households with derived "Disabled Veteran" (Veteran + Disabled counted only there).
    /// </summary>
    public const string StatisticsDemographicsByVeteranStatusWithDisabledVeteranInRange = @"
        SELECT
            CASE
                WHEN TRIM(COALESCE(m.veteran_status, '')) = 'Veteran' AND TRIM(COALESCE(m.disabled_status, '')) = 'Disabled' THEN 'Disabled Veteran'
                WHEN TRIM(COALESCE(m.veteran_status, '')) = 'Veteran' THEN 'Veteran'
                ELSE COALESCE(NULLIF(TRIM(m.veteran_status), ''), 'Not Specified')
            END as label,
            COUNT(*) as count
        FROM household_members m
        INNER JOIN (
            SELECT DISTINCT household_id
            FROM service_events
            WHERE event_status = 'Completed'
            AND event_date >= @start_date AND event_date <= @end_date
        ) served ON served.household_id = m.household_id
        GROUP BY
            CASE
                WHEN TRIM(COALESCE(m.veteran_status, '')) = 'Veteran' AND TRIM(COALESCE(m.disabled_status, '')) = 'Disabled' THEN 'Disabled Veteran'
                WHEN TRIM(COALESCE(m.veteran_status, '')) = 'Veteran' THEN 'Veteran'
                ELSE COALESCE(NULLIF(TRIM(m.veteran_status), ''), 'Not Specified')
            END
        ORDER BY count DESC";

    // Monthly Activity Report: qualifying visit types = Shop with TEFAP, TEFAP Only, Shop, Deck Only
    public const string ActivityReportCountUniqueHouseholdsInMonth = @"
        SELECT COUNT(DISTINCT household_id)
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @start_date AND event_date <= @end_date
        AND (visit_type IN ('Shop with TEFAP', 'TEFAP Only', 'Shop', 'Deck Only') OR visit_type IS NULL OR TRIM(COALESCE(visit_type, '')) = '')";

    public const string ActivityReportSelectHouseholdIdsInMonth = @"
        SELECT DISTINCT household_id
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @start_date AND event_date <= @end_date
        AND (visit_type IN ('Shop with TEFAP', 'TEFAP Only', 'Shop', 'Deck Only') OR visit_type IS NULL OR TRIM(COALESCE(visit_type, '')) = '')
        ORDER BY household_id";

    public const string ActivityReportTotalPeopleInMonth = @"
        SELECT COALESCE(SUM(member_count), 0) FROM (
            SELECT COUNT(m.id) as member_count
            FROM household_members m
            INNER JOIN (
                SELECT DISTINCT household_id
                FROM service_events
                WHERE event_status = 'Completed'
                AND event_date >= @start_date AND event_date <= @end_date
                AND (visit_type IN ('Shop with TEFAP', 'TEFAP Only', 'Shop', 'Deck Only') OR visit_type IS NULL OR TRIM(COALESCE(visit_type, '')) = '')
            ) served ON served.household_id = m.household_id
            GROUP BY m.household_id
        ) counts";

    public const string ActivityReportFirstQualifyingDateInReportingYear = @"
        SELECT household_id, MIN(event_date) as first_date
        FROM service_events
        WHERE event_status = 'Completed'
        AND event_date >= @reporting_year_start AND event_date <= @reporting_year_end
        AND (visit_type IN ('Shop with TEFAP', 'TEFAP Only', 'Shop', 'Deck Only') OR visit_type IS NULL OR TRIM(COALESCE(visit_type, '')) = '')
        GROUP BY household_id";

    public const string ActivityReportCountPantryDaysInMonth = @"
        SELECT COUNT(*)
        FROM pantry_days
        WHERE pantry_date >= @start_date AND pantry_date <= @end_date
        AND is_active = 1";
}
