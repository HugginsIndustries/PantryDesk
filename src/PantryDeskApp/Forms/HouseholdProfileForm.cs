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
        SetupMembersGrid();
        SetupServiceHistoryGrid();
        SetupFilters();
        SetupContextMenu();
        
        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
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
        dgvServiceHistory.Columns.Add("Member", "Member");
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
        var memberColumn = dgvServiceHistory.Columns["Member"];
        var scheduledTextColumn = dgvServiceHistory.Columns["ScheduledText"];
        var notesColumn = dgvServiceHistory.Columns["Notes"];

        if (dateColumn != null) dateColumn.Width = 80;
        if (typeColumn != null) typeColumn.Width = 190;
        if (statusColumn != null) statusColumn.Width = 80;
        if (memberColumn != null) memberColumn.Width = 160;
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

    private void SetupMembersGrid()
    {
        grdMembers.Columns.Clear();
        grdMembers.Columns.Add("FirstName", "First Name");
        grdMembers.Columns.Add("LastName", "Last Name");
        grdMembers.Columns.Add("Birthday", "Birthday");
        grdMembers.Columns.Add("Primary", "Primary");
        grdMembers.Columns.Add("Race", "Race");
        grdMembers.Columns.Add("Veteran", "Veteran");
        grdMembers.Columns.Add("Disabled", "Disabled");

        // AllCells for content-fit; Fill for last column so table fills available width
        for (var i = 0; i < grdMembers.Columns.Count - 1; i++)
        {
            grdMembers.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
        var disabledCol = grdMembers.Columns["Disabled"];
        if (disabledCol != null)
            disabledCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    private void RefreshMembersGrid()
    {
        grdMembers.Rows.Clear();
        if (_household?.Members == null) return;
        foreach (var m in _household.Members)
        {
            var idx = grdMembers.Rows.Add(
                m.FirstName,
                m.LastName,
                m.Birthday.ToString("yyyy-MM-dd"),
                m.IsPrimary ? "Yes" : "",
                m.Race ?? "",
                m.VeteranStatus ?? "",
                m.DisabledStatus ?? ""
            );
            grdMembers.Rows[idx].Tag = m;
        }
    }

    private void BtnAddMember_Click(object? sender, EventArgs e)
    {
        if (_household == null) return;
        using var form = new MemberEditForm(null, true);
        if (form.ShowDialog() != DialogResult.OK) return;

        var member = form.Member;
        member.HouseholdId = _householdId;
        if (member.IsPrimary)
        {
            foreach (var m in _household.Members)
                m.IsPrimary = false;
        }
        if (_household.Members.Count == 0)
            member.IsPrimary = true;

        _household.Members.Add(member);
        RefreshMembersGrid();
    }

    private void BtnEditMember_Click(object? sender, EventArgs e)
    {
        if (grdMembers.SelectedRows.Count == 0 || _household == null)
        {
            MessageBox.Show("Please select a member to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var existing = (HouseholdMember)grdMembers.SelectedRows[0].Tag!;
        using var form = new MemberEditForm(existing, true);
        if (form.ShowDialog() != DialogResult.OK) return;

        var member = form.Member;
        if (member.IsPrimary && !existing.IsPrimary)
        {
            foreach (var m in _household.Members)
                m.IsPrimary = false;
        }
        member.Id = existing.Id;
        member.HouseholdId = _householdId;
        var idx = _household.Members.IndexOf(existing);
        _household.Members[idx] = member;
        RefreshMembersGrid();
    }

    private void BtnRemoveMember_Click(object? sender, EventArgs e)
    {
        if (grdMembers.SelectedRows.Count == 0 || _household == null)
        {
            MessageBox.Show("Please select a member to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var member = (HouseholdMember)grdMembers.SelectedRows[0].Tag!;
        var wasPrimary = member.IsPrimary;
        _household.Members.Remove(member);
        if (wasPrimary && _household.Members.Count > 0)
            _household.Members[0].IsPrimary = true;
        RefreshMembersGrid();
    }

    private void BtnSetPrimary_Click(object? sender, EventArgs e)
    {
        if (grdMembers.SelectedRows.Count == 0 || _household == null)
        {
            MessageBox.Show("Please select a member to set as primary.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var member = (HouseholdMember)grdMembers.SelectedRows[0].Tag!;
        foreach (var m in _household.Members)
            m.IsPrimary = false;
        member.IsPrimary = true;
        RefreshMembersGrid();
    }

    private void SetupContextMenu()
    {
        _contextMenu = new ContextMenuStrip();
        var editItem = new ToolStripMenuItem("Edit");
        editItem.Click += EditServiceHistory_Click;
        _contextMenu.Items.Add(editItem);
        dgvServiceHistory.ContextMenuStrip = _contextMenu;
    }

    private void EditServiceHistory_Click(object? sender, EventArgs e)
    {
        var selectedEvent = GetSelectedEvent();
        if (selectedEvent == null)
        {
            MessageBox.Show("Please select an event.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        try
        {
            using var dlg = new EditServiceEventDialog(selectedEvent);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                LoadServiceHistory();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
            txtNotes.Text = _household.Notes ?? string.Empty;
            lblStatusValue.Text = _household.IsActive ? "Active" : "Inactive";

            RefreshMembersGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading household: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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

        if (_household == null)
        {
            ShowError("Household data not loaded.");
            return;
        }

        if (_household.Members.Count == 0)
        {
            ShowError("At least one household member is required.");
            return;
        }

        var primary = _household.Members.FirstOrDefault(m => m.IsPrimary);
        if (primary == null)
        {
            ShowError("One member must be set as primary contact.");
            return;
        }

        try
        {
            _household.PrimaryName = primary.FullName;
            _household.Address1 = string.IsNullOrWhiteSpace(txtAddress1.Text) ? null : txtAddress1.Text.Trim();
            _household.City = string.IsNullOrWhiteSpace(txtCity.Text) ? null : txtCity.Text.Trim();
            _household.State = string.IsNullOrWhiteSpace(txtState.Text) ? null : txtState.Text.Trim();
            _household.Zip = string.IsNullOrWhiteSpace(txtZip.Text) ? null : txtZip.Text.Trim();
            _household.Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
            _household.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            _household.ChildrenCount = 0;
            _household.AdultsCount = 0;
            _household.SeniorsCount = 0;
            _household.Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            using var connection = DatabaseManager.GetConnection();
            HouseholdRepository.Update(connection, _household);

            HouseholdMemberRepository.DeleteByHouseholdId(connection, _householdId);
            foreach (var member in _household.Members)
            {
                member.HouseholdId = _householdId;
                HouseholdMemberRepository.Create(connection, member);
            }

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

    private string GetMemberDisplayForEvent(ServiceEvent evt, string primaryName)
    {
        if (!evt.ScheduledForMemberId.HasValue || _household?.Members == null)
            return evt.EventType == "Appointment" ? primaryName : string.Empty;

        var member = _household.Members.FirstOrDefault(m => m.Id == evt.ScheduledForMemberId.Value);
        if (member == null)
            return primaryName;

        return member.IsPrimary ? member.FullName : $"{member.FullName} • {primaryName} Household";
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

        var primaryName = _household?.PrimaryName ?? string.Empty;
        foreach (var evt in filteredEvents)
        {
            var scheduledText = evt.EventStatus == "Scheduled" ? evt.ScheduledText ?? string.Empty : string.Empty;
            var notes = evt.Notes ?? string.Empty;
            var typeDisplay = string.IsNullOrWhiteSpace(evt.VisitType)
                ? evt.EventType
                : $"{evt.EventType} - {evt.VisitType}";

            var memberDisplay = GetMemberDisplayForEvent(evt, primaryName);

            var row = dgvServiceHistory.Rows.Add(
                evt.EventDate.ToString("yyyy-MM-dd"),
                typeDisplay,
                evt.EventStatus,
                memberDisplay,
                scheduledText,
                notes,
                evt.Id
            );
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

}
