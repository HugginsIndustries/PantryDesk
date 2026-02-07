using PantryDeskCore.Models;

namespace PantryDeskSeeder;

/// <summary>
/// Generates realistic household data with guardrails.
/// </summary>
public static class HouseholdGenerator
{
    /// <summary>
    /// Generates households according to the configuration.
    /// </summary>
    public static List<Household> GenerateHouseholds(SeederConfig config, Random rng, DateTime baseDate)
    {
        var households = new List<Household>();
        var startDate = baseDate.AddMonths(-config.MonthsBack);

        // Generate household sizes based on distribution
        var householdSizes = GenerateHouseholdSizes(config, rng);

        foreach (var size in householdSizes)
        {
            var household = GenerateSingleHousehold(config, rng, baseDate, startDate, size);
            households.Add(household);
        }

        return households;
    }

    private static List<int> GenerateHouseholdSizes(SeederConfig config, Random rng)
    {
        var sizes = new List<int>();
        var totalRequested = config.HouseholdsCount;

        // If no distribution specified, use default distribution
        if (config.HouseholdSizeDistribution.Count == 0)
        {
            // Default: mostly 1-4 person households
            config.HouseholdSizeDistribution = new Dictionary<int, int>
            {
                { 1, 20 },
                { 2, 30 },
                { 3, 25 },
                { 4, 15 },
                { 5, 7 },
                { 6, 3 }
            };
        }

        // Generate sizes based on weighted distribution
        var totalWeight = config.HouseholdSizeDistribution.Values.Sum();
        var generated = 0;

        while (generated < totalRequested)
        {
            var size = DataGenerators.SelectWeightedRandom(config.HouseholdSizeDistribution, rng);
            sizes.Add(size);
            generated++;
        }

        return sizes;
    }

    private static Household GenerateSingleHousehold(
        SeederConfig config,
        Random rng,
        DateTime baseDate,
        DateTime startDate,
        int totalSize)
    {
        // Select city based on weights
        var city = DataGenerators.SelectWeightedRandom(config.CityWeights, rng);

        // Generate name
        var primaryName = DataGenerators.GenerateName(rng);

        // Generate address
        var (address1, state, zip) = DataGenerators.GenerateAddress(city, rng);

        // Generate phone
        var phone = DataGenerators.GeneratePhone(rng);

        // Generate optional email
        var email = DataGenerators.GenerateEmail(primaryName, rng);

        // Generate household composition (Children, Adults, Seniors)
        // Guardrail: No child-only households (at least 1 adult or senior)
        var composition = GenerateComposition(config, rng, totalSize);

        // Ensure at least one adult or senior
        if (composition.Children == totalSize)
        {
            // Convert one child to adult
            composition.Children = Math.Max(0, totalSize - 1);
            composition.Adults = 1;
        }

        // Verify total matches
        var actualTotal = composition.Children + composition.Adults + composition.Seniors;
        if (actualTotal != totalSize)
        {
            // Adjust adults to match total
            composition.Adults = totalSize - composition.Children - composition.Seniors;
            if (composition.Adults < 0)
            {
                composition.Adults = 0;
                composition.Seniors = totalSize - composition.Children;
            }
        }

        // Random creation date within the range
        var daysRange = (baseDate - startDate).Days;
        var randomDays = rng.Next(daysRange);
        var createdAt = startDate.AddDays(randomDays);
        var updatedAt = createdAt.AddDays(rng.Next(0, (baseDate - createdAt).Days + 1));

        // IsActive is system-managed; sync will derive from last service date
        var isActive = true;

        return new Household
        {
            PrimaryName = primaryName,
            Address1 = address1,
            City = city,
            State = state,
            Zip = zip,
            Phone = phone,
            Email = email,
            ChildrenCount = composition.Children,
            AdultsCount = composition.Adults,
            SeniorsCount = composition.Seniors,
            Notes = rng.NextDouble() < 0.1 ? GenerateNotes(rng) : null, // 10% have notes
            IsActive = isActive,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    private static (int Children, int Adults, int Seniors) GenerateComposition(
        SeederConfig config,
        Random rng,
        int totalSize)
    {
        if (totalSize == 1)
        {
            // Single person: must be adult or senior
            if (rng.NextDouble() < 0.7)
            {
                return (0, 1, 0); // 70% adult
            }
            else
            {
                return (0, 0, 1); // 30% senior
            }
        }

        // For larger households, use weighted selection
        var children = 0;
        var adults = 0;
        var seniors = 0;

        // Distribute totalSize across the three categories using weights
        var totalWeight = config.AgeWeights.Values.Sum();
        var remaining = totalSize;

        // Allocate based on weights, but ensure at least one adult or senior
        var childWeight = config.AgeWeights.GetValueOrDefault("Child", 0);
        var adultWeight = config.AgeWeights.GetValueOrDefault("Adult", 0);
        var seniorWeight = config.AgeWeights.GetValueOrDefault("Senior", 0);

        // Simple proportional allocation
        children = (int)Math.Round(totalSize * childWeight / (double)totalWeight);
        adults = (int)Math.Round(totalSize * adultWeight / (double)totalWeight);
        seniors = totalSize - children - adults;

        // Ensure at least one adult or senior
        if (adults == 0 && seniors == 0)
        {
            if (children > 0)
            {
                children--;
                adults = 1;
            }
        }

        // Ensure we don't exceed totalSize
        var actualTotal = children + adults + seniors;
        if (actualTotal != totalSize)
        {
            var diff = totalSize - actualTotal;
            adults += diff; // Adjust adults to match
            if (adults < 0)
            {
                seniors += adults;
                adults = 0;
            }
        }

        return (children, adults, seniors);
    }

    private static string GenerateNotes(Random rng)
    {
        var notes = new[]
        {
            "Prefers morning appointments",
            "Has transportation",
            "Needs assistance with heavy items",
            "Special dietary restrictions",
            "Works during pantry hours",
            "Has medical appointment conflicts",
            "Prefers specific pantry day",
            "Has language preference",
            "Needs home delivery",
            "Has mobility concerns"
        };

        return notes[rng.Next(notes.Length)];
    }
}
