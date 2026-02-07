namespace PantryDeskCore.Helpers;

/// <summary>
/// Derives age group from birthday.
/// Age groups: Infant (0-2), Child (2-18), Adult (18-55), Senior (55+).
/// Uses half-open intervals: [0,2) Infant, [2,18) Child, [18,55) Adult, [55,∞) Senior.
/// </summary>
public static class AgeGroupHelper
{
    /// <summary>
    /// Gets the age group for a person based on their birthday.
    /// </summary>
    /// <param name="birthday">The member's birthday.</param>
    /// <param name="asOf">Reference date for age calculation. Defaults to today if null.</param>
    /// <returns>One of: "Infant", "Child", "Adult", "Senior".</returns>
    public static string GetAgeGroup(DateTime birthday, DateTime? asOf = null)
    {
        var reference = asOf ?? DateTime.Today;
        var ageYears = CalculateAgeInYears(birthday, reference);

        if (ageYears < 2)
            return "Infant";
        if (ageYears < 18)
            return "Child";
        if (ageYears < 55)
            return "Adult";
        return "Senior";
    }

    /// <summary>
    /// Calculates age in full years as of the reference date.
    /// </summary>
    public static int CalculateAgeInYears(DateTime birthday, DateTime reference)
    {
        var age = reference.Year - birthday.Year;
        if (reference.Month < birthday.Month || (reference.Month == birthday.Month && reference.Day < birthday.Day))
        {
            age--;
        }
        return Math.Max(0, age);
    }
}
