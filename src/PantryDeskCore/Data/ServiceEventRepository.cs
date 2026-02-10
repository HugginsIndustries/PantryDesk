using Microsoft.Data.Sqlite;
using PantryDeskCore.Models;

namespace PantryDeskCore.Data;

/// <summary>
/// Repository for service event data access operations.
/// </summary>
public static class ServiceEventRepository
{
    /// <summary>
    /// Creates a new service event.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="serviceEvent">The service event to create.</param>
    /// <returns>The created service event with the generated ID.</returns>
    public static ServiceEvent Create(SqliteConnection connection, ServiceEvent serviceEvent)
    {
        var now = DateTime.UtcNow;
        var createdAt = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var eventDate = serviceEvent.EventDate.ToString("yyyy-MM-dd");

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventInsert, connection);
            cmd.Parameters.AddWithValue("@household_id", serviceEvent.HouseholdId);
            cmd.Parameters.AddWithValue("@event_type", serviceEvent.EventType);
            cmd.Parameters.AddWithValue("@event_status", serviceEvent.EventStatus);
            cmd.Parameters.AddWithValue("@event_date", eventDate);
            cmd.Parameters.AddWithValue("@scheduled_text", (object?)serviceEvent.ScheduledText ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@override_reason", (object?)serviceEvent.OverrideReason ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@notes", (object?)serviceEvent.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@visit_type", (object?)serviceEvent.VisitType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@scheduled_for_member_id", (object?)serviceEvent.ScheduledForMemberId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@created_at", createdAt);

            cmd.ExecuteNonQuery();

            // Get the generated ID
            using var idCmd = new SqliteCommand("SELECT last_insert_rowid()", connection);
            var id = (long)idCmd.ExecuteScalar()!;
            serviceEvent.Id = (int)id;
            serviceEvent.CreatedAt = now;

            return serviceEvent;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets a service event by ID.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="id">The service event ID.</param>
    /// <returns>The service event, or null if not found.</returns>
    public static ServiceEvent? GetById(SqliteConnection connection, int id)
    {
        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectById, connection);
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
    /// Gets all service events for a specific household.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="householdId">The household ID.</param>
    /// <returns>A list of service events for the household.</returns>
    public static List<ServiceEvent> GetByHouseholdId(SqliteConnection connection, int householdId)
    {
        var events = new List<ServiceEvent>();

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectByHouseholdId, connection);
            cmd.Parameters.AddWithValue("@household_id", householdId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                events.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return events;
    }

    /// <summary>
    /// Gets all service events.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <returns>A list of all service events.</returns>
    public static List<ServiceEvent> GetAll(SqliteConnection connection)
    {
        var events = new List<ServiceEvent>();

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectAll, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                events.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return events;
    }

    /// <summary>
    /// Gets all service events within a date range.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="startDate">The start date (inclusive).</param>
    /// <param name="endDate">The end date (inclusive).</param>
    /// <returns>A list of service events within the date range.</returns>
    public static List<ServiceEvent> GetByDateRange(SqliteConnection connection, DateTime startDate, DateTime endDate)
    {
        var events = new List<ServiceEvent>();

        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectByDateRange, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                events.Add(MapFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return events;
    }

    /// <summary>
    /// Checks if a household has any completed service events within a date range.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="householdId">The household ID.</param>
    /// <param name="startDate">The start date (inclusive).</param>
    /// <param name="endDate">The end date (inclusive).</param>
    /// <returns>True if any completed events exist in the range, false otherwise.</returns>
    public static bool HasCompletedInDateRange(SqliteConnection connection, int householdId, DateTime startDate, DateTime endDate)
    {
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectCompletedByHouseholdAndDateRange, connection);
            cmd.Parameters.AddWithValue("@household_id", householdId);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            return reader.Read(); // If any row exists, household has completed service in this range
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets the most recent completed service event for a household.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="householdId">The household ID.</param>
    /// <returns>The most recent completed service event, or null if none exists.</returns>
    public static ServiceEvent? GetLastCompletedByHouseholdId(SqliteConnection connection, int householdId)
    {
        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectLastCompletedByHouseholdId, connection);
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

    /// <summary>
    /// Updates an existing service event.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="serviceEvent">The service event to update.</param>
    public static void Update(SqliteConnection connection, ServiceEvent serviceEvent)
    {
        var eventDate = serviceEvent.EventDate.ToString("yyyy-MM-dd");

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventUpdate, connection);
            cmd.Parameters.AddWithValue("@id", serviceEvent.Id);
            cmd.Parameters.AddWithValue("@event_type", serviceEvent.EventType);
            cmd.Parameters.AddWithValue("@event_status", serviceEvent.EventStatus);
            cmd.Parameters.AddWithValue("@event_date", eventDate);
            cmd.Parameters.AddWithValue("@scheduled_text", (object?)serviceEvent.ScheduledText ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@override_reason", (object?)serviceEvent.OverrideReason ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@notes", (object?)serviceEvent.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@visit_type", (object?)serviceEvent.VisitType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@scheduled_for_member_id", (object?)serviceEvent.ScheduledForMemberId ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Checks if a household has any completed qualifying visits (Shop with TEFAP or Shop) within a date range.
    /// TEFAP Only and Deck Only do not count toward the monthly limit.
    /// </summary>
    public static bool HasCompletedQualifyingVisitInDateRange(SqliteConnection connection, int householdId, DateTime startDate, DateTime endDate)
    {
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectCompletedQualifyingByHouseholdAndDateRange, connection);
            cmd.Parameters.AddWithValue("@household_id", householdId);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Checks if a household has any completed qualifying visits in the date range, excluding a specific event.
    /// Used when editing an existing event to avoid counting it against itself.
    /// </summary>
    public static bool HasCompletedQualifyingVisitInDateRangeExcluding(
        SqliteConnection connection, int householdId, DateTime startDate, DateTime endDate, int excludeEventId)
    {
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectCompletedQualifyingByHouseholdAndDateRangeExcludingEvent, connection);
            cmd.Parameters.AddWithValue("@household_id", householdId);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);
            cmd.Parameters.AddWithValue("@exclude_event_id", excludeEventId);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Gets past appointments (Completed/Cancelled/NoShow) within a date range for the Appointments form.
    /// </summary>
    public static List<AppointmentRow> GetAppointmentsPast(SqliteConnection connection, DateTime startDate, DateTime endDate, string statusFilter)
    {
        var rows = new List<AppointmentRow>();
        var startDateStr = startDate.ToString("yyyy-MM-dd");
        var endDateStr = endDate.ToString("yyyy-MM-dd");

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectAppointmentsPast, connection);
            cmd.Parameters.AddWithValue("@start_date", startDateStr);
            cmd.Parameters.AddWithValue("@end_date", endDateStr);
            cmd.Parameters.AddWithValue("@status_filter", statusFilter ?? "All");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rows.Add(MapAppointmentRowFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return rows;
    }

    /// <summary>
    /// Gets scheduled appointments for the Appointments form Future panel.
    /// </summary>
    public static List<AppointmentRow> GetAppointmentsScheduled(SqliteConnection connection)
    {
        var rows = new List<AppointmentRow>();

        DatabaseManager.OpenWithForeignKeys(connection);
        try
        {
            using var cmd = new SqliteCommand(Sql.ServiceEventSelectAppointmentsScheduled, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rows.Add(MapAppointmentRowFromReader(reader));
            }
        }
        finally
        {
            connection.Close();
        }

        return rows;
    }

    private static AppointmentRow MapAppointmentRowFromReader(SqliteDataReader reader)
    {
        var primaryName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);
        var hasScheduledMember = !reader.IsDBNull(9);
        string displayName;
        if (hasScheduledMember && !reader.IsDBNull(12) && !reader.IsDBNull(13))
        {
            var memberFirstName = reader.GetString(12);
            var memberLastName = reader.GetString(13);
            var memberFullName = $"{memberFirstName} {memberLastName}".Trim();
            var isPrimary = !reader.IsDBNull(14) && reader.GetInt32(14) == 1;
            displayName = isPrimary ? memberFullName : $"{memberFullName} • {primaryName} Household";
        }
        else
        {
            displayName = primaryName;
        }

        return new AppointmentRow
        {
            Id = reader.GetInt32(0),
            HouseholdId = reader.GetInt32(1),
            EventType = reader.GetString(2),
            EventStatus = reader.GetString(3),
            EventDate = DateTime.Parse(reader.GetString(4)),
            ScheduledText = reader.IsDBNull(5) ? null : reader.GetString(5),
            OverrideReason = reader.IsDBNull(6) ? null : reader.GetString(6),
            Notes = reader.IsDBNull(7) ? null : reader.GetString(7),
            VisitType = reader.IsDBNull(8) ? null : reader.GetString(8),
            CreatedAt = DateTime.Parse(reader.GetString(10)),
            PrimaryName = primaryName,
            DisplayName = displayName
        };
    }

    private static ServiceEvent MapFromReader(SqliteDataReader reader)
    {
        return new ServiceEvent
        {
            Id = reader.GetInt32(0),
            HouseholdId = reader.GetInt32(1),
            EventType = reader.GetString(2),
            EventStatus = reader.GetString(3),
            EventDate = DateTime.Parse(reader.GetString(4)),
            ScheduledText = reader.IsDBNull(5) ? null : reader.GetString(5),
            OverrideReason = reader.IsDBNull(6) ? null : reader.GetString(6),
            Notes = reader.IsDBNull(7) ? null : reader.GetString(7),
            VisitType = reader.IsDBNull(8) ? null : reader.GetString(8),
            ScheduledForMemberId = reader.IsDBNull(9) ? null : reader.GetInt32(9),
            CreatedAt = DateTime.Parse(reader.GetString(10))
        };
    }
}
