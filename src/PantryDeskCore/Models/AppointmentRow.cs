namespace PantryDeskCore.Models;

/// <summary>
/// Display row for appointment lists (Past/Future panels) with household name.
/// </summary>
public class AppointmentRow
{
    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventStatus { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string? ScheduledText { get; set; }
    public string? OverrideReason { get; set; }
    public string? Notes { get; set; }
    public string? VisitType { get; set; }
    public DateTime CreatedAt { get; set; }
    public string PrimaryName { get; set; } = string.Empty;
    /// <summary>
    /// Formatted display name: primary name only, or "Member • Primary Household" when scheduled for non-primary member.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
}
