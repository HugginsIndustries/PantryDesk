using PantryDeskCore.Data;
using PantryDeskCore.Services;
using PantryDeskApp.Forms;

namespace PantryDeskApp;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        // Initialize database
        using (var connection = DatabaseManager.InitializeDatabase())
        {
            // Initialize app version if not already set
            ConfigRepository.InitializeAppVersion(connection);
            // Sync household active status from last qualifying service date
            ActiveStatusSyncService.SyncAllHouseholds(connection);
        }

        // Check if automatic backup is needed for today
        try
        {
            var lastAutoBackupDate = BackupService.GetLastAutoBackupDate();
            var today = DateTime.Today;

            if (lastAutoBackupDate == null || lastAutoBackupDate < today)
            {
                // Create automatic backup
                BackupService.CreateBackup(targetFolder: null, passphrase: null, isAutomatic: true);
                // Silent success - backup created automatically
            }
        }
        catch (Exception ex)
        {
            // Log error but don't block app startup
            // In a production app, you might want to log this to a file
            System.Diagnostics.Debug.WriteLine($"Automatic backup failed: {ex.Message}");
        }

        // Check if roles exist
        bool hasRoles;
        using (var connection = DatabaseManager.GetConnection())
        {
            hasRoles = AuthRoleRepository.HasAnyRoles(connection);
        }

        // If no roles exist, show initial setup
        if (!hasRoles)
        {
            using var setupForm = new InitialSetupForm();
            if (setupForm.ShowDialog() != DialogResult.OK)
            {
                // User cancelled setup, exit application
                return;
            }
        }

        // Show login form
        using var loginForm = new LoginForm();
        if (loginForm.ShowDialog() != DialogResult.OK)
        {
            // User cancelled login, exit application
            return;
        }

        // Login successful, show main check-in form
        Application.Run(new Forms.CheckInForm());
    }    
}