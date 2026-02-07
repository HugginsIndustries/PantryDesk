using Microsoft.Data.Sqlite;
using PantryDeskCore.Models;

namespace PantryDeskCore.Data;

/// <summary>
/// Repository for household data access operations.
/// </summary>
public static class HouseholdRepository
{
    /// <summary>
    /// Creates a new household record.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="household">The household to create.</param>
    /// <returns>The created household with the generated ID.</returns>
    public static Household Create(SqliteConnection connection, Household household)
    {
        var now = DateTime.UtcNow;
        var createdAt = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var updatedAt = createdAt;

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdInsert, connection);
            cmd.Parameters.AddWithValue("@primary_name", household.PrimaryName);
            cmd.Parameters.AddWithValue("@address1", (object?)household.Address1 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@city", (object?)household.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@state", (object?)household.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@zip", (object?)household.Zip ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)household.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)household.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@children_count", household.ChildrenCount);
            cmd.Parameters.AddWithValue("@adults_count", household.AdultsCount);
            cmd.Parameters.AddWithValue("@seniors_count", household.SeniorsCount);
            cmd.Parameters.AddWithValue("@notes", (object?)household.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@is_active", household.IsActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@created_at", createdAt);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            cmd.ExecuteNonQuery();

            // Get the generated ID
            using var idCmd = new SqliteCommand("SELECT last_insert_rowid()", connection);
            var id = (long)idCmd.ExecuteScalar()!;
            household.Id = (int)id;
            household.CreatedAt = now;
            household.UpdatedAt = now;

            return household;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets a household by ID.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="id">The household ID.</param>
    /// <returns>The household, or null if not found.</returns>
    public static Household? GetById(SqliteConnection connection, int id)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdSelectById, connection);
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
    /// Gets all households.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <returns>A list of all households.</returns>
    public static List<Household> GetAll(SqliteConnection connection)
    {
        var households = new List<Household>();

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdSelectAll, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                households.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return households;
    }

    /// <summary>
    /// Updates an existing household.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="household">The household to update.</param>
    public static void Update(SqliteConnection connection, Household household)
    {
        var updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdUpdate, connection);
            cmd.Parameters.AddWithValue("@id", household.Id);
            cmd.Parameters.AddWithValue("@primary_name", household.PrimaryName);
            cmd.Parameters.AddWithValue("@address1", (object?)household.Address1 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@city", (object?)household.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@state", (object?)household.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@zip", (object?)household.Zip ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", (object?)household.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object?)household.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@children_count", household.ChildrenCount);
            cmd.Parameters.AddWithValue("@adults_count", household.AdultsCount);
            cmd.Parameters.AddWithValue("@seniors_count", household.SeniorsCount);
            cmd.Parameters.AddWithValue("@notes", (object?)household.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@is_active", household.IsActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            cmd.ExecuteNonQuery();

            household.UpdatedAt = DateTime.UtcNow;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Sets IsActive and UpdatedAt for a household by ID. Use for lightweight updates when only status changes.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="householdId">The household ID.</param>
    /// <param name="isActive">The new IsActive value.</param>
    public static void SetIsActive(SqliteConnection connection, int householdId, bool isActive)
    {
        var updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdUpdateIsActive, connection);
            cmd.Parameters.AddWithValue("@id", householdId);
            cmd.Parameters.AddWithValue("@is_active", isActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Soft deletes a household by setting IsActive to false.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="id">The household ID.</param>
    public static void Delete(SqliteConnection connection, int id)
    {
        var updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdSoftDelete, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Searches households by name using partial, case-insensitive matching.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="searchTerm">The search term (will be wrapped with % for LIKE matching).</param>
    /// <returns>A list of matching households ordered by primary_name.</returns>
    public static List<Household> SearchByName(SqliteConnection connection, string searchTerm)
    {
        var households = new List<Household>();

        // If search term is empty, return all households
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return GetAll(connection);
        }

        // Wrap search term with % for partial matching
        var likePattern = $"%{searchTerm.Trim()}%";

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdSearchByName, connection);
            cmd.Parameters.AddWithValue("@search_term", likePattern);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                households.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return households;
    }

    /// <summary>
    /// Finds potential duplicate households based on primary name, with optional city and phone filters.
    /// Intended for use in UI warning flows only; does not enforce uniqueness.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="primaryName">The primary name to check for similarity.</param>
    /// <param name="city">Optional city filter (exact match when provided).</param>
    /// <param name="phone">Optional phone filter (exact match when provided).</param>
    /// <returns>A list of potential duplicate households.</returns>
    public static List<Household> FindPotentialDuplicates(SqliteConnection connection, string primaryName, string? city, string? phone)
    {
        var households = new List<Household>();

        // Wrap primary name with % for partial matching
        var likePattern = $"%{primaryName.Trim()}%";

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.HouseholdFindPotentialDuplicates, connection);
            cmd.Parameters.AddWithValue("@primary_name", likePattern);
            cmd.Parameters.AddWithValue("@city", string.IsNullOrWhiteSpace(city) ? DBNull.Value : city.Trim());
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(phone) ? DBNull.Value : phone.Trim());

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                households.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return households;
    }

    private static Household MapFromReader(SqliteDataReader reader)
    {
        return new Household
        {
            Id = reader.GetInt32(0),
            PrimaryName = reader.GetString(1),
            Address1 = reader.IsDBNull(2) ? null : reader.GetString(2),
            City = reader.IsDBNull(3) ? null : reader.GetString(3),
            State = reader.IsDBNull(4) ? null : reader.GetString(4),
            Zip = reader.IsDBNull(5) ? null : reader.GetString(5),
            Phone = reader.IsDBNull(6) ? null : reader.GetString(6),
            Email = reader.IsDBNull(7) ? null : reader.GetString(7),
            ChildrenCount = reader.GetInt32(8),
            AdultsCount = reader.GetInt32(9),
            SeniorsCount = reader.GetInt32(10),
            Notes = reader.IsDBNull(11) ? null : reader.GetString(11),
            IsActive = reader.GetInt32(12) != 0,
            CreatedAt = DateTime.Parse(reader.GetString(13)),
            UpdatedAt = DateTime.Parse(reader.GetString(14))
        };
    }
}
