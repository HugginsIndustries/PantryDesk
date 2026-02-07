namespace PantryDeskCore.Models;

/// <summary>
/// Represents an individual member of a household.
/// </summary>
public class HouseholdMember
{
    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public bool IsPrimary { get; set; }
    public string? Race { get; set; }
    public string? VeteranStatus { get; set; }
    public string? DisabledStatus { get; set; }

    /// <summary>
    /// Full display name: FirstName LastName.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
}
