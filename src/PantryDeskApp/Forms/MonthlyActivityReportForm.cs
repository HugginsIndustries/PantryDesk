using Microsoft.Data.Sqlite;
using PantryDeskCore.Configuration;
using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form to view/generate the Monthly Activity Report (one page, landscape). Header fields persisted in config.
/// </summary>
public partial class MonthlyActivityReportForm : Form
{
    public MonthlyActivityReportForm()
    {
        InitializeComponent();
        LoadHeaderFromConfig();
        SetDefaultMonth();

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void SetDefaultMonth()
    {
        var lastMonth = DateTime.Now.AddMonths(-1);
        cmbYear.Items.Clear();
        for (var y = DateTime.Now.Year; y >= DateTime.Now.Year - 5; y--)
        {
            cmbYear.Items.Add(y);
        }
        cmbYear.SelectedItem = lastMonth.Year;
        cmbMonth.SelectedIndex = lastMonth.Month - 1;
    }

    private void LoadHeaderFromConfig()
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            txtFoodBankName.Text = ConfigRepository.GetValue(connection, ReportHeaderConfigKeys.FoodBankName)
                ?? "Winlock-Vader Food Bank";
            txtCounty.Text = ConfigRepository.GetValue(connection, ReportHeaderConfigKeys.County)
                ?? "Lewis";
            txtPreparedBy.Text = ConfigRepository.GetValue(connection, ReportHeaderConfigKeys.PreparedBy)
                ?? "RyLee Camps";
            txtPhone.Text = ConfigRepository.GetValue(connection, ReportHeaderConfigKeys.Phone)
                ?? "(360) 785-2185";
        }
        catch
        {
            // Non-fatal; use defaults
            txtFoodBankName.Text = "Winlock-Vader Food Bank";
            txtCounty.Text = "Lewis";
            txtPreparedBy.Text = "RyLee Camps";
            txtPhone.Text = "(360) 785-2185";
        }
    }

    private void SaveHeaderToConfig()
    {
        try
        {
            using var connection = DatabaseManager.GetConnection();
            ConfigRepository.SetValue(connection, ReportHeaderConfigKeys.FoodBankName, txtFoodBankName.Text ?? string.Empty);
            ConfigRepository.SetValue(connection, ReportHeaderConfigKeys.County, txtCounty.Text ?? string.Empty);
            ConfigRepository.SetValue(connection, ReportHeaderConfigKeys.PreparedBy, txtPreparedBy.Text ?? string.Empty);
            ConfigRepository.SetValue(connection, ReportHeaderConfigKeys.Phone, txtPhone.Text ?? string.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not save header for next time: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private (int year, int month) GetSelectedYearMonth()
    {
        var year = (int)(cmbYear.SelectedItem ?? DateTime.Now.Year);
        var month = cmbMonth.SelectedIndex + 1;
        return (year, month);
    }

    private MonthlyActivityReportHeader GetHeader()
    {
        return new MonthlyActivityReportHeader
        {
            FoodBankName = txtFoodBankName.Text ?? string.Empty,
            County = txtCounty.Text ?? string.Empty,
            PreparedBy = txtPreparedBy.Text ?? string.Empty,
            Phone = txtPhone.Text ?? string.Empty
        };
    }

    private static (string? cityLine, string? raceLine, string? veteranLine, string? disabilityLine) GetDemographicLinesWithPercentages(SqliteConnection connection, DateTime monthStart, DateTime monthEnd)
    {
        var cityBreakdown = StatisticsService.GetStatsByCity(connection, monthStart, monthEnd);
        var sorted = cityBreakdown.OrderByDescending(c => c.HouseholdsServed).ToList();
        var cityTotal = sorted.Sum(c => c.HouseholdsServed);
        var cityLine = sorted.Count > 0
            ? string.Join(" · ", sorted.Select(c =>
            {
                var pct = cityTotal > 0 ? (int)Math.Round(100.0 * c.HouseholdsServed / cityTotal) : 0;
                return $"{c.City}: {c.HouseholdsServed} ({pct}%)";
            }))
            : null;

        var race = StatisticsService.GetDemographicsByRace(connection, monthStart, monthEnd).OrderByDescending(x => x.Count).ToList();
        var raceTotal = race.Sum(x => x.Count);
        var raceLine = race.Count > 0
            ? string.Join(" · ", race.Select(x =>
            {
                var pct = raceTotal > 0 ? (int)Math.Round(100.0 * x.Count / raceTotal) : 0;
                return $"{x.Label}: {x.Count} ({pct}%)";
            }))
            : null;

        var veteran = StatisticsService.GetVeteranStatusWithDisabledVeteranBreakdown(connection, monthStart, monthEnd);
        var veteranTotal = veteran.Sum(x => x.Count);
        var veteranLine = veteran.Count > 0
            ? string.Join(" · ", veteran.Select(x =>
            {
                var pct = veteranTotal > 0 ? (int)Math.Round(100.0 * x.Count / veteranTotal) : 0;
                return $"{x.Label}: {x.Count} ({pct}%)";
            }))
            : null;

        var disability = StatisticsService.GetDemographicsByDisabledStatus(connection, monthStart, monthEnd).OrderByDescending(x => x.Count).ToList();
        var disabilityTotal = disability.Sum(x => x.Count);
        var disabilityLine = disability.Count > 0
            ? string.Join(" · ", disability.Select(x =>
            {
                var pct = disabilityTotal > 0 ? (int)Math.Round(100.0 * x.Count / disabilityTotal) : 0;
                return $"{x.Label}: {x.Count} ({pct}%)";
            }))
            : null;

        return (cityLine, raceLine, veteranLine, disabilityLine);
    }

    private void BtnExportPdf_Click(object? sender, EventArgs e)
    {
        if (!ValidateHeader())
        {
            return;
        }

        var (year, month) = GetSelectedYearMonth();
        var monthName = new DateTime(year, month, 1).ToString("MMMM yyyy");

        using var saveDialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            FileName = $"MonthlyReport-{year}-{month:D2}.pdf",
            DefaultExt = "pdf"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                SaveHeaderToConfig();
                string? cityLine;
                string? raceLine;
                string? veteranLine;
                string? disabilityLine;
                using (var connection = DatabaseManager.GetConnection())
                {
                    var monthStart = new DateTime(year, month, 1);
                    var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                    (cityLine, raceLine, veteranLine, disabilityLine) = GetDemographicLinesWithPercentages(connection, monthStart, monthEnd);
                }
                using (var connection = DatabaseManager.GetConnection())
                {
                    ReportService.GenerateMonthlyActivityReportPdf(connection, year, month, GetHeader(), saveDialog.FileName, cityBreakdownLine: cityLine, raceLine: raceLine, veteranLine: veteranLine, disabilityLine: disabilityLine);
                }
                MessageBox.Show($"PDF exported successfully to:\n{saveDialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        if (!ValidateHeader())
        {
            return;
        }

        var (year, month) = GetSelectedYearMonth();

        try
        {
            SaveHeaderToConfig();
            string? cityLine;
            string? raceLine;
            string? veteranLine;
            string? disabilityLine;
            using (var connection = DatabaseManager.GetConnection())
            {
                var monthStart = new DateTime(year, month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                (cityLine, raceLine, veteranLine, disabilityLine) = GetDemographicLinesWithPercentages(connection, monthStart, monthEnd);
            }
            var tempPdfPath = Path.Combine(Path.GetTempPath(), $"MonthlyReport-{year}-{month:D2}_{Guid.NewGuid()}.pdf");
            using (var connection = DatabaseManager.GetConnection())
            {
                ReportService.GenerateMonthlyActivityReportPdf(connection, year, month, GetHeader(), tempPdfPath, cityBreakdownLine: cityLine, raceLine: raceLine, veteranLine: veteranLine, disabilityLine: disabilityLine);
            }

            // Try to print the PDF using the print verb
            bool printSuccess = false;
            try
            {
                var printProcess = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempPdfPath,
                    UseShellExecute = true,
                    Verb = "print"
                };
                var process = System.Diagnostics.Process.Start(printProcess);
                if (process != null)
                {
                    printSuccess = true;
                }
            }
            catch
            {
                // Print verb failed, will fall through to open normally
            }

            if (!printSuccess)
            {
                // If print verb doesn't work, open the PDF normally
                try
                {
                    var openProcess = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = tempPdfPath,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(openProcess);
                    MessageBox.Show(
                        "PDF opened in default viewer. Please use File > Print or Ctrl+P to print the document.",
                        "Print Document",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception openEx)
                {
                    MessageBox.Show(
                        $"Could not open PDF for printing. File saved to:\n{tempPdfPath}\n\nError: {openEx.Message}",
                        "Print Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            // Clean up temp file after a delay
            Task.Delay(10000).ContinueWith(_ =>
            {
                try
                {
                    if (File.Exists(tempPdfPath))
                    {
                        File.Delete(tempPdfPath);
                    }
                }
                catch
                {
                    // Ignore cleanup errors
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error preparing print: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool ValidateHeader()
    {
        if (string.IsNullOrWhiteSpace(txtFoodBankName.Text))
        {
            MessageBox.Show("Please enter the Food Bank name.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFoodBankName.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtCounty.Text))
        {
            MessageBox.Show("Please enter the County.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCounty.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtPreparedBy.Text))
        {
            MessageBox.Show("Please enter Prepared by.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPreparedBy.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtPhone.Text))
        {
            MessageBox.Show("Please enter the Phone number.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPhone.Focus();
            return false;
        }
        return true;
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
