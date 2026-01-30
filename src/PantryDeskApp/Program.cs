using PantryDeskCore.Data;
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
        DatabaseManager.InitializeDatabase();

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