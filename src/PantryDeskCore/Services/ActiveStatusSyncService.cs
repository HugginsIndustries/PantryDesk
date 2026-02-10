using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;

namespace PantryDeskCore.Services;

/// <summary>
/// Syncs household IsActive status based on last qualifying service date vs the annual reset date.
/// Status is system-managed; qualifying service = any completed service event.
/// </summary>
public static class ActiveStatusSyncService
{
    private const string ConfigKeyResetMonth = "active_status_reset_month";
    private const string ConfigKeyResetDay = "active_status_reset_day";
    private const int DefaultMonth = 1;
    private const int DefaultDay = 1;

    /// <summary>
    /// Gets the first day of the reporting year that contains the given month.
    /// Uses config keys active_status_reset_month and active_status_reset_day; defaults to Jan 1.
    /// Connection must be open; does not close it.
    /// </summary>
    public static DateTime GetReportingYearStartForMonth(SqliteConnection connection, int forYear, int forMonth)
    {
        var month = ReadConfigInt(connection, ConfigKeyResetMonth, DefaultMonth);
        var dayConfig = ReadConfigInt(connection, ConfigKeyResetDay, DefaultDay);
        month = Math.Clamp(month, 1, 12);

        var firstOfMonth = new DateTime(forYear, forMonth, 1);
        var resetDayInYear = Math.Min(dayConfig, DateTime.DaysInMonth(forYear, month));
        var resetInYear = new DateTime(forYear, month, resetDayInYear);

        if (resetInYear <= firstOfMonth)
        {
            return resetInYear;
        }

        var resetDayLastYear = Math.Min(dayConfig, DateTime.DaysInMonth(forYear - 1, month));
        return new DateTime(forYear - 1, month, resetDayLastYear);
    }

    /// <summary>
    /// Gets the current reset date: the most recent occurrence of (month, day) that is on or before today.
    /// Uses config keys active_status_reset_month and active_status_reset_day; defaults to Jan 1.
    /// </summary>
    public static DateTime GetCurrentResetDate(SqliteConnection connection)
    {
        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            var month = ReadConfigInt(connection, ConfigKeyResetMonth, DefaultMonth);
            var dayConfig = ReadConfigInt(connection, ConfigKeyResetDay, DefaultDay);

            month = Math.Clamp(month, 1, 12);

            var today = DateTime.Today;
            var maxDayThisYear = DateTime.DaysInMonth(today.Year, month);
            var dayThisYear = Math.Clamp(dayConfig, 1, maxDayThisYear);

            var thisYear = new DateTime(today.Year, month, dayThisYear);
            if (thisYear <= today)
            {
                return thisYear;
            }

            var maxDayLastYear = DateTime.DaysInMonth(today.Year - 1, month);
            var dayLastYear = Math.Min(dayConfig, maxDayLastYear);
            var lastYear = new DateTime(today.Year - 1, month, dayLastYear);
            return lastYear;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Syncs IsActive for all households: active if last completed service date >= reset date; otherwise inactive.
    /// Never-served households become inactive.
    /// </summary>
    public static void SyncAllHouseholds(SqliteConnection connection)
    {
        var resetDate = GetCurrentResetDate(connection);

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            var resetDateStr = resetDate.ToString("yyyy-MM-dd");
            var updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            using var cmd = new SqliteCommand(Sql.HouseholdBulkSyncIsActive, connection);
            cmd.Parameters.AddWithValue("@reset_date", resetDateStr);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    private static int ReadConfigInt(SqliteConnection connection, string key, int defaultValue)
    {
        using var cmd = new SqliteCommand(Sql.ConfigGetValue, connection);
        cmd.Parameters.AddWithValue("@key", key);
        var result = cmd.ExecuteScalar();
        if (result != null && int.TryParse(result.ToString(), out var value))
        {
            return value;
        }
        return defaultValue;
    }
}
