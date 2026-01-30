namespace PantryDeskSeeder;

/// <summary>
/// Configuration for the database seeder tool.
/// </summary>
public class SeederConfig
{
    public int HouseholdsCount { get; set; } = 300;
    public int MonthsBack { get; set; } = 6;
    public Dictionary<string, int> CityWeights { get; set; } = new();
    public Dictionary<string, int> AgeWeights { get; set; } = new();
    public Dictionary<int, int> HouseholdSizeDistribution { get; set; } = new();
    public (int Min, int Max) EventsPerPantryDayRange { get; set; } = (25, 50);
    public (int Min, int Max) AppointmentsPerWeekRange { get; set; } = (2, 8);
    public int? RngSeed { get; set; }
    public string OutputPath { get; set; } = "demo_pantrydesk.db";

    /// <summary>
    /// Gets default configuration with sensible defaults.
    /// </summary>
    public static SeederConfig GetDefault()
    {
        return new SeederConfig
        {
            HouseholdsCount = 300,
            MonthsBack = 6,
            CityWeights = new Dictionary<string, int>
            {
                { "Winlock", 50 },
                { "Vader", 30 },
                { "Ryderwood", 20 }
            },
            AgeWeights = new Dictionary<string, int>
            {
                { "Child", 30 },
                { "Adult", 50 },
                { "Senior", 20 }
            },
            HouseholdSizeDistribution = new Dictionary<int, int>
            {
                { 1, 20 },
                { 2, 30 },
                { 3, 25 },
                { 4, 15 },
                { 5, 7 },
                { 6, 3 }
            },
            EventsPerPantryDayRange = (25, 50),
            AppointmentsPerWeekRange = (2, 8),
            RngSeed = null,
            OutputPath = "demo_pantrydesk.db"
        };
    }

    /// <summary>
    /// Loads configuration from command-line arguments.
    /// </summary>
    public static SeederConfig LoadFromArgs(string[] args)
    {
        var config = GetDefault();

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            switch (arg)
            {
                case "--households" when i + 1 < args.Length:
                    if (int.TryParse(args[i + 1], out var households) && households > 0)
                    {
                        config.HouseholdsCount = households;
                        i++;
                    }
                    break;

                case "--months-back" when i + 1 < args.Length:
                    if (int.TryParse(args[i + 1], out var monthsBack) && monthsBack > 0)
                    {
                        config.MonthsBack = monthsBack;
                        i++;
                    }
                    break;

                case "--seed" when i + 1 < args.Length:
                    if (int.TryParse(args[i + 1], out var seed))
                    {
                        config.RngSeed = seed;
                        i++;
                    }
                    break;

                case "--output" when i + 1 < args.Length:
                    config.OutputPath = args[i + 1];
                    i++;
                    break;

                case "--help":
                    PrintUsage();
                    Environment.Exit(0);
                    break;
            }
        }

        ValidateConfig(config);
        return config;
    }

    private static void ValidateConfig(SeederConfig config)
    {
        if (config.HouseholdsCount <= 0)
        {
            throw new ArgumentException("Households count must be greater than 0");
        }

        if (config.MonthsBack <= 0)
        {
            throw new ArgumentException("Months back must be greater than 0");
        }

        if (config.CityWeights.Count == 0 || config.CityWeights.Values.Sum() <= 0)
        {
            throw new ArgumentException("City weights must have at least one positive weight");
        }

        if (config.AgeWeights.Count == 0 || config.AgeWeights.Values.Sum() <= 0)
        {
            throw new ArgumentException("Age weights must have at least one positive weight");
        }

        if (config.EventsPerPantryDayRange.Min < 0 || config.EventsPerPantryDayRange.Max < config.EventsPerPantryDayRange.Min)
        {
            throw new ArgumentException("Events per pantry day range is invalid");
        }

        if (config.AppointmentsPerWeekRange.Min < 0 || config.AppointmentsPerWeekRange.Max < config.AppointmentsPerWeekRange.Min)
        {
            throw new ArgumentException("Appointments per week range is invalid");
        }
    }

    private static void PrintUsage()
    {
        Console.WriteLine("PantryDesk Seeder Tool");
        Console.WriteLine();
        Console.WriteLine("Usage: PantryDeskSeeder [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --households <count>     Number of households to generate (default: 300)");
        Console.WriteLine("  --months-back <months>   How many months back to generate data (default: 6)");
        Console.WriteLine("  --seed <number>         RNG seed for deterministic generation (default: random)");
        Console.WriteLine("  --output <path>          Output database path (default: demo_pantrydesk.db)");
        Console.WriteLine("  --help                   Show this help message");
    }
}
