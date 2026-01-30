namespace PantryDeskCore.Security;

/// <summary>
/// Manages the current user session state for the application.
/// Tracks the logged-in role (Entry or Admin) for permission checking.
/// </summary>
public static class SessionManager
{
    private static string? _currentRole;

    /// <summary>
    /// Gets whether a user is currently logged in.
    /// </summary>
    public static bool IsLoggedIn => _currentRole != null;

    /// <summary>
    /// Gets the current logged-in role ("Entry", "Admin", or null if not logged in).
    /// </summary>
    public static string? CurrentRole => _currentRole;

    /// <summary>
    /// Gets whether the current user is an Admin.
    /// </summary>
    public static bool IsAdmin => _currentRole == "Admin";

    /// <summary>
    /// Logs in a user with the specified role.
    /// </summary>
    /// <param name="roleName">The role name ("Entry" or "Admin").</param>
    public static void Login(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
        }

        if (roleName != "Entry" && roleName != "Admin")
        {
            throw new ArgumentException("Role name must be 'Entry' or 'Admin'.", nameof(roleName));
        }

        _currentRole = roleName;
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    public static void Logout()
    {
        _currentRole = null;
    }
}
