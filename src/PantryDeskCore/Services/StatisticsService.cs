using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;
using PantryDeskCore.Helpers;
using PantryDeskCore.Models;

namespace PantryDeskCore.Services;

/// <summary>
/// Service for calculating statistics and metrics.
/// </summary>
public static class StatisticsService
{
    /// <summary>
    /// Gets statistics for the current month.
    /// </summary>
    public static MonthlyStatistics GetCurrentMonthStats(SqliteConnection connection)
    {
        var now = DateTime.Now;
        return GetMonthlyStats(connection, now.Year, now.Month);
    }

    /// <summary>
    /// Gets statistics for a specific month.
    /// </summary>
    public static MonthlyStatistics GetMonthlyStats(SqliteConnection connection, int year, int month)
    {
        var monthStart = new DateTime(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);
        var startDateStr = monthStart.ToString("yyyy-MM-dd");
        var endDateStr = monthEnd.ToString("yyyy-MM-dd");

        var stats = new MonthlyStatistics();

        connection.Open();
        try
        {
            // Total active households
            using (var cmd = new SqliteCommand(Sql.StatisticsCountActiveHouseholds, connection))
            {
                var result = cmd.ExecuteScalar();
                stats.TotalActiveHouseholds = result != null ? Convert.ToInt32(result) : 0;
            }

            // Total people (sum of household sizes)
            using (var cmd = new SqliteCommand(Sql.StatisticsSumTotalPeople, connection))
            {
                var result = cmd.ExecuteScalar();
                stats.TotalPeople = result != null ? Convert.ToInt32(result) : 0;
            }

            // Completed services in range
            using (var cmd = new SqliteCommand(Sql.StatisticsCountCompletedServicesInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var result = cmd.ExecuteScalar();
                stats.CompletedServices = result != null ? Convert.ToInt32(result) : 0;
            }

            // Unique households served in range
            using (var cmd = new SqliteCommand(Sql.StatisticsCountUniqueHouseholdsServedInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var result = cmd.ExecuteScalar();
                stats.UniqueHouseholdsServed = result != null ? Convert.ToInt32(result) : 0;
            }

            // PantryDay vs Appointment breakdown
            using (var cmd = new SqliteCommand(Sql.StatisticsCountCompletedServicesByTypeInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var eventType = reader.GetString(0);
                    var count = reader.GetInt32(1);
                    if (eventType == "PantryDay")
                    {
                        stats.PantryDayCompletions = count;
                    }
                    else if (eventType == "Appointment")
                    {
                        stats.AppointmentCompletions = count;
                    }
                }
            }

            // Overrides count
            using (var cmd = new SqliteCommand(Sql.StatisticsCountOverridesInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var result = cmd.ExecuteScalar();
                stats.OverridesCount = result != null ? Convert.ToInt32(result) : 0;
            }

            // Deck total for single month
            var deckStats = DeckStatsRepository.Get(connection, year, month);
            stats.DeckTotal = deckStats != null ? (int)Math.Round(deckStats.HouseholdTotalAvg) : 0;
        }
        finally
        {
            connection.Close();
        }

        return stats;
    }

    /// <summary>
    /// Gets breakdown of services by city for a date range.
    /// </summary>
    public static List<CityBreakdown> GetStatsByCity(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        var breakdowns = new List<CityBreakdown>();
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.StatisticsBreakdownByCityInRange, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                breakdowns.Add(new CityBreakdown
                {
                    City = reader.GetString(0),
                    HouseholdsServed = reader.GetInt32(1),
                    ServicesCompleted = reader.GetInt32(2)
                });
            }
        }
        finally
        {
            connection.Close();
        }

        return breakdowns;
    }

    /// <summary>
    /// Gets breakdown of override reasons for a date range.
    /// </summary>
    public static List<OverrideBreakdown> GetOverrideBreakdown(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        var breakdowns = new List<OverrideBreakdown>();
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.StatisticsBreakdownByOverrideReasonInRange, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                breakdowns.Add(new OverrideBreakdown
                {
                    Reason = reader.GetString(0),
                    Count = reader.GetInt32(1)
                });
            }
        }
        finally
        {
            connection.Close();
        }

        return breakdowns;
    }

    /// <summary>
    /// Gets breakdown of completed services per pantry day for a month.
    /// </summary>
    public static List<PantryDayBreakdown> GetPantryDayBreakdown(SqliteConnection connection, int year, int month)
    {
        var breakdowns = new List<PantryDayBreakdown>();
        var monthStart = new DateTime(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);
        var startDateStr = monthStart.ToString("yyyy-MM-dd");
        var endDateStr = monthEnd.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.StatisticsPantryDayBreakdownInRange, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                breakdowns.Add(new PantryDayBreakdown
                {
                    PantryDate = DateTime.Parse(reader.GetString(0)),
                    CompletedServices = reader.GetInt32(1)
                });
            }
        }
        finally
        {
            connection.Close();
        }

        return breakdowns;
    }

    /// <summary>
    /// Gets household composition totals by age group (Infant, Child, Adult, Senior) for unique households served in a date range.
    /// Age groups derived from member birthdays using endDate as reference.
    /// </summary>
    public static (int Infant, int Child, int Adult, int Senior) GetCompositionServed(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        var householdIds = new List<int>();
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.StatisticsSelectServedHouseholdIdsInRange, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                householdIds.Add(reader.GetInt32(0));
            }
        }
        finally
        {
            connection.Close();
        }

        var infant = 0;
        var child = 0;
        var adult = 0;
        var senior = 0;

        foreach (var householdId in householdIds)
        {
            var members = HouseholdMemberRepository.GetByHouseholdId(connection, householdId);
            foreach (var member in members)
            {
                var ageGroup = AgeGroupHelper.GetAgeGroup(member.Birthday, endDate);
                switch (ageGroup)
                {
                    case "Infant": infant++; break;
                    case "Child": child++; break;
                    case "Adult": adult++; break;
                    case "Senior": senior++; break;
                }
            }
        }

        return (infant, child, adult, senior);
    }

    /// <summary>
    /// Gets deck total (sum of averaged household values) for all months in the date range.
    /// For each month in range, sums HouseholdTotalAvg (the averaged value = entered total ÷ number of pages).
    /// </summary>
    private static int GetDeckTotalForDateRange(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        double sum = 0.0;

        // Iterate through each calendar month in the date range
        var currentMonth = new DateTime(startDate.Year, startDate.Month, 1);
        var endMonth = new DateTime(endDate.Year, endDate.Month, 1);

        while (currentMonth <= endMonth)
        {
            var deckStats = DeckStatsRepository.Get(connection, currentMonth.Year, currentMonth.Month);
            if (deckStats != null)
            {
                sum += deckStats.HouseholdTotalAvg;
            }

            currentMonth = currentMonth.AddMonths(1);
        }

        return (int)Math.Round(sum);
    }

    /// <summary>
    /// Gets statistics for an arbitrary date range.
    /// </summary>
    public static MonthlyStatistics GetStatsForDateRange(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        var stats = new MonthlyStatistics();

        connection.Open();
        try
        {
            // Total active households = unique households with >=1 completed service in range
            using (var cmd = new SqliteCommand(Sql.StatisticsCountUniqueHouseholdsServedInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var result = cmd.ExecuteScalar();
                stats.TotalActiveHouseholds = result != null ? Convert.ToInt32(result) : 0;
            }

            // Total people = sum of individuals in served households in range
            using (var cmd = new SqliteCommand(Sql.StatisticsTotalPeopleServedInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var result = cmd.ExecuteScalar();
                stats.TotalPeople = result != null ? Convert.ToInt32(result) : 0;
            }

            // Completed services in range
            using (var cmd = new SqliteCommand(Sql.StatisticsCountCompletedServicesInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var result = cmd.ExecuteScalar();
                stats.CompletedServices = result != null ? Convert.ToInt32(result) : 0;
            }

            // Unique households served in range
            using (var cmd = new SqliteCommand(Sql.StatisticsCountUniqueHouseholdsServedInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var result = cmd.ExecuteScalar();
                stats.UniqueHouseholdsServed = result != null ? Convert.ToInt32(result) : 0;
            }

            // PantryDay vs Appointment breakdown
            using (var cmd = new SqliteCommand(Sql.StatisticsCountCompletedServicesByTypeInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var eventType = reader.GetString(0);
                    var count = reader.GetInt32(1);
                    if (eventType == "PantryDay")
                    {
                        stats.PantryDayCompletions = count;
                    }
                    else if (eventType == "Appointment")
                    {
                        stats.AppointmentCompletions = count;
                    }
                }
            }

            // Overrides count
            using (var cmd = new SqliteCommand(Sql.StatisticsCountOverridesInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var result = cmd.ExecuteScalar();
                stats.OverridesCount = result != null ? Convert.ToInt32(result) : 0;
            }

            // Deck total = sum of averaged household values for each month in range
            stats.DeckTotal = GetDeckTotalForDateRange(connection, startDate, endDate);
        }
        finally
        {
            connection.Close();
        }

        return stats;
    }

    /// <summary>
    /// Gets monthly visits trend (completed services grouped by month) for a date range.
    /// </summary>
    public static List<MonthlyVisitsTrend> GetMonthlyVisitsTrend(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        var trends = new List<MonthlyVisitsTrend>();
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.StatisticsMonthlyVisitsTrend, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                trends.Add(new MonthlyVisitsTrend
                {
                    Month = reader.GetString(0),
                    Count = reader.GetInt32(1)
                });
            }
        }
        finally
        {
            connection.Close();
        }

        return trends;
    }

    /// <summary>
    /// Gets pantry day volume by event (completed services per pantry day) for a date range.
    /// </summary>
    public static List<PantryDayBreakdown> GetPantryDayVolumeByEvent(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        var breakdowns = new List<PantryDayBreakdown>();
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.StatisticsPantryDayBreakdownInRange, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                breakdowns.Add(new PantryDayBreakdown
                {
                    PantryDate = DateTime.Parse(reader.GetString(0)),
                    CompletedServices = reader.GetInt32(1)
                });
            }
        }
        finally
        {
            connection.Close();
        }

        return breakdowns;
    }

    /// <summary>
    /// Gets breakdown of completed services by visit type for a date range.
    /// </summary>
    public static List<DemographicsBreakdown> GetVisitTypeBreakdown(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        return GetDemographicsBreakdown(connection, startDate, endDate, Sql.StatisticsBreakdownByVisitTypeInRange);
    }

    /// <summary>
    /// Gets demographics by race for members of households served in a date range.
    /// </summary>
    public static List<DemographicsBreakdown> GetDemographicsByRace(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        return GetDemographicsBreakdown(connection, startDate, endDate, Sql.StatisticsDemographicsByRaceInRange);
    }

    /// <summary>
    /// Gets demographics by veteran status for members of households served in a date range.
    /// </summary>
    public static List<DemographicsBreakdown> GetDemographicsByVeteranStatus(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        return GetDemographicsBreakdown(connection, startDate, endDate, Sql.StatisticsDemographicsByVeteranStatusInRange);
    }

    /// <summary>
    /// Gets demographics by disabled status for members of households served in a date range.
    /// </summary>
    public static List<DemographicsBreakdown> GetDemographicsByDisabledStatus(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        return GetDemographicsBreakdown(connection, startDate, endDate, Sql.StatisticsDemographicsByDisabledStatusInRange);
    }

    /// <summary>
    /// Gets veteran status breakdown for served households with derived "Disabled Veteran" category.
    /// Anyone who is both Veteran and Disabled is counted only in "Disabled Veteran", not in "Veteran".
    /// </summary>
    public static List<DemographicsBreakdown> GetVeteranStatusWithDisabledVeteranBreakdown(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        return GetDemographicsBreakdown(connection, startDate, endDate, Sql.StatisticsDemographicsByVeteranStatusWithDisabledVeteranInRange);
    }

    private static List<DemographicsBreakdown> GetDemographicsBreakdown(SqliteConnection connection, DateTime startDate, DateTime endDate, string query)
    {
        var breakdowns = new List<DemographicsBreakdown>();
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                breakdowns.Add(new DemographicsBreakdown
                {
                    Label = reader.GetString(0),
                    Count = reader.GetInt32(1)
                });
            }
        }
        finally
        {
            connection.Close();
        }

        return breakdowns;
    }

    /// <summary>
    /// Gets stats for the Monthly Activity Report. Uses same definition as Statistics Dashboard: all completed services in range (no visit_type filter).
    /// If deckStats is provided for the month, adds deck averages to duplicated individuals only (rounded to int).
    /// </summary>
    public static MonthlyActivityReportStats GetMonthlyActivityReportStats(SqliteConnection connection, int year, int month, DeckStatsMonthly? deckStats = null)
    {
        var monthStart = new DateTime(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);
        var startDateStr = monthStart.ToString("yyyy-MM-dd");
        var endDateStr = monthEnd.ToString("yyyy-MM-dd");

        var result = new MonthlyActivityReportStats();
        connection.Open();
        try
        {
            var reportingYearStart = ActiveStatusSyncService.GetReportingYearStartForMonth(connection, year, month);
            var reportingYearEnd = reportingYearStart.AddYears(1).AddDays(-1);
            var reportingYearStartStr = reportingYearStart.ToString("yyyy-MM-dd");
            var reportingYearEndStr = reportingYearEnd.ToString("yyyy-MM-dd");

            // Total days open (pantry days in month)
            using (var cmd = new SqliteCommand(Sql.ActivityReportCountPantryDaysInMonth, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var r = cmd.ExecuteScalar();
                result.TotalDaysOpen = r != null ? Convert.ToInt32(r) : 0;
            }

            // Total households served in month (all completed services; same definition as Statistics Dashboard)
            using (var cmd = new SqliteCommand(Sql.StatisticsCountUniqueHouseholdsServedInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                var r = cmd.ExecuteScalar();
                result.HouseholdsTotal = r != null ? Convert.ToInt32(r) : 0;
            }

            if (result.HouseholdsTotal == 0)
            {
                result.TotalPounds = 0;
                connection.Close();
                return result;
            }

            // Household IDs served in month (all completed; same as Statistics Dashboard)
            var householdIdsInMonth = new List<int>();
            using (var cmd = new SqliteCommand(Sql.StatisticsSelectServedHouseholdIdsInRange, connection))
            {
                cmd.Parameters.AddWithValue("@start_date", startDateStr);
                cmd.Parameters.AddWithValue("@end_date", endDateStr);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    householdIdsInMonth.Add(reader.GetInt32(0));
                }
            }

            // First completed (any type) date in reporting year per household for undup/dup split
            var firstDateByHousehold = new Dictionary<int, DateTime>();
            using (var cmd = new SqliteCommand(Sql.StatisticsFirstCompletedDateInReportingYearPerHousehold, connection))
            {
                cmd.Parameters.AddWithValue("@reporting_year_start", reportingYearStartStr);
                cmd.Parameters.AddWithValue("@reporting_year_end", reportingYearEndStr);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var hid = reader.GetInt32(0);
                    var firstDate = DateTime.Parse(reader.GetString(1));
                    firstDateByHousehold[hid] = firstDate;
                }
            }

            var undupIds = new List<int>();
            var dupIds = new List<int>();
            foreach (var hid in householdIdsInMonth)
            {
                if (firstDateByHousehold.TryGetValue(hid, out var firstDate) && firstDate >= monthStart && firstDate <= monthEnd)
                {
                    undupIds.Add(hid);
                }
                else
                {
                    dupIds.Add(hid);
                }
            }

            result.HouseholdsUnduplicated = undupIds.Count;
            result.HouseholdsDuplicated = dupIds.Count;

            // Composition for unduplicated and duplicated
            var (infantU, childU, adultU, seniorU) = GetCompositionForHouseholdIds(connection, undupIds, monthEnd);
            var (infantD, childD, adultD, seniorD) = GetCompositionForHouseholdIds(connection, dupIds, monthEnd);

            result.InfantUnduplicated = infantU;
            result.ChildUnduplicated = childU;
            result.AdultUnduplicated = adultU;
            result.SeniorUnduplicated = seniorU;
            result.InfantDuplicated = infantD;
            result.ChildDuplicated = childD;
            result.AdultDuplicated = adultD;
            result.SeniorDuplicated = seniorD;

            result.IndividualsUnduplicated = infantU + childU + adultU + seniorU;
            result.IndividualsDuplicated = infantD + childD + adultD + seniorD;
            result.IndividualsTotal = result.IndividualsUnduplicated + result.IndividualsDuplicated;

            // Add deck-only averages to duplicated only (rounded)
            if (deckStats != null)
            {
                result.IndividualsDuplicated += (int)Math.Round(deckStats.HouseholdTotalAvg);
                result.InfantDuplicated += (int)Math.Round(deckStats.InfantAvg);
                result.ChildDuplicated += (int)Math.Round(deckStats.ChildAvg);
                result.AdultDuplicated += (int)Math.Round(deckStats.AdultAvg);
                result.SeniorDuplicated += (int)Math.Round(deckStats.SeniorAvg);
                result.IndividualsTotal = result.IndividualsUnduplicated + result.IndividualsDuplicated;
            }

            result.TotalPounds = result.HouseholdsTotal * 65;
        }
        finally
        {
            connection.Close();
        }

        return result;
    }

    /// <summary>
    /// Gets age-group composition (Infant, Child, Adult, Senior) for the given household IDs, using endDate as reference for age.
    /// </summary>
    private static (int Infant, int Child, int Adult, int Senior) GetCompositionForHouseholdIds(SqliteConnection connection, List<int> householdIds, DateTime endDate)
    {
        if (householdIds.Count == 0)
        {
            return (0, 0, 0, 0);
        }

        var infant = 0;
        var child = 0;
        var adult = 0;
        var senior = 0;

        foreach (var householdId in householdIds)
        {
            var members = HouseholdMemberRepository.GetByHouseholdId(connection, householdId);
            foreach (var member in members)
            {
                var ageGroup = AgeGroupHelper.GetAgeGroup(member.Birthday, endDate);
                switch (ageGroup)
                {
                    case "Infant": infant++; break;
                    case "Child": child++; break;
                    case "Adult": adult++; break;
                    case "Senior": senior++; break;
                }
            }
        }

        return (infant, child, adult, senior);
    }
}
