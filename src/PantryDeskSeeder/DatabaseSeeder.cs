using System.Text.Json;
using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;
using PantryDeskCore.Models;

namespace PantryDeskSeeder;

/// <summary>
/// Main orchestrator for seeding the database.
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds the database with demo data according to the configuration.
    /// </summary>
    public static void SeedDatabase(string dbPath, SeederConfig config)
    {
        // Delete existing database if it exists
        if (File.Exists(dbPath))
        {
            File.Delete(dbPath);
            Console.WriteLine($"Deleted existing database: {dbPath}");
        }

        // Create database connection
        var connectionString = $"Data Source={dbPath}";
        using var connection = new SqliteConnection(connectionString);

        // Initialize database (create schema)
        Console.WriteLine("Initializing database schema...");
        DatabaseMigrator.MigrateToLatest(connection);

        // Initialize RNG
        var rng = new Random(config.RngSeed ?? Environment.TickCount);
        var baseDate = DateTime.Today;
        var currentYear = baseDate.Year;

        // Generate pantry days for current year and past months
        Console.WriteLine("Generating pantry days...");
        var pantryDays = PantryDayGenerator.GeneratePantryDays(currentYear, rng);
        
        // Also generate for previous year if needed
        if (config.MonthsBack > baseDate.Month)
        {
            var previousYearPantryDays = PantryDayGenerator.GeneratePantryDays(currentYear - 1, rng);
            pantryDays.AddRange(previousYearPantryDays);
        }

        // Filter pantry days to only include those within the date range
        var startDate = baseDate.AddMonths(-config.MonthsBack);
        var pantryDaysInRange = pantryDays
            .Where(pd => pd.PantryDate >= startDate && pd.PantryDate <= baseDate)
            .ToList();

        // Insert pantry days (only those in range)
        foreach (var pantryDay in pantryDaysInRange)
        {
            PantryDayRepository.Create(connection, pantryDay);
        }
        Console.WriteLine($"  Created {pantryDaysInRange.Count} pantry days");

        // Generate households
        Console.WriteLine($"Generating {config.HouseholdsCount} households...");
        var households = HouseholdGenerator.GenerateHouseholds(config, rng, baseDate);

        // Insert households
        foreach (var household in households)
        {
            HouseholdRepository.Create(connection, household);
        }
        Console.WriteLine($"  Created {households.Count} households");

        // Generate service events
        Console.WriteLine("Generating service events...");
        var serviceEvents = ServiceEventGenerator.GenerateServiceEvents(
            households,
            pantryDaysInRange,
            config,
            rng,
            baseDate);

        // Insert service events
        foreach (var serviceEvent in serviceEvents)
        {
            ServiceEventRepository.Create(connection, serviceEvent);
        }
        Console.WriteLine($"  Created {serviceEvents.Count} service events");

        // Seed auth roles
        Console.WriteLine("Seeding auth roles...");
        AuthRoleSeeder.SeedAuthRoles(connection);

        // Store seeder config metadata
        Console.WriteLine("Storing seeder configuration...");
        var configJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        ConfigRepository.SetValue(connection, "seeder_config", configJson);
        ConfigRepository.SetValue(connection, "seeder_generated_at", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        ConfigRepository.SetValue(connection, "seeder_version", "1.0");

        Console.WriteLine();
        Console.WriteLine("Database seeding completed successfully!");
        Console.WriteLine($"Database file: {Path.GetFullPath(dbPath)}");
        Console.WriteLine();
        Console.WriteLine("Summary:");
        Console.WriteLine($"  Households: {households.Count}");
        Console.WriteLine($"  Pantry Days: {pantryDaysInRange.Count}");
        Console.WriteLine($"  Service Events: {serviceEvents.Count}");
        Console.WriteLine($"    PantryDay Events: {serviceEvents.Count(e => e.EventType == "PantryDay")}");
        Console.WriteLine($"    Appointment Events: {serviceEvents.Count(e => e.EventType == "Appointment")}");
        Console.WriteLine($"  Completed Events: {serviceEvents.Count(e => e.EventStatus == "Completed")}");
        Console.WriteLine($"  Scheduled Events: {serviceEvents.Count(e => e.EventStatus == "Scheduled")}");
        Console.WriteLine($"  Events with Overrides: {serviceEvents.Count(e => !string.IsNullOrEmpty(e.OverrideReason))}");
    }
}
