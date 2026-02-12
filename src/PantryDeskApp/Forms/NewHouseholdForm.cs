using PantryDeskCore.Data;
using PantryDeskCore.Models;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form for creating a new household.
/// </summary>
public partial class NewHouseholdForm : Form
{
    private List<HouseholdMember> _members = new();

    public NewHouseholdForm()
    {
        InitializeComponent();
        SetupMembersGrid();

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
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

        // AllCells for content-fit; Fill for last column so table fills available width (match Household Profile)
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
        foreach (var m in _members)
        {
            var idx = grdMembers.Rows.Add(
                m.FirstName,
                m.LastName,
                m.Birthday.ToString("yyyy-MM-dd"),
                m.IsPrimary ? "Yes" : "",
                m.Race ?? "",
                m.VeteranStatus ?? "",
                m.DisabledStatus ?? "");
            grdMembers.Rows[idx].Tag = m;
        }
    }

    private void BtnAddMember_Click(object? sender, EventArgs e)
    {
        using var form = new MemberEditForm(null, _members.Count > 0);
        if (form.ShowDialog() != DialogResult.OK)
            return;

        var member = form.Member;
        if (member.IsPrimary && _members.Count > 0)
        {
            foreach (var m in _members)
                m.IsPrimary = false;
        }
        if (_members.Count == 0)
            member.IsPrimary = true;

        _members.Add(member);
        RefreshMembersGrid();
        RefreshDuplicateWarning();
    }

    private void BtnEditMember_Click(object? sender, EventArgs e)
    {
        if (grdMembers.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a member to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var existing = (HouseholdMember)grdMembers.SelectedRows[0].Tag!;
        using var form = new MemberEditForm(existing, true);
        if (form.ShowDialog() != DialogResult.OK)
            return;

        var member = form.Member;
        if (member.IsPrimary && !existing.IsPrimary)
        {
            foreach (var m in _members)
                m.IsPrimary = false;
        }
        var idx = _members.IndexOf(existing);
        _members[idx] = member;
        RefreshMembersGrid();
        RefreshDuplicateWarning();
    }

    private void BtnRemoveMember_Click(object? sender, EventArgs e)
    {
        if (grdMembers.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a member to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var member = (HouseholdMember)grdMembers.SelectedRows[0].Tag!;
        var wasPrimary = member.IsPrimary;
        _members.Remove(member);
        if (wasPrimary && _members.Count > 0)
            _members[0].IsPrimary = true;
        RefreshMembersGrid();
        RefreshDuplicateWarning();
    }

    private void BtnSetPrimary_Click(object? sender, EventArgs e)
    {
        if (grdMembers.SelectedRows.Count == 0)
        {
            MessageBox.Show("Please select a member to set as primary.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var member = (HouseholdMember)grdMembers.SelectedRows[0].Tag!;
        foreach (var m in _members)
            m.IsPrimary = false;
        member.IsPrimary = true;
        RefreshMembersGrid();
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        lblError.Visible = false;

        if (_members.Count == 0)
        {
            ShowError("At least one household member is required.");
            return;
        }

        var primary = _members.FirstOrDefault(m => m.IsPrimary);
        if (primary == null)
        {
            ShowError("One member must be set as primary contact.");
            return;
        }

        if (HasPotentialDuplicateMembers())
        {
            var result = MessageBox.Show(
                "Possible duplicate member(s) found. Create this household anyway?",
                "Possible Duplicate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result == DialogResult.No)
                return;
        }

        var primaryName = primary.FullName;

        try
        {
            var household = new Household
            {
                PrimaryName = primaryName,
                Address1 = string.IsNullOrWhiteSpace(txtAddress1.Text) ? null : txtAddress1.Text.Trim(),
                City = string.IsNullOrWhiteSpace(txtCity.Text) ? null : txtCity.Text.Trim(),
                State = string.IsNullOrWhiteSpace(txtState.Text) ? null : txtState.Text.Trim(),
                Zip = string.IsNullOrWhiteSpace(txtZip.Text) ? null : txtZip.Text.Trim(),
                Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                ChildrenCount = 0,
                AdultsCount = 0,
                SeniorsCount = 0,
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                IsActive = true
            };

            using var connection = DatabaseManager.GetConnection();

            var potentialDuplicates = HouseholdRepository.FindPotentialDuplicates(
                connection,
                primaryName,
                household.City,
                household.Phone);

            if (potentialDuplicates.Count > 0)
            {
                var summaryLines = potentialDuplicates
                    .Select(h =>
                    {
                        var cityZip = !string.IsNullOrWhiteSpace(h.City) ? h.City + (string.IsNullOrWhiteSpace(h.Zip) ? "" : ", " + h.Zip) : h.Zip ?? "—";
                        return $"{h.PrimaryName} ({cityZip})";
                    })
                    .Distinct()
                    .Take(5);

                var result = MessageBox.Show(
                    "Households with a similar name already exist:\n\n" +
                    string.Join("\n", summaryLines) +
                    "\n\nDo you still want to create this new household?",
                    "Possible Duplicate Household",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                    return;
            }

            HouseholdRepository.Create(connection, household);

            foreach (var member in _members)
            {
                member.HouseholdId = household.Id;
                HouseholdMemberRepository.Create(connection, member);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            ShowError($"Error creating household: {ex.Message}");
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

    private void RefreshDuplicateWarning()
    {
        if (_members.Count == 0)
        {
            lblDuplicateWarning.Visible = false;
            return;
        }
        try
        {
            using var connection = DatabaseManager.GetConnection();
            var candidates = _members.Select(m => (m.FirstName, m.LastName, m.Birthday));
            var hasDuplicate = HouseholdMemberRepository.FindPotentialDuplicateMembers(connection, candidates);
            lblDuplicateWarning.Visible = hasDuplicate;
        }
        catch
        {
            lblDuplicateWarning.Visible = false;
        }
    }

    private bool HasPotentialDuplicateMembers()
    {
        if (_members.Count == 0)
            return false;
        try
        {
            using var connection = DatabaseManager.GetConnection();
            var candidates = _members.Select(m => (m.FirstName, m.LastName, m.Birthday));
            return HouseholdMemberRepository.FindPotentialDuplicateMembers(connection, candidates);
        }
        catch
        {
            return false;
        }
    }
}
