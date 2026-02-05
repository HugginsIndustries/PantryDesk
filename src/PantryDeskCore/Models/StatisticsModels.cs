namespace PantryDeskCore.Models;

/// <summary>
/// Data models for statistics and reporting.
/// </summary>
public class MonthlyStatistics
{
    public int TotalActiveHouseholds { get; set; }
    public int TotalPeople { get; set; }
    public int CompletedServices { get; set; }
    public int UniqueHouseholdsServed { get; set; }
    public int PantryDayCompletions { get; set; }
    public int AppointmentCompletions { get; set; }
    public int OverridesCount { get; set; }
}

public class CityBreakdown
{
    public string City { get; set; } = string.Empty;
    public int HouseholdsServed { get; set; }
    public int ServicesCompleted { get; set; }
}

public class OverrideBreakdown
{
    public string Reason { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class PantryDayBreakdown
{
    public DateTime PantryDate { get; set; }
    public int CompletedServices { get; set; }
}

public class MonthlyVisitsTrend
{
    public string Month { get; set; } = string.Empty; // Format: "YYYY-MM"
    public int Count { get; set; }
}