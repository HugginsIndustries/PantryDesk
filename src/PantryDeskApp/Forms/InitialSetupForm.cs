using PantryDeskCore.Data;

namespace PantryDeskApp.Forms;

/// <summary>
/// Initial setup form for configuring Entry and Admin role passwords on first launch.
/// </summary>
public partial class InitialSetupForm : Form
{
    public InitialSetupForm()
    {
        InitializeComponent();
    }

    private void BtnCompleteSetup_Click(object? sender, EventArgs e)
    {
        // Clear previous errors
        lblError.Text = string.Empty;
        lblError.Visible = false;
        lblWarning.Text = string.Empty;
        lblWarning.Visible = false;

        // Get password values
        var entryPassword = txtEntryPassword.Text;
        var entryPasswordConfirm = txtEntryPasswordConfirm.Text;
        var adminPassword = txtAdminPassword.Text;
        var adminPasswordConfirm = txtAdminPasswordConfirm.Text;

        // Validate Entry password
        if (string.IsNullOrWhiteSpace(entryPassword))
        {
            ShowError("Entry password is required.");
            return;
        }

        if (entryPassword.Length < 8)
        {
            ShowError("Entry password must be at least 8 characters long.");
            return;
        }

        if (entryPassword != entryPasswordConfirm)
        {
            ShowError("Entry password confirmation does not match.");
            return;
        }

        // Validate Admin password
        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            ShowError("Admin password is required.");
            return;
        }

        if (adminPassword.Length < 8)
        {
            ShowError("Admin password must be at least 8 characters long.");
            return;
        }

        if (adminPassword != adminPasswordConfirm)
        {
            ShowError("Admin password confirmation does not match.");
            return;
        }

        // Warn if passwords are identical (but allow it)
        if (entryPassword == adminPassword)
        {
            lblWarning.Text = "Warning: Entry and Admin passwords are identical. Consider using different passwords for better security.";
            lblWarning.Visible = true;
            // Continue anyway - just a warning
        }

        // Save passwords
        try
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                AuthRoleRepository.UpdatePassword(connection, "Entry", entryPassword);
                AuthRoleRepository.UpdatePassword(connection, "Admin", adminPassword);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to save passwords: {ex.Message}");
        }
    }

    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
    }

    private void TxtPassword_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            BtnCompleteSetup_Click(sender, e);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
