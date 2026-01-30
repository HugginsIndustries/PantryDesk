using PantryDeskCore.Models;

namespace PantryDeskSeeder;

/// <summary>
/// Generates pantry days according to the business rules.
/// </summary>
public static class PantryDayGenerator
{
    /// <summary>
    /// Generates pantry days for the specified year.
    /// Jan-Oct: 2nd, 3rd, 4th Wednesday
    /// Nov-Dec: 1st, 2nd, 3rd Wednesday
    /// </summary>
    public static List<PantryDay> GeneratePantryDays(int year, Random rng)
    {
        var pantryDays = new List<PantryDay>();
        var now = DateTime.UtcNow;

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
                pantryDays.Add(new PantryDay
                {
                    PantryDate = date,
                    IsActive = true,
                    Notes = null,
                    CreatedAt = now
                });
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
                pantryDays.Add(new PantryDay
                {
                    PantryDate = date,
                    IsActive = true,
                    Notes = null,
                    CreatedAt = now
                });
            }
        }

        return pantryDays;
    }

    /// <summary>
    /// Gets the nth occurrence of a weekday in a month.
    /// Duplicated from PantryDaysForm to avoid touching core code.
    /// </summary>
    private static DateTime GetNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int occurrence)
    {
        // Find the first occurrence of the weekday in the month
        var firstDay = new DateTime(year, month, 1);
        var firstDayOfWeek = (int)firstDay.DayOfWeek;
        var targetDayOfWeek = (int)dayOfWeek;

        // Calculate days to add to get to the first occurrence
        // If first day is the target day, daysToAdd = 0
        // Otherwise, calculate how many days forward to the first occurrence
        var daysToAdd = (targetDayOfWeek - firstDayOfWeek + 7) % 7;
        if (daysToAdd == 0 && firstDay.DayOfWeek != dayOfWeek)
        {
            // This shouldn't happen with the modulo, but handle edge case
            daysToAdd = 7;
        }

        var firstOccurrence = firstDay.AddDays(daysToAdd);

        // Add weeks for the nth occurrence (occurrence is 1-based)
        var targetDate = firstOccurrence.AddDays((occurrence - 1) * 7);

        // Verify we're still in the same month
        if (targetDate.Month != month)
        {
            throw new ArgumentException($"Month {month} does not have {occurrence} occurrences of {dayOfWeek}");
        }

        return targetDate;
    }
}
