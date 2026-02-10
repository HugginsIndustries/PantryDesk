namespace PantryDeskCore.Models;

/// <summary>
/// Result of member-centric search: a member with their household context.
/// </summary>
public class MemberSearchResult
{
    public HouseholdMember Member { get; set; } = null!;
    public Household Household { get; set; } = null!;
    public string PrimaryName { get; set; } = string.Empty;
}
