using PantryDeskCore.Data;
using PantryDeskCore.Models;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form for creating a new household.
/// </summary>
public partial class NewHouseholdForm : Form
{
    public NewHouseholdForm()
    {
        InitializeComponent();
    }

    private void BtnSave_Click(object? sender, EventArgs e)
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

        try
        {
            // Create household
            var household = new Household
            {
                PrimaryName = txtPrimaryName.Text.Trim(),
                Address1 = string.IsNullOrWhiteSpace(txtAddress1.Text) ? null : txtAddress1.Text.Trim(),
                City = string.IsNullOrWhiteSpace(txtCity.Text) ? null : txtCity.Text.Trim(),
                State = string.IsNullOrWhiteSpace(txtState.Text) ? null : txtState.Text.Trim(),
                Zip = string.IsNullOrWhiteSpace(txtZip.Text) ? null : txtZip.Text.Trim(),
                Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                ChildrenCount = childrenCount,
                AdultsCount = adultsCount,
                SeniorsCount = seniorsCount,
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                IsActive = true
            };

            using var connection = DatabaseManager.GetConnection();
            HouseholdRepository.Create(connection, household);

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
}
