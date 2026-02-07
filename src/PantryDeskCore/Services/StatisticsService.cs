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
            // Total active households (not date-dependent, but included for consistency)
            using (var cmd = new SqliteCommand(Sql.StatisticsCountActiveHouseholds, connection))
            {
                var result = cmd.ExecuteScalar();
                stats.TotalActiveHouseholds = result != null ? Convert.ToInt32(result) : 0;
            }

            // Total people (not date-dependent, but included for consistency)
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
}
