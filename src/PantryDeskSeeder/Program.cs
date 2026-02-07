using PantryDeskSeeder;

try
{
    // Parse command-line arguments
    var config = SeederConfig.LoadFromArgs(args);

    var seedText = config.RngSeed.HasValue ? config.RngSeed.Value.ToString() : "random";
    Console.WriteLine($"Generating database with {config.HouseholdsCount} households, {config.MonthsBack} months back, output: {config.OutputPath}, seed: {seedText}");

    // Seed the database
    DatabaseSeeder.SeedDatabase(config.OutputPath, config);

    return 0;
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Console.Error.WriteLine("Use --help for usage information.");
    return 1;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
    Console.Error.WriteLine(ex.StackTrace);
    return 1;
}
