using System.Data;
using Microsoft.Data.Sqlite;
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
