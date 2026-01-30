using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Security;
using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form for viewing and editing household profile, service history, and appointments.
/// </summary>
public partial class HouseholdProfileForm : Form
{
    private readonly int _householdId;
    private Household? _household;
    private List<ServiceEvent> _allServiceEvents = new();
    private ContextMenuStrip? _contextMenu;

    public HouseholdProfileForm(int householdId)
    {
        _householdId = householdId;
        InitializeComponent();
        SetupServiceHistoryGrid();
        SetupFilters();
        SetupContextMenu();
    }

    private void HouseholdProfileForm_Load(object? sender, EventArgs e)
    {
        LoadHousehold();
        LoadServiceHistory();
    }

    private void SetupServiceHistoryGrid()
    {
        dgvServiceHistory.Columns.Clear();
        dgvServiceHistory.Columns.Add("EventDate", "Date");
        dgvServiceHistory.Columns.Add("EventType", "Type");
        dgvServiceHistory.Columns.Add("EventStatus", "Status");
        dgvServiceHistory.Columns.Add("ScheduledText", "Scheduled Text");
        dgvServiceHistory.Columns.Add("Notes", "Notes");
        dgvServiceHistory.Columns.Add("EventId", "EventId");
        var eventIdColumn = dgvServiceHistory.Columns["EventId"];
        if (eventIdColumn != null)
        {
            eventIdColumn.Visible = false;
        }

        var dateColumn = dgvServiceHistory.Columns["EventDate"];
        var typeColumn = dgvServiceHistory.Columns["EventType"];
        var statusColumn = dgvServiceHistory.Columns["EventStatus"];
        var scheduledTextColumn = dgvServiceHistory.Columns["ScheduledText"];
        var notesColumn = dgvServiceHistory.Columns["Notes"];

        if (dateColumn != null) dateColumn.Width = 100;
        if (typeColumn != null) typeColumn.Width = 100;
        if (statusColumn != null) statusColumn.Width = 100;
        if (scheduledTextColumn != null) scheduledTextColumn.Width = 200;
        if (notesColumn != null) notesColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    private void SetupFilters()
    {
        cmbFilterStatus.Items.AddRange(new object[] { "All", "Completed", "Scheduled", "Cancelled", "NoShow" });
        cmbFilterStatus.SelectedIndex = 0;

        cmbFilterType.Items.AddRange(new object[] { "All", "PantryDay", "Appointment" });
        cmbFilterType.SelectedIndex = 0;
    }

    private void SetupContextMenu()
    {
        _contextMenu = new ContextMenuStrip();
        var markCompleted = new ToolStripMenuItem("Mark Completed");
        markCompleted.Click += MarkCompleted_Click;
        _contextMenu.Items.Add(markCompleted);

        var markCancelled = new ToolStripMenuItem("Mark Cancelled");
        markCancelled.Click += MarkCancelled_Click;
        _contextMenu.Items.Add(markCancelled);

        var markNoShow = new ToolStripMenuItem("Mark NoShow");
        markNoShow.Click += MarkNoShow_Click;
        _contextMenu.Items.Add(markNoShow);

        dgvServiceHistory.ContextMenuStrip = _contextMenu;
    }

    private void LoadHousehold()
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            _household = HouseholdRepository.GetById(connection, _householdId);

            if (_household == null)
            {
                MessageBox.Show("Household not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            // Populate form fields
            txtPrimaryName.Text = _household.PrimaryName;
            txtAddress1.Text = _household.Address1 ?? string.Empty;
            txtCity.Text = _household.City ?? string.Empty;
            txtState.Text = _household.State ?? string.Empty;
            txtZip.Text = _household.Zip ?? string.Empty;
            txtPhone.Text = _household.Phone ?? string.Empty;
            txtEmail.Text = _household.Email ?? string.Empty;
            numChildren.Value = _household.ChildrenCount;
            numAdults.Value = _household.AdultsCount;
            numSeniors.Value = _household.SeniorsCount;
            txtNotes.Text = _household.Notes ?? string.Empty;
            chkIsActive.Checked = _household.IsActive;

            UpdateTotalSize();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading household: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateTotalSize()
    {
        var total = (int)numChildren.Value + (int)numAdults.Value + (int)numSeniors.Value;
        lblTotalSizeValue.Text = total.ToString();
    }

    private void NumCount_ValueChanged(object? sender, EventArgs e)
    {
        UpdateTotalSize();
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        SaveHousehold();
    }

    private void SaveHousehold()
    {
        // Clear previous errors
        lblError.Text = string.Empty;
        lblError.Visible = false;

        // Validate PrimaryName
        if (string.IsNullOrWhiteSpace(txtPrimaryName.Text))
        {
            ShowError("Primary Name is required.");
            txtPrimaryName.Focus();
            return;
        }

        // Validate at least one person in household
        var childrenCount = (int)numChildren.Value;
        var adultsCount = (int)numAdults.Value;
        var seniorsCount = (int)numSeniors.Value;

        if (childrenCount == 0 && adultsCount == 0 && seniorsCount == 0)
        {
            ShowError("At least one person (child, adult, or senior) must be specified.");
            numChildren.Focus();
            return;
        }

        if (_household == null)
        {
            ShowError("Household data not loaded.");
            return;
        }

        try
        {
            // Update household object
            _household.PrimaryName = txtPrimaryName.Text.Trim();
            _household.Address1 = string.IsNullOrWhiteSpace(txtAddress1.Text) ? null : txtAddress1.Text.Trim();
            _household.City = string.IsNullOrWhiteSpace(txtCity.Text) ? null : txtCity.Text.Trim();
            _household.State = string.IsNullOrWhiteSpace(txtState.Text) ? null : txtState.Text.Trim();
            _household.Zip = string.IsNullOrWhiteSpace(txtZip.Text) ? null : txtZip.Text.Trim();
            _household.Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
            _household.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            _household.ChildrenCount = childrenCount;
            _household.AdultsCount = adultsCount;
            _household.SeniorsCount = seniorsCount;
            _household.Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();
            _household.IsActive = chkIsActive.Checked;

            using var connection = DatabaseManager.GetConnection();
            HouseholdRepository.Update(connection, _household);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            ShowError($"Error saving household: {ex.Message}");
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
    }

    private void LoadServiceHistory()
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            _allServiceEvents = ServiceEventRepository.GetByHouseholdId(connection, _householdId);
            ApplyFilters();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading service history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Filter_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        dgvServiceHistory.Rows.Clear();

        var statusFilter = cmbFilterStatus.SelectedItem?.ToString() ?? "All";
        var typeFilter = cmbFilterType.SelectedItem?.ToString() ?? "All";

        var filteredEvents = _allServiceEvents.Where(e =>
        {
            var statusMatch = statusFilter == "All" || e.EventStatus == statusFilter;
            var typeMatch = typeFilter == "All" || e.EventType == typeFilter;
            return statusMatch && typeMatch;
        }).ToList();

        foreach (var evt in filteredEvents)
        {
            var scheduledText = evt.EventStatus == "Scheduled" ? evt.ScheduledText ?? string.Empty : string.Empty;
            var notes = evt.Notes ?? string.Empty;

            var row = dgvServiceHistory.Rows.Add(
                evt.EventDate.ToString("yyyy-MM-dd"),
                evt.EventType,
                evt.EventStatus,
                scheduledText,
                notes,
                evt.Id
            );
        }
    }

    private void BtnSchedule_Click(object? sender, EventArgs e)
    {
        ScheduleAppointment();
    }

    private void ScheduleAppointment()
    {
        // Validate ScheduledDate
        var scheduledDate = dtpScheduledDate.Value.Date;

        // Validate ScheduledText
        if (string.IsNullOrWhiteSpace(txtScheduledText.Text))
        {
            MessageBox.Show("Scheduled Text is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtScheduledText.Focus();
            return;
        }

        try
        {
            var serviceEvent = new ServiceEvent
            {
                HouseholdId = _householdId,
                EventType = "Appointment",
                EventStatus = "Scheduled",
                EventDate = scheduledDate,
                ScheduledText = txtScheduledText.Text.Trim(),
                Notes = string.IsNullOrWhiteSpace(txtAppointmentNotes.Text) ? null : txtAppointmentNotes.Text.Trim()
            };

            using var connection = DatabaseManager.GetConnection();
            ServiceEventRepository.Create(connection, serviceEvent);

            // Clear form fields
            txtScheduledText.Clear();
            txtAppointmentNotes.Clear();
            dtpScheduledDate.Value = DateTime.Today;

            // Refresh service history
            LoadServiceHistory();

            MessageBox.Show("Appointment scheduled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error scheduling appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private ServiceEvent? GetSelectedEvent()
    {
        if (dgvServiceHistory.SelectedRows.Count == 0)
        {
            return null;
        }

        var selectedRow = dgvServiceHistory.SelectedRows[0];
        var eventIdValue = selectedRow.Cells["EventId"].Value;
        if (eventIdValue == null)
        {
            return null;
        }

        var eventId = (int)eventIdValue;
        return _allServiceEvents.FirstOrDefault(e => e.Id == eventId);
    }

    private void MarkCompleted_Click(object? sender, EventArgs e)
    {
        var selectedEvent = GetSelectedEvent();
        if (selectedEvent == null || selectedEvent.EventStatus != "Scheduled")
        {
            MessageBox.Show("Please select a Scheduled appointment.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        MarkAppointmentStatus(selectedEvent, "Completed");
    }

    private void MarkCancelled_Click(object? sender, EventArgs e)
    {
        var selectedEvent = GetSelectedEvent();
        if (selectedEvent == null || selectedEvent.EventStatus != "Scheduled")
        {
            MessageBox.Show("Please select a Scheduled appointment.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        MarkAppointmentStatus(selectedEvent, "Cancelled");
    }

    private void MarkNoShow_Click(object? sender, EventArgs e)
    {
        var selectedEvent = GetSelectedEvent();
        if (selectedEvent == null || selectedEvent.EventStatus != "Scheduled")
        {
            MessageBox.Show("Please select a Scheduled appointment.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        MarkAppointmentStatus(selectedEvent, "NoShow");
    }

    private void MarkAppointmentStatus(ServiceEvent serviceEvent, string newStatus)
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();

            string? overrideReason = null;
            string? overrideNotes = null;

            // If marking as Completed, check eligibility and handle override if needed
            if (newStatus == "Completed")
            {
                var isEligible = EligibilityService.IsEligibleThisMonth(connection, _householdId, DateTime.Today);

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

                // Set EventDate to today (completion date)
                serviceEvent.EventDate = DateTime.Today;
            }

            // Update the event
            serviceEvent.EventStatus = newStatus;
            serviceEvent.OverrideReason = overrideReason;
            
            // Preserve existing notes and append override notes if provided
            if (!string.IsNullOrWhiteSpace(overrideNotes))
            {
                if (!string.IsNullOrWhiteSpace(serviceEvent.Notes))
                {
                    // Append override notes to existing notes
                    serviceEvent.Notes = $"{serviceEvent.Notes}\n\nOverride: {overrideNotes}";
                }
                else
                {
                    // No existing notes, use override notes
                    serviceEvent.Notes = overrideNotes;
                }
            }

            ServiceEventRepository.Update(connection, serviceEvent);

            // Refresh service history
            LoadServiceHistory();

            MessageBox.Show($"Appointment marked as {newStatus}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating appointment status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
