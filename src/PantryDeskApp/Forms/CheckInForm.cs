using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Security;
using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

/// <summary>
/// Main check-in form for searching households and completing services.
/// </summary>
public partial class CheckInForm : Form
{
    private Household? _selectedHousehold;

    public CheckInForm()
    {
        InitializeComponent();
        UpdateMenuVisibility();
        SetupDataGridView();
        
        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void CheckInForm_Load(object? sender, EventArgs e)
    {
        // Load all households on initial load
        SearchHouseholds(string.Empty);
    }

    private void UpdateMenuVisibility()
    {
        menuAdmin.Visible = SessionManager.IsAdmin;
    }

    private void SetupDataGridView()
    {
        dgvResults.Columns.Clear();
        dgvResults.Columns.Add("Name", "Name");
        dgvResults.Columns.Add("CityZip", "City/Zip");
        dgvResults.Columns.Add("Size", "Size");
        dgvResults.Columns.Add("LastService", "Last Service");
        dgvResults.Columns.Add("Eligibility", "Eligibility");
        dgvResults.Columns.Add("Status", "Status");
        dgvResults.Columns.Add("HouseholdId", "HouseholdId");
        var householdIdColumn = dgvResults.Columns["HouseholdId"];
        if (householdIdColumn != null)
        {
            householdIdColumn.Visible = false;
        }

        // Set column widths
        var nameColumn = dgvResults.Columns["Name"];
        var cityZipColumn = dgvResults.Columns["CityZip"];
        var sizeColumn = dgvResults.Columns["Size"];
        var lastServiceColumn = dgvResults.Columns["LastService"];
        var eligibilityColumn = dgvResults.Columns["Eligibility"];
        var statusColumn = dgvResults.Columns["Status"];

        if (nameColumn != null) nameColumn.Width = 200;
        if (cityZipColumn != null) cityZipColumn.Width = 150;
        if (sizeColumn != null) sizeColumn.Width = 100;
        if (lastServiceColumn != null) lastServiceColumn.Width = 150;
        if (eligibilityColumn != null) eligibilityColumn.Width = 150;
        if (statusColumn != null) statusColumn.Width = 100;
    }

    private void TxtSearch_TextChanged(object? sender, EventArgs e)
    {
        // Debounce: search after user stops typing
        // For now, search immediately (can add timer-based debounce if needed)
        SearchHouseholds(txtSearch.Text);
    }

    private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            SearchHouseholds(txtSearch.Text);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    private void SearchHouseholds(string searchTerm)
    {
        dgvResults.Rows.Clear();
        _selectedHousehold = null;
        btnCompleteService.Enabled = false;
        btnOpenProfile.Enabled = false;

        try
        {
            using var connection = DatabaseManager.GetConnection();
            var households = HouseholdRepository.SearchByName(connection, searchTerm);

            foreach (var household in households)
            {
                // Get last completed service
                var lastService = ServiceEventRepository.GetLastCompletedByHouseholdId(connection, household.Id);

                // Format last service display
                string lastServiceText = "Never";
                if (lastService != null)
                {
                    lastServiceText = $"{lastService.EventDate:yyyy-MM-dd} {lastService.EventType}";
                }

                // Check eligibility
                var isEligible = EligibilityService.IsEligibleThisMonth(connection, household.Id, DateTime.Today);
                string eligibilityText = isEligible ? "Eligible" : "Already served this month";
                var eligibilityColor = isEligible ? Color.Green : Color.OrangeRed;

                // Format city/zip
                string cityZip = string.Empty;
                if (!string.IsNullOrWhiteSpace(household.City))
                {
                    cityZip = household.City;
                    if (!string.IsNullOrWhiteSpace(household.Zip))
                    {
                        cityZip += $", {household.Zip}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(household.Zip))
                {
                    cityZip = household.Zip;
                }
                if (string.IsNullOrWhiteSpace(cityZip))
                {
                    cityZip = "—";
                }

                // Format size breakdown
                var totalSize = household.ChildrenCount + household.AdultsCount + household.SeniorsCount;
                string sizeText = $"{totalSize} ({household.ChildrenCount}C/{household.AdultsCount}A/{household.SeniorsCount}S)";

                // Status
                string statusText = household.IsActive ? "Active" : "Inactive";
                var statusColor = household.IsActive ? Color.Black : Color.Gray;

                // Add row
                var row = dgvResults.Rows.Add(
                    household.PrimaryName,
                    cityZip,
                    sizeText,
                    lastServiceText,
                    eligibilityText,
                    statusText,
                    household.Id
                );

                // Color code eligibility and status
                var rowObj = dgvResults.Rows[row];
                rowObj.Cells["Eligibility"].Style.ForeColor = eligibilityColor;
                rowObj.Cells["Status"].Style.ForeColor = statusColor;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error searching households: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvResults_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvResults.SelectedRows.Count == 0)
        {
            _selectedHousehold = null;
            btnCompleteService.Enabled = false;
            btnOpenProfile.Enabled = false;
            return;
        }

        var selectedRow = dgvResults.SelectedRows[0];
        var householdIdValue = selectedRow.Cells["HouseholdId"].Value;
        if (householdIdValue == null)
        {
            return;
        }
        var householdId = (int)householdIdValue;

        try
        {
            using var connection = DatabaseManager.GetConnection();
            _selectedHousehold = HouseholdRepository.GetById(connection, householdId);

            btnCompleteService.Enabled = _selectedHousehold != null;
            btnOpenProfile.Enabled = _selectedHousehold != null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading household: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCompleteService_Click(object? sender, EventArgs e)
    {
        if (_selectedHousehold == null)
        {
            MessageBox.Show("Please select a household first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            using var connection = DatabaseManager.GetConnection();

            // Warn if household is marked inactive
            if (!_selectedHousehold.IsActive)
            {
                var inactiveResult = MessageBox.Show(
                    "This household is marked as Inactive.\n\nDo you still want to record a completed service for this household?",
                    "Inactive Household",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (inactiveResult == DialogResult.No)
                {
                    return;
                }
            }

            // Check eligibility
            var isEligible = EligibilityService.IsEligibleThisMonth(connection, _selectedHousehold.Id, DateTime.Today);

            string? overrideReason = null;
            string? overrideNotes = null;

            // If not eligible, show override modal
            if (!isEligible)
            {
                using var overrideForm = new OverrideReasonForm();
                if (overrideForm.ShowDialog() != DialogResult.OK)
                {
                    return; // User cancelled
                }

                overrideReason = overrideForm.OverrideReason;
                overrideNotes = overrideForm.Notes;
            }

            // Determine event type: check if today is a pantry day
            string eventType = "Appointment";
            var pantryDay = PantryDayRepository.GetByDate(connection, DateTime.Today);
            if (pantryDay != null && pantryDay.IsActive)
            {
                eventType = "PantryDay";
            }

            // Create service event
            var serviceEvent = new ServiceEvent
            {
                HouseholdId = _selectedHousehold.Id,
                EventType = eventType,
                EventStatus = "Completed",
                EventDate = DateTime.Today,
                OverrideReason = overrideReason,
                Notes = overrideNotes
            };

            ServiceEventRepository.Create(connection, serviceEvent);

            // Refresh search results
            SearchHouseholds(txtSearch.Text);

            MessageBox.Show("Service completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error completing service: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnNewHousehold_Click(object? sender, EventArgs e)
    {
        using var newHouseholdForm = new NewHouseholdForm();
        if (newHouseholdForm.ShowDialog() == DialogResult.OK)
        {
            // Refresh search to show new household
            SearchHouseholds(txtSearch.Text);
        }
    }

    private void BtnOpenProfile_Click(object? sender, EventArgs e)
    {
        if (_selectedHousehold == null)
        {
            return;
        }

        using var profileForm = new HouseholdProfileForm(_selectedHousehold.Id);
        if (profileForm.ShowDialog() == DialogResult.OK)
        {
            // Refresh search to show updated data
            SearchHouseholds(txtSearch.Text);
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

    private void MenuItemStatisticsDashboard_Click(object? sender, EventArgs e)
    {
        using var statsForm = new StatsForm();
        statsForm.ShowDialog();
    }

    private void MenuItemLogout_Click(object? sender, EventArgs e)
    {
        SessionManager.Logout();
        Application.Exit();
    }

    private void MenuItemBackupNow_Click(object? sender, EventArgs e)
    {
        try
        {
            PermissionChecker.RequireAdmin();

            Cursor = Cursors.WaitCursor;
            var backupPath = BackupService.CreateBackup();
            Cursor = Cursors.Default;

            MessageBox.Show(
                $"Backup created successfully.\n\nLocation: {backupPath}",
                "Backup Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show(
                "This action requires Admin privileges.",
                "Access Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            Cursor = Cursors.Default;
            MessageBox.Show(
                $"Failed to create backup:\n\n{ex.Message}",
                "Backup Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void MenuItemBackupToUsb_Click(object? sender, EventArgs e)
    {
        try
        {
            PermissionChecker.RequireAdmin();

            using var dialog = new FolderBrowserDialog
            {
                Description = "Select folder for backup (e.g., USB drive)",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var targetFolder = dialog.SelectedPath;

                // Validate folder is writable
                try
                {
                    var testFile = Path.Combine(targetFolder, "test_write.tmp");
                    File.WriteAllText(testFile, "test");
                    File.Delete(testFile);
                }
                catch
                {
                    MessageBox.Show(
                        "Selected folder is not writable. Please choose a different location.",
                        "Invalid Folder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Cursor = Cursors.WaitCursor;
                var backupPath = BackupService.CreateBackup(targetFolder);
                Cursor = Cursors.Default;

                MessageBox.Show(
                    $"Backup created successfully.\n\nLocation: {backupPath}",
                    "Backup Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show(
                "This action requires Admin privileges.",
                "Access Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            Cursor = Cursors.Default;
            MessageBox.Show(
                $"Failed to create backup:\n\n{ex.Message}",
                "Backup Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void MenuItemRestore_Click(object? sender, EventArgs e)
    {
        try
        {
            PermissionChecker.RequireAdmin();

            using var restoreForm = new BackupRestoreForm();
            if (restoreForm.ShowDialog() == DialogResult.OK)
            {
                // Restore completed, prompt for restart
                var result = MessageBox.Show(
                    "Database has been restored successfully.\n\n" +
                    "The application must restart to use the restored database.\n\n" +
                    "Do you want to restart now?",
                    "Restart Required",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show(
                "This action requires Admin privileges.",
                "Access Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Failed to restore backup:\n\n{ex.Message}",
                "Restore Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void MenuItemExport_Click(object? sender, EventArgs e)
    {
        try
        {
            PermissionChecker.RequireAdmin();

            using var exportForm = new ExportForm();
            exportForm.ShowDialog();
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show(
                "This action requires Admin privileges.",
                "Access Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Failed to export data:\n\n{ex.Message}",
                "Export Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
