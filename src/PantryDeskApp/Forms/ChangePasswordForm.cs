using PantryDeskCore.Data;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form for Admin to change role passwords.
/// </summary>
public partial class ChangePasswordForm : Form
{
    public ChangePasswordForm()
    {
        InitializeComponent();
    }

    private void BtnChangePassword_Click(object? sender, EventArgs e)
    {
        // Clear previous messages
        lblError.Text = string.Empty;
        lblError.Visible = false;
        lblSuccess.Text = string.Empty;
        lblSuccess.Visible = false;

        // Get values
        var roleName = cmbRole.SelectedItem?.ToString() ?? string.Empty;
        var currentPassword = txtCurrentPassword.Text;
        var newPassword = txtNewPassword.Text;
        var confirmPassword = txtConfirmPassword.Text;

        // Validate inputs
        if (string.IsNullOrWhiteSpace(roleName))
        {
            ShowError("Please select a role.");
            return;
        }

        if (string.IsNullOrWhiteSpace(currentPassword))
        {
            ShowError("Please enter the current password.");
            return;
        }

        if (string.IsNullOrWhiteSpace(newPassword))
        {
            ShowError("Please enter a new password.");
            return;
        }

        if (newPassword.Length < 8)
        {
            ShowError("New password must be at least 8 characters long.");
            return;
        }

        if (newPassword != confirmPassword)
        {
            ShowError("New password confirmation does not match.");
            return;
        }

        // Verify current password
        try
        {
            bool isValid;
            using (var connection = DatabaseManager.GetConnection())
            {
                isValid = AuthRoleRepository.VerifyPassword(connection, roleName, currentPassword);
            }

            if (!isValid)
            {
                ShowError("Current password is incorrect.");
                txtCurrentPassword.Clear();
                txtCurrentPassword.Focus();
                return;
            }

            // Check if new password is same as current
            if (currentPassword == newPassword)
            {
                ShowError("New password must be different from the current password.");
                return;
            }

            // Update password
            using (var connection = DatabaseManager.GetConnection())
            {
                AuthRoleRepository.UpdatePassword(connection, roleName, newPassword);
            }

            // Show success
            lblSuccess.Text = $"Password for {roleName} role has been changed successfully.";
            lblSuccess.Visible = true;

            // Clear fields
            txtCurrentPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to change password: {ex.Message}");
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
        lblSuccess.Visible = false;
    }

    private void TxtPassword_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            BtnChangePassword_Click(sender, e);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
