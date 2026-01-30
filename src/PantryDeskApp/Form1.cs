using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Security;
using PantryDeskApp.Forms;

namespace PantryDeskApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        UpdateTitle();
        UpdateMenuVisibility();
    }

    private void UpdateTitle()
    {
        var role = SessionManager.CurrentRole ?? "Not Logged In";
        this.Text = $"PantryDesk - {role}";
    }

    private void UpdateMenuVisibility()
    {
        menuAdmin.Visible = SessionManager.IsAdmin;
    }

    private void BtnTest_Click(object? sender, EventArgs e)
    {
        try
        {
            // Initialize database
            var connection = DatabaseManager.InitializeDatabase();
            ConfigRepository.InitializeAppVersion(connection);

            var results = new List<string> { "Phase 1 Database Test Results:" };

            // 1. Create a household
            var household = new Household
            {
                PrimaryName = "Test Household",
                Address1 = "123 Test St",
                City = "Winlock",
                State = "WA",
                Zip = "98596",
                Phone = "360-555-1234",
                Email = "test@example.com",
                ChildrenCount = 2,
                AdultsCount = 2,
                SeniorsCount = 0,
                Notes = "Test household for Phase 1 verification",
                IsActive = true
            };

            household = HouseholdRepository.Create(connection, household);
            results.Add($"✓ Created household ID: {household.Id}, Name: {household.PrimaryName}");

            // 2. Create or get a pantry day
            var testDate = DateTime.Today.AddDays(7);
            var pantryDay = PantryDayRepository.GetByDate(connection, testDate);
            
            if (pantryDay == null)
            {
                pantryDay = new PantryDay
                {
                    PantryDate = testDate,
                    IsActive = true,
                    Notes = "Test pantry day"
                };
                pantryDay = PantryDayRepository.Create(connection, pantryDay);
                results.Add($"✓ Created pantry day ID: {pantryDay.Id}, Date: {pantryDay.PantryDate:yyyy-MM-dd}");
            }
            else
            {
                results.Add($"✓ Found existing pantry day ID: {pantryDay.Id}, Date: {pantryDay.PantryDate:yyyy-MM-dd}");
            }

            // 3. Create a service event
            var serviceEvent = new ServiceEvent
            {
                HouseholdId = household.Id,
                EventType = "PantryDay",
                EventStatus = "Completed",
                EventDate = DateTime.Today,
                Notes = "Test service event"
            };

            serviceEvent = ServiceEventRepository.Create(connection, serviceEvent);
            results.Add($"✓ Created service event ID: {serviceEvent.Id}, Type: {serviceEvent.EventType}");

            // 4. Read them back
            var readHousehold = HouseholdRepository.GetById(connection, household.Id);
            if (readHousehold != null && readHousehold.PrimaryName == household.PrimaryName)
            {
                results.Add($"✓ Read household: {readHousehold.PrimaryName}");
            }
            else
            {
                results.Add("✗ Failed to read household");
            }

            var readPantryDay = PantryDayRepository.GetById(connection, pantryDay.Id);
            if (readPantryDay != null && readPantryDay.PantryDate == pantryDay.PantryDate)
            {
                results.Add($"✓ Read pantry day: {readPantryDay.PantryDate:yyyy-MM-dd}");
            }
            else
            {
                results.Add("✗ Failed to read pantry day");
            }

            var readServiceEvent = ServiceEventRepository.GetById(connection, serviceEvent.Id);
            if (readServiceEvent != null && readServiceEvent.HouseholdId == serviceEvent.HouseholdId)
            {
                results.Add($"✓ Read service event: {readServiceEvent.EventType}");
            }
            else
            {
                results.Add("✗ Failed to read service event");
            }

            // 5. Verify schema version
            var schemaVersion = ConfigRepository.GetSchemaVersion(connection);
            results.Add($"✓ Schema version: {schemaVersion}");

            // Show results
            MessageBox.Show(string.Join(Environment.NewLine, results), "Phase 1 Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Test failed: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void MenuItemChangePasswords_Click(object? sender, EventArgs e)
    {
        using var changePasswordForm = new ChangePasswordForm();
        changePasswordForm.ShowDialog();
    }

    private void MenuItemPantryDays_Click(object? sender, EventArgs e)
    {
        using var pantryDaysForm = new PantryDaysForm();
        pantryDaysForm.ShowDialog();
    }

    private void MenuItemLogout_Click(object? sender, EventArgs e)
    {
        SessionManager.Logout();
        Application.Exit();
    }
}
