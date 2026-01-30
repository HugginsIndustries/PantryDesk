using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;

namespace PantryDeskSeeder;

/// <summary>
/// Seeds authentication roles for the demo database.
/// </summary>
public static class AuthRoleSeeder
{
    /// <summary>
    /// Seeds Entry and Admin roles with default passwords if they don't exist.
    /// </summary>
    public static void SeedAuthRoles(SqliteConnection connection)
    {
        // Check if roles already exist
        if (AuthRoleRepository.HasAnyRoles(connection))
        {
            Console.WriteLine("Auth roles already exist, skipping seed.");
            return;
        }

        // Create Entry role with default password "entry"
        AuthRoleRepository.UpdatePassword(connection, "Entry", "entry");
        Console.WriteLine("Created Entry role with default password 'entry'");

        // Create Admin role with default password "admin"
        AuthRoleRepository.UpdatePassword(connection, "Admin", "admin");
        Console.WriteLine("Created Admin role with default password 'admin'");
    }
}
