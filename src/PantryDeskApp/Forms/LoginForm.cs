using PantryDeskCore.Data;
using PantryDeskCore.Security;

namespace PantryDeskApp.Forms;

/// <summary>
/// Login form for authenticating users with Entry or Admin roles.
/// </summary>
public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
        cmbRole.SelectedIndex = 0; // Default to "Entry"
        ActiveControl = txtPassword;

        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        // Clear previous errors
        lblError.Text = string.Empty;
        lblError.Visible = false;

        // Get selected role and password
        var roleName = cmbRole.SelectedItem?.ToString() ?? string.Empty;
        var password = txtPassword.Text;

        if (string.IsNullOrWhiteSpace(roleName))
        {
            ShowError("Please select a role.");
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            ShowError("Please enter a password.");
            return;
        }

        // Verify password
        try
        {
            bool isValid;
            using (var connection = DatabaseManager.GetConnection())
            {
                isValid = AuthRoleRepository.VerifyPassword(connection, roleName, password);
            }

            if (isValid)
            {
                SessionManager.Login(roleName);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                ShowError("Invalid password.");
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
        catch (Exception ex)
        {
            ShowError($"Login failed: {ex.Message}");
            txtPassword.Clear();
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

    private void TxtPassword_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            BtnLogin_Click(sender, e);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
