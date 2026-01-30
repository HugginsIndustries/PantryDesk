namespace PantryDeskCore.Models;

/// <summary>
/// Represents a pantry day calendar entry.
/// </summary>
public class PantryDay
{
    public int Id { get; set; }
    public DateTime PantryDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
