namespace PantryDeskCore.Configuration;

/// <summary>
/// Application configuration settings.
/// </summary>
public static class AppConfig
{
    private static string? _dataRootOverride;
    private static string? _demoDatabasePath;

    /// <summary>
    /// Gets the root directory for application data files.
    /// Defaults to C:\ProgramData\PantryDesk\ on Windows.
    /// Can be overridden via environment variable PANTRYDESK_DATA_ROOT.
    /// </summary>
    /// <returns>The data root directory path.</returns>
    public static string GetDataRoot()
    {
        if (_dataRootOverride != null)
        {
            return _dataRootOverride;
        }

        // Check for environment variable override
        var envOverride = Environment.GetEnvironmentVariable("PANTRYDESK_DATA_ROOT");
        if (!string.IsNullOrWhiteSpace(envOverride))
        {
            _dataRootOverride = envOverride;
            return _dataRootOverride;
        }

        // Default to ProgramData\PantryDesk
        var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var dataRoot = Path.Combine(programData, "PantryDesk");

        // Ensure directory exists
        if (!Directory.Exists(dataRoot))
        {
            Directory.CreateDirectory(dataRoot);
        }

        return dataRoot;
    }

    /// <summary>
    /// Gets an optional demo database path from a local config file near the application.
    /// When present and valid, this path can be used instead of the default data root DB.
    /// </summary>
    /// <returns>The demo database path, or null if not configured or not found.</returns>
    public static string? GetDemoDatabasePath()
    {
        if (_demoDatabasePath != null)
        {
            return _demoDatabasePath;
        }

        try
        {
            // Look for a simple config file next to the application binaries.
            var baseDirectory = AppContext.BaseDirectory;
            var configPath = Path.Combine(baseDirectory, "PantryDesk.demo.config");

            if (!File.Exists(configPath))
            {
                return null;
            }

            var lines = File.ReadAllLines(configPath);
            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                var parts = line.Split('=', 2);
                if (parts.Length != 2)
                {
                    continue;
                }

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                if (!string.Equals(key, "DemoDatabasePath", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                // Expand environment variables if present
                var expanded = Environment.ExpandEnvironmentVariables(value);

                if (!File.Exists(expanded))
                {
                    // If the configured file does not exist, treat as not configured
                    return null;
                }

                _demoDatabasePath = expanded;
                return _demoDatabasePath;
            }
        }
        catch
        {
            // On any error, fall back to normal behavior without surfacing to the user.
            return null;
        }

        return null;
    }

    /// <summary>
    /// Sets a custom data root path (for testing or configuration).
    /// </summary>
    /// <param name="path">The custom data root path.</param>
    internal static void SetDataRootOverride(string? path)
    {
        _dataRootOverride = path;
    }
}
