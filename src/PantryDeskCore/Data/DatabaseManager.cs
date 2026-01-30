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
        var dataRoot = AppConfig.GetDataRoot();
        var dbPath = Path.Combine(dataRoot, "pantrydesk.db");
        return $"Data Source={dbPath}";
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
    /// <returns>A new database connection.</returns>
    public static SqliteConnection GetConnection()
    {
        var connectionString = GetConnectionString();
        return new SqliteConnection(connectionString);
    }
}
