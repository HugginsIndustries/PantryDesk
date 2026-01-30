using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

public partial class ExportForm : Form
{
    public ExportForm()
    {
        InitializeComponent();
        radioCsv.Checked = true;
        
        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        if (radioCsv.Checked)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Select folder for CSV export files",
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtOutputPath.Text = dialog.SelectedPath;
            }
        }
        else
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Save JSON Export",
                FileName = $"pantrydesk_export_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtOutputPath.Text = dialog.FileName;
            }
        }
    }

    private void RadioCsv_CheckedChanged(object? sender, EventArgs e)
    {
        if (radioCsv.Checked)
        {
            lblOutputPath.Text = "Output Folder:";
            txtOutputPath.ReadOnly = true;
            btnBrowse.Text = "Browse Folder...";
        }
    }

    private void RadioJson_CheckedChanged(object? sender, EventArgs e)
    {
        if (radioJson.Checked)
        {
            lblOutputPath.Text = "Output File:";
            txtOutputPath.ReadOnly = true;
            btnBrowse.Text = "Browse File...";
        }
    }

    private void BtnExport_Click(object? sender, EventArgs e)
    {
        var outputPath = txtOutputPath.Text.Trim();
        if (string.IsNullOrEmpty(outputPath))
        {
            MessageBox.Show("Please select an output location.", "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;
            btnExport.Enabled = false;

            if (radioCsv.Checked)
            {
                if (!Directory.Exists(outputPath))
                {
                    MessageBox.Show("Output folder does not exist.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var files = ExportService.ExportToCsv(outputPath, $"pantrydesk_export_{timestamp}");

                MessageBox.Show(
                    $"CSV export completed successfully.\n\n" +
                    $"Files created:\n" +
                    $"{string.Join("\n", files.Select(Path.GetFileName))}\n\n" +
                    $"Location: {outputPath}",
                    "Export Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                var dir = Path.GetDirectoryName(outputPath);
                if (dir != null && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                ExportService.ExportToJson(outputPath);

                MessageBox.Show(
                    $"JSON export completed successfully.\n\n" +
                    $"File: {Path.GetFileName(outputPath)}\n" +
                    $"Location: {Path.GetDirectoryName(outputPath)}",
                    "Export Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Export failed:\n\n{ex.Message}",
                "Export Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
            btnExport.Enabled = true;
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
