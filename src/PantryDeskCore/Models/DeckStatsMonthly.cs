namespace PantryDeskCore.Models;

/// <summary>
/// One record per (year, month) for deck-only bulk entry averaged totals.
/// Averages are stored (total ÷ number of pages); page_count stored for audit.
/// </summary>
public class DeckStatsMonthly
{
    public int Year { get; set; }
    public int Month { get; set; }
    public double HouseholdTotalAvg { get; set; }
    public double InfantAvg { get; set; }
    public double ChildAvg { get; set; }
    public double AdultAvg { get; set; }
    public double SeniorAvg { get; set; }
    public int? PageCount { get; set; }
    public string UpdatedAt { get; set; } = string.Empty;
}
