using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form for viewing and managing appointments: Past (Completed/Cancelled/NoShow) and Future (Scheduled).
/// </summary>
public partial class AppointmentsForm : Form
{
    private ContextMenuStrip? _contextMenuFuture;
    private ContextMenuStrip? _contextMenuPast;

    public AppointmentsForm()
    {
        InitializeComponent();
        SetupPastGrid();
        SetupFutureGrid();
        SetupFilters();
        SetupContextMenu();

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void SetupPastGrid()
    {
        dgvPast.Columns.Clear();
        dgvPast.Columns.Add("EventDate", "Date");
        dgvPast.Columns.Add("PrimaryName", "Household");
        dgvPast.Columns.Add("EventStatus", "Status");
        dgvPast.Columns.Add("Notes", "Notes");
        dgvPast.Columns.Add("Id", "Id");
        dgvPast.Columns.Add("HouseholdId", "HouseholdId");
        var idCol = dgvPast.Columns["Id"];
        var hhCol = dgvPast.Columns["HouseholdId"];
        if (idCol != null) idCol.Visible = false;
        if (hhCol != null) hhCol.Visible = false;
        dgvPast.Columns["EventDate"]!.Width = 43;
        dgvPast.Columns["PrimaryName"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dgvPast.Columns["EventStatus"]!.Width = 80;
        dgvPast.Columns["Notes"]!.Width = 120;
    }

    private void SetupFutureGrid()
    {
        dgvFuture.Columns.Clear();
        dgvFuture.Columns.Add("EventDate", "Date");
        dgvFuture.Columns.Add("PrimaryName", "Household");
        dgvFuture.Columns.Add("ScheduledText", "Scheduled Text");
        dgvFuture.Columns.Add("Notes", "Notes");
        dgvFuture.Columns.Add("Id", "Id");
        dgvFuture.Columns.Add("HouseholdId", "HouseholdId");
        var idCol = dgvFuture.Columns["Id"];
        var hhCol = dgvFuture.Columns["HouseholdId"];
        if (idCol != null) idCol.Visible = false;
        if (hhCol != null) hhCol.Visible = false;
        dgvFuture.Columns["EventDate"]!.Width = 43;
        dgvFuture.Columns["PrimaryName"]!.Width = 130;
        dgvFuture.Columns["ScheduledText"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dgvFuture.Columns["Notes"]!.Width = 120;
    }

    private void SetupFilters()
    {
        dtpDateFrom.Value = DateTime.Today.AddYears(-1);
        dtpDateTo.Value = DateTime.Today.AddMonths(3);
        cmbFilterStatus.Items.AddRange(new object[] { "All", "Completed", "Cancelled", "NoShow" });
        cmbFilterStatus.SelectedIndex = 0;
    }

    private void SetupContextMenu()
    {
        _contextMenuFuture = new ContextMenuStrip();
        var editFuture = new ToolStripMenuItem("Edit");
        editFuture.Click += EditFuture_Click;
        _contextMenuFuture.Items.Add(editFuture);
        dgvFuture.ContextMenuStrip = _contextMenuFuture;

        _contextMenuPast = new ContextMenuStrip();
        var editPast = new ToolStripMenuItem("Edit");
        editPast.Click += EditPast_Click;
        _contextMenuPast.Items.Add(editPast);
        dgvPast.ContextMenuStrip = _contextMenuPast;
    }

    private void AppointmentsForm_Load(object? sender, EventArgs e)
    {
        LoadData();
    }

    private void Filter_Changed(object? sender, EventArgs e)
    {
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            var startDate = dtpDateFrom.Value.Date;
            var endDate = dtpDateTo.Value.Date;
            var statusFilter = cmbFilterStatus.SelectedItem?.ToString() ?? "All";

            var past = ServiceEventRepository.GetAppointmentsPast(connection, startDate, endDate, statusFilter);
            var scheduled = ServiceEventRepository.GetAppointmentsScheduled(connection);

            dgvPast.Rows.Clear();
            foreach (var row in past)
            {
                var idx = dgvPast.Rows.Add(
                    row.EventDate.ToString("yyyy-MM-dd"),
                    row.DisplayName,
                    row.EventStatus,
                    row.Notes ?? string.Empty,
                    row.Id,
                    row.HouseholdId
                );
                var dgvRow = dgvPast.Rows[idx];
                dgvRow.Tag = row;
                dgvRow.DefaultCellStyle.ForeColor = row.EventStatus == "Completed" ? Color.Green : Color.Red;
            }

            dgvFuture.Rows.Clear();
            var today = DateTime.Today;
            foreach (var row in scheduled)
            {
                var idx = dgvFuture.Rows.Add(
                    row.EventDate.ToString("yyyy-MM-dd"),
                    row.DisplayName,
                    row.ScheduledText ?? string.Empty,
                    row.Notes ?? string.Empty,
                    row.Id,
                    row.HouseholdId
                );
                var dgvRow = dgvFuture.Rows[idx];
                dgvRow.Tag = row;
                if (row.EventDate < today)
                {
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCreateNew_Click(object? sender, EventArgs e)
    {
        using var dlg = new CreateAppointmentDialog();
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            LoadData();
        }
    }

    private AppointmentRow? GetSelectedFutureRow()
    {
        if (dgvFuture.SelectedRows.Count == 0) return null;
        return dgvFuture.SelectedRows[0].Tag as AppointmentRow;
    }

    private void BtnMarkComplete_Click(object? sender, EventArgs e) => MarkCompleted_Click(sender, e);
    private void BtnMarkCancelled_Click(object? sender, EventArgs e) => MarkCancelled_Click(sender, e);
    private void BtnMarkNoShow_Click(object? sender, EventArgs e) => MarkNoShow_Click(sender, e);

    private void EditFuture_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedFutureRow();
        if (row == null)
        {
            MessageBox.Show("Please select an appointment.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        EditAppointment(row.Id);
    }

    private void EditPast_Click(object? sender, EventArgs e)
    {
        if (dgvPast.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select an appointment.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var row = dgvPast.SelectedRows[0].Tag as AppointmentRow;
        if (row == null) return;
        EditAppointment(row.Id);
    }

    private void EditAppointment(int eventId)
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            var serviceEvent = ServiceEventRepository.GetById(connection, eventId);
            if (serviceEvent == null)
            {
                MessageBox.Show("Event not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using var dlg = new EditServiceEventDialog(serviceEvent);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void MarkCompleted_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedFutureRow();
        if (row == null)
        {
            MessageBox.Show("Please select a scheduled appointment.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        MarkAppointmentStatus(row, "Completed");
    }

    private void MarkCancelled_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedFutureRow();
        if (row == null)
        {
            MessageBox.Show("Please select a scheduled appointment.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        MarkAppointmentStatus(row, "Cancelled");
    }

    private void MarkNoShow_Click(object? sender, EventArgs e)
    {
        var row = GetSelectedFutureRow();
        if (row == null)
        {
            MessageBox.Show("Please select a scheduled appointment.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        MarkAppointmentStatus(row, "NoShow");
    }

    private void MarkAppointmentStatus(AppointmentRow row, string newStatus)
    {
        string? visitType = null;
        string? dialogNotes = null;

        if (newStatus == "Completed")
        {
            using (var completeServiceDialog = new CompleteServiceDialog())
            {
                if (completeServiceDialog.ShowDialog(this) != DialogResult.OK)
                    return;
                visitType = completeServiceDialog.VisitType;
                dialogNotes = completeServiceDialog.Notes;
            }
        }

        try
        {
            using var connection = DatabaseManager.GetConnection();
            var serviceEvent = ServiceEventRepository.GetById(connection, row.Id);
            if (serviceEvent == null)
            {
                MessageBox.Show("Appointment not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string? overrideReason = null;
            string? overrideNotes = null;

            if (newStatus == "Completed")
            {
                var countsTowardLimit = visitType == "Shop with TEFAP" || visitType == "Shop";
                if (countsTowardLimit)
                {
                    var isEligible = EligibilityService.IsEligibleThisMonth(connection, row.HouseholdId, DateTime.Today);
                    if (!isEligible)
                    {
                        using var overrideForm = new OverrideReasonForm();
                        if (overrideForm.ShowDialog(this) != DialogResult.OK)
                            return;
                        overrideReason = overrideForm.OverrideReason;
                        overrideNotes = overrideForm.Notes;
                    }
                }

                var notes = dialogNotes;
                if (!string.IsNullOrWhiteSpace(overrideNotes))
                {
                    notes = string.IsNullOrWhiteSpace(dialogNotes)
                        ? overrideNotes
                        : $"{dialogNotes}\n\n- Override: {overrideNotes}";
                }
                if (!string.IsNullOrWhiteSpace(serviceEvent.Notes) && !string.IsNullOrWhiteSpace(notes))
                {
                    notes = $"{serviceEvent.Notes}\n\n- {notes}";
                }
                else if (!string.IsNullOrWhiteSpace(serviceEvent.Notes))
                {
                    notes = serviceEvent.Notes;
                }

                serviceEvent.VisitType = visitType;
                serviceEvent.Notes = notes;
                serviceEvent.EventDate = DateTime.Today;
            }

            serviceEvent.EventStatus = newStatus;
            serviceEvent.OverrideReason = overrideReason;
            ServiceEventRepository.Update(connection, serviceEvent);

            if (newStatus == "Completed")
            {
                HouseholdRepository.SetIsActive(connection, row.HouseholdId, true);
            }

            LoadData();
            MessageBox.Show($"Appointment marked as {newStatus}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating appointment status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
