using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

public partial class BackupRestoreForm : Form
{
    public BackupRestoreForm()
    {
        InitializeComponent();
    }

    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Backup files (*.zip)|*.zip|All files (*.*)|*.*",
            Title = "Select Backup File"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtBackupPath.Text = dialog.FileName;
            ValidateBackupFile();
        }
    }

    private void TxtBackupPath_TextChanged(object? sender, EventArgs e)
    {
        ValidateBackupFile();
    }

    private void ValidateBackupFile()
    {
        var path = txtBackupPath.Text.Trim();
        btnRestore.Enabled = false;
        lblBackupInfo.Text = string.Empty;

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        if (!File.Exists(path))
        {
            lblBackupInfo.Text = "File not found.";
            lblBackupInfo.ForeColor = Color.Red;
            return;
        }

        if (!path.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
        {
            lblBackupInfo.Text = "Please select a .zip backup file.";
            lblBackupInfo.ForeColor = Color.Red;
            return;
        }

        // Try to read backup metadata (this validates the file structure)
        try
        {
            var metadata = BackupService.ReadBackupMetadata(path);
            lblBackupInfo.Text = $"Backup Date: {metadata.BackupDate:yyyy-MM-dd HH:mm:ss} UTC\nSchema Version: {metadata.SchemaVersion}";
            lblBackupInfo.ForeColor = Color.Green;
            btnRestore.Enabled = true;
        }
        catch (Exception ex)
        {
            lblBackupInfo.Text = $"Invalid backup file: {ex.Message}";
            lblBackupInfo.ForeColor = Color.Red;
        }
    }

    private void BtnRestore_Click(object? sender, EventArgs e)
    {
        var path = txtBackupPath.Text.Trim();
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            MessageBox.Show("Please select a valid backup file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Confirm restore
        var result = MessageBox.Show(
            "This will replace the current database with the backup.\n\n" +
            "A safety copy of the current database will be created before restoring.\n\n" +
            "The application will need to restart after restore.\n\n" +
            "Do you want to continue?",
            "Confirm Restore",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
        {
            return;
        }

        try
        {
            var metadata = BackupService.RestoreFromBackup(path);
            
            MessageBox.Show(
                $"Database restored successfully.\n\n" +
                $"Backup Date: {metadata.BackupDate:yyyy-MM-dd HH:mm:ss} UTC\n" +
                $"Schema Version: {metadata.SchemaVersion}\n\n" +
                $"The application will now restart.",
                "Restore Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Failed to restore backup:\n\n{ex.Message}",
                "Restore Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
