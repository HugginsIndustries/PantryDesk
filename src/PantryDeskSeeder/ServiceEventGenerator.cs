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

    // Visit types for completed events; mostly Shop with TEFAP, some Shop, occasional TEFAP Only/Deck Only
    private static readonly string[] VisitTypes = new[] { "Shop with TEFAP", "Shop", "TEFAP Only", "Deck Only" };

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
        List<(Household Household, List<HouseholdMember> Members)> householdData,
        List<PantryDay> pantryDays,
        SeederConfig config,
        Random rng,
        DateTime baseDate)
    {
        var events = new List<ServiceEvent>();
        var startDate = baseDate.AddMonths(-config.MonthsBack);
        var allHouseholds = householdData.Select(x => x.Household).ToList();

        if (allHouseholds.Count == 0)
        {
            return events;
        }

        // Generate completed PantryDay events
        var pantryDaysInRange = pantryDays
            .Where(pd => pd.PantryDate >= startDate && pd.PantryDate <= baseDate && pd.IsActive)
            .ToList();

        // Track households served per month to prevent duplicates
        var householdsServedPerMonth = new Dictionary<(int Year, int Month), HashSet<int>>();

        foreach (var pantryDay in pantryDaysInRange)
        {
            var eventCount = rng.Next(config.EventsPerPantryDayRange.Min, config.EventsPerPantryDayRange.Max + 1);
            eventCount = Math.Min(eventCount, allHouseholds.Count);

            var monthKey = (pantryDay.PantryDate.Year, pantryDay.PantryDate.Month);
            if (!householdsServedPerMonth.ContainsKey(monthKey))
            {
                householdsServedPerMonth[monthKey] = new HashSet<int>();
            }
            var householdsServedThisMonth = householdsServedPerMonth[monthKey];

            // NEVER allow a household to visit more than one pantry day per month
            // Only select from households not yet served this month
            var availableHouseholds = allHouseholds
                .Where(h => !householdsServedThisMonth.Contains(h.Id))
                .OrderBy(_ => rng.Next())
                .Take(eventCount)
                .ToList();

            foreach (var household in availableHouseholds)
            {
                // Pantry day events never have overrides (households can only visit once per month)
                events.Add(new ServiceEvent
                {
                    HouseholdId = household.Id,
                    EventType = "PantryDay",
                    EventStatus = "Completed",
                    EventDate = pantryDay.PantryDate,
                    VisitType = PickVisitType(rng),
                    OverrideReason = null,
                    Notes = null,
                    CreatedAt = pantryDay.PantryDate.AddHours(10 + rng.Next(8)) // Random time during pantry hours
                });

                // Track that this household was served this month
                householdsServedThisMonth.Add(household.Id);
            }
        }

        // Generate completed Appointment events (not on pantry days)
        var pantryDayDates = pantryDaysInRange.Select(pd => pd.PantryDate.Date).ToHashSet();
        var appointmentDates = GenerateAppointmentDates(startDate, baseDate, pantryDayDates, config, rng);

        // Calculate target number of override appointments (1-2% of total appointments)
        var totalAppointmentSlots = appointmentDates.Count;
        var overrideAppointmentCount = Math.Max(1, (int)(totalAppointmentSlots * (0.01 + rng.NextDouble() * 0.01))); // 1-2% range
        var overrideAppointmentsCreated = 0;

        foreach (var appointmentDate in appointmentDates)
        {
            var monthKey = (appointmentDate.Year, appointmentDate.Month);
            if (!householdsServedPerMonth.ContainsKey(monthKey))
            {
                householdsServedPerMonth[monthKey] = new HashSet<int>();
            }
            var householdsServedThisMonth = householdsServedPerMonth[monthKey];

            Household household;
            bool isOverride = false;
            
            // 1-2% of appointments can be overrides (households already served this month)
            if (overrideAppointmentsCreated < overrideAppointmentCount && 
                householdsServedThisMonth.Count > 0 && 
                rng.NextDouble() < (overrideAppointmentCount / (double)Math.Max(1, totalAppointmentSlots - overrideAppointmentsCreated)))
            {
                // Create an override appointment (household already served this month)
                var duplicateHouseholds = allHouseholds
                    .Where(h => householdsServedThisMonth.Contains(h.Id))
                    .ToList();
                if (duplicateHouseholds.Count > 0)
                {
                    household = duplicateHouseholds[rng.Next(duplicateHouseholds.Count)];
                    isOverride = true;
                    overrideAppointmentsCreated++;
                }
                else
                {
                    // Fallback if no duplicates available
                    var availableHouseholds = allHouseholds
                        .Where(h => !householdsServedThisMonth.Contains(h.Id))
                        .ToList();
                    household = availableHouseholds.Count > 0 
                        ? availableHouseholds[rng.Next(availableHouseholds.Count)]
                        : allHouseholds[rng.Next(allHouseholds.Count)];
                }
            }
            else
            {
                // Normal appointment (household not yet served this month)
                var availableHouseholds = allHouseholds
                    .Where(h => !householdsServedThisMonth.Contains(h.Id))
                    .ToList();
                household = availableHouseholds.Count > 0
                    ? availableHouseholds[rng.Next(availableHouseholds.Count)]
                    : allHouseholds[rng.Next(allHouseholds.Count)];
            }

            var scheduledForMemberId = PickScheduledForMember(household.Id, householdData, rng);
            var statusRoll = rng.NextDouble();
            var eventStatus = statusRoll < 0.92 ? "Completed" : statusRoll < 0.96 ? "Cancelled" : "NoShow";

            // Override reason only valid for Completed appointments that exceed monthly limit
            string? overrideReason = null;
            string? notes = null;
            if (isOverride && eventStatus == "Completed")
            {
                overrideReason = OverrideReasons[rng.Next(OverrideReasons.Length)];
                notes = rng.NextDouble() < 0.5 ? GenerateOverrideNotes(rng) : null;
            }

            events.Add(new ServiceEvent
            {
                HouseholdId = household.Id,
                EventType = "Appointment",
                EventStatus = eventStatus,
                EventDate = appointmentDate,
                VisitType = eventStatus == "Completed" ? PickVisitType(rng) : null,
                ScheduledText = ScheduledTexts[rng.Next(ScheduledTexts.Length)],
                OverrideReason = overrideReason,
                Notes = notes,
                ScheduledForMemberId = scheduledForMemberId,
                CreatedAt = appointmentDate.AddHours(9 + rng.Next(8))
            });

            // Track that this household was served this month only for Completed (matches IsEligibleThisMonth)
            if (eventStatus == "Completed" && !householdsServedThisMonth.Contains(household.Id))
            {
                householdsServedThisMonth.Add(household.Id);
            }
        }

        // Generate scheduled Appointment events (future)
        var futureStart = baseDate.AddDays(1);
        var futureEnd = baseDate.AddDays(28); // Next 4 weeks
        var scheduledCount = rng.Next(5, 20); // 5-20 scheduled appointments

        for (int i = 0; i < scheduledCount; i++)
        {
            var daysAhead = rng.Next((futureEnd - futureStart).Days + 1);
            var eventDate = futureStart.AddDays(daysAhead);

            if (pantryDayDates.Contains(eventDate.Date))
                continue;

            var household = allHouseholds[rng.Next(allHouseholds.Count)];
            var scheduledText = ScheduledTexts[rng.Next(ScheduledTexts.Length)];
            var scheduledForMemberId = PickScheduledForMember(household.Id, householdData, rng);
            var isCancelled = rng.NextDouble() < 0.05;

            events.Add(new ServiceEvent
            {
                HouseholdId = household.Id,
                EventType = "Appointment",
                EventStatus = isCancelled ? "Cancelled" : "Scheduled",
                EventDate = eventDate,
                VisitType = null,
                ScheduledText = scheduledText,
                Notes = rng.NextDouble() < 0.3 ? GenerateScheduledNotes(rng) : null,
                ScheduledForMemberId = scheduledForMemberId,
                CreatedAt = baseDate.AddHours(-rng.Next(168)) // Created within last week
            });
        }


        return events;
    }

    /// <summary>
    /// For ~30-40% of appointments, returns a non-primary member ID. Otherwise returns null (primary).
    /// </summary>
    private static int? PickScheduledForMember(
        int householdId,
        List<(Household Household, List<HouseholdMember> Members)> householdData,
        Random rng)
    {
        var data = householdData.FirstOrDefault(x => x.Household.Id == householdId);
        if (data.Members == null || data.Members.Count == 0)
            return null;

        // 30-40% use a non-primary member for testing
        if (rng.NextDouble() >= 0.35)
            return null;

        var nonPrimary = data.Members.Where(m => !m.IsPrimary).ToList();
        if (nonPrimary.Count == 0)
            return null;

        var member = nonPrimary[rng.Next(nonPrimary.Count)];
        return member.Id;
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

    private static string PickVisitType(Random rng)
    {
        // Mostly Shop with TEFAP, some Shop, occasional TEFAP Only or Deck Only
        var roll = rng.NextDouble();
        if (roll < 0.70) return "Shop with TEFAP";
        if (roll < 0.95) return "Shop";
        if (roll < 0.98) return "TEFAP Only";
        return "Deck Only";
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
