using PantryDeskCore.Models;

namespace PantryDeskSeeder;

/// <summary>
/// Generates service events (completed and scheduled) for demo data.
/// </summary>
public static class ServiceEventGenerator
{
    private static readonly string[] OverrideReasons = new[]
    {
        "Special Circumstance",
        "Emergency Need",
        "Admin Override",
        "Other"
    };

    private static readonly string[] ScheduledTexts = new[]
    {
        "10:00 AM - 11:00 AM",
        "11:00 AM - 12:00 PM",
        "1:00 PM - 2:00 PM",
        "2:00 PM - 3:00 PM",
        "3:00 PM - 4:00 PM",
        "9:00 AM - 10:00 AM",
        "4:00 PM - 5:00 PM"
    };

    /// <summary>
    /// Generates service events for households and pantry days.
    /// </summary>
    public static List<ServiceEvent> GenerateServiceEvents(
        List<Household> households,
        List<PantryDay> pantryDays,
        SeederConfig config,
        Random rng,
        DateTime baseDate)
    {
        var events = new List<ServiceEvent>();
        var startDate = baseDate.AddMonths(-config.MonthsBack);
        var activeHouseholds = households.Where(h => h.IsActive).ToList();

        if (activeHouseholds.Count == 0)
        {
            return events;
        }

        // Generate completed PantryDay events
        var pantryDaysInRange = pantryDays
            .Where(pd => pd.PantryDate >= startDate && pd.PantryDate <= baseDate && pd.IsActive)
            .ToList();

        foreach (var pantryDay in pantryDaysInRange)
        {
            var eventCount = rng.Next(config.EventsPerPantryDayRange.Min, config.EventsPerPantryDayRange.Max + 1);
            eventCount = Math.Min(eventCount, activeHouseholds.Count);

            // Select random households for this pantry day
            var selectedHouseholds = activeHouseholds
                .OrderBy(_ => rng.Next())
                .Take(eventCount)
                .ToList();

            foreach (var household in selectedHouseholds)
            {
                var hasOverride = rng.NextDouble() < 0.15; // 15% have overrides
                var overrideReason = hasOverride ? OverrideReasons[rng.Next(OverrideReasons.Length)] : null;
                var notes = hasOverride && rng.NextDouble() < 0.5 ? GenerateOverrideNotes(rng) : null;

                events.Add(new ServiceEvent
                {
                    HouseholdId = household.Id,
                    EventType = "PantryDay",
                    EventStatus = "Completed",
                    EventDate = pantryDay.PantryDate,
                    OverrideReason = overrideReason,
                    Notes = notes,
                    CreatedAt = pantryDay.PantryDate.AddHours(10 + rng.Next(8)) // Random time during pantry hours
                });
            }
        }

        // Generate completed Appointment events (not on pantry days)
        var pantryDayDates = pantryDaysInRange.Select(pd => pd.PantryDate.Date).ToHashSet();
        var appointmentDates = GenerateAppointmentDates(startDate, baseDate, pantryDayDates, config, rng);

        foreach (var appointmentDate in appointmentDates)
        {
            var household = activeHouseholds[rng.Next(activeHouseholds.Count)];
            var hasOverride = rng.NextDouble() < 0.15; // 15% have overrides
            var overrideReason = hasOverride ? OverrideReasons[rng.Next(OverrideReasons.Length)] : null;
            var notes = hasOverride && rng.NextDouble() < 0.5 ? GenerateOverrideNotes(rng) : null;

            events.Add(new ServiceEvent
            {
                HouseholdId = household.Id,
                EventType = "Appointment",
                EventStatus = "Completed",
                EventDate = appointmentDate,
                ScheduledText = ScheduledTexts[rng.Next(ScheduledTexts.Length)],
                OverrideReason = overrideReason,
                Notes = notes,
                CreatedAt = appointmentDate.AddHours(9 + rng.Next(8))
            });
        }

        // Generate scheduled Appointment events (future)
        var futureStart = baseDate.AddDays(1);
        var futureEnd = baseDate.AddDays(28); // Next 4 weeks
        var scheduledCount = rng.Next(5, 20); // 5-20 scheduled appointments

        for (int i = 0; i < scheduledCount; i++)
        {
            var daysAhead = rng.Next((futureEnd - futureStart).Days + 1);
            var scheduledDate = futureStart.AddDays(daysAhead);

            // Don't schedule on pantry days
            if (pantryDayDates.Contains(scheduledDate.Date))
            {
                continue;
            }

            var household = activeHouseholds[rng.Next(activeHouseholds.Count)];
            var scheduledText = ScheduledTexts[rng.Next(ScheduledTexts.Length)];

            events.Add(new ServiceEvent
            {
                HouseholdId = household.Id,
                EventType = "Appointment",
                EventStatus = "Scheduled",
                EventDate = scheduledDate,
                ScheduledText = scheduledText,
                Notes = rng.NextDouble() < 0.3 ? GenerateScheduledNotes(rng) : null,
                CreatedAt = baseDate.AddHours(-rng.Next(168)) // Created within last week
            });
        }

        // Ensure some households are ineligible this month (already served)
        // This is handled by the completed events above, but we can add a few more
        // to ensure demo moments are visible
        var currentMonthStart = new DateTime(baseDate.Year, baseDate.Month, 1);
        var currentMonthEvents = events.Where(e => 
            e.EventDate >= currentMonthStart && 
            e.EventDate <= baseDate && 
            e.EventStatus == "Completed").ToList();

        if (currentMonthEvents.Count < activeHouseholds.Count / 3)
        {
            // Add a few more completed events this month to ensure ineligibility is visible
            var additionalCount = Math.Min(10, activeHouseholds.Count / 4);
            var householdsToAdd = activeHouseholds
                .Where(h => !currentMonthEvents.Any(e => e.HouseholdId == h.Id))
                .OrderBy(_ => rng.Next())
                .Take(additionalCount)
                .ToList();

            foreach (var household in householdsToAdd)
            {
                var eventDate = currentMonthStart.AddDays(rng.Next((baseDate - currentMonthStart).Days + 1));
                if (!pantryDayDates.Contains(eventDate.Date))
                {
                    events.Add(new ServiceEvent
                    {
                        HouseholdId = household.Id,
                        EventType = "Appointment",
                        EventStatus = "Completed",
                        EventDate = eventDate,
                        ScheduledText = ScheduledTexts[rng.Next(ScheduledTexts.Length)],
                        CreatedAt = eventDate.AddHours(9 + rng.Next(8))
                    });
                }
            }
        }

        return events;
    }

    private static List<DateTime> GenerateAppointmentDates(
        DateTime startDate,
        DateTime endDate,
        HashSet<DateTime> pantryDayDates,
        SeederConfig config,
        Random rng)
    {
        var dates = new List<DateTime>();
        var currentDate = startDate;
        var totalDays = (endDate - startDate).Days;
        var totalWeeks = Math.Max(1, totalDays / 7);

        // Generate appointments per week
        for (int week = 0; week < totalWeeks; week++)
        {
            var appointmentsThisWeek = rng.Next(config.AppointmentsPerWeekRange.Min, config.AppointmentsPerWeekRange.Max + 1);
            
            for (int i = 0; i < appointmentsThisWeek; i++)
            {
                // Pick a random day in this week (Mon-Fri, not pantry days)
                var weekStart = currentDate.AddDays(week * 7);
                var attempts = 0;
                DateTime appointmentDate;

                do
                {
                    var dayOffset = rng.Next(5); // Mon-Fri
                    appointmentDate = weekStart.AddDays(dayOffset);
                    attempts++;
                } while (pantryDayDates.Contains(appointmentDate.Date) && attempts < 10);

                if (!pantryDayDates.Contains(appointmentDate.Date) && appointmentDate <= endDate)
                {
                    dates.Add(appointmentDate);
                }
            }
        }

        return dates;
    }

    private static string GenerateOverrideNotes(Random rng)
    {
        var notes = new[]
        {
            "Family emergency",
            "Medical need",
            "Lost food due to power outage",
            "Temporary housing situation",
            "Unusual circumstances",
            "Special request approved",
            "One-time exception"
        };

        return notes[rng.Next(notes.Length)];
    }

    private static string GenerateScheduledNotes(Random rng)
    {
        var notes = new[]
        {
            "Follow-up appointment",
            "Special request",
            "Preferred time slot",
            "Transportation arranged",
            "First-time appointment",
            "Regular monthly appointment"
        };

        return notes[rng.Next(notes.Length)];
    }
}
