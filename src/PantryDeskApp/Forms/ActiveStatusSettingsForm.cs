using PantryDeskCore.Data;
using PantryDeskCore.Security;

namespace PantryDeskApp.Forms;

/// <summary>
/// Admin-only form to configure the annual active-status reset date (month and day).
/// </summary>
public partial class ActiveStatusSettingsForm : Form
{
    private const string ConfigKeyResetMonth = "active_status_reset_month";
    private const string ConfigKeyResetDay = "active_status_reset_day";
    private const int DefaultMonth = 1;
    private const int DefaultDay = 1;

    public ActiveStatusSettingsForm()
    {
        InitializeComponent();
        LoadSettings();

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void LoadSettings()
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            var monthStr = ConfigRepository.GetValue(connection, ConfigKeyResetMonth);
            var dayStr = ConfigRepository.GetValue(connection, ConfigKeyResetDay);

            var month = int.TryParse(monthStr, out var m) ? Math.Clamp(m, 1, 12) : DefaultMonth;
            var day = int.TryParse(dayStr, out var d) ? Math.Clamp(d, 1, 31) : DefaultDay;

            day = Math.Min(day, DateTime.DaysInMonth(DateTime.Today.Year, month));

            cmbMonth.SelectedIndex = month - 1;
            numDay.Value = day;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        lblError.Visible = false;

        var month = cmbMonth.SelectedIndex + 1;
        var day = (int)numDay.Value;

        if (month < 1 || month > 12)
        {
            ShowError("Please select a valid month.");
            return;
        }

        var maxDay = DateTime.DaysInMonth(DateTime.Today.Year, month);
        if (day < 1 || day > maxDay)
        {
            ShowError($"Day must be between 1 and {maxDay} for the selected month.");
            return;
        }

        try
        {
            PermissionChecker.RequireAdmin();

            using var connection = DatabaseManager.GetConnection();
            ConfigRepository.SetValue(connection, ConfigKeyResetMonth, month.ToString());
            ConfigRepository.SetValue(connection, ConfigKeyResetDay, day.ToString());

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Show("This action requires Admin privileges.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            ShowError($"Error saving settings: {ex.Message}");
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
