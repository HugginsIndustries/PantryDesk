using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Services;
using System.Diagnostics;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form for viewing and exporting monthly summary reports.
/// </summary>
public partial class MonthlySummaryForm : Form
{
    private string _currentReportText = string.Empty;

    public MonthlySummaryForm()
    {
        InitializeComponent();
        
        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void MonthlySummaryForm_Load(object? sender, EventArgs e)
    {
        UpdatePreview();
    }

    private void DtpMonth_ValueChanged(object? sender, EventArgs e)
    {
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        var selectedDate = dtpMonth.Value;
        var year = selectedDate.Year;
        var month = selectedDate.Month;

        try
        {
            using var connection = DatabaseManager.GetConnection();
            var stats = StatisticsService.GetMonthlyStats(connection, year, month);
            var cityBreakdown = StatisticsService.GetStatsByCity(connection, GetMonthStart(year, month), GetMonthEnd(year, month));
            var overrideBreakdown = StatisticsService.GetOverrideBreakdown(connection, GetMonthStart(year, month), GetMonthEnd(year, month));
            var pantryDayBreakdown = StatisticsService.GetPantryDayBreakdown(connection, year, month);
            var composition = StatisticsService.GetCompositionServed(connection, GetMonthStart(year, month), GetMonthEnd(year, month));

            _currentReportText = GenerateReportText(year, month, stats, cityBreakdown, overrideBreakdown, pantryDayBreakdown, composition);
            txtPreview.Text = _currentReportText;

            // Enable/disable buttons based on data
            var hasData = stats.CompletedServices > 0;
            btnExportPdf.Enabled = hasData;
            btnPrint.Enabled = hasData;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading monthly summary: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtPreview.Text = "Error loading data.";
            btnExportPdf.Enabled = false;
            btnPrint.Enabled = false;
        }
    }

    private DateTime GetMonthStart(int year, int month)
    {
        return new DateTime(year, month, 1);
    }

    private DateTime GetMonthEnd(int year, int month)
    {
        return new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
    }

    private string GenerateReportText(int year, int month, MonthlyStatistics stats, List<CityBreakdown> cityBreakdown, 
        List<OverrideBreakdown> overrideBreakdown, List<PantryDayBreakdown> pantryDayBreakdown, (int Children, int Adults, int Seniors) composition)
    {
        var monthName = new DateTime(year, month, 1).ToString("MMMM yyyy");
        var sb = new System.Text.StringBuilder();

        sb.AppendLine($"MONTHLY SUMMARY REPORT - {monthName.ToUpper()}");
        sb.AppendLine(new string('=', 60));
        sb.AppendLine();

        // Section 1: Totals
        sb.AppendLine("TOTALS");
        sb.AppendLine(new string('-', 60));
        sb.AppendLine($"Total Active Households: {stats.TotalActiveHouseholds:N0}");
        sb.AppendLine($"Total People: {stats.TotalPeople:N0}");
        sb.AppendLine($"Completed Services: {stats.CompletedServices:N0}");
        sb.AppendLine($"Unique Households Served: {stats.UniqueHouseholdsServed:N0}");
        sb.AppendLine();

        // Section 2: Pantry Day Breakdown
        sb.AppendLine("PANTRY DAY BREAKDOWN");
        sb.AppendLine(new string('-', 60));
        if (pantryDayBreakdown.Count > 0)
        {
            sb.AppendLine($"{"Date",-12} {"Completed Services",15}");
            sb.AppendLine(new string('-', 60));
            foreach (var pd in pantryDayBreakdown)
            {
                sb.AppendLine($"{pd.PantryDate:yyyy-MM-dd,-12} {pd.CompletedServices,15:N0}");
            }
        }
        else
        {
            sb.AppendLine("No pantry day services completed this month.");
        }
        sb.AppendLine();

        // Section 3: Household Composition Served
        sb.AppendLine("HOUSEHOLD COMPOSITION SERVED");
        sb.AppendLine(new string('-', 60));
        sb.AppendLine($"Children: {composition.Children:N0}");
        sb.AppendLine($"Adults: {composition.Adults:N0}");
        sb.AppendLine($"Seniors: {composition.Seniors:N0}");
        sb.AppendLine($"Total: {composition.Children + composition.Adults + composition.Seniors:N0}");
        sb.AppendLine();
        sb.AppendLine("(Totals across unique households served this month)");
        sb.AppendLine();

        // Section 4: Area Breakdown
        sb.AppendLine("AREA BREAKDOWN");
        sb.AppendLine(new string('-', 60));
        if (cityBreakdown.Count > 0)
        {
            sb.AppendLine($"{"City",-20} {"Households Served",20} {"Services Completed",20}");
            sb.AppendLine(new string('-', 60));
            foreach (var city in cityBreakdown)
            {
                sb.AppendLine($"{city.City,-20} {city.HouseholdsServed,20:N0} {city.ServicesCompleted,20:N0}");
            }
        }
        else
        {
            sb.AppendLine("No services completed this month.");
        }
        sb.AppendLine();

        // Footer
        sb.AppendLine(new string('=', 60));
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        return sb.ToString();
    }

    private void BtnExportPdf_Click(object? sender, EventArgs e)
    {
        var selectedDate = dtpMonth.Value;
        var year = selectedDate.Year;
        var month = selectedDate.Month;

        using var saveDialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            FileName = $"MonthlySummary_{year}_{month:D2}.pdf",
            DefaultExt = "pdf"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                using var connection = DatabaseManager.GetConnection();
                ReportService.GenerateMonthlySummaryPdf(connection, year, month, saveDialog.FileName);
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
        var selectedDate = dtpMonth.Value;
        var year = selectedDate.Year;
        var month = selectedDate.Month;

        try
        {
            // Generate PDF to temporary file
            var tempPdfPath = Path.Combine(Path.GetTempPath(), $"MonthlySummary_{year}_{month:D2}_{Guid.NewGuid()}.pdf");
            
            using var connection = DatabaseManager.GetConnection();
            ReportService.GenerateMonthlySummaryPdf(connection, year, month, tempPdfPath);

            // Try to print the PDF using the print verb
            // If that fails, open it normally and let user print manually
            bool printSuccess = false;
            try
            {
                var printProcess = new ProcessStartInfo
                {
                    FileName = tempPdfPath,
                    UseShellExecute = true,
                    Verb = "print"
                };
                var process = Process.Start(printProcess);
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
                // User can then print from the PDF viewer
                try
                {
                    var openProcess = new ProcessStartInfo
                    {
                        FileName = tempPdfPath,
                        UseShellExecute = true
                    };
                    Process.Start(openProcess);
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

            // Clean up temp file after a delay (give time for print spooler to read it)
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
            MessageBox.Show($"Error printing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
