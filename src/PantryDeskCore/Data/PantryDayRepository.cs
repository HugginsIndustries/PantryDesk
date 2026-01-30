using Microsoft.Data.Sqlite;
using PantryDeskCore.Models;

namespace PantryDeskCore.Data;

/// <summary>
/// Repository for pantry day data access operations.
/// </summary>
public static class PantryDayRepository
{
    /// <summary>
    /// Creates a new pantry day.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="pantryDay">The pantry day to create.</param>
    /// <returns>The created pantry day with the generated ID.</returns>
    public static PantryDay Create(SqliteConnection connection, PantryDay pantryDay)
    {
        var now = DateTime.UtcNow;
        var createdAt = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var pantryDate = pantryDay.PantryDate.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.PantryDayInsert, connection);
            cmd.Parameters.AddWithValue("@pantry_date", pantryDate);
            cmd.Parameters.AddWithValue("@is_active", pantryDay.IsActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@notes", (object?)pantryDay.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@created_at", createdAt);

            cmd.ExecuteNonQuery();

            // Get the generated ID
            using var idCmd = new SqliteCommand("SELECT last_insert_rowid()", connection);
            var id = (long)idCmd.ExecuteScalar()!;
            pantryDay.Id = (int)id;
            pantryDay.CreatedAt = now;

            return pantryDay;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets a pantry day by ID.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="id">The pantry day ID.</param>
    /// <returns>The pantry day, or null if not found.</returns>
    public static PantryDay? GetById(SqliteConnection connection, int id)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.PantryDaySelectById, connection);
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
    /// Gets a pantry day by date.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="date">The pantry date.</param>
    /// <returns>The pantry day, or null if not found.</returns>
    public static PantryDay? GetByDate(SqliteConnection connection, DateTime date)
    {
        var dateStr = date.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.PantryDaySelectByDate, connection);
            cmd.Parameters.AddWithValue("@pantry_date", dateStr);

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
    /// Gets all pantry days.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <returns>A list of all pantry days.</returns>
    public static List<PantryDay> GetAll(SqliteConnection connection)
    {
        var pantryDays = new List<PantryDay>();

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.PantryDaySelectAll, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                pantryDays.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return pantryDays;
    }

    /// <summary>
    /// Updates an existing pantry day.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="pantryDay">The pantry day to update.</param>
    public static void Update(SqliteConnection connection, PantryDay pantryDay)
    {
        var pantryDate = pantryDay.PantryDate.ToString("yyyy-MM-dd");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.PantryDayUpdate, connection);
            cmd.Parameters.AddWithValue("@id", pantryDay.Id);
            cmd.Parameters.AddWithValue("@pantry_date", pantryDate);
            cmd.Parameters.AddWithValue("@is_active", pantryDay.IsActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@notes", (object?)pantryDay.Notes ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Deletes a pantry day by ID.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="id">The pantry day ID.</param>
    public static void Delete(SqliteConnection connection, int id)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.PantryDayDelete, connection);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    private static PantryDay MapFromReader(SqliteDataReader reader)
    {
        return new PantryDay
        {
            Id = reader.GetInt32(0),
            PantryDate = DateTime.Parse(reader.GetString(1)),
            IsActive = reader.GetInt32(2) != 0,
            Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
            CreatedAt = DateTime.Parse(reader.GetString(4))
        };
    }
}
