using Microsoft.Data.Sqlite;
using System.Reflection;

namespace PantryDeskCore.Data;

/// <summary>
/// Repository for managing application configuration values.
/// </summary>
public static class ConfigRepository
{
    /// <summary>
    /// Gets a configuration value by key.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="key">The configuration key.</param>
    /// <returns>The configuration value, or null if not found.</returns>
    public static string? GetValue(SqliteConnection connection, string key)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.ConfigGetValue, connection);
            cmd.Parameters.AddWithValue("@key", key);
            var result = cmd.ExecuteScalar();
            return result?.ToString();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Sets a configuration value by key.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="key">The configuration key.</param>
    /// <param name="value">The configuration value.</param>
    public static void SetValue(SqliteConnection connection, string key, string value)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.ConfigSetValue, connection);
            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@value", value);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets the current schema version.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <returns>The schema version, or 0 if not set.</returns>
    public static int GetSchemaVersion(SqliteConnection connection)
    {
        var value = GetValue(connection, "schema_version");
        if (value != null && int.TryParse(value, out var version))
        {
            return version;
        }
        return 0;
    }

    /// <summary>
    /// Sets the schema version.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="version">The schema version.</param>
    public static void SetSchemaVersion(SqliteConnection connection, int version)
    {
        SetValue(connection, "schema_version", version.ToString());
    }

    /// <summary>
    /// Gets the application version.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <returns>The application version, or null if not set.</returns>
    public static string? GetAppVersion(SqliteConnection connection)
    {
        return GetValue(connection, "app_version");
    }

    /// <summary>
    /// Sets the application version.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="version">The application version.</param>
    public static void SetAppVersion(SqliteConnection connection, string version)
    {
        SetValue(connection, "app_version", version);
    }

    /// <summary>
    /// Initializes the app version from the assembly version if not already set.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    public static void InitializeAppVersion(SqliteConnection connection)
    {
        var currentVersion = GetAppVersion(connection);
        if (currentVersion == null)
        {
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var versionString = assemblyVersion?.ToString() ?? "1.0.0";
            SetAppVersion(connection, versionString);
        }
    }
}
