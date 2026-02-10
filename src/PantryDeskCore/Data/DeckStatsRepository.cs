using Microsoft.Data.Sqlite;
using PantryDeskCore.Models;

namespace PantryDeskCore.Data;

/// <summary>
/// Repository for deck_stats_monthly (one row per year/month).
/// </summary>
public static class DeckStatsRepository
{
    /// <summary>
    /// Gets deck stats for a given year and month, or null if none.
    /// </summary>
    public static DeckStatsMonthly? Get(SqliteConnection connection, int year, int month)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.DeckStatsMonthlySelectByYearMonth, connection);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);

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
    /// Returns true if a record exists for the given year and month.
    /// </summary>
    public static bool Exists(SqliteConnection connection, int year, int month)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.DeckStatsMonthlyExists, connection);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@month", month);
            return cmd.ExecuteScalar() != null;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Inserts or updates the deck stats for the given year/month.
    /// </summary>
    public static void Upsert(SqliteConnection connection, DeckStatsMonthly entity)
    {
        var updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.DeckStatsMonthlyUpsert, connection);
            cmd.Parameters.AddWithValue("@year", entity.Year);
            cmd.Parameters.AddWithValue("@month", entity.Month);
            cmd.Parameters.AddWithValue("@household_total_avg", entity.HouseholdTotalAvg);
            cmd.Parameters.AddWithValue("@infant_avg", entity.InfantAvg);
            cmd.Parameters.AddWithValue("@child_avg", entity.ChildAvg);
            cmd.Parameters.AddWithValue("@adult_avg", entity.AdultAvg);
            cmd.Parameters.AddWithValue("@senior_avg", entity.SeniorAvg);
            cmd.Parameters.AddWithValue("@page_count", (object?)entity.PageCount ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets all deck stats rows, ordered by year desc, month desc.
    /// </summary>
    public static List<DeckStatsMonthly> GetAll(SqliteConnection connection)
    {
        var list = new List<DeckStatsMonthly>();
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.DeckStatsMonthlySelectAll, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }
        return list;
    }

    private static DeckStatsMonthly MapFromReader(SqliteDataReader reader)
    {
        return new DeckStatsMonthly
        {
            Year = reader.GetInt32(0),
            Month = reader.GetInt32(1),
            HouseholdTotalAvg = reader.GetDouble(2),
            InfantAvg = reader.GetDouble(3),
            ChildAvg = reader.GetDouble(4),
            AdultAvg = reader.GetDouble(5),
            SeniorAvg = reader.GetDouble(6),
            PageCount = reader.IsDBNull(7) ? null : reader.GetInt32(7),
            UpdatedAt = reader.GetString(8)
        };
    }
}
