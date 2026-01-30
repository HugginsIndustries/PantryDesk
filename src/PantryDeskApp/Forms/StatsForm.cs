using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form displaying statistics dashboard for current month.
/// </summary>
public partial class StatsForm : Form
{
    public StatsForm()
    {
        InitializeComponent();
        SetupDataGridViews();
    }

    private void SetupDataGridViews()
    {
        // Setup city breakdown grid
        dgvCityBreakdown.Columns.Clear();
        dgvCityBreakdown.Columns.Add("City", "City");
        dgvCityBreakdown.Columns.Add("HouseholdsServed", "Households Served");
        dgvCityBreakdown.Columns.Add("ServicesCompleted", "Services Completed");
        var cityColumn = dgvCityBreakdown.Columns["City"];
        var householdsColumn = dgvCityBreakdown.Columns["HouseholdsServed"];
        var servicesColumn = dgvCityBreakdown.Columns["ServicesCompleted"];
        if (cityColumn != null) cityColumn.Width = 150;
        if (householdsColumn != null) householdsColumn.Width = 100;
        if (servicesColumn != null) servicesColumn.Width = 100;

        // Setup override breakdown grid
        dgvOverrideBreakdown.Columns.Clear();
        dgvOverrideBreakdown.Columns.Add("Reason", "Reason");
        dgvOverrideBreakdown.Columns.Add("Count", "Count");
        var reasonColumn = dgvOverrideBreakdown.Columns["Reason"];
        var countColumn = dgvOverrideBreakdown.Columns["Count"];
        if (reasonColumn != null) reasonColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        if (countColumn != null) countColumn.Width = 100;
    }

    private void StatsForm_Load(object? sender, EventArgs e)
    {
        LoadStats();
    }

    private void LoadStats()
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            var stats = StatisticsService.GetCurrentMonthStats(connection);
            var cityBreakdown = StatisticsService.GetStatsByCity(connection, GetCurrentMonthStart(), GetCurrentMonthEnd());
            var overrideBreakdown = StatisticsService.GetOverrideBreakdown(connection, GetCurrentMonthStart(), GetCurrentMonthEnd());

            // Update main stats
            txtTotalActiveHouseholds.Text = stats.TotalActiveHouseholds.ToString("N0");
            txtTotalPeople.Text = stats.TotalPeople.ToString("N0");
            txtCompletedServices.Text = stats.CompletedServices.ToString("N0");
            txtUniqueHouseholdsServed.Text = stats.UniqueHouseholdsServed.ToString("N0");
            txtPantryDayCompletions.Text = stats.PantryDayCompletions.ToString("N0");
            txtAppointmentCompletions.Text = stats.AppointmentCompletions.ToString("N0");
            txtOverridesCount.Text = stats.OverridesCount.ToString("N0");

            // Update city breakdown
            dgvCityBreakdown.Rows.Clear();
            foreach (var city in cityBreakdown)
            {
                dgvCityBreakdown.Rows.Add(city.City, city.HouseholdsServed, city.ServicesCompleted);
            }

            // Update override breakdown
            dgvOverrideBreakdown.Rows.Clear();
            foreach (var ovr in overrideBreakdown)
            {
                dgvOverrideBreakdown.Rows.Add(ovr.Reason, ovr.Count);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private DateTime GetCurrentMonthStart()
    {
        var now = DateTime.Now;
        return new DateTime(now.Year, now.Month, 1);
    }

    private DateTime GetCurrentMonthEnd()
    {
        var now = DateTime.Now;
        return new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
    }

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        LoadStats();
    }

    private void BtnMonthlySummary_Click(object? sender, EventArgs e)
    {
        using var monthlySummaryForm = new MonthlySummaryForm();
        monthlySummaryForm.ShowDialog();
    }
}
