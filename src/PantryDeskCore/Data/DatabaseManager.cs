using Microsoft.Data.Sqlite;
using PantryDeskCore.Configuration;

namespace PantryDeskCore.Data;

/// <summary>
/// Manages database connections and initialization.
/// </summary>
public static class DatabaseManager
{
    /// <summary>
    /// Gets the connection string for the database.
    /// </summary>
    /// <returns>The SQLite connection string.</returns>
    public static string GetConnectionString()
    {
        // First, check if a demo database path has been configured.
        var demoPath = AppConfig.GetDemoDatabasePath();
        if (!string.IsNullOrWhiteSpace(demoPath))
        {
            return $"Data Source={demoPath}";
        }

        var dataRoot = AppConfig.GetDataRoot();
        var dbPath = Path.Combine(dataRoot, "pantrydesk.db");
        return $"Data Source={dbPath}";
    }

    /// <summary>
    /// Gets the actual database file path (respects demo mode).
    /// </summary>
    /// <returns>The full path to the database file.</returns>
    public static string GetDatabasePath()
    {
        // First, check if a demo database path has been configured.
        var demoPath = AppConfig.GetDemoDatabasePath();
        if (!string.IsNullOrWhiteSpace(demoPath))
        {
            return demoPath;
        }

        var dataRoot = AppConfig.GetDataRoot();
        return Path.Combine(dataRoot, "pantrydesk.db");
    }

    /// <summary>
    /// Initializes the database by creating it if it doesn't exist and running migrations.
    /// </summary>
    /// <returns>A new database connection that is ready to use.</returns>
    public static SqliteConnection InitializeDatabase()
    {
        var connectionString = GetConnectionString();
        var connection = new SqliteConnection(connectionString);

        // Run migrations
        DatabaseMigrator.MigrateToLatest(connection);

        return connection;
    }

    /// <summary>
    /// Creates a new database connection without running migrations.
    /// Use this for normal operations after the database has been initialized.
    /// </summary>
    /// <returns>A new database connection with foreign keys enabled.</returns>
    public static SqliteConnection GetConnection()
    {
        var connectionString = GetConnectionString();
        var connection = new SqliteConnection(connectionString);
        // Foreign keys are enabled when connection is opened (see OpenConnectionWithForeignKeys)
        return connection;
    }

    /// <summary>
    /// Opens a connection and enables foreign key constraints.
    /// Call this after getting a connection from GetConnection() and before using it.
    /// </summary>
    /// <param name="connection">The connection to open and configure.</param>
    public static void OpenWithForeignKeys(SqliteConnection connection)
    {
        connection.Open();
        using var cmd = new SqliteCommand("PRAGMA foreign_keys=ON", connection);
        cmd.ExecuteNonQuery();
    }
}
