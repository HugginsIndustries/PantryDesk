using System.Data;
using Microsoft.Data.Sqlite;
using PantryDeskCore.Helpers;
using PantryDeskCore.Models;

namespace PantryDeskCore.Data;

/// <summary>
/// Repository for household member data access operations.
/// </summary>
public static class HouseholdMemberRepository
{
    /// <summary>
    /// Creates a new household member record.
    /// </summary>
    public static HouseholdMember Create(SqliteConnection connection, HouseholdMember member)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberInsert, connection);
            cmd.Parameters.AddWithValue("@household_id", member.HouseholdId);
            cmd.Parameters.AddWithValue("@first_name", member.FirstName);
            cmd.Parameters.AddWithValue("@last_name", member.LastName);
            cmd.Parameters.AddWithValue("@birthday", member.Birthday.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@is_primary", member.IsPrimary ? 1 : 0);
            cmd.Parameters.AddWithValue("@race", (object?)member.Race ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@veteran_status", (object?)member.VeteranStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@disabled_status", (object?)member.DisabledStatus ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            using var idCmd = new SqliteCommand("SELECT last_insert_rowid()", connection);
            var id = (long)idCmd.ExecuteScalar()!;
            member.Id = (int)id;

            return member;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Updates an existing household member.
    /// </summary>
    public static void Update(SqliteConnection connection, HouseholdMember member)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberUpdate, connection);
            cmd.Parameters.AddWithValue("@id", member.Id);
            cmd.Parameters.AddWithValue("@first_name", member.FirstName);
            cmd.Parameters.AddWithValue("@last_name", member.LastName);
            cmd.Parameters.AddWithValue("@birthday", member.Birthday.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@is_primary", member.IsPrimary ? 1 : 0);
            cmd.Parameters.AddWithValue("@race", (object?)member.Race ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@veteran_status", (object?)member.VeteranStatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@disabled_status", (object?)member.DisabledStatus ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Deletes a household member by ID.
    /// </summary>
    public static void Delete(SqliteConnection connection, int id)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberDelete, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Deletes all members for a household.
    /// </summary>
    public static void DeleteByHouseholdId(SqliteConnection connection, int householdId)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberDeleteByHouseholdId, connection);
            cmd.Parameters.AddWithValue("@household_id", householdId);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets a member by ID.
    /// </summary>
    public static HouseholdMember? GetById(SqliteConnection connection, int id)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberSelectById, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapFromReader(reader);
            }

            return null;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets all members for a household, ordered by primary first, then last name, first name.
    /// </summary>
    public static List<HouseholdMember> GetByHouseholdId(SqliteConnection connection, int householdId)
    {
        var members = new List<HouseholdMember>();
        var wasOpen = connection.State == ConnectionState.Open;
        if (!wasOpen)
            connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberSelectByHouseholdId, connection);
            cmd.Parameters.AddWithValue("@household_id", householdId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                members.Add(MapFromReader(reader));
            }
        }
        finally
        {
            if (!wasOpen)
                connection.Close();
        }

        return members;
    }

    /// <summary>
    /// Gets all household members across all households.
    /// </summary>
    public static List<HouseholdMember> GetAll(SqliteConnection connection)
    {
        var members = new List<HouseholdMember>();

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberSelectAll, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                members.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return members;
    }

    /// <summary>
    /// Searches members by name (first, last, or full). Returns only members whose names match.
    /// Each result includes the member and their household for display.
    /// </summary>
    public static List<MemberSearchResult> SearchMembersByName(SqliteConnection connection, string searchTerm)
    {
        var results = new List<MemberSearchResult>();
        if (string.IsNullOrWhiteSpace(searchTerm))
            return results;

        var likePattern = $"%{searchTerm.Trim()}%";

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberSearchByName, connection);
            cmd.Parameters.AddWithValue("@search_term", likePattern);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var member = new HouseholdMember
                {
                    Id = reader.GetInt32(0),
                    HouseholdId = reader.GetInt32(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    Birthday = DateTime.Parse(reader.GetString(4)),
                    IsPrimary = reader.GetInt32(5) != 0,
                    Race = reader.IsDBNull(6) ? null : reader.GetString(6),
                    VeteranStatus = reader.IsDBNull(7) ? null : reader.GetString(7),
                    DisabledStatus = reader.IsDBNull(8) ? null : reader.GetString(8)
                };
                var household = new Household
                {
                    Id = reader.GetInt32(9),
                    PrimaryName = reader.GetString(10),
                    Address1 = reader.IsDBNull(11) ? null : reader.GetString(11),
                    City = reader.IsDBNull(12) ? null : reader.GetString(12),
                    State = reader.IsDBNull(13) ? null : reader.GetString(13),
                    Zip = reader.IsDBNull(14) ? null : reader.GetString(14),
                    Phone = reader.IsDBNull(15) ? null : reader.GetString(15),
                    Email = reader.IsDBNull(16) ? null : reader.GetString(16),
                    ChildrenCount = reader.GetInt32(17),
                    AdultsCount = reader.GetInt32(18),
                    SeniorsCount = reader.GetInt32(19),
                    Notes = reader.IsDBNull(20) ? null : reader.GetString(20),
                    IsActive = reader.GetInt32(21) != 0,
                    CreatedAt = DateTime.Parse(reader.GetString(22)),
                    UpdatedAt = DateTime.Parse(reader.GetString(23))
                };
                results.Add(new MemberSearchResult
                {
                    Member = member,
                    Household = household,
                    PrimaryName = household.PrimaryName
                });
            }
        }
        finally
        {
            connection.Close();
        }

        return results;
    }

    /// <summary>
    /// Returns true if any candidate (first name, last name, birthday) matches an existing member
    /// in the database (same birthday and exact or fuzzy name match). Used for New Household
    /// duplicate warning only; does not consider household id (candidates are not yet saved).
    /// All SQL is parameterized; no PII is logged.
    /// </summary>
    /// <param name="connection">The database connection (caller manages open/close).</param>
    /// <param name="candidates">Candidate members to check: FirstName, LastName, Birthday.</param>
    /// <returns>True if at least one candidate has a possible duplicate in the DB.</returns>
    public static bool FindPotentialDuplicateMembers(
        SqliteConnection connection,
        IEnumerable<(string FirstName, string LastName, DateTime Birthday)> candidates)
    {
        var list = candidates.ToList();
        if (list.Count == 0)
            return false;

        var existing = new List<HouseholdMember>();
        var wasOpen = connection.State == ConnectionState.Open;
        if (!wasOpen)
            connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberSelectAll, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                existing.Add(MapFromReader(reader));
            }
        }
        finally
        {
            if (!wasOpen)
                connection.Close();
        }

        foreach (var (firstName, lastName, birthday) in list)
        {
            var candDate = birthday.Date;
            foreach (var m in existing)
            {
                if (m.Birthday.Date != candDate)
                    continue;
                if (StringSimilarity.NamesMatchFuzzy(firstName, lastName, m.FirstName, m.LastName))
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the primary member for a household.
    /// </summary>
    public static HouseholdMember? GetPrimaryByHouseholdId(SqliteConnection connection, int householdId)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdMemberSelectPrimaryByHouseholdId, connection);
            cmd.Parameters.AddWithValue("@household_id", householdId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapFromReader(reader);
            }

            return null;
        }
        finally
        {
            connection.Close();
        }
    }

    private static HouseholdMember MapFromReader(SqliteDataReader reader)
    {
        return new HouseholdMember
        {
            Id = reader.GetInt32(0),
            HouseholdId = reader.GetInt32(1),
            FirstName = reader.GetString(2),
            LastName = reader.GetString(3),
            Birthday = DateTime.Parse(reader.GetString(4)),
            IsPrimary = reader.GetInt32(5) != 0,
            Race = reader.IsDBNull(6) ? null : reader.GetString(6),
            VeteranStatus = reader.IsDBNull(7) ? null : reader.GetString(7),
            DisabledStatus = reader.IsDBNull(8) ? null : reader.GetString(8)
        };
    }
}
