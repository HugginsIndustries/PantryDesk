namespace PantryDeskSeeder;

/// <summary>
/// Helper methods for generating realistic demo data.
/// </summary>
public static class DataGenerators
{
    private static readonly string[] FirstNames = new[]
    {
        "James", "Mary", "John", "Patricia", "Robert", "Jennifer", "Michael", "Linda",
        "William", "Elizabeth", "David", "Barbara", "Richard", "Susan", "Joseph", "Jessica",
        "Thomas", "Sarah", "Charles", "Karen", "Christopher", "Nancy", "Daniel", "Lisa",
        "Matthew", "Betty", "Anthony", "Margaret", "Mark", "Sandra", "Donald", "Ashley",
        "Steven", "Kimberly", "Paul", "Emily", "Andrew", "Donna", "Joshua", "Michelle",
        "Kenneth", "Carol", "Kevin", "Dorothy", "Brian", "Amanda", "George", "Melissa",
        "Timothy", "Deborah", "Ronald", "Stephanie", "Jason", "Rebecca", "Edward", "Sharon",
        "Jeffrey", "Laura", "Ryan", "Cynthia", "Jacob", "Kathleen", "Gary", "Amy",
        "Nicholas", "Angela", "Eric", "Shirley", "Jonathan", "Anna", "Stephen", "Brenda",
        "Larry", "Pamela", "Justin", "Emma", "Scott", "Nicole", "Brandon", "Helen",
        "Benjamin", "Samantha", "Samuel", "Katherine", "Frank", "Christine", "Gregory", "Debra",
        "Raymond", "Rachel", "Alexander", "Carolyn", "Patrick", "Janet", "Jack", "Virginia",
        "Dennis", "Maria", "Jerry", "Heather", "Tyler", "Diane", "Aaron", "Julie",
        "Jose", "Joyce", "Adam", "Victoria", "Henry", "Kelly", "Nathan", "Christina",
        "Douglas", "Joan", "Zachary", "Evelyn", "Kyle", "Lauren", "Noah", "Judith",
        "Ethan", "Megan", "Jeremy", "Cheryl", "Walter", "Andrea", "Christian", "Hannah",
        "Keith", "Jacqueline", "Roger", "Martha", "Terry", "Gloria", "Gerald", "Teresa",
        "Harold", "Sara", "Sean", "Janice", "Austin", "Marie", "Carl", "Julia",
        "Arthur", "Grace", "Lawrence", "Judy", "Dylan", "Theresa", "Jesse", "Madison",
        "Jordan", "Beverly", "Bryan", "Denise", "Billy", "Marilyn", "Joe", "Amber",
        "Bruce", "Danielle", "Gabriel", "Rose", "Logan", "Brittany", "Alan", "Diana",
        "Juan", "Abigail", "Wayne", "Jane", "Roy", "Lori", "Ralph", "Olivia", "Randy", "Janet"
    };

    private static readonly string[] LastNames = new[]
    {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
        "Rodriguez", "Martinez", "Hernandez", "Lopez", "Wilson", "Anderson", "Thomas", "Taylor",
        "Moore", "Jackson", "Martin", "Lee", "Thompson", "White", "Harris", "Sanchez",
        "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young", "Allen", "King",
        "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green", "Adams",
        "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts",
        "Gomez", "Phillips", "Evans", "Turner", "Diaz", "Parker", "Cruz", "Edwards",
        "Collins", "Reyes", "Stewart", "Morris", "Morales", "Murphy", "Cook", "Rogers",
        "Gutierrez", "Ortiz", "Morgan", "Cooper", "Peterson", "Bailey", "Reed", "Kelly",
        "Howard", "Ramos", "Kim", "Cox", "Ward", "Richardson", "Watson", "Brooks",
        "Chavez", "Wood", "James", "Bennett", "Gray", "Mendoza", "Ruiz", "Hughes",
        "Price", "Alvarez", "Castillo", "Sanders", "Patel", "Myers", "Long", "Ross",
        "Foster", "Jimenez", "Powell", "Jenkins", "Perry", "Russell", "Sullivan", "Bell",
        "Coleman", "Butler", "Henderson", "Simmons", "Foster", "Gonzales", "Bryant", "Alexander",
        "Russell", "Griffin", "Diaz", "Hayes", "Myers", "Ford", "Hamilton", "Graham",
        "Sullivan", "Wallace", "Woods", "Cole", "West", "Jordan", "Owens", "Reynolds",
        "Fisher", "Ellis", "Harrison", "Gibson", "Mcdonald", "Cruz", "Marshall", "Ortiz",
        "Gomez", "Murray", "Freeman", "Wells", "Webb", "Simpson", "Stevens", "Tucker",
        "Porter", "Hunter", "Hicks", "Crawford", "Henry", "Boyd", "Mason", "Morales",
        "Kennedy", "Warren", "Dixon", "Ramos", "Reyes", "Burns", "Gordon", "Shaw",
        "Holmes", "Rice", "Robertson", "Hunt", "Black", "Daniels", "Palmer", "Mills",
        "Nichols", "Grant", "Knight", "Ferguson", "Rose", "Stone", "Hawkins", "Dunn",
        "Perkins", "Hudson", "Spencer", "Gardner", "Stephens", "Payne", "Pierce", "Berry",
        "Matthews", "Arnold", "Wagner", "Willis", "Ray", "Watkins", "Olson", "Carroll",
        "Duncan", "Snyder", "Hart", "Cunningham", "Bradley", "Lane", "Andrews", "Ruiz",
        "Harper", "Fox", "Riley", "Armstrong", "Carpenter", "Weaver", "Greene", "Lawrence",
        "Elliott", "Chavez", "Sims", "Austin", "Peters", "Kelley", "Franklin", "Lawson",
        "Fields", "Gutierrez", "Ryan", "Schmidt", "Carr", "Vasquez", "Castillo", "Wheeler",
        "Chapman", "Oliver", "Montgomery", "Richards", "Williamson", "Johnston", "Banks", "Meyer",
        "Bishop", "Mccoy", "Howell", "Alvarez", "Morrison", "Hansen", "Fernandez", "Garza",
        "Harvey", "Little", "Burton", "Stanley", "Nguyen", "George", "Jacobs", "Reid",
        "Kim", "Fuller", "Lynch", "Dean", "Gilbert", "Garrett", "Romero", "Welch",
        "Larson", "Frazier", "Burke", "Hanson", "Day", "Mendoza", "Moreno", "Bowman",
        "Medina", "Fowler", "Brewer", "Hoffman", "Carlson", "Silva", "Pearson", "Holland",
        "Douglas", "Fleming", "Jensen", "Vargas", "Byrd", "Davidson", "Hopkins", "May",
        "Terry", "Herrera", "Wade", "Soto", "Walters", "Curtis", "Neal", "Caldwell",
        "Lowe", "Jennings", "Barnett", "Gates", "Casey", "Vega", "Guzman", "Hendricks",
        "Pena", "Hubbard", "Valdez", "Preston", "Bush", "Cortez", "Booker", "Pruitt",
        "Cain", "Holt", "Morton", "Schroeder", "Lambert", "Bush", "Combs", "Brock",
        "Ortega", "Kline", "Mullins", "Barrera", "Vazquez", "Robbins", "Newton", "Todd",
        "Potter", "Hampton", "Orr", "Strickland", "Farmer", "Bates", "Hendrix", "Benson",
        "Marsh", "Mckinney", "Mann", "Zimmerman", "Dawson", "Lara", "Fletcher", "Page",
        "Mccarthy", "Love", "Robles", "Cain", "Rivas", "Khan", "Rogers", "Serrano",
        "Higgins", "Ingram", "Harmon", "Hess", "Coffey", "Hess", "Monroe", "Marsh",
        "Mckinney", "Mann", "Zimmerman", "Dawson", "Lara", "Fletcher", "Page", "Mccarthy", "Huggins"
    };

    private static readonly string[] StreetNames = new[]
    {
        "Main", "Oak", "Elm", "Park", "Maple", "Cedar", "Pine", "Washington",
        "Lincoln", "Jefferson", "Madison", "Adams", "Jackson", "Monroe", "Harrison", "Tyler",
        "Polk", "Taylor", "Fillmore", "Pierce", "Buchanan", "Johnson", "Grant", "Hayes",
        "Garfield", "Arthur", "Cleveland", "McKinley", "Roosevelt", "Taft", "Wilson", "Harding",
        "Coolidge", "Hoover", "Truman", "Eisenhower", "Kennedy", "Johnson", "Nixon", "Ford",
        "Carter", "Reagan", "Bush", "Clinton", "Bush", "Obama", "Trump", "Biden",
        "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth",
        "Ninth", "Tenth", "Broadway", "Church", "Center", "High", "Market", "State",
        "Union", "Liberty", "Freedom", "Independence", "Victory", "Peace", "Harmony", "Valley",
        "Hill", "Ridge", "View", "Heights", "Terrace", "Lane", "Drive", "Avenue",
        "Boulevard", "Court", "Place", "Circle", "Way", "Street", "Road", "Trail"
    };

    private static readonly string[] StreetSuffixes = new[]
    {
        "Street", "Avenue", "Road", "Drive", "Lane", "Court", "Place", "Circle",
        "Way", "Boulevard", "Terrace", "Trail", "Parkway", "Highway", "Path", "Row"
    };

    /// <summary>
    /// Generates a random full name.
    /// </summary>
    public static string GenerateName(Random rng)
    {
        return $"{GenerateFirstName(rng)} {GenerateLastName(rng)}";
    }

    /// <summary>
    /// Generates a random first name.
    /// </summary>
    public static string GenerateFirstName(Random rng)
    {
        return FirstNames[rng.Next(FirstNames.Length)];
    }

    /// <summary>
    /// Generates a random last name.
    /// </summary>
    public static string GenerateLastName(Random rng)
    {
        return LastNames[rng.Next(LastNames.Length)];
    }

    /// <summary>
    /// Generates a birthday within the specified age group, as of the reference date.
    /// Age groups: Infant (0-2), Child (2-18), Adult (18-55), Senior (55+).
    /// </summary>
    public static DateTime GenerateBirthdayFromAgeGroup(string ageGroup, DateTime asOfDate, Random rng)
    {
        return ageGroup switch
        {
            "Infant" => asOfDate.AddYears(-2).AddDays(rng.Next(1, 730)),
            "Child" => asOfDate.AddYears(-18).AddDays(rng.Next(1, 365 * 16)),
            "Adult" => asOfDate.AddYears(-55).AddDays(rng.Next(1, 365 * 37)),
            "Senior" => asOfDate.AddYears(-100).AddDays(rng.Next(1, 365 * 45)),
            _ => asOfDate.AddYears(-30)
        };
    }

    /// <summary>
    /// Generates a random address for the specified city.
    /// </summary>
    public static (string Address1, string State, string Zip) GenerateAddress(string city, Random rng)
    {
        var streetNumber = rng.Next(1, 9999);
        var streetName = StreetNames[rng.Next(StreetNames.Length)];
        var streetSuffix = StreetSuffixes[rng.Next(StreetSuffixes.Length)];
        var address1 = $"{streetNumber} {streetName} {streetSuffix}";

        var (state, zip) = city switch
        {
            "Winlock" => ("WA", "98596"),
            "Vader" => ("WA", "98593"),
            "Ryderwood" => ("WA", "98581"),
            _ => ("WA", "98596") // Default to Winlock
        };

        return (address1, state, zip);
    }

    /// <summary>
    /// Generates a phone number in the format 360-555-XXXX.
    /// </summary>
    public static string GeneratePhone(Random rng)
    {
        var lastFour = rng.Next(1000, 10000).ToString("D4");
        return $"360-555-{lastFour}";
    }

    /// <summary>
    /// Generates an optional email address from a name.
    /// Returns null about 30% of the time to simulate missing emails.
    /// </summary>
    public static string? GenerateEmail(string name, Random rng)
    {
        if (rng.NextDouble() < 0.3)
        {
            return null; // 30% chance of no email
        }

        var cleanName = name.ToLowerInvariant().Replace(" ", ".");
        var domains = new[] { "email.com", "mail.com", "example.com", "test.com" };
        var domain = domains[rng.Next(domains.Length)];
        return $"{cleanName}@{domain}";
    }

    /// <summary>
    /// Selects a random item from a weighted dictionary.
    /// </summary>
    public static T SelectWeightedRandom<T>(Dictionary<T, int> weights, Random rng) where T : notnull
    {
        if (weights.Count == 0)
        {
            throw new ArgumentException("Weights dictionary cannot be empty", nameof(weights));
        }

        var totalWeight = weights.Values.Sum();
        if (totalWeight <= 0)
        {
            throw new ArgumentException("Total weight must be greater than 0", nameof(weights));
        }

        var randomValue = rng.Next(totalWeight);
        var currentWeight = 0;

        foreach (var kvp in weights)
        {
            currentWeight += kvp.Value;
            if (randomValue < currentWeight)
            {
                return kvp.Key;
            }
        }

        // Fallback to last item (shouldn't happen, but safety)
        return weights.Keys.Last();
    }
}
