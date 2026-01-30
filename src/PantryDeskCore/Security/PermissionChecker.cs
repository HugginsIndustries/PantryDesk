namespace PantryDeskCore.Security;

/// <summary>
/// Provides permission checking utilities for role-based access control.
/// </summary>
public static class PermissionChecker
{
    /// <summary>
    /// Checks if the current user is an Admin.
    /// </summary>
    /// <returns>True if the current user is an Admin, false otherwise.</returns>
    public static bool IsAdmin()
    {
        return SessionManager.IsAdmin;
    }

    /// <summary>
    /// Requires that a user is logged in. Throws an exception if not logged in.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">Thrown if no user is logged in.</exception>
    public static void RequireLoggedIn()
    {
        if (!SessionManager.IsLoggedIn)
        {
            throw new UnauthorizedAccessException("User must be logged in to perform this action.");
        }
    }

    /// <summary>
    /// Requires that the current user is an Admin. Throws an exception if not admin.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">Thrown if the current user is not an Admin.</exception>
    public static void RequireAdmin()
    {
        RequireLoggedIn();
        if (!SessionManager.IsAdmin)
        {
            throw new UnauthorizedAccessException("This action requires Admin privileges.");
        }
    }
}
