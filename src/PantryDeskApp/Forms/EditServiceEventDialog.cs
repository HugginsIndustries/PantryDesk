using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

/// <summary>
/// Dialog for editing service events (PantryDay or Appointment). Supports all editable fields.
/// </summary>
public partial class EditServiceEventDialog : Form
{
    private readonly ServiceEvent _event;
    private List<HouseholdMember> _members = new();

    public EditServiceEventDialog(ServiceEvent serviceEvent)
    {
        _event = serviceEvent ?? throw new ArgumentNullException(nameof(serviceEvent));
        InitializeComponent();
        LoadMembers();
        PopulateFields();

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
            this.Icon = new Icon(iconPath);
    }

    private void LoadMembers()
    {
        if (_event.EventType != "Appointment")
            return;

        try
        {
            using var connection = DatabaseManager.GetConnection();
            _members = HouseholdMemberRepository.GetByHouseholdId(connection, _event.HouseholdId);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading household members: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void PopulateFields()
    {
        Text = $"Edit {_event.EventType}";
        lblEventTypeValue.Text = _event.EventType;

        dtpEventDate.Value = _event.EventDate.Date;
        dtpEventDate.Enabled = _event.EventType == "Appointment";

        txtNotes.Text = _event.Notes ?? string.Empty;

        grpAppointment.Visible = _event.EventType == "Appointment";
        if (_event.EventType == "Appointment")
        {
            cmbStatus.Items.AddRange(new object[] { "Scheduled", "Completed", "Cancelled", "NoShow" });
            cmbStatus.SelectedItem = _event.EventStatus;

            txtScheduledText.Text = _event.ScheduledText ?? string.Empty;

            cmbMember.Items.Clear();
            var primary = _members.FirstOrDefault(m => m.IsPrimary);
            var primaryName = primary?.FullName ?? (_members.Count > 0 ? _members[0].FullName : string.Empty);
            foreach (var m in _members)
            {
                var display = m.IsPrimary ? m.FullName : $"{m.FullName} • {primaryName} Household";
                cmbMember.Items.Add(new MemberItem(m.Id, display, m.IsPrimary));
            }
            for (int i = 0; i < cmbMember.Items.Count; i++)
            {
                var item = cmbMember.Items[i] as MemberItem;
                if (item != null && item.Id == _event.ScheduledForMemberId)
                {
                    cmbMember.SelectedIndex = i;
                    break;
                }
            }
            if (cmbMember.SelectedIndex < 0 && cmbMember.Items.Count > 0)
                cmbMember.SelectedIndex = 0;

            cmbVisitType.Items.AddRange(new object[] { "Shop with TEFAP", "Shop", "TEFAP Only", "Deck Only" });
            if (!string.IsNullOrEmpty(_event.VisitType))
                cmbVisitType.SelectedItem = _event.VisitType;
            else if (cmbVisitType.Items.Count > 0)
                cmbVisitType.SelectedIndex = 0;

            pnlVisitType.Visible = _event.EventStatus == "Completed";
            pnlVisitType.Location = new Point(12, 333);
        }
        else
        {
            cmbVisitType.Items.AddRange(new object[] { "Shop with TEFAP", "Shop", "TEFAP Only", "Deck Only" });
            if (!string.IsNullOrEmpty(_event.VisitType))
                cmbVisitType.SelectedItem = _event.VisitType;
            else if (cmbVisitType.Items.Count > 0)
                cmbVisitType.SelectedIndex = 0;
            pnlVisitType.Location = new Point(12, 95);
            pnlVisitType.Visible = true;
            lblNotes.Location = new Point(12, 165);
            txtNotes.Location = new Point(12, 183);
            pnlOverride.Location = new Point(12, 260);
        }

        pnlOverride.Visible = !string.IsNullOrEmpty(_event.OverrideReason);
        cmbOverrideReason.Items.AddRange(new object[] { "Special Circumstance", "Emergency Need", "Admin Override", "Other" });
        if (!string.IsNullOrEmpty(_event.OverrideReason))
            cmbOverrideReason.SelectedItem = _event.OverrideReason;
        else if (cmbOverrideReason.Items.Count > 0)
            cmbOverrideReason.SelectedIndex = 0;
    }

    private void CmbStatus_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_event.EventType != "Appointment") return;
        pnlVisitType.Visible = cmbStatus.SelectedItem?.ToString() == "Completed";
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_event.EventType == "Appointment")
        {
            if (string.IsNullOrWhiteSpace(txtScheduledText.Text))
            {
                MessageBox.Show("Scheduled Text is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtScheduledText.Focus();
                return;
            }
            if (cmbMember.SelectedItem == null)
            {
                MessageBox.Show("Please select a household member.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMember.Focus();
                return;
            }
            if (cmbStatus.SelectedItem?.ToString() == "Completed" && cmbVisitType.SelectedItem == null)
            {
                MessageBox.Show("Visit Type is required when status is Completed.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbVisitType.Focus();
                return;
            }
        }
        else
        {
            if (cmbVisitType.SelectedItem == null)
            {
                MessageBox.Show("Visit Type is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbVisitType.Focus();
                return;
            }
        }

        string? overrideReason = null;
        string? overrideNotes = null;

        if (_event.EventType == "Appointment" && cmbStatus.SelectedItem?.ToString() == "Completed")
        {
            var visitType = cmbVisitType.SelectedItem?.ToString();
            var countsTowardLimit = visitType == "Shop with TEFAP" || visitType == "Shop";
            if (countsTowardLimit)
            {
                using var connection = DatabaseManager.GetConnection();
                var eventDate = dtpEventDate.Value.Date;
                var isEligible = _event.EventStatus == "Completed"
                    ? EligibilityService.IsEligibleThisMonthExcludingEvent(connection, _event.HouseholdId, eventDate, _event.Id)
                    : EligibilityService.IsEligibleThisMonth(connection, _event.HouseholdId, eventDate);
                if (!isEligible)
                {
                    using var overrideForm = new OverrideReasonForm();
                    if (overrideForm.ShowDialog(this) != DialogResult.OK)
                        return;
                    overrideReason = overrideForm.OverrideReason;
                    overrideNotes = overrideForm.Notes;
                }
            }
        }

        if (overrideReason == null && pnlOverride.Visible && cmbOverrideReason.SelectedItem != null)
            overrideReason = cmbOverrideReason.SelectedItem.ToString();

        try
        {
            _event.EventDate = dtpEventDate.Value.Date;
            var notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            if (_event.EventType == "Appointment")
            {
                _event.EventStatus = cmbStatus.SelectedItem?.ToString() ?? _event.EventStatus;
                _event.ScheduledText = txtScheduledText.Text.Trim();
                _event.ScheduledForMemberId = ((MemberItem)cmbMember.SelectedItem!).Id;
                _event.VisitType = cmbStatus.SelectedItem?.ToString() == "Completed"
                    ? (cmbVisitType.SelectedItem?.ToString() ?? null)
                    : null;

                if (!string.IsNullOrWhiteSpace(overrideNotes))
                {
                    notes = string.IsNullOrWhiteSpace(notes)
                        ? overrideNotes
                        : $"{notes}\n\n- Override: {overrideNotes}";
                }
            }
            else
            {
                _event.VisitType = cmbVisitType.SelectedItem?.ToString();
            }

            _event.Notes = notes;
            _event.OverrideReason = overrideReason;

            using var connection = DatabaseManager.GetConnection();
            ServiceEventRepository.Update(connection, _event);

            if (_event.EventType == "Appointment" && _event.EventStatus == "Completed")
                HouseholdRepository.SetIsActive(connection, _event.HouseholdId, true);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private sealed record MemberItem(int Id, string Display, bool IsPrimary)
    {
        public override string ToString() => Display;
    }
}
