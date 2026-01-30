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
    private const string BackupMetadataFileExtension = ".meta.json";

    /// <summary>
    /// Creates an encrypted backup of the database.
    /// </summary>
    /// <param name="targetFolder">Optional target folder. If null, uses default backup location.</param>
    /// <param name="passphrase">Optional passphrase for manual/USB backups. If null, uses DPAPI for automatic backups.</param>
    /// <returns>The full path to the created backup file.</returns>
    /// <exception cref="IOException">Thrown if backup creation fails.</exception>
    public static string CreateBackup(string? targetFolder = null, string? passphrase = null)
    {
        var dbPath = DatabaseManager.GetDatabasePath();

        if (!File.Exists(dbPath))
        {
            throw new FileNotFoundException("Database file not found.", dbPath);
        }

        // Determine backup folder
        var dataRoot = AppConfig.GetDataRoot();
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
            // Use SQLite backup API for atomic, consistent snapshot
            var tempDbPath = Path.Combine(tempDir, "pantrydesk.db");
            using (var sourceConnection = DatabaseManager.GetConnection())
            {
                sourceConnection.Open();
                using (var backupConnection = new SqliteConnection($"Data Source={tempDbPath}"))
                {
                    backupConnection.Open();
                    sourceConnection.BackupDatabase(backupConnection);
                }
            }

            // Generate encryption key and parameters
            byte[] encryptionKey;
            string encryptionMethod;
            byte[]? salt = null;
            byte[]? encryptedKey = null;

            if (passphrase != null)
            {
                // Use passphrase-based encryption
                encryptionMethod = "Passphrase";
                salt = new byte[16];
                RandomNumberGenerator.Fill(salt);
                encryptionKey = DeriveKeyFromPassphrase(passphrase, salt);
            }
            else
            {
                // Use DPAPI for automatic backups
                encryptionMethod = "DPAPI";
                encryptionKey = new byte[32];
                RandomNumberGenerator.Fill(encryptionKey);
                // Encrypt the key with DPAPI
                encryptedKey = System.Security.Cryptography.ProtectedData.Protect(
                    encryptionKey,
                    null,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
            }

            // Create metadata
            var metadata = new BackupMetadata
            {
                BackupDate = DateTime.UtcNow,
                SchemaVersion = GetSchemaVersion(),
                AppVersion = GetAppVersion(),
                EncryptionMethod = encryptionMethod,
                Salt = salt != null ? Convert.ToBase64String(salt) : null,
                EncryptedKey = encryptedKey != null ? Convert.ToBase64String(encryptedKey) : null
            };

            // Write metadata to temp directory (will be encrypted in zip)
            var metadataJson = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
            var metadataPath = Path.Combine(tempDir, BackupMetadataFileName);
            File.WriteAllText(metadataPath, metadataJson, Encoding.UTF8);

            // Create encrypted zip file
            CreateEncryptedZip(tempDir, backupPath, encryptionKey);

            // Write metadata file next to zip (unencrypted, for easy access during restore)
            var metadataFileNextToZip = backupPath + BackupMetadataFileExtension;
            File.WriteAllText(metadataFileNextToZip, metadataJson, Encoding.UTF8);

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
    /// <param name="passphrase">Optional passphrase if backup was encrypted with passphrase.</param>
    /// <returns>The backup metadata.</returns>
    /// <exception cref="FileNotFoundException">Thrown if backup file doesn't exist.</exception>
    /// <exception cref="InvalidDataException">Thrown if backup is invalid or corrupted.</exception>
    public static BackupMetadata ReadBackupMetadata(string backupZipPath, string? passphrase = null)
    {
        if (!File.Exists(backupZipPath))
        {
            throw new FileNotFoundException("Backup file not found.", backupZipPath);
        }

        var tempDir = Path.Combine(Path.GetTempPath(), $"pantrydesk_read_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);

        try
        {
            ExtractEncryptedZip(backupZipPath, tempDir, passphrase);

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
    /// <param name="passphrase">Optional passphrase if backup was encrypted with passphrase.</param>
    /// <returns>The backup metadata.</returns>
    /// <exception cref="FileNotFoundException">Thrown if backup file doesn't exist.</exception>
    /// <exception cref="InvalidDataException">Thrown if backup is invalid or corrupted.</exception>
    public static BackupMetadata RestoreFromBackup(string backupZipPath, string? passphrase = null)
    {
        if (!File.Exists(backupZipPath))
        {
            throw new FileNotFoundException("Backup file not found.", backupZipPath);
        }

        var dbPath = DatabaseManager.GetDatabasePath();
        var dataRoot = AppConfig.GetDataRoot();

        // Create temporary directory for extraction
        var tempDir = Path.Combine(Path.GetTempPath(), $"pantrydesk_restore_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDir);

        try
        {
            // Extract encrypted zip
            ExtractEncryptedZip(backupZipPath, tempDir, passphrase);

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

    private static void CreateEncryptedZip(string sourceDirectory, string zipPath, byte[] encryptionKey)
    {
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

            // Encrypt the file content using AES-GCM
            EncryptStreamGcm(fileStream, entryStream, encryptionKey);
        }
    }

    private static void ExtractEncryptedZip(string zipPath, string targetDirectory, string? passphrase = null)
    {
        // First, read metadata from file next to zip (or try to decrypt from zip for old backups)
        BackupMetadata? metadata = null;
        var metadataFileNextToZip = zipPath + BackupMetadataFileExtension;
        
        if (File.Exists(metadataFileNextToZip))
        {
            // New format: metadata stored outside zip
            var metadataJson = File.ReadAllText(metadataFileNextToZip, Encoding.UTF8);
            metadata = JsonSerializer.Deserialize<BackupMetadata>(metadataJson);
        }
        else
        {
            // Old format or metadata in zip: try to extract and decrypt metadata entry first
            // For old backups, we'll try DPAPI first, then passphrase if provided
            metadata = TryExtractMetadataFromZip(zipPath, passphrase);
        }

        if (metadata == null)
        {
            throw new InvalidDataException("Could not read backup metadata. The backup file may be corrupted or in an unsupported format.");
        }

        // Derive encryption key based on method
        byte[] encryptionKey;
        if (metadata.EncryptionMethod == "Passphrase")
        {
            if (string.IsNullOrEmpty(passphrase))
            {
                throw new InvalidOperationException("This backup requires a passphrase. Please provide the passphrase used during backup creation.");
            }
            if (string.IsNullOrEmpty(metadata.Salt))
            {
                throw new InvalidDataException("Backup metadata is missing salt for passphrase decryption.");
            }
            var salt = Convert.FromBase64String(metadata.Salt);
            encryptionKey = DeriveKeyFromPassphrase(passphrase, salt);
        }
        else if (metadata.EncryptionMethod == "DPAPI")
        {
            if (string.IsNullOrEmpty(metadata.EncryptedKey))
            {
                throw new InvalidDataException("Backup metadata is missing encrypted key for DPAPI decryption.");
            }
            var encryptedKey = Convert.FromBase64String(metadata.EncryptedKey);
            encryptionKey = System.Security.Cryptography.ProtectedData.Unprotect(
                encryptedKey,
                null,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
        }
        else
        {
            throw new InvalidDataException($"Unsupported encryption method: {metadata.EncryptionMethod}");
        }

        // Extract and decrypt all files
        using var zipStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read);
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            // Skip metadata entry (already processed)
            if (entry.Name == BackupMetadataFileName)
            {
                continue;
            }

            var targetPath = Path.Combine(targetDirectory, entry.FullName);
            var targetDir = Path.GetDirectoryName(targetPath);
            if (targetDir != null && !Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            using var entryStream = entry.Open();
            using var targetStream = File.Create(targetPath);

            // Decrypt the file content using AES-GCM
            DecryptStreamGcm(entryStream, targetStream, encryptionKey);
        }

        // Write metadata file to target directory (decrypted from zip)
        var metadataPath = Path.Combine(targetDirectory, BackupMetadataFileName);
        var finalMetadataJson = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(metadataPath, finalMetadataJson, Encoding.UTF8);
    }

    private static BackupMetadata? TryExtractMetadataFromZip(string zipPath, string? passphrase)
    {
        // Try to extract metadata from zip (for old backup format compatibility)
        // This is a fallback - new backups store metadata outside zip
        try
        {
            using var zipStream = new FileStream(zipPath, FileMode.Open, FileAccess.Read);
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
            
            var metadataEntry = archive.GetEntry(BackupMetadataFileName);
            if (metadataEntry == null)
            {
                return null;
            }

            // For old backups, try to decrypt with old method (deterministic key)
            // This is for backward compatibility only
            var oldKey = DeriveOldEncryptionKey();
            using var entryStream = metadataEntry.Open();
            using var memoryStream = new MemoryStream();
            
            try
            {
                DecryptStreamOld(entryStream, memoryStream, oldKey);
                memoryStream.Position = 0;
                using var reader = new StreamReader(memoryStream, Encoding.UTF8);
                var metadataJson = reader.ReadToEnd();
                return JsonSerializer.Deserialize<BackupMetadata>(metadataJson);
            }
            catch
            {
                // Old decryption failed, return null to try other methods
                return null;
            }
        }
        catch
        {
            return null;
        }
    }

    private static byte[] DeriveOldEncryptionKey()
    {
        // Old deterministic key derivation (for backward compatibility)
        var dataRoot = AppConfig.GetDataRoot();
        var salt = Encoding.UTF8.GetBytes("PantryDeskBackup2026");
        var passwordBytes = Encoding.UTF8.GetBytes(dataRoot);

        return Rfc2898DeriveBytes.Pbkdf2(
            passwordBytes,
            salt,
            10000,
            HashAlgorithmName.SHA256,
            32);
    }

    private static void DecryptStreamOld(Stream input, Stream output, byte[] key)
    {
        // Old CBC decryption (for backward compatibility)
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

    private static byte[] DeriveKeyFromPassphrase(string passphrase, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(passphrase);
        return Rfc2898DeriveBytes.Pbkdf2(
            passwordBytes,
            salt,
            100000, // Increased iterations for better security
            HashAlgorithmName.SHA256,
            32); // 256-bit key for AES-256
    }

    private static void EncryptStreamGcm(Stream input, Stream output, byte[] key)
    {
        using var aes = new AesGcm(key, 16); // 16-byte tag for GCM
        
        // Generate random nonce (12 bytes for GCM)
        var nonce = new byte[12];
        RandomNumberGenerator.Fill(nonce);
        
        // Write nonce to output
        output.Write(nonce, 0, nonce.Length);
        
        // Read input into memory for encryption
        using var inputMemory = new MemoryStream();
        input.CopyTo(inputMemory);
        var plaintext = inputMemory.ToArray();
        
        // Encrypt with GCM (produces ciphertext + tag)
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[16]; // GCM tag is 16 bytes
        aes.Encrypt(nonce, plaintext, ciphertext, tag);
        
        // Write ciphertext
        output.Write(ciphertext, 0, ciphertext.Length);
        
        // Write authentication tag
        output.Write(tag, 0, tag.Length);
    }

    private static void DecryptStreamGcm(Stream input, Stream output, byte[] key)
    {
        using var aes = new AesGcm(key, 16); // 16-byte tag for GCM
        
        // Read nonce (12 bytes)
        var nonce = new byte[12];
        var bytesRead = input.Read(nonce, 0, nonce.Length);
        if (bytesRead != nonce.Length)
        {
            throw new InvalidDataException("Invalid backup file format: missing nonce.");
        }
        
        // Read all remaining data
        using var ciphertextMemory = new MemoryStream();
        input.CopyTo(ciphertextMemory);
        var allData = ciphertextMemory.ToArray();
        
        if (allData.Length < 16)
        {
            throw new InvalidDataException("Invalid backup file format: missing authentication tag.");
        }
        
        // Split ciphertext and tag (last 16 bytes are tag)
        var tag = new byte[16];
        Array.Copy(allData, allData.Length - 16, tag, 0, 16);
        var ciphertext = new byte[allData.Length - 16];
        Array.Copy(allData, 0, ciphertext, 0, ciphertext.Length);
        
        // Decrypt with GCM (will throw if tampered)
        var plaintext = new byte[ciphertext.Length];
        aes.Decrypt(nonce, ciphertext, tag, plaintext);
        
        // Write decrypted data
        output.Write(plaintext, 0, plaintext.Length);
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
        public string EncryptionMethod { get; set; } = "DPAPI";
        public string? Salt { get; set; } // Base64-encoded salt for passphrase encryption
        public string? EncryptedKey { get; set; } // Base64-encoded DPAPI-protected key
    }
}
