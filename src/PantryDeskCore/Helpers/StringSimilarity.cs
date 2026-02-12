namespace PantryDeskCore.Helpers;

/// <summary>
/// Used for duplicate detection only: fuzzy name matching (e.g. typos) via Levenshtein distance.
/// </summary>
public static class StringSimilarity
{
    /// <summary>
    /// Maximum Levenshtein distance allowed per name part to consider two names a possible match.
    /// Tuned for typical typos (e.g. Jhon/John, Smith/Smyth).
    /// </summary>
    private const int MaxDistance = 2;

    /// <summary>
    /// Returns true if the two name pairs are an exact match (case-insensitive, trimmed)
    /// or a fuzzy match (Levenshtein distance ≤ MaxDistance for both first and last).
    /// Used only for member duplicate detection; no PII is logged.
    /// </summary>
    public static bool NamesMatchFuzzy(string first1, string last1, string first2, string last2)
    {
        var f1 = (first1 ?? string.Empty).Trim();
        var l1 = (last1 ?? string.Empty).Trim();
        var f2 = (first2 ?? string.Empty).Trim();
        var l2 = (last2 ?? string.Empty).Trim();

        if (string.Equals(f1, f2, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(l1, l2, StringComparison.OrdinalIgnoreCase))
            return true;

        return LevenshteinDistance(f1, f2) <= MaxDistance && LevenshteinDistance(l1, l2) <= MaxDistance;
    }

    /// <summary>
    /// Levenshtein distance between two strings (number of single-character edits).
    /// </summary>
    public static int LevenshteinDistance(string a, string b)
    {
        if (a == null) a = string.Empty;
        if (b == null) b = string.Empty;

        var m = a.Length;
        var n = b.Length;
        if (m == 0) return n;
        if (n == 0) return m;

        var d = new int[m + 1, n + 1];
        for (var i = 0; i <= m; i++) d[i, 0] = i;
        for (var j = 0; j <= n; j++) d[0, j] = j;

        for (var i = 1; i <= m; i++)
        {
            for (var j = 1; j <= n; j++)
            {
                var cost = char.ToLowerInvariant(a[i - 1]) == char.ToLowerInvariant(b[j - 1]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        }

        return d[m, n];
    }
}
