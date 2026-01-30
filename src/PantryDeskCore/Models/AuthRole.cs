namespace PantryDeskCore.Models;

/// <summary>
/// Represents a role authentication record.
/// </summary>
public class AuthRole
{
    public string RoleName { get; set; } = string.Empty; // 'Entry' or 'Admin'
    public string PasswordHash { get; set; } = string.Empty; // Base64-encoded hash
    public string Salt { get; set; } = string.Empty; // Base64-encoded salt
    public DateTime UpdatedAt { get; set; }
}
