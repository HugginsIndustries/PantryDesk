namespace PantryDeskApp.Forms;

/// <summary>
/// Modal form for completing a service: Visit Type (required) and Notes (optional).
/// </summary>
public partial class CompleteServiceDialog : Form
{
    public string VisitType { get; private set; } = string.Empty;
    public string? Notes { get; private set; }

    public CompleteServiceDialog()
    {
        InitializeComponent();

        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void BtnOK_Click(object? sender, EventArgs e)
    {
        if (cmbVisitType.SelectedItem == null)
        {
            MessageBox.Show("Please select a visit type.", "Visit Type Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        VisitType = cmbVisitType.SelectedItem.ToString() ?? string.Empty;
        Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
