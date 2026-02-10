using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;

namespace PantryDeskCore.Services;

/// <summary>
/// Service for checking household eligibility for monthly service.
/// </summary>
public static class EligibilityService
{
    /// <summary>
    /// Checks if a household is eligible for service in the same calendar month as the reference date.
    /// A household is eligible if they have not completed any service events in that month.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="householdId">The household ID to check.</param>
    /// <param name="referenceDate">The reference date (typically today).</param>
    /// <returns>True if eligible (no completed services this month), false if already served this month.</returns>
    public static bool IsEligibleThisMonth(SqliteConnection connection, int householdId, DateTime referenceDate)
    {
        // Get start and end of the reference month
        var monthStart = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        // Use optimized query that filters by household, status, date range, and qualifying visit type in SQL
        // Only Shop with TEFAP and Shop count toward the monthly limit; TEFAP Only and Deck Only do not
        var hasCompleted = ServiceEventRepository.HasCompletedQualifyingVisitInDateRange(connection, householdId, monthStart, monthEnd);

        // Eligible if no completed services this month
        return !hasCompleted;
    }

    /// <summary>
    /// Same as IsEligibleThisMonth but excludes a specific event (e.g. when editing an existing completed event).
    /// </summary>
    public static bool IsEligibleThisMonthExcludingEvent(
        SqliteConnection connection, int householdId, DateTime referenceDate, int excludeEventId)
    {
        var monthStart = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        var hasCompleted = ServiceEventRepository.HasCompletedQualifyingVisitInDateRangeExcluding(
            connection, householdId, monthStart, monthEnd, excludeEventId);

        return !hasCompleted;
    }
}
