namespace PantryDeskApp.Forms;

/// <summary>
/// Modal form for entering override reason when completing service for an ineligible household.
/// </summary>
public partial class OverrideReasonForm : Form
{
    public string OverrideReason { get; private set; } = string.Empty;
    public string? Notes { get; private set; }

    public OverrideReasonForm()
    {
        InitializeComponent();
    }

    private void CmbReason_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // Enable OK button only when reason is selected
        btnOK.Enabled = cmbReason.SelectedIndex >= 0;
    }

    private void BtnOK_Click(object? sender, EventArgs e)
    {
        if (cmbReason.SelectedItem == null)
        {
            MessageBox.Show("Please select a reason.", "Reason Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        OverrideReason = cmbReason.SelectedItem.ToString() ?? string.Empty;
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
