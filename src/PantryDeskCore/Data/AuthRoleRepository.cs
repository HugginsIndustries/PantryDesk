using Microsoft.Data.Sqlite;
using PantryDeskCore.Models;
using PantryDeskCore.Security;

namespace PantryDeskCore.Data;

/// <summary>
/// Repository for authentication role data access operations.
/// </summary>
public static class AuthRoleRepository
{
    /// <summary>
    /// Gets an authentication role by role name.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="roleName">The role name ('Entry' or 'Admin').</param>
    /// <returns>The authentication role, or null if not found.</returns>
    public static AuthRole? GetByRoleName(SqliteConnection connection, string roleName)
    {
        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.AuthRoleSelectByRoleName, connection);
            cmd.Parameters.AddWithValue("@role_name", roleName);

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
    /// Updates the password for a role. The password is hashed before storage.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="roleName">The role name ('Entry' or 'Admin').</param>
    /// <param name="password">The plain text password to hash and store.</param>
    public static void UpdatePassword(SqliteConnection connection, string roleName, string password)
    {
        var (hash, salt) = PasswordHasher.HashPassword(password);
        var updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        connection.Open();
        try
        {
            using var cmd = new SqliteCommand(Sql.AuthRoleUpdatePassword, connection);
            cmd.Parameters.AddWithValue("@role_name", roleName);
            cmd.Parameters.AddWithValue("@password_hash", hash);
            cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// Verifies a password for a role.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="roleName">The role name ('Entry' or 'Admin').</param>
    /// <param name="password">The plain text password to verify.</param>
    /// <returns>True if the password matches, false otherwise.</returns>
    public static bool VerifyPassword(SqliteConnection connection, string roleName, string password)
    {
        var role = GetByRoleName(connection, roleName);
        if (role == null)
        {
            return false;
        }

        return PasswordHasher.VerifyPassword(password, role.PasswordHash, role.Salt);
    }

    private static AuthRole MapFromReader(SqliteDataReader reader)
    {
        return new AuthRole
        {
            RoleName = reader.GetString(0),
            PasswordHash = reader.GetString(1),
            Salt = reader.GetString(2),
            UpdatedAt = DateTime.Parse(reader.GetString(3))
        };
    }
}
