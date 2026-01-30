using Microsoft.Data.Sqlite;
using PantryDeskCore.Configuration;

namespace PantryDeskCore.Data;

/// <summary>
/// Handles database schema migrations and version tracking.
/// </summary>
public static class DatabaseMigrator
{
    private const int CurrentSchemaVersion = 1;

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
                    // Future migrations would go here
                    // For Phase 1, we only have version 1, so this is a placeholder
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
        ExecuteCommand(connection, transaction, Sql.CreateServiceEventsTable);
        ExecuteCommand(connection, transaction, Sql.CreatePantryDaysTable);
        ExecuteCommand(connection, transaction, Sql.CreateAuthRolesTable);
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
