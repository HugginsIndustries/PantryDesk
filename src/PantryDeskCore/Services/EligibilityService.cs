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

        // Use optimized query that filters by household, status, and date range in SQL
        var hasCompleted = ServiceEventRepository.HasCompletedInDateRange(connection, householdId, monthStart, monthEnd);

        // Eligible if no completed services this month
        return !hasCompleted;
    }
}
