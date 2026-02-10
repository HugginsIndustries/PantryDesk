using System.Text;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;
using PantryDeskCore.Models;

namespace PantryDeskCore.Services;

/// <summary>
/// Service for exporting database data to CSV and JSON formats.
/// </summary>
public static class ExportService
{
    /// <summary>
    /// Exports households, service events, and pantry days to CSV files.
    /// </summary>
    /// <param name="outputFolder">The folder where CSV files will be created.</param>
    /// <param name="baseFileName">Base name for the CSV files (without extension).</param>
    /// <returns>Array of created file paths.</returns>
    public static string[] ExportToCsv(string outputFolder, string baseFileName)
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        using var connection = DatabaseManager.GetConnection();
        var files = new List<string>();

        // Export households
        var households = HouseholdRepository.GetAll(connection);
        var householdsPath = Path.Combine(outputFolder, $"{baseFileName}_households.csv");
        WriteHouseholdsCsv(households, householdsPath);
        files.Add(householdsPath);

        // Export service events
        var serviceEvents = ServiceEventRepository.GetAll(connection);
        var serviceEventsPath = Path.Combine(outputFolder, $"{baseFileName}_service_events.csv");
        WriteServiceEventsCsv(serviceEvents, serviceEventsPath);
        files.Add(serviceEventsPath);

        // Export pantry days
        var pantryDays = PantryDayRepository.GetAll(connection);
        var pantryDaysPath = Path.Combine(outputFolder, $"{baseFileName}_pantry_days.csv");
        WritePantryDaysCsv(pantryDays, pantryDaysPath);
        files.Add(pantryDaysPath);

        // Export household members
        var members = HouseholdMemberRepository.GetAll(connection);
        var membersPath = Path.Combine(outputFolder, $"{baseFileName}_household_members.csv");
        WriteHouseholdMembersCsv(members, membersPath);
        files.Add(membersPath);

        // Export deck stats monthly
        var deckStats = DeckStatsRepository.GetAll(connection);
        var deckStatsPath = Path.Combine(outputFolder, $"{baseFileName}_deck_stats.csv");
        WriteDeckStatsCsv(deckStats, deckStatsPath);
        files.Add(deckStatsPath);

        return files.ToArray();
    }

    /// <summary>
    /// Exports all data to a structured JSON file.
    /// </summary>
    /// <param name="outputPath">The full path where the JSON file will be created.</param>
    public static void ExportToJson(string outputPath)
    {
        var outputDir = Path.GetDirectoryName(outputPath);
        if (outputDir != null && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        using var connection = DatabaseManager.GetConnection();

        var exportData = new ExportData
        {
            ExportDate = DateTime.UtcNow,
            Households = HouseholdRepository.GetAll(connection),
            HouseholdMembers = HouseholdMemberRepository.GetAll(connection),
            ServiceEvents = ServiceEventRepository.GetAll(connection),
            PantryDays = PantryDayRepository.GetAll(connection),
            DeckStatsMonthly = DeckStatsRepository.GetAll(connection)
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(exportData, options);
        File.WriteAllText(outputPath, json, Encoding.UTF8);
    }

    private static void WriteHouseholdsCsv(List<Household> households, string filePath)
    {
        using var writer = new StreamWriter(filePath, false, new UTF8Encoding(true)); // UTF-8 with BOM

        // Write header
        writer.WriteLine("Id,PrimaryName,Address1,City,State,Zip,Phone,Email,ChildrenCount,AdultsCount,SeniorsCount,Notes,IsActive,CreatedAt,UpdatedAt");

        // Write data rows
        foreach (var household in households)
        {
            var row = new List<string>
            {
                EscapeCsvField(household.Id.ToString()),
                EscapeCsvField(household.PrimaryName),
                EscapeCsvField(household.Address1),
                EscapeCsvField(household.City),
                EscapeCsvField(household.State),
                EscapeCsvField(household.Zip),
                EscapeCsvField(household.Phone),
                EscapeCsvField(household.Email),
                EscapeCsvField(household.ChildrenCount.ToString()),
                EscapeCsvField(household.AdultsCount.ToString()),
                EscapeCsvField(household.SeniorsCount.ToString()),
                EscapeCsvField(household.Notes),
                EscapeCsvField(household.IsActive ? "1" : "0"),
                EscapeCsvField(household.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")),
                EscapeCsvField(household.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            };
            writer.WriteLine(string.Join(",", row));
        }
    }

    private static void WriteServiceEventsCsv(List<ServiceEvent> serviceEvents, string filePath)
    {
        using var writer = new StreamWriter(filePath, false, new UTF8Encoding(true)); // UTF-8 with BOM

        // Write header
        writer.WriteLine("Id,HouseholdId,EventType,EventStatus,EventDate,ScheduledText,OverrideReason,Notes,VisitType,CreatedAt");

        // Write data rows
        foreach (var evt in serviceEvents)
        {
            var row = new List<string>
            {
                EscapeCsvField(evt.Id.ToString()),
                EscapeCsvField(evt.HouseholdId.ToString()),
                EscapeCsvField(evt.EventType),
                EscapeCsvField(evt.EventStatus),
                EscapeCsvField(evt.EventDate.ToString("yyyy-MM-dd")),
                EscapeCsvField(evt.ScheduledText),
                EscapeCsvField(evt.OverrideReason),
                EscapeCsvField(evt.Notes),
                EscapeCsvField(evt.VisitType),
                EscapeCsvField(evt.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            };
            writer.WriteLine(string.Join(",", row));
        }
    }

    private static void WriteHouseholdMembersCsv(List<HouseholdMember> members, string filePath)
    {
        using var writer = new StreamWriter(filePath, false, new UTF8Encoding(true));

        writer.WriteLine("Id,HouseholdId,FirstName,LastName,Birthday,IsPrimary,Race,VeteranStatus,DisabledStatus");

        foreach (var m in members)
        {
            var row = new List<string>
            {
                EscapeCsvField(m.Id.ToString()),
                EscapeCsvField(m.HouseholdId.ToString()),
                EscapeCsvField(m.FirstName),
                EscapeCsvField(m.LastName),
                EscapeCsvField(m.Birthday.ToString("yyyy-MM-dd")),
                EscapeCsvField(m.IsPrimary ? "1" : "0"),
                EscapeCsvField(m.Race),
                EscapeCsvField(m.VeteranStatus),
                EscapeCsvField(m.DisabledStatus)
            };
            writer.WriteLine(string.Join(",", row));
        }
    }

    private static void WritePantryDaysCsv(List<PantryDay> pantryDays, string filePath)
    {
        using var writer = new StreamWriter(filePath, false, new UTF8Encoding(true)); // UTF-8 with BOM

        // Write header
        writer.WriteLine("Id,PantryDate,IsActive,Notes,CreatedAt");

        // Write data rows
        foreach (var pantryDay in pantryDays)
        {
            var row = new List<string>
            {
                EscapeCsvField(pantryDay.Id.ToString()),
                EscapeCsvField(pantryDay.PantryDate.ToString("yyyy-MM-dd")),
                EscapeCsvField(pantryDay.IsActive ? "1" : "0"),
                EscapeCsvField(pantryDay.Notes),
                EscapeCsvField(pantryDay.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            };
            writer.WriteLine(string.Join(",", row));
        }
    }

    private static void WriteDeckStatsCsv(List<DeckStatsMonthly> deckStats, string filePath)
    {
        using var writer = new StreamWriter(filePath, false, new UTF8Encoding(true)); // UTF-8 with BOM

        writer.WriteLine("Year,Month,HouseholdTotalAvg,InfantAvg,ChildAvg,AdultAvg,SeniorAvg,PageCount,UpdatedAt");

        foreach (var d in deckStats)
        {
            var row = new List<string>
            {
                EscapeCsvField(d.Year.ToString()),
                EscapeCsvField(d.Month.ToString()),
                EscapeCsvField(d.HouseholdTotalAvg.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                EscapeCsvField(d.InfantAvg.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                EscapeCsvField(d.ChildAvg.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                EscapeCsvField(d.AdultAvg.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                EscapeCsvField(d.SeniorAvg.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                EscapeCsvField(d.PageCount?.ToString() ?? ""),
                EscapeCsvField(d.UpdatedAt)
            };
            writer.WriteLine(string.Join(",", row));
        }
    }

    private static string EscapeCsvField(string? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        // If field contains comma, quote, or newline, wrap in quotes and escape quotes
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        return value;
    }

    /// <summary>
    /// Data structure for JSON export.
    /// </summary>
    public class ExportData
    {
        public DateTime ExportDate { get; set; }
        public List<Household> Households { get; set; } = new();
        public List<HouseholdMember> HouseholdMembers { get; set; } = new();
        public List<ServiceEvent> ServiceEvents { get; set; } = new();
        public List<PantryDay> PantryDays { get; set; } = new();
        public List<DeckStatsMonthly> DeckStatsMonthly { get; set; } = new();
    }
}
