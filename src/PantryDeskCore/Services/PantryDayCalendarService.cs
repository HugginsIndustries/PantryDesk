using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;
using PantryDeskCore.Models;

namespace PantryDeskCore.Services;

/// <summary>
/// Ensures pantry days for a year exist using the standard rule.
/// Jan-Oct: 2nd, 3rd, 4th Wednesday; Nov-Dec: 1st, 2nd, 3rd Wednesday.
/// </summary>
public static class PantryDayCalendarService
{
    /// <summary>
    /// Ensures all pantry days for the given year exist. Creates only missing dates;
    /// does not duplicate or overwrite existing pantry days.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="year">The year to ensure pantry days for.</param>
    /// <returns>The number of pantry days created.</returns>
    public static int EnsurePantryDaysForYear(SqliteConnection connection, int year)
    {
        var createdCount = 0;

        // Jan-Oct: 2nd, 3rd, 4th Wednesday
        for (int month = 1; month <= 10; month++)
        {
            var dates = new[]
            {
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 2),
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 3),
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 4)
            };

            foreach (var date in dates)
            {
                var existing = PantryDayRepository.GetByDate(connection, date);
                if (existing != null)
                {
                    continue;
                }

                var pantryDay = new PantryDay
                {
                    PantryDate = date,
                    IsActive = true,
                    Notes = null
                };

                PantryDayRepository.Create(connection, pantryDay);
                createdCount++;
            }
        }

        // Nov-Dec: 1st, 2nd, 3rd Wednesday
        for (int month = 11; month <= 12; month++)
        {
            var dates = new[]
            {
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 1),
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 2),
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 3)
            };

            foreach (var date in dates)
            {
                var existing = PantryDayRepository.GetByDate(connection, date);
                if (existing != null)
                {
                    continue;
                }

                var pantryDay = new PantryDay
                {
                    PantryDate = date,
                    IsActive = true,
                    Notes = null
                };

                PantryDayRepository.Create(connection, pantryDay);
                createdCount++;
            }
        }

        return createdCount;
    }

    private static DateTime GetNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int occurrence)
    {
        var firstDay = new DateTime(year, month, 1);
        var firstDayOfWeek = (int)firstDay.DayOfWeek;
        var targetDayOfWeek = (int)dayOfWeek;

        var daysToAdd = (targetDayOfWeek - firstDayOfWeek + 7) % 7;
        if (daysToAdd == 0 && firstDay.DayOfWeek != dayOfWeek)
        {
            daysToAdd = 7;
        }

        var firstOccurrence = firstDay.AddDays(daysToAdd);
        var targetDate = firstOccurrence.AddDays((occurrence - 1) * 7);

        if (targetDate.Month != month)
        {
            throw new ArgumentException($"Month {month} does not have {occurrence} occurrences of {dayOfWeek}");
        }

        return targetDate;
    }
}
