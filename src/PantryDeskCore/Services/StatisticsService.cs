using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;
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
    /// Gets household composition totals (Children/Adults/Seniors) for unique households served in a date range.
    /// </summary>
    public static (int Children, int Adults, int Seniors) GetCompositionServed(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.StatisticsCompositionServedInRange, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (
                    reader.GetInt32(0), // total_children
                    reader.GetInt32(1), // total_adults
                    reader.GetInt32(2)  // total_seniors
                );
            }
        }
        finally
        {
            connection.Close();
        }

        return (0, 0, 0);
    }
}
