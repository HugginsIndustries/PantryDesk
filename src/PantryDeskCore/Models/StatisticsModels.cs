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

/// <summary>
/// Label and count for visit type or demographics breakdowns.
/// </summary>
public class DemographicsBreakdown
{
    public string Label { get; set; } = string.Empty;
    public int Count { get; set; }
}

/// <summary>
/// Result for Monthly Activity Report: qualifying services only, with optional deck-only added to duplicated.
/// </summary>
public class MonthlyActivityReportStats
{
    public int TotalDaysOpen { get; set; }
    public int HouseholdsTotal { get; set; }
    public int HouseholdsUnduplicated { get; set; }
    public int HouseholdsDuplicated { get; set; }
    public int TotalPounds { get; set; }
    public int IndividualsTotal { get; set; }
    public int IndividualsUnduplicated { get; set; }
    public int IndividualsDuplicated { get; set; }
    public int InfantUnduplicated { get; set; }
    public int ChildUnduplicated { get; set; }
    public int AdultUnduplicated { get; set; }
    public int SeniorUnduplicated { get; set; }
    public int InfantDuplicated { get; set; }
    public int ChildDuplicated { get; set; }
    public int AdultDuplicated { get; set; }
    public int SeniorDuplicated { get; set; }
}

/// <summary>
/// Header fields for the Monthly Activity Report PDF.
/// </summary>
public class MonthlyActivityReportHeader
{
    public string FoodBankName { get; set; } = string.Empty;
    public string County { get; set; } = string.Empty;
    public string PreparedBy { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}