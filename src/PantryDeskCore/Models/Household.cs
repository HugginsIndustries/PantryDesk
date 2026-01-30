namespace PantryDeskCore.Models;

/// <summary>
/// Represents a household/client record.
/// </summary>
public class Household
{
    public int Id { get; set; }
    public string PrimaryName { get; set; } = string.Empty;
    public string? Address1 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int ChildrenCount { get; set; }
    public int AdultsCount { get; set; }
    public int SeniorsCount { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
