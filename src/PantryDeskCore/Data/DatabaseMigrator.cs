using Microsoft.Data.Sqlite;
using PantryDeskCore.Configuration;

namespace PantryDeskCore.Data;

/// <summary>
/// Handles database schema migrations and version tracking.
/// </summary>
public static class DatabaseMigrator
{
    private const int CurrentSchemaVersion = 5;

    /// <summary>
    /// Migrates the database to the latest schema version.
    /// Creates all tables if the database is new.
    /// </summary>
    /// <param name="connection">The database connection to use.</param>
    public static void MigrateToLatest(SqliteConnection connection)
    {
        connection.Open();

        try
        {
            // Enable foreign key constraints
            using var fkCmd = new SqliteCommand("PRAGMA foreign_keys=ON", connection);
            fkCmd.ExecuteNonQuery();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Check if config table exists
                var configTableExists = TableExists(connection, transaction, "config");

                int currentVersion = 0;
                if (configTableExists)
                {
                    // Try to get current schema version
                    using var versionCmd = new SqliteCommand(Sql.ConfigGetValue, connection, transaction);
                    versionCmd.Parameters.AddWithValue("@key", "schema_version");
                    var versionResult = versionCmd.ExecuteScalar();
                    if (versionResult != null && int.TryParse(versionResult.ToString(), out var parsedVersion))
                    {
                        currentVersion = parsedVersion;
                    }
                }

                // If database is new or version is 0, create all tables
                if (currentVersion == 0)
                {
                    CreateTables(connection, transaction);
                    SetSchemaVersion(connection, transaction, CurrentSchemaVersion);
                }
                else if (currentVersion < CurrentSchemaVersion)
                {
                    if (currentVersion < 2)
                    {
                        MigrateFromV1ToV2(connection, transaction);
                    }
                    if (currentVersion < 3)
                    {
                        MigrateFromV2ToV3(connection, transaction);
                    }
                    if (currentVersion < 4)
                    {
                        MigrateFromV3ToV4(connection, transaction);
                    }
                    if (currentVersion < 5)
                    {
                        MigrateFromV4ToV5(connection, transaction);
                    }
                    SetSchemaVersion(connection, transaction, CurrentSchemaVersion);
                }
                else if (currentVersion > CurrentSchemaVersion)
                {
                    // Database is newer than this code - log warning but don't fail
                    // In a real app, you might want to log this
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        finally
        {
            connection.Close();
        }
    }

    private static bool TableExists(SqliteConnection connection, SqliteTransaction transaction, string tableName)
    {
        using var cmd = new SqliteCommand(
            "SELECT name FROM sqlite_master WHERE type='table' AND name=@name",
            connection,
            transaction);
        cmd.Parameters.AddWithValue("@name", tableName);
        return cmd.ExecuteScalar() != null;
    }

    private static void CreateTables(SqliteConnection connection, SqliteTransaction transaction)
    {
        // Create all tables in order (respecting foreign key dependencies)
        ExecuteCommand(connection, transaction, Sql.CreateConfigTable);
        ExecuteCommand(connection, transaction, Sql.CreateHouseholdsTable);
        ExecuteCommand(connection, transaction, Sql.CreateHouseholdMembersTable);
        ExecuteCommand(connection, transaction, Sql.CreateHouseholdMembersIndexHouseholdId);
        ExecuteCommand(connection, transaction, Sql.CreateHouseholdMembersIndexName);
        ExecuteCommand(connection, transaction, Sql.CreateServiceEventsTable);
        ExecuteCommand(connection, transaction, Sql.CreatePantryDaysTable);
        ExecuteCommand(connection, transaction, Sql.CreateAuthRolesTable);
    }

    private static void MigrateFromV2ToV3(SqliteConnection connection, SqliteTransaction transaction)
    {
        ExecuteCommand(connection, transaction, Sql.ServiceEventAlterAddVisitType);
        ExecuteCommand(connection, transaction, Sql.ServiceEventBackfillVisitType);
    }

    private static void MigrateFromV3ToV4(SqliteConnection connection, SqliteTransaction transaction)
    {
        ExecuteCommand(connection, transaction, Sql.HouseholdMembersMigrateVeteranStatus);
        ExecuteCommand(connection, transaction, Sql.HouseholdMembersMigrateDisabledStatus);
    }

    private static void MigrateFromV4ToV5(SqliteConnection connection, SqliteTransaction transaction)
    {
        ExecuteCommand(connection, transaction, Sql.HouseholdMembersMigrateVeteranStatusToThreeOptions);
    }

    private static void MigrateFromV1ToV2(SqliteConnection connection, SqliteTransaction transaction)
    {
        // Create household_members table
        ExecuteCommand(connection, transaction, Sql.CreateHouseholdMembersTable);
        ExecuteCommand(connection, transaction, Sql.CreateHouseholdMembersIndexHouseholdId);
        ExecuteCommand(connection, transaction, Sql.CreateHouseholdMembersIndexName);

        // Backfill members from existing households
        using var selectCmd = new SqliteCommand(
            "SELECT id, primary_name, children_count, adults_count, seniors_count FROM households",
            connection,
            transaction);
        using var reader = selectCmd.ExecuteReader();

        var households = new List<(int Id, string PrimaryName, int Children, int Adults, int Seniors)>();
        while (reader.Read())
        {
            households.Add((
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetInt32(3),
                reader.GetInt32(4)));
        }
        reader.Close();

        foreach (var (id, primaryName, children, adults, seniors) in households)
        {
            // Parse primary name: "FirstName LastName" or single word
            var nameParts = primaryName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var primaryFirstName = nameParts.Length >= 2 ? nameParts[0] : primaryName.Trim();
            var primaryLastName = nameParts.Length >= 2 ? nameParts[1] : "Household";

            if (string.IsNullOrWhiteSpace(primaryFirstName))
            {
                primaryFirstName = "Primary";
            }

            // Primary member: adult or senior (if no adults). Never child-only.
            var primaryIsAdult = adults > 0 || (adults == 0 && seniors == 0);
            var primaryBirthday = primaryIsAdult
                ? new DateTime(1980, 6, 15).ToString("yyyy-MM-dd")
                : new DateTime(1950, 6, 15).ToString("yyyy-MM-dd");
            InsertMember(connection, transaction, id, primaryFirstName, primaryLastName, primaryBirthday, isPrimary: true);

            // Additional members from counts (primary counts as one adult or one senior)
            // Child-only households: primary replaces one child, so add (children - 1) children
            var isChildOnly = children > 0 && adults == 0 && seniors == 0;
            var childrenToAdd = isChildOnly ? children - 1 : children;
            var memberIdx = 1;
            for (var i = 0; i < childrenToAdd; i++)
            {
                var bday = new DateTime(2010, 1, 1).AddDays(memberIdx).ToString("yyyy-MM-dd");
                InsertMember(connection, transaction, id, "Child", (memberIdx).ToString(), bday, false);
                memberIdx++;
            }
            var extraAdults = primaryIsAdult ? adults - 1 : adults;
            for (var i = 0; i < extraAdults; i++)
            {
                var bday = new DateTime(1985, 1, 1).AddDays(memberIdx).ToString("yyyy-MM-dd");
                InsertMember(connection, transaction, id, "Adult", (memberIdx).ToString(), bday, false);
                memberIdx++;
            }
            var extraSeniors = primaryIsAdult ? seniors : seniors - 1;
            for (var i = 0; i < extraSeniors; i++)
            {
                var bday = new DateTime(1950, 1, 1).AddDays(memberIdx).ToString("yyyy-MM-dd");
                InsertMember(connection, transaction, id, "Senior", (memberIdx).ToString(), bday, false);
                memberIdx++;
            }
        }
    }

    private static void InsertMember(SqliteConnection connection, SqliteTransaction transaction,
        int householdId, string firstName, string lastName, string birthday, bool isPrimary)
    {
        using var cmd = new SqliteCommand(Sql.HouseholdMemberInsert, connection, transaction);
        cmd.Parameters.AddWithValue("@household_id", householdId);
        cmd.Parameters.AddWithValue("@first_name", firstName);
        cmd.Parameters.AddWithValue("@last_name", lastName);
        cmd.Parameters.AddWithValue("@birthday", birthday);
        cmd.Parameters.AddWithValue("@is_primary", isPrimary ? 1 : 0);
        cmd.Parameters.AddWithValue("@race", DBNull.Value);
        cmd.Parameters.AddWithValue("@veteran_status", DBNull.Value);
        cmd.Parameters.AddWithValue("@disabled_status", DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    private static void ExecuteCommand(SqliteConnection connection, SqliteTransaction transaction, string sql)
    {
        using var cmd = new SqliteCommand(sql, connection, transaction);
        cmd.ExecuteNonQuery();
    }

    private static void SetSchemaVersion(SqliteConnection connection, SqliteTransaction transaction, int version)
    {
        using var cmd = new SqliteCommand(Sql.ConfigSetValue, connection, transaction);
        cmd.Parameters.AddWithValue("@key", "schema_version");
        cmd.Parameters.AddWithValue("@value", version.ToString());
        cmd.ExecuteNonQuery();
    }
}
