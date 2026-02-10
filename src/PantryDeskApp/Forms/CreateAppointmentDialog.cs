using PantryDeskCore.Data;
using PantryDeskCore.Models;

namespace PantryDeskApp.Forms;

/// <summary>
/// Dialog for creating a new appointment: search by member name, select member, set date and details.
/// </summary>
public partial class CreateAppointmentDialog : Form
{
    private MemberSearchResult? _selectedResult;
    private System.Windows.Forms.Timer? _searchDebounceTimer;

    public CreateAppointmentDialog()
    {
        InitializeComponent();
        SetupResultsGrid();

        _searchDebounceTimer = new System.Windows.Forms.Timer();
        _searchDebounceTimer.Interval = 250;
        _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
        Disposed += (_, _) =>
        {
            _searchDebounceTimer?.Stop();
            _searchDebounceTimer?.Dispose();
            _searchDebounceTimer = null;
        };

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void SearchDebounceTimer_Tick(object? sender, EventArgs e)
    {
        _searchDebounceTimer?.Stop();
        SearchHouseholds(txtSearch.Text);
    }

    private void SetupResultsGrid()
    {
        dgvResults.Columns.Clear();
        dgvResults.Columns.Add("DisplayName", "Member");
        dgvResults.Columns.Add("CityZip", "City/Zip");
        dgvResults.Columns.Add("HouseholdId", "HouseholdId");
        var idCol = dgvResults.Columns["HouseholdId"];
        if (idCol != null) idCol.Visible = false;
        dgvResults.Columns["DisplayName"]!.Width = 280;
        dgvResults.Columns["CityZip"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    private void TxtSearch_TextChanged(object? sender, EventArgs e)
    {
        _searchDebounceTimer?.Stop();
        _searchDebounceTimer?.Start();
    }

    private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            _searchDebounceTimer?.Stop();
            SearchHouseholds(txtSearch.Text);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    private void SearchHouseholds(string searchTerm)
    {
        dgvResults.Rows.Clear();
        _selectedResult = null;
        grpAppointment.Enabled = false;
        txtScheduledText.Clear();
        txtNotes.Clear();
        dtpScheduledDate.Value = DateTime.Today;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return;
        }

        try
        {
            using var connection = DatabaseManager.GetConnection();
            var results = HouseholdMemberRepository.SearchMembersByName(connection, searchTerm.Trim());

            foreach (var result in results)
            {
                var displayName = result.Member.IsPrimary
                    ? result.Member.FullName
                    : $"{result.Member.FullName} • {result.PrimaryName} Household";

                string cityZip = string.Empty;
                if (!string.IsNullOrWhiteSpace(result.Household.City))
                {
                    cityZip = result.Household.City;
                    if (!string.IsNullOrWhiteSpace(result.Household.Zip))
                        cityZip += $", {result.Household.Zip}";
                }
                else if (!string.IsNullOrWhiteSpace(result.Household.Zip))
                {
                    cityZip = result.Household.Zip;
                }
                if (string.IsNullOrWhiteSpace(cityZip))
                    cityZip = "—";

                var idx = dgvResults.Rows.Add(displayName, cityZip, result.Household.Id);
                dgvResults.Rows[idx].Tag = result;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error searching: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvResults_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvResults.SelectedRows.Count == 0)
        {
            _selectedResult = null;
            grpAppointment.Enabled = false;
            return;
        }

        _selectedResult = dgvResults.SelectedRows[0].Tag as MemberSearchResult;
        grpAppointment.Enabled = _selectedResult != null;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_selectedResult == null)
        {
            MessageBox.Show("Please select a member from the search results.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtSearch.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtScheduledText.Text))
        {
            MessageBox.Show("Scheduled Text is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtScheduledText.Focus();
            return;
        }

        try
        {
            var serviceEvent = new ServiceEvent
            {
                HouseholdId = _selectedResult.Household.Id,
                EventType = "Appointment",
                EventStatus = "Scheduled",
                EventDate = dtpScheduledDate.Value.Date,
                ScheduledText = txtScheduledText.Text.Trim(),
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                ScheduledForMemberId = _selectedResult.Member.Id
            };

            using var connection = DatabaseManager.GetConnection();
            ServiceEventRepository.Create(connection, serviceEvent);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error scheduling appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
