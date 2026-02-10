namespace PantryDeskSeeder;

/// <summary>
/// Configuration for the database seeder tool.
/// </summary>
public class SeederConfig
{
    public int HouseholdsCount { get; set; } = 500;
    public int MonthsBack { get; set; } = 24;
    public Dictionary<string, int> CityWeights { get; set; } = new();
    public Dictionary<string, int> AgeWeights { get; set; } = new();
    public Dictionary<string, int> AgeGroupWeights { get; set; } = new();
    public Dictionary<string, int> RaceWeights { get; set; } = new();
    public Dictionary<string, int> VeteranWeights { get; set; } = new();
    public Dictionary<string, int> DisabledWeights { get; set; } = new();
    public Dictionary<int, int> HouseholdSizeDistribution { get; set; } = new();
    public (int Min, int Max) EventsPerPantryDayRange { get; set; } = (25, 50);
    public (int Min, int Max) AppointmentsPerWeekRange { get; set; } = (2, 8);
    public int? RngSeed { get; set; }
    public string OutputPath { get; set; } = "demo_pantrydesk.db";

    private static readonly HashSet<string> OptionsRequiringValue = new(StringComparer.Ordinal)
    {
        "--households", "--months-back", "--seed", "--output",
        "--city-weights", "--age-weights", "--household-size-dist",
        "--events-per-pantry-day", "--appointments-per-week"
    };

    /// <summary>
    /// Gets default configuration with sensible defaults.
    /// </summary>
    public static SeederConfig GetDefault()
    {
        return new SeederConfig
        {
            HouseholdsCount = 500,
            MonthsBack = 24,
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
            AgeGroupWeights = new Dictionary<string, int>
            {
                { "Infant", 2 },
                { "Child", 19 },
                { "Adult", 45 },
                { "Senior", 34 }
            },
            RaceWeights = new Dictionary<string, int>
            {
                { "White", 81 },
                { "Hispanic", 12 },
                { "Native American", 2 },
                { "Black", 1 },
                { "Not Specified", 4 }
            },
            VeteranWeights = new Dictionary<string, int>
            {
                { "Not Veteran", 865 },
                { "Veteran", 110 },
                { "Not Specified", 25 }
            },
            DisabledWeights = new Dictionary<string, int>
            {
                { "Not Disabled", 82 },
                { "Disabled", 14 },
                { "Not Specified", 4 }
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

                case "--city-weights" when i + 1 < args.Length:
                    config.CityWeights = ParseKeyValuePairs(args[i + 1]);
                    i++;
                    break;

                case "--age-weights" when i + 1 < args.Length:
                    config.AgeWeights = ParseKeyValuePairs(args[i + 1]);
                    i++;
                    break;

                case "--household-size-dist" when i + 1 < args.Length:
                    config.HouseholdSizeDistribution = ParseIntKeyValuePairs(args[i + 1]);
                    i++;
                    break;

                case "--events-per-pantry-day" when i + 1 < args.Length:
                    config.EventsPerPantryDayRange = ParseRange(args[i + 1]);
                    i++;
                    break;

                case "--appointments-per-week" when i + 1 < args.Length:
                    config.AppointmentsPerWeekRange = ParseRange(args[i + 1]);
                    i++;
                    break;

                case "--help":
                    PrintUsage();
                    Environment.Exit(0);
                    break;

                default:
                    if (arg.StartsWith("--"))
                    {
                        if (OptionsRequiringValue.Contains(arg))
                        {
                            throw new ArgumentException($"Missing value for {arg}. Use --help for usage information.");
                        }
                        throw new ArgumentException($"Unknown argument: {arg}. Use --help for usage information.");
                    }
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

    private static Dictionary<string, int> ParseKeyValuePairs(string input)
    {
        var dict = new Dictionary<string, int>();
        var pairs = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2, StringSplitOptions.TrimEntries);
            if (parts.Length == 2 && int.TryParse(parts[1], out var value) && value > 0)
            {
                dict[parts[0]] = value;
            }
            else
            {
                throw new ArgumentException($"Invalid key=value pair: {pair}");
            }
        }
        return dict;
    }

    private static Dictionary<int, int> ParseIntKeyValuePairs(string input)
    {
        var dict = new Dictionary<int, int>();
        var pairs = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2, StringSplitOptions.TrimEntries);
            if (parts.Length == 2 && int.TryParse(parts[0], out var key) && int.TryParse(parts[1], out var value) && value > 0)
            {
                dict[key] = value;
            }
            else
            {
                throw new ArgumentException($"Invalid key=value pair: {pair}");
            }
        }
        return dict;
    }

    private static (int Min, int Max) ParseRange(string input)
    {
        var parts = input.Split('-', StringSplitOptions.TrimEntries);
        if (parts.Length == 2 && int.TryParse(parts[0], out var min) && int.TryParse(parts[1], out var max))
        {
            if (min < 0 || max < min)
            {
                throw new ArgumentException($"Invalid range: {input}. Min must be >= 0 and max must be >= min.");
            }
            return (min, max);
        }
        throw new ArgumentException($"Invalid range format: {input}. Expected format: min-max (e.g., 25-50)");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("PantryDesk Seeder Tool");
        Console.WriteLine();
        Console.WriteLine("Usage: PantryDeskSeeder [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --households <count>              Number of households to generate (default: 500)");
        Console.WriteLine("  --months-back <months>            How many months back to generate data (default: 24)");
        Console.WriteLine("  --seed <number>                   RNG seed for deterministic generation (default: random)");
        Console.WriteLine("  --output <path>                   Output database path (default: demo_pantrydesk.db)");
        Console.WriteLine("  --city-weights <pairs>            City weights as key=value pairs (default: Winlock=50,Vader=30,Ryderwood=20)");
        Console.WriteLine("                                    Example: --city-weights \"Winlock=50,Vader=30,Ryderwood=20\"");
        Console.WriteLine("  --age-weights <pairs>             Age weights as key=value pairs (default: Child=30,Adult=50,Senior=20)");
        Console.WriteLine("                                    Example: --age-weights \"Child=30,Adult=50,Senior=20\"");
        Console.WriteLine("  --household-size-dist <pairs>    Household size distribution as key=value pairs");
        Console.WriteLine("                                    Example: --household-size-dist \"1=20,2=30,3=25,4=15,5=7,6=3\"");
        Console.WriteLine("  --events-per-pantry-day <range>   Range of events per pantry day (default: 25-50)");
        Console.WriteLine("                                    Example: --events-per-pantry-day \"25-50\"");
        Console.WriteLine("  --appointments-per-week <range>   Range of appointments per week (default: 2-8)");
        Console.WriteLine("                                    Example: --appointments-per-week \"2-8\"");
        Console.WriteLine("  --help                            Show this help message");
    }
}
