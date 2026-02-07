using PantryDeskCore.Models;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form for adding or editing a household member.
/// </summary>
public partial class MemberEditForm : Form
{
    public HouseholdMember Member { get; private set; } = new();

    public MemberEditForm(HouseholdMember? existing = null, bool canSetPrimary = false)
    {
        InitializeComponent();
        chkPrimary.Visible = canSetPrimary;

        if (existing != null)
        {
            txtFirstName.Text = existing.FirstName;
            txtLastName.Text = existing.LastName;
            dtpBirthday.Value = existing.Birthday;
            chkPrimary.Checked = existing.IsPrimary;
            cmbRace.SelectedItem = existing.Race ?? "Not Specified";
            cmbVeteran.SelectedItem = existing.VeteranStatus ?? "Prefer Not To Answer";
            cmbDisabled.SelectedItem = existing.DisabledStatus ?? "Prefer Not To Answer";
        }
        else
        {
            dtpBirthday.Value = DateTime.Today.AddYears(-30);
            chkPrimary.Checked = true;
        }

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        lblError.Visible = false;

        var firstName = txtFirstName.Text.Trim();
        var lastName = txtLastName.Text.Trim();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            ShowError("First name is required.");
            txtFirstName.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            ShowError("Last name is required.");
            txtLastName.Focus();
            return;
        }

        Member = new HouseholdMember
        {
            FirstName = firstName,
            LastName = lastName,
            Birthday = dtpBirthday.Value.Date,
            IsPrimary = chkPrimary.Checked,
            Race = cmbRace.SelectedItem?.ToString(),
            VeteranStatus = cmbVeteran.SelectedItem?.ToString(),
            DisabledStatus = cmbDisabled.SelectedItem?.ToString()
        };

        DialogResult = DialogResult.OK;
        Close();
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
}
