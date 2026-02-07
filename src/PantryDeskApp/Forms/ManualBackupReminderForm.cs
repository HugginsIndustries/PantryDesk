using PantryDeskCore.Security;
using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

/// <summary>
/// Reminder shown on app launch when no manual backup has been run in 7+ days.
/// </summary>
public partial class ManualBackupReminderForm : Form
{
    private readonly CheckInForm? _checkInForm;

    public ManualBackupReminderForm(CheckInForm? checkInForm)
    {
        _checkInForm = checkInForm;
        InitializeComponent();
        lblMessage.Text = "No manual backup has been run in the last 7 days. Consider backing up to USB or another location.";
        btnBackupNow.Enabled = SessionManager.IsAdmin;
        if (!SessionManager.IsAdmin)
        {
            lblAdminNote.Visible = true;
            lblAdminNote.Text = "Login as Admin to complete manual backup ASAP.";
        }

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            Icon = new Icon(iconPath);
        }
    }

    private void BtnSnooze_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void BtnBackupNow_Click(object? sender, EventArgs e)
    {
        if (!SessionManager.IsAdmin)
        {
            return;
        }

        using var dialog = new FolderBrowserDialog
        {
            Description = "Select folder for backup (e.g., USB drive)",
            ShowNewFolderButton = true
        };

        if (dialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        var targetFolder = dialog.SelectedPath;
        try
        {
            var testFile = Path.Combine(targetFolder, "test_write.tmp");
            File.WriteAllText(testFile, "test");
            File.Delete(testFile);
        }
        catch
        {
            MessageBox.Show(
                "Selected folder is not writable. Please choose a different location.",
                "Invalid Folder",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;
            var backupPath = BackupService.CreateBackup(targetFolder, passphrase: null, isAutomatic: false);
            Cursor = Cursors.Default;
            MessageBox.Show(
                $"Backup created successfully.\n\nLocation: {backupPath}",
                "Backup Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            _checkInForm?.RefreshStatusBar();
            Close();
        }
        catch (Exception ex)
        {
            Cursor = Cursors.Default;
            MessageBox.Show(
                $"Failed to create backup:\n\n{ex.Message}",
                "Backup Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
