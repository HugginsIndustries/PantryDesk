using PantryDeskCore.Models;

namespace PantryDeskSeeder;

/// <summary>
/// Generates realistic household data with guardrails.
/// </summary>
public static class HouseholdGenerator
{
    /// <summary>
    /// Generates households with members according to the configuration.
    /// </summary>
    public static List<(Household Household, List<HouseholdMember> Members)> GenerateHouseholds(SeederConfig config, Random rng, DateTime baseDate)
    {
        var result = new List<(Household, List<HouseholdMember>)>();
        var startDate = baseDate.AddMonths(-config.MonthsBack);
        var householdSizes = GenerateHouseholdSizes(config, rng);

        foreach (var size in householdSizes)
        {
            var (household, members) = GenerateSingleHousehold(config, rng, baseDate, startDate, size);
            result.Add((household, members));
        }

        return result;
    }

    private static List<int> GenerateHouseholdSizes(SeederConfig config, Random rng)
    {
        var sizes = new List<int>();
        var totalRequested = config.HouseholdsCount;

        if (config.HouseholdSizeDistribution.Count == 0)
        {
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

    private static (Household, List<HouseholdMember>) GenerateSingleHousehold(
        SeederConfig config,
        Random rng,
        DateTime baseDate,
        DateTime startDate,
        int totalSize)
    {
        var city = DataGenerators.SelectWeightedRandom(config.CityWeights, rng);
        var (address1, state, zip) = DataGenerators.GenerateAddress(city, rng);
        var phone = DataGenerators.GeneratePhone(rng);

        var ageGroupWeights = config.AgeGroupWeights.Count > 0
            ? config.AgeGroupWeights
            : new Dictionary<string, int> { { "Infant", 2 }, { "Child", 19 }, { "Adult", 45 }, { "Senior", 34 } };

        var members = new List<HouseholdMember>();
        var primaryName = "";

        for (var i = 0; i < totalSize; i++)
        {
            var ageGroup = DataGenerators.SelectWeightedRandom(ageGroupWeights, rng);
            if (i == 0 && (ageGroup == "Infant" || ageGroup == "Child"))
            {
                ageGroup = rng.NextDouble() < 0.7 ? "Adult" : "Senior";
            }

            var firstName = DataGenerators.GenerateFirstName(rng);
            var lastName = DataGenerators.GenerateLastName(rng);
            var birthday = DataGenerators.GenerateBirthdayFromAgeGroup(ageGroup, baseDate, rng);

            var race = SelectOptional(config.RaceWeights, rng, "White", "Black", "Hispanic", "Native American", "Not Specified");
            var veteranStatus = SelectOptional(config.VeteranWeights, rng, "Veteran", "Not Veteran", "Not Specified");
            var disabledStatus = SelectOptional(config.DisabledWeights, rng, "Not Disabled", "Disabled", "Not Specified");

            var member = new HouseholdMember
            {
                FirstName = firstName,
                LastName = lastName,
                Birthday = birthday,
                IsPrimary = i == 0,
                Race = race,
                VeteranStatus = veteranStatus,
                DisabledStatus = disabledStatus
            };
            members.Add(member);
            if (i == 0)
                primaryName = member.FullName;
        }

        var email = DataGenerators.GenerateEmail(primaryName, rng);
        var daysRange = Math.Max(1, (baseDate - startDate).Days);
        var randomDays = rng.Next(daysRange);
        var createdAt = startDate.AddDays(randomDays);
        var updatedAt = createdAt.AddDays(rng.Next(0, Math.Max(1, (baseDate - createdAt).Days) + 1));

        var household = new Household
        {
            PrimaryName = primaryName,
            Address1 = address1,
            City = city,
            State = state,
            Zip = zip,
            Phone = phone,
            Email = email,
            ChildrenCount = 0,
            AdultsCount = 0,
            SeniorsCount = 0,
            Notes = rng.NextDouble() < 0.1 ? GenerateNotes(rng) : null,
            IsActive = true,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        return (household, members);
    }

    private static string? SelectOptional(Dictionary<string, int> weights, Random rng, params string[] options)
    {
        if (weights == null || weights.Count == 0)
            return options.Length > 0 ? options[0] : null;

        var filtered = weights.Where(kv => options.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);
        if (filtered.Count == 0)
            return options[0];

        return DataGenerators.SelectWeightedRandom(filtered, rng);
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
