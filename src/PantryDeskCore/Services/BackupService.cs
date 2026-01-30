using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using PantryDeskCore.Configuration;
using PantryDeskCore.Data;

namespace PantryDeskCore.Services;

/// <summary>
/// Service for creating and restoring encrypted database backups.
/// </summary>
public static class BackupService
{
    private const string BackupMetadataFileName = "backup_metadata.json";
    private const string ConfigKeyLastBackupDate = "last_backup_date";

    /// <summary>
    /// Creates an encrypted backup of the database.
    /// </summary>
    /// <param name="targetFolder">Optional target folder. If null, uses default backup location.</param>
    /// <returns>The full path to the created backup file.</returns>
    /// <exception cref="IOException">Thrown if backup creation fails.</exception>
    public static string CreateBackup(string? targetFolder = null)
    {
        var dataRoot = AppConfig.GetDataRoot();
        var dbPath = Path.Combine(dataRoot, "pantrydesk.db");

        if (!File.Exists(dbPath))
        {
            throw new FileNotFoundException("Database file not found.", dbPath);
        }

        // Determine backup folder
        var backupFolder = targetFolder ?? Path.Combine(dataRoot, "Backups");
        if (!Directory.Exists(backupFolder))
        {
            Directory.CreateDirectory(backupFolder);
        }

        // Generate backup filename
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var backupFileName = $"pantrydesk_backup_{timestamp}.zip";
        var backupPath = Path.Combine(backupFolder, backupFileName);

        // Create temporary directory for backup contents
        var tempDir = Path.Combine(Path.GetTempPath(), $"pantrydesk_backup_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);

        try
        {
            // Copy database file to temp directory
            var tempDbPath = Path.Combine(tempDir, "pantrydesk.db");
            File.Copy(dbPath, tempDbPath, true);

            // Create metadata
            var metadata = new BackupMetadata
            {
                BackupDate = DateTime.UtcNow,
                SchemaVersion = GetSchemaVersion(),
                AppVersion = GetAppVersion()
            };

            var metadataJson = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
            var metadataPath = Path.Combine(tempDir, BackupMetadataFileName);
            File.WriteAllText(metadataPath, metadataJson, Encoding.UTF8);

            // Create encrypted zip file
            CreateEncryptedZip(tempDir, backupPath);

            // Update last backup date
            UpdateLastBackupDate(DateTime.Today);

            return backupPath;
        }
        finally
        {
            // Clean up temp directory
            try
            {
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    /// <summary>
    /// Reads metadata from a backup file without restoring.
    /// </summary>
    /// <param name="backupZipPath">Path to the backup zip file.</param>
    /// <returns>The backup metadata.</returns>
    /// <exception cref="FileNotFoundException">Thrown if backup file doesn't exist.</exception>
    /// <exception cref="InvalidDataException">Thrown if backup is invalid or corrupted.</exception>
    public static BackupMetadata ReadBackupMetadata(string backupZipPath)
    {
        if (!File.Exists(backupZipPath))
        {
            throw new FileNotFoundException("Backup file not found.", backupZipPath);
        }

        var tempDir = Path.Combine(Path.GetTempPath(), $"pantrydesk_read_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);

        try
        {
            ExtractEncryptedZip(backupZipPath, tempDir);

            // Validate backup contains required files
            var extractedDbPath = Path.Combine(tempDir, "pantrydesk.db");
            var extractedMetadataPath = Path.Combine(tempDir, BackupMetadataFileName);

            if (!File.Exists(extractedDbPath))
            {
                throw new InvalidDataException("Backup does not contain database file.");
            }

            if (!File.Exists(extractedMetadataPath))
            {
                throw new InvalidDataException("Backup does not contain metadata file.");
            }

            var metadataJson = File.ReadAllText(extractedMetadataPath, Encoding.UTF8);
            var metadata = JsonSerializer.Deserialize<BackupMetadata>(metadataJson);
            if (metadata == null)
            {
                throw new InvalidDataException("Backup metadata is invalid.");
            }

            return metadata;
        }
        finally
        {
            try
            {
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    /// <summary>
    /// Restores the database from an encrypted backup file.
    /// </summary>
    /// <param name="backupZipPath">Path to the backup zip file.</param>
    /// <returns>The backup metadata.</returns>
    /// <exception cref="FileNotFoundException">Thrown if backup file doesn't exist.</exception>
    /// <exception cref="InvalidDataException">Thrown if backup is invalid or corrupted.</exception>
    public static BackupMetadata RestoreFromBackup(string backupZipPath)
    {
        if (!File.Exists(backupZipPath))
        {
            throw new FileNotFoundException("Backup file not found.", backupZipPath);
        }

        var dataRoot = AppConfig.GetDataRoot();
        var dbPath = Path.Combine(dataRoot, "pantrydesk.db");

        // Create temporary directory for extraction
        var tempDir = Path.Combine(Path.GetTempPath(), $"pantrydesk_restore_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);

        try
        {
            // Extract encrypted zip
            ExtractEncryptedZip(backupZipPath, tempDir);

            // Validate backup contents
            var extractedDbPath = Path.Combine(tempDir, "pantrydesk.db");
            var extractedMetadataPath = Path.Combine(tempDir, BackupMetadataFileName);

            if (!File.Exists(extractedDbPath))
            {
                throw new InvalidDataException("Backup does not contain database file.");
            }

            if (!File.Exists(extractedMetadataPath))
            {
                throw new InvalidDataException("Backup does not contain metadata file.");
            }

            // Read and validate metadata
            var metadataJson = File.ReadAllText(extractedMetadataPath, Encoding.UTF8);
            var metadata = JsonSerializer.Deserialize<BackupMetadata>(metadataJson);
            if (metadata == null)
            {
                throw new InvalidDataException("Backup metadata is invalid.");
            }

            // Validate database file by trying to open it
            ValidateDatabaseFile(extractedDbPath);

            // Create safety copy of current database
            if (File.Exists(dbPath))
            {
                var safetyCopyName = $"pantrydesk.db.pre_restore_{DateTime.Now:yyyyMMdd_HHmmss}.backup";
                var safetyCopyPath = Path.Combine(dataRoot, safetyCopyName);
                File.Copy(dbPath, safetyCopyPath, true);
            }

            // Restore database file
            File.Copy(extractedDbPath, dbPath, true);

            return metadata;
        }
        finally
        {
            // Clean up temp directory
            try
            {
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    /// <summary>
    /// Gets the last backup date from the config table.
    /// </summary>
    /// <returns>The last backup date, or null if never backed up.</returns>
    public static DateTime? GetLastBackupDate()
    {
        using var connection = DatabaseManager.GetConnection();
        var dateStr = ConfigRepository.GetValue(connection, ConfigKeyLastBackupDate);
        if (dateStr != null && DateTime.TryParse(dateStr, out var date))
        {
            return date.Date;
        }
        return null;
    }

    /// <summary>
    /// Sets the last backup date in the config table.
    /// </summary>
    /// <param name="date">The backup date to set.</param>
    private static void UpdateLastBackupDate(DateTime date)
    {
        using var connection = DatabaseManager.GetConnection();
        ConfigRepository.SetValue(connection, ConfigKeyLastBackupDate, date.ToString("yyyy-MM-dd"));
    }

    private static int GetSchemaVersion()
    {
        using var connection = DatabaseManager.GetConnection();
        return ConfigRepository.GetSchemaVersion(connection);
    }

    private static string? GetAppVersion()
    {
        using var connection = DatabaseManager.GetConnection();
        return ConfigRepository.GetAppVersion(connection);
    }

    private static void CreateEncryptedZip(string sourceDirectory, string zipPath)
    {
        // For Phase 7, we'll use a simple encryption approach:
        // Derive a key from the data root path (deterministic but app-specific)
        var key = DeriveEncryptionKey();

        // Create zip file
        if (File.Exists(zipPath))
        {
            File.Delete(zipPath);
        }

        using var zipStream = new FileStream(zipPath, FileMode.Create);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create);

        // Add all files from source directory
        foreach (var file in Directory.GetFiles(sourceDirectory))
        {
            var entryName = Path.GetFileName(file);
            var entry = archive.CreateEntry(entryName);

            using var entryStream = entry.Open();
            using var fileStream = File.OpenRead(file);

            // Encrypt the file content
            EncryptStream(fileStream, entryStream, key);
        }
    }

    private static void ExtractEncryptedZip(string zipPath, string targetDirectory)
    {
        var key = DeriveEncryptionKey();

        using var zipStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            var targetPath = Path.Combine(targetDirectory, entry.FullName);
            var targetDir = Path.GetDirectoryName(targetPath);
            if (targetDir != null && !Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            using var entryStream = entry.Open();
            using var targetStream = File.Create(targetPath);

            // Decrypt the file content
            DecryptStream(entryStream, targetStream, key);
        }
    }

    private static byte[] DeriveEncryptionKey()
    {
        // Derive a deterministic key from the data root path
        // This ensures backups are app-specific but don't require user key management
        var dataRoot = AppConfig.GetDataRoot();
        var salt = Encoding.UTF8.GetBytes("PantryDeskBackup2026");
        var passwordBytes = Encoding.UTF8.GetBytes(dataRoot);

        return Rfc2898DeriveBytes.Pbkdf2(
            passwordBytes,
            salt,
            10000,
            HashAlgorithmName.SHA256,
            32); // 256-bit key for AES-256
    }

    private static void EncryptStream(Stream input, Stream output, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.GenerateIV();

        // Write IV to output
        output.Write(aes.IV, 0, aes.IV.Length);

        using var encryptor = aes.CreateEncryptor();
        using var cryptoStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write);
        input.CopyTo(cryptoStream);
        cryptoStream.FlushFinalBlock();
    }

    private static void DecryptStream(Stream input, Stream output, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        // Read IV from input
        var iv = new byte[aes.BlockSize / 8];
        var bytesRead = input.Read(iv, 0, iv.Length);
        if (bytesRead != iv.Length)
        {
            throw new InvalidDataException("Invalid backup file format.");
        }
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var cryptoStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read);
        cryptoStream.CopyTo(output);
    }

    private static void ValidateDatabaseFile(string dbPath)
    {
        // Try to open the database to validate it's a valid SQLite file
        try
        {
            var connectionString = $"Data Source={dbPath}";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            // Try a simple query
            using var cmd = new SqliteCommand("SELECT COUNT(*) FROM sqlite_master", connection);
            cmd.ExecuteScalar();
            connection.Close();
        }
        catch (Exception ex)
        {
            throw new InvalidDataException($"Backup database file is invalid or corrupted: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Metadata stored in backup files.
    /// </summary>
    public class BackupMetadata
    {
        public DateTime BackupDate { get; set; }
        public int SchemaVersion { get; set; }
        public string? AppVersion { get; set; }
    }
}
