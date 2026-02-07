using PantryDeskCore.Data;
using PantryDeskCore.Helpers;
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
    private System.Windows.Forms.Timer? _searchDebounceTimer;

    public CheckInForm()
    {
        InitializeComponent();
        UpdateMenuVisibility();
        SetupDataGridView();

        _searchDebounceTimer = new System.Windows.Forms.Timer();
        _searchDebounceTimer.Interval = 250;
        _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
        Disposed += (_, _) =>
        {
            _searchDebounceTimer?.Stop();
            _searchDebounceTimer?.Dispose();
            _searchDebounceTimer = null;
        };

        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void SearchDebounceTimer_Tick(object? sender, EventArgs e)
    {
        _searchDebounceTimer?.Stop();
        SearchHouseholds(txtSearch.Text);
    }

    private void CheckInForm_Load(object? sender, EventArgs e)
    {
        // Load all households on initial load
        SearchHouseholds(string.Empty);
        RefreshStatusBar();
        ShowManualBackupReminderIfNeeded();
    }

    private void ShowManualBackupReminderIfNeeded()
    {
        var lastManual = BackupService.GetLastManualBackupDate();
        if (lastManual.HasValue && (DateTime.Today - lastManual.Value).TotalDays < 7)
        {
            return;
        }
        using var reminderForm = new ManualBackupReminderForm(this);
        reminderForm.ShowDialog();
    }

    private void UpdateMenuVisibility()
    {
        menuAdmin.Visible = SessionManager.IsAdmin;
    }

    internal void RefreshStatusBar()
    {
        if (statusStrip == null || statusLabelRole == null || statusLabelBackup == null)
        {
            return;
        }
        statusLabelRole.Text = SessionManager.CurrentRole ?? string.Empty;
        var autoDate = BackupService.GetLastAutoBackupDate();
        var manualDate = BackupService.GetLastManualBackupDate();
        statusLabelBackup.Text = $"Last Auto Backup: {(autoDate.HasValue ? autoDate.Value.ToString("yyyy-MM-dd") : "No backup yet")}  Last Manual Backup: {(manualDate.HasValue ? manualDate.Value.ToString("yyyy-MM-dd") : "No backup yet")}";
    }

    private void SetupDataGridView()
    {
        dgvResults.Columns.Clear();
        // City/Zip in far-right column per client requirement
        dgvResults.Columns.Add("Name", "Name (Primary)");
        dgvResults.Columns.Add("Members", "Members");
        dgvResults.Columns.Add("Size", "Size");
        dgvResults.Columns.Add("Ages", "Age(s)");
        dgvResults.Columns.Add("LastService", "Last Service");
        dgvResults.Columns.Add("Eligibility", "Eligibility");
        dgvResults.Columns.Add("Status", "Status");
        dgvResults.Columns.Add("CityZip", "City/Zip");
        dgvResults.Columns.Add("HouseholdId", "HouseholdId");
        var householdIdColumn = dgvResults.Columns["HouseholdId"];
        if (householdIdColumn != null)
        {
            householdIdColumn.Visible = false;
        }

        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        // Fixed column widths (always respected; Members uses Fill; horizontal scrollbar if window too small)
        dgvResults.Columns[0].Width = 210;   // Name (Primary)
        dgvResults.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dgvResults.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;  // Members
        dgvResults.Columns[1].MinimumWidth = 200;
        dgvResults.Columns[2].Width = 45;    // Size
        dgvResults.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dgvResults.Columns[3].Width = 110;   // Age(s)
        dgvResults.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dgvResults.Columns[4].Width = 320;   // Last Service
        dgvResults.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dgvResults.Columns[5].Width = 150;   // Eligibility
        dgvResults.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dgvResults.Columns[6].Width = 75;    // Status
        dgvResults.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dgvResults.Columns[7].Width = 180;   // City/Zip
        dgvResults.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

        dgvResults.CellToolTipTextNeeded += DgvResults_CellToolTipTextNeeded;
    }

    private void DgvResults_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
        var colName = dgvResults.Columns[e.ColumnIndex].Name;
        if (colName == "Members" || colName == "LastService" || colName == "Eligibility" || colName == "Status" || colName == "CityZip")
        {
            var value = dgvResults.Rows[e.RowIndex].Cells[colName].Value?.ToString();
            if (!string.IsNullOrEmpty(value))
                e.ToolTipText = value;
        }
    }

    private void TxtSearch_TextChanged(object? sender, EventArgs e)
    {
        _searchDebounceTimer?.Stop();
        _searchDebounceTimer?.Start();
    }

    private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            _searchDebounceTimer?.Stop();
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
                    var typePart = string.IsNullOrWhiteSpace(lastService.VisitType)
                        ? lastService.EventType
                        : $"{lastService.EventType} - {lastService.VisitType}";
                    lastServiceText = $"{lastService.EventDate:yyyy-MM-dd} {typePart}";
                }

                // Check eligibility
                var isEligible = EligibilityService.IsEligibleThisMonth(connection, household.Id, DateTime.Today);
                string eligibilityText = isEligible ? "✅ Eligible" : "❌ Already Served";
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

                // Load members for primary name and members column
                var members = HouseholdMemberRepository.GetByHouseholdId(connection, household.Id);
                var primaryName = members.FirstOrDefault(m => m.IsPrimary)?.FullName ?? household.PrimaryName;
                var nonPrimary = members.Where(m => !m.IsPrimary)
                    .OrderBy(m => m.LastName).ThenBy(m => m.FirstName)
                    .Select(m => m.FullName)
                    .ToList();
                var membersText = string.Join(", ", nonPrimary);
                var totalSize = members.Count;

                // Size: total count. Ages: age-group breakdown (I/C/A/S)
                var today = DateTime.Today;
                var infants = members.Count(m => AgeGroupHelper.GetAgeGroup(m.Birthday, today) == "Infant");
                var children = members.Count(m => AgeGroupHelper.GetAgeGroup(m.Birthday, today) == "Child");
                var adults = members.Count(m => AgeGroupHelper.GetAgeGroup(m.Birthday, today) == "Adult");
                var seniors = members.Count(m => AgeGroupHelper.GetAgeGroup(m.Birthday, today) == "Senior");
                string sizeText = totalSize.ToString();
                string agesText = $"{infants}I/{children}C/{adults}A/{seniors}S";

                // Status
                string statusText = household.IsActive ? "Active" : "Inactive";
                var statusColor = household.IsActive ? Color.Black : Color.Gray;

                // Add row (column order: Name, Members, Size, Ages, LastService, Eligibility, Status, CityZip, HouseholdId)
                var row = dgvResults.Rows.Add(
                    primaryName,
                    membersText,
                    sizeText,
                    agesText,
                    lastServiceText,
                    eligibilityText,
                    statusText,
                    cityZip,
                    household.Id
                );

                var rowObj = dgvResults.Rows[row];
                rowObj.Cells["Eligibility"].Style.ForeColor = eligibilityColor;
                rowObj.Cells["Status"].Style.ForeColor = statusColor;
                rowObj.Cells["Size"].Style.Font = new Font(dgvResults.Font, FontStyle.Bold);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error searching households: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
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

        // Show Complete Service dialog first (Visit Type + Notes)
        string visitType;
        string? dialogNotes;
        using (var completeServiceDialog = new CompleteServiceDialog())
        {
            if (completeServiceDialog.ShowDialog() != DialogResult.OK)
            {
                return; // User cancelled
            }
            visitType = completeServiceDialog.VisitType;
            dialogNotes = completeServiceDialog.Notes;
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

            string? overrideReason = null;
            string? overrideNotes = null;

            // Only Shop with TEFAP and Shop count toward monthly limit; check eligibility for those
            var countsTowardLimit = visitType == "Shop with TEFAP" || visitType == "Shop";
            if (countsTowardLimit)
            {
                var isEligible = EligibilityService.IsEligibleThisMonth(connection, _selectedHousehold.Id, DateTime.Today);

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
            }

            // Merge notes: dialog notes + override notes when applicable
            string? notes = dialogNotes;
            if (!string.IsNullOrWhiteSpace(overrideNotes))
            {
                notes = string.IsNullOrWhiteSpace(dialogNotes)
                    ? overrideNotes
                    : $"{dialogNotes}\n\nOverride: {overrideNotes}";
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
                VisitType = visitType,
                OverrideReason = overrideReason,
                Notes = notes
            };

            ServiceEventRepository.Create(connection, serviceEvent);
            HouseholdRepository.SetIsActive(connection, _selectedHousehold.Id, true);

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

    private void MenuItemActiveStatusSettings_Click(object? sender, EventArgs e)
    {
        try
        {
            PermissionChecker.RequireAdmin();
            using var form = new ActiveStatusSettingsForm();
            form.ShowDialog();
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show(
                "This action requires Admin privileges.",
                "Access Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void MenuItemStatisticsDashboard_Click(object? sender, EventArgs e)
    {
        using var statsForm = new StatsForm();
        statsForm.ShowDialog();
    }

    private void MenuItemSwitchRole_Click(object? sender, EventArgs e)
    {
        using var loginForm = new LoginForm();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            UpdateMenuVisibility();
            RefreshStatusBar();
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
                var backupPath = BackupService.CreateBackup(targetFolder, passphrase: null, isAutomatic: false);
                Cursor = Cursors.Default;

                MessageBox.Show(
                    $"Backup created successfully.\n\nLocation: {backupPath}",
                    "Backup Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                RefreshStatusBar();
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
