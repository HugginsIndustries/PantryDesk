namespace PantryDeskCore.Models;

/// <summary>
/// Represents a service delivery record.
/// </summary>
public class ServiceEvent
{
    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public string EventType { get; set; } = string.Empty; // 'PantryDay' or 'Appointment'
    public string EventStatus { get; set; } = string.Empty; // 'Scheduled', 'Completed', 'Cancelled', 'NoShow'
    public DateTime EventDate { get; set; }
    public string? ScheduledText { get; set; }
    public string? OverrideReason { get; set; }
    public string? Notes { get; set; }
    public string? VisitType { get; set; } // 'Shop with TEFAP', 'Shop', 'TEFAP Only', 'Deck Only'
    public DateTime CreatedAt { get; set; }
}
