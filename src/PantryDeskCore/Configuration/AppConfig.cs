namespace PantryDeskCore.Configuration;

/// <summary>
/// Application configuration settings.
/// </summary>
public static class AppConfig
{
    private static string? _dataRootOverride;

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
    /// Sets a custom data root path (for testing or configuration).
    /// </summary>
    /// <param name="path">The custom data root path.</param>
    internal static void SetDataRootOverride(string? path)
    {
        _dataRootOverride = path;
    }
}
