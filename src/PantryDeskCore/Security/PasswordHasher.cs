using System.Security.Cryptography;
using System.Text;

namespace PantryDeskCore.Security;

/// <summary>
/// Provides password hashing and verification using PBKDF2.
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16; // 128 bits
    private const int HashSize = 32; // 256 bits
    private const int Iterations = 100000; // PBKDF2 iteration count

    /// <summary>
    /// Hashes a password using PBKDF2 with a random salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A tuple containing the base64-encoded hash and salt.</returns>
    public static (string Hash, string Salt) HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        // Generate random salt
        var saltBytes = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        // Hash the password
        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        // Convert to base64 strings
        var hash = Convert.ToBase64String(hashBytes);
        var salt = Convert.ToBase64String(saltBytes);

        return (hash, salt);
    }

    /// <summary>
    /// Verifies a password against a stored hash and salt.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="storedHash">The stored base64-encoded hash.</param>
    /// <param name="storedSalt">The stored base64-encoded salt.</param>
    /// <returns>True if the password matches, false otherwise.</returns>
    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
        {
            return false;
        }

        try
        {
            // Decode salt and hash
            var saltBytes = Convert.FromBase64String(storedSalt);
            var storedHashBytes = Convert.FromBase64String(storedHash);

            // Hash the provided password with the stored salt
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            // Compare hashes using constant-time comparison
            return CryptographicOperations.FixedTimeEquals(hashBytes, storedHashBytes);
        }
        catch
        {
            // If decoding fails, password doesn't match
            return false;
        }
    }
}
