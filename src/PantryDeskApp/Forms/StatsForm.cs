using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Services;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using OxyPlot.Legends;
using OxyPlot.ImageSharp;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form displaying statistics dashboard with date range selection and charts.
/// </summary>
public partial class StatsForm : Form
{
    private int _selectedYear;

    // Colorblind-friendly color palette (ColorBrewer Set2)
    private static readonly OxyColor[] ChartColors = new[]
    {
        OxyColor.FromRgb(102, 194, 165), // Teal
        OxyColor.FromRgb(252, 141, 98), // Orange
        OxyColor.FromRgb(141, 160, 203), // Blue
        OxyColor.FromRgb(231, 138, 195), // Pink
        OxyColor.FromRgb(166, 216, 84),  // Green
        OxyColor.FromRgb(255, 217, 47),  // Yellow
        OxyColor.FromRgb(229, 196, 148), // Tan
        OxyColor.FromRgb(179, 179, 179)  // Gray
    };

    /// <summary>
    /// Creates a PieSeries configured for outside labels with extended connector lines.
    /// Labels use format "{label} {n}%".
    /// </summary>
    private static PieSeries CreatePieSeriesWithOutsideLabels()
    {
        return new PieSeries
        {
            AngleSpan = 360,
            StartAngle = 0,
            OutsideLabelFormat = "{1} {2:0}%",
            InsideLabelFormat = null,
            InsideLabelColor = OxyColors.Undefined,
            TickRadialLength = 20,
            TickHorizontalLength = 25,
            TickLabelDistance = 8
        };
    }

    /// <summary>
    /// Creates a PieSeries with outside labels but no horizontal connector (avoids label cutoff in tight layouts).
    /// </summary>
    private static PieSeries CreatePieSeriesWithOutsideLabelsNoHorizontalTick()
    {
        return new PieSeries
        {
            AngleSpan = 360,
            StartAngle = 0,
            OutsideLabelFormat = "{1} {2:0}%",
            InsideLabelFormat = null,
            InsideLabelColor = OxyColors.Undefined,
            TickRadialLength = 20,
            TickHorizontalLength = 0,
            TickLabelDistance = 8
        };
    }

    public StatsForm()
    {
        InitializeComponent();
        InitializeDateRangeSelector();
        InitializeChartTooltips();
        
        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void InitializeChartTooltips()
    {
        ConfigureHoverTooltips(plotViewCityDistribution);
        ConfigureHoverTooltips(plotViewAgeGroupDistribution);
        ConfigureHoverTooltips(plotViewRaceDistribution);
        ConfigureHoverTooltips(plotViewVeteranDistribution);
        ConfigureHoverTooltips(plotViewDisabilityDistribution);
        ConfigureHoverTooltips(plotViewVisitType);
        ConfigureHoverTooltips(plotViewEventType);
        ConfigureHoverTooltips(plotViewMonthlyVisitsTrend);
        ConfigureHoverTooltips(plotViewPantryDayVolume);
    }

    private void ConfigureHoverTooltips(OxyPlot.WindowsForms.PlotView plotView)
    {
        // Don't unbind click - keep default click behavior
        // Add hover by calling HandleMouseDown on mouse move
        // This simulates a click to show the tracker
        
        // Track last update time to throttle updates
        DateTime lastUpdate = DateTime.MinValue;
        const int throttleMs = 50; // Update at most every 50ms
        
        // Handle mouse move to show tooltips on hover
        plotView.MouseMove += (s, e) =>
        {
            if (plotView.Model == null) return;
            
            // Throttle updates to reduce flicker
            var now = DateTime.Now;
            if ((now - lastUpdate).TotalMilliseconds < throttleMs)
            {
                return;
            }
            lastUpdate = now;
            
            try
            {
                // Convert screen coordinates
                var screenPoint = new ScreenPoint(e.X, e.Y);
                
                // Create mouse down event args to simulate a click
                var mouseDownArgs = new OxyMouseDownEventArgs
                {
                    Position = screenPoint,
                    ChangedButton = OxyMouseButton.Left,
                    ClickCount = 1
                };
                
                // Handle as if it were a mouse down event (which shows tracker)
                // This enables hover tooltips while keeping click functionality
                plotView.ActualController.HandleMouseDown(plotView, mouseDownArgs);
            }
            catch (Exception ex)
            {
                // Log error for debugging
                System.Diagnostics.Debug.WriteLine($"Tooltip error: {ex.Message}");
            }
        };
        
        // Hide tracker when mouse leaves
        plotView.MouseLeave += (s, e) =>
        {
            try
            {
                plotView.HideTracker();
            }
            catch
            {
                // Ignore errors
            }
        };
    }

    private void InitializeDateRangeSelector()
    {
        cmbDateRange.Items.Clear();
        var currentYear = DateTime.Now.Year;
        for (var year = currentYear; year >= currentYear - 10; year--)
        {
            cmbDateRange.Items.Add(year.ToString());
        }
        _selectedYear = currentYear - 1; // Default to last year
        cmbDateRange.SelectedItem = _selectedYear.ToString();

        lblStartDate.Visible = false;
        dtpStartDate.Visible = false;
        lblEndDate.Visible = false;
        dtpEndDate.Visible = false;
    }

    private void StatsForm_Load(object? sender, EventArgs e)
    {
        ShowDemographicsPage();
        LoadStats();
    }

    private void BtnPageDemographics_Click(object? sender, EventArgs e)
    {
        ShowDemographicsPage();
    }

    private void BtnPageServices_Click(object? sender, EventArgs e)
    {
        ShowServicesPage();
    }

    private void ShowDemographicsPage()
    {
        pnlDemographics.Visible = true;
        pnlServices.Visible = false;
    }

    private void ShowServicesPage()
    {
        pnlDemographics.Visible = false;
        pnlServices.Visible = true;
    }

    private (DateTime startDate, DateTime endDate) GetCurrentDateRange()
    {
        return (new DateTime(_selectedYear, 1, 1), new DateTime(_selectedYear, 12, 31));
    }

    private void LoadStats()
    {
        try
        {
            var (startDate, endDate) = GetCurrentDateRange();

            using var connection = DatabaseManager.GetConnection();
            var stats = StatisticsService.GetStatsForDateRange(connection, startDate, endDate);
            var cityBreakdown = StatisticsService.GetStatsByCity(connection, startDate, endDate);
            var composition = StatisticsService.GetCompositionServed(connection, startDate, endDate);
            var raceBreakdown = StatisticsService.GetDemographicsByRace(connection, startDate, endDate);
            var veteranBreakdown = StatisticsService.GetDemographicsByVeteranStatus(connection, startDate, endDate);
            var disabilityBreakdown = StatisticsService.GetDemographicsByDisabledStatus(connection, startDate, endDate);
            var visitTypeBreakdown = StatisticsService.GetVisitTypeBreakdown(connection, startDate, endDate);
            var monthlyTrend = StatisticsService.GetMonthlyVisitsTrend(connection, startDate, endDate);
            var pantryDayVolume = StatisticsService.GetPantryDayVolumeByEvent(connection, startDate, endDate);

            // Update summary cards
            lblCardTotalActiveHouseholdsValue.Text = stats.UniqueHouseholdsServed.ToString("N0");
            lblCardTotalPeopleValue.Text = stats.TotalPeople.ToString("N0");
            lblCardCompletedServicesValue.Text = stats.CompletedServices.ToString("N0");
            lblCardUniqueHouseholdsServedValue.Text = stats.DeckTotal.ToString("N0");

            // Demographics page charts
            PopulateCityDistributionChart(cityBreakdown);
            PopulateAgeGroupDistributionChart(composition);
            PopulateDemographicsBreakdownChart(plotViewRaceDistribution, "Race Distribution", raceBreakdown);
            PopulateDemographicsBreakdownChart(plotViewVeteranDistribution, "Veteran Status Distribution", veteranBreakdown);
            PopulateDemographicsBreakdownChart(plotViewDisabilityDistribution, "Disability Status Distribution", disabilityBreakdown);

            // Services page charts
            PopulateVisitTypeChart(visitTypeBreakdown);
            PopulateEventTypeChart(stats.PantryDayCompletions, stats.AppointmentCompletions);
            PopulateMonthlyVisitsTrendChart(monthlyTrend);
            PopulatePantryDayVolumeChart(pantryDayVolume);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void PopulateCityDistributionChart(List<CityBreakdown> cityBreakdown)
    {
        var plotModel = new PlotModel 
        { 
            Title = "City Distribution",
            PlotAreaBorderThickness = new OxyThickness(0),
            PlotMargins = new OxyThickness(120, 40, 120, 120)
        };
        
        if (cityBreakdown.Count == 0)
        {
            plotModel.Title = "City Distribution (No Data)";
            plotViewCityDistribution.Model = plotModel;
            return;
        }

        var pieSeries = CreatePieSeriesWithOutsideLabels();

        int colorIndex = 0;
        foreach (var city in cityBreakdown)
        {
            pieSeries.Slices.Add(new PieSlice(city.City, city.HouseholdsServed)
            {
                Fill = ChartColors[colorIndex % ChartColors.Length],
                IsExploded = false
            });
            colorIndex++;
        }

        plotModel.Series.Add(pieSeries);
        plotViewCityDistribution.Model = plotModel;
    }

    private void PopulateAgeGroupDistributionChart((int Infant, int Child, int Adult, int Senior) composition)
    {
        var plotModel = new PlotModel 
        { 
            Title = "Age Group Distribution",
            PlotAreaBorderThickness = new OxyThickness(0),
            PlotMargins = new OxyThickness(120, 40, 120, 120)
        };
        
        var total = composition.Infant + composition.Child + composition.Adult + composition.Senior;
        if (total == 0)
        {
            plotModel.Title = "Age Group Distribution (No Data)";
            plotViewAgeGroupDistribution.Model = plotModel;
            return;
        }

        var pieSeries = CreatePieSeriesWithOutsideLabels();

        var slices = new[] { ("Infant (0-2)", composition.Infant), ("Child (2-18)", composition.Child), ("Adult (18-55)", composition.Adult), ("Senior (55+)", composition.Senior) };
        int colorIndex = 0;
        foreach (var (label, count) in slices)
        {
            if (count > 0)
            {
                pieSeries.Slices.Add(new PieSlice(label, count) { Fill = ChartColors[colorIndex % ChartColors.Length], IsExploded = false });
                colorIndex++;
            }
        }

        plotModel.Series.Add(pieSeries);
        plotViewAgeGroupDistribution.Model = plotModel;
    }

    private void PopulateDemographicsBreakdownChart(OxyPlot.WindowsForms.PlotView plotView, string title, List<DemographicsBreakdown> breakdown)
    {
        var plotModel = new PlotModel
        {
            Title = title,
            PlotAreaBorderThickness = new OxyThickness(0),
            PlotMargins = new OxyThickness(120, 40, 120, 120)
        };

        if (breakdown.Count == 0)
        {
            plotModel.Title = $"{title} (No Data)";
            plotView.Model = plotModel;
            return;
        }

        var pieSeries = CreatePieSeriesWithOutsideLabels();

        int colorIndex = 0;
        foreach (var item in breakdown)
        {
            pieSeries.Slices.Add(new PieSlice(item.Label, item.Count)
            {
                Fill = ChartColors[colorIndex % ChartColors.Length],
                IsExploded = false
            });
            colorIndex++;
        }

        plotModel.Series.Add(pieSeries);
        plotView.Model = plotModel;
    }

    private void PopulateVisitTypeChart(List<DemographicsBreakdown> visitTypeBreakdown)
    {
        var plotModel = new PlotModel
        {
            Title = "Visit Type",
            PlotAreaBorderThickness = new OxyThickness(0),
            PlotMargins = new OxyThickness(120, 40, 120, 120)
        };

        if (visitTypeBreakdown.Count == 0)
        {
            plotModel.Title = "Visit Type (No Data)";
            plotViewVisitType.Model = plotModel;
            return;
        }

        var pieSeries = CreatePieSeriesWithOutsideLabelsNoHorizontalTick();

        int colorIndex = 0;
        foreach (var item in visitTypeBreakdown)
        {
            pieSeries.Slices.Add(new PieSlice(item.Label, item.Count)
            {
                Fill = ChartColors[colorIndex % ChartColors.Length],
                IsExploded = false
            });
            colorIndex++;
        }

        plotModel.Series.Add(pieSeries);
        plotViewVisitType.Model = plotModel;
    }

    private void PopulateEventTypeChart(int pantryDayCount, int appointmentCount)
    {
        var plotModel = new PlotModel
        {
            Title = "Event Type",
            PlotAreaBorderThickness = new OxyThickness(0),
            PlotMargins = new OxyThickness(120, 40, 120, 120)
        };

        var total = pantryDayCount + appointmentCount;
        if (total == 0)
        {
            plotModel.Title = "Event Type (No Data)";
            plotViewEventType.Model = plotModel;
            return;
        }

        var pieSeries = CreatePieSeriesWithOutsideLabelsNoHorizontalTick();

        pieSeries.Slices.Add(new PieSlice("Pantry Day", pantryDayCount) { Fill = ChartColors[0], IsExploded = false });
        pieSeries.Slices.Add(new PieSlice("Appointment", appointmentCount) { Fill = ChartColors[1], IsExploded = false });

        plotModel.Series.Add(pieSeries);
        plotViewEventType.Model = plotModel;
    }

    private void PopulateMonthlyVisitsTrendChart(List<MonthlyVisitsTrend> monthlyTrend)
    {
        var plotModel = new PlotModel { Title = "Monthly Visits Trend" };

        if (monthlyTrend.Count == 0)
        {
            plotModel.Title = "Monthly Visits Trend (No Data)";
            plotViewMonthlyVisitsTrend.Model = plotModel;
            return;
        }

        // Bar width 10 days on each side (20 total) - avoids overlap in February (28 days)
        const double halfBarWidth = 10.0;

        var barSeries = new RectangleBarSeries
        {
            Title = "Completed Services",
            FillColor = ChartColors[0],
            TrackerFormatString = "Month: {3:yyyy-MM}\nCount: {Y1:0}"
        };

        foreach (var trend in monthlyTrend.OrderBy(t => t.Month))
        {
            if (DateTime.TryParseExact(trend.Month + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDate))
            {
                var monthValue = DateTimeAxis.ToDouble(monthDate);
                barSeries.Items.Add(new RectangleBarItem(monthValue - halfBarWidth, 0, monthValue + halfBarWidth, trend.Count));
            }
        }

        plotModel.Axes.Add(new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            StringFormat = "MMM yyyy",
            Title = "Month",
            MajorStep = 30.44,
            MinorTickSize = 0
        });
        plotModel.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Count",
            Minimum = 0,
            MajorGridlineStyle = LineStyle.Solid,
            MajorGridlineColor = OxyColor.FromArgb(128, 200, 200, 200),
            MinorGridlineStyle = LineStyle.None,
            MajorStep = 10
        });

        plotModel.Series.Add(barSeries);
        plotViewMonthlyVisitsTrend.Model = plotModel;
    }

    private void PopulatePantryDayVolumeChart(List<PantryDayBreakdown> pantryDayVolume)
    {
        var plotModel = new PlotModel { Title = "Pantry Day Volume by Event" };
        
        if (pantryDayVolume.Count == 0)
        {
            plotModel.Title = "Pantry Day Volume by Event (No Data)";
            plotViewPantryDayVolume.Model = plotModel;
            return;
        }

        // Use RectangleBarSeries for date-based bar chart
        var barSeries = new RectangleBarSeries
        {
            Title = "Completed Services",
            FillColor = ChartColors[0],
            TrackerFormatString = "Pantry Day: {3:yyyy-MM-dd}\nCount: {Y1:0}"
        };

        foreach (var volume in pantryDayVolume.OrderBy(v => v.PantryDate))
        {
            var dateValue = DateTimeAxis.ToDouble(volume.PantryDate);
            // Create a bar with width of 0.5 days centered on the date
            barSeries.Items.Add(new RectangleBarItem(dateValue - 0.25, 0, dateValue + 0.25, volume.CompletedServices));
        }

        plotModel.Axes.Add(new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            StringFormat = "MMM dd",
            Title = "Pantry Day"
        });
        plotModel.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Count",
            Minimum = 0,
            MajorGridlineStyle = LineStyle.Solid,
            MajorGridlineColor = OxyColor.FromArgb(128, 200, 200, 200),
            MinorGridlineStyle = LineStyle.None,
            MajorStep = 10
        });

        plotModel.Series.Add(barSeries);
        plotViewPantryDayVolume.Model = plotModel;
    }

    /// <summary>
    /// Gets a formatted label for the selected year.
    /// </summary>
    private string GetDateRangeLabel(DateTime startDate, DateTime endDate)
    {
        if (startDate.Month == 1 && startDate.Day == 1 && endDate.Month == 12 && endDate.Day == 31 && startDate.Year == endDate.Year)
        {
            return startDate.Year.ToString();
        }
        if (startDate.Year == endDate.Year && startDate.Month == endDate.Month)
        {
            return startDate.ToString("MMMM yyyy");
        }
        if (startDate.Year == endDate.Year)
        {
            return $"{startDate:MMM} - {endDate:MMM} {startDate:yyyy}";
        }
        return $"{startDate:MMM yyyy} - {endDate:MMM yyyy}";
    }

    private void CmbDateRange_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbDateRange.SelectedItem is string yearStr && int.TryParse(yearStr, out var year))
        {
            _selectedYear = year;
            LoadStats();
        }
    }

    private void BtnExportPdf_Click(object? sender, EventArgs e)
    {
        var (startDate, endDate) = GetCurrentDateRange();
        var dateLabel = GetDateRangeLabel(startDate, endDate);

        using var saveDialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            FileName = $"Statistics_{dateLabel.Replace(" ", "_").Replace("/", "-")}.pdf",
            DefaultExt = "pdf"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Export charts to temporary images
                var chartImages = new Dictionary<string, string>();
                
                if (plotViewCityDistribution.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewCityDistribution.Model, "CityDistribution");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["CityDistribution"] = imgPath;
                }
                
                if (plotViewAgeGroupDistribution.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewAgeGroupDistribution.Model, "AgeGroupDistribution");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["AgeGroupDistribution"] = imgPath;
                }
                
                if (plotViewMonthlyVisitsTrend.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewMonthlyVisitsTrend.Model, "MonthlyVisitsTrend");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["MonthlyVisitsTrend"] = imgPath;
                }
                
                if (plotViewPantryDayVolume.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewPantryDayVolume.Model, "PantryDayVolume");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["PantryDayVolume"] = imgPath;
                }

                if (plotViewRaceDistribution.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewRaceDistribution.Model, "RaceDistribution");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["RaceDistribution"] = imgPath;
                }

                if (plotViewVeteranDistribution.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewVeteranDistribution.Model, "VeteranDistribution");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["VeteranDistribution"] = imgPath;
                }

                if (plotViewDisabilityDistribution.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewDisabilityDistribution.Model, "DisabilityDistribution");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["DisabilityDistribution"] = imgPath;
                }

                if (plotViewVisitType.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewVisitType.Model, "VisitType");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["VisitType"] = imgPath;
                }

                if (plotViewEventType.Model != null)
                {
                    var imgPath = ExportChartToImage(plotViewEventType.Model, "EventType");
                    if (!string.IsNullOrEmpty(imgPath))
                        chartImages["EventType"] = imgPath;
                }

                using var connection = DatabaseManager.GetConnection();
                ReportService.GenerateStatisticsPdf(connection, startDate, endDate, saveDialog.FileName, chartImages);
                
                // Clean up temp image files
                foreach (var imgPath in chartImages.Values)
                {
                    try
                    {
                        if (File.Exists(imgPath))
                            File.Delete(imgPath);
                    }
                    catch { }
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
        var (startDate, endDate) = GetCurrentDateRange();

        try
        {
            // Export charts to temporary images
            var chartImages = new Dictionary<string, string>();
            
            if (plotViewCityDistribution.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewCityDistribution.Model, "CityDistribution");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["CityDistribution"] = imgPath;
            }
            
            if (plotViewAgeGroupDistribution.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewAgeGroupDistribution.Model, "AgeGroupDistribution");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["AgeGroupDistribution"] = imgPath;
            }
            
            if (plotViewMonthlyVisitsTrend.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewMonthlyVisitsTrend.Model, "MonthlyVisitsTrend");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["MonthlyVisitsTrend"] = imgPath;
            }
            
            if (plotViewPantryDayVolume.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewPantryDayVolume.Model, "PantryDayVolume");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["PantryDayVolume"] = imgPath;
            }

            if (plotViewRaceDistribution.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewRaceDistribution.Model, "RaceDistribution");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["RaceDistribution"] = imgPath;
            }

            if (plotViewVeteranDistribution.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewVeteranDistribution.Model, "VeteranDistribution");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["VeteranDistribution"] = imgPath;
            }

            if (plotViewDisabilityDistribution.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewDisabilityDistribution.Model, "DisabilityDistribution");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["DisabilityDistribution"] = imgPath;
            }

            if (plotViewVisitType.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewVisitType.Model, "VisitType");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["VisitType"] = imgPath;
            }

            if (plotViewEventType.Model != null)
            {
                var imgPath = ExportChartToImage(plotViewEventType.Model, "EventType");
                if (!string.IsNullOrEmpty(imgPath))
                    chartImages["EventType"] = imgPath;
            }

            // Generate PDF to temporary file
            var tempPdfPath = Path.Combine(Path.GetTempPath(), $"Statistics_{Guid.NewGuid()}.pdf");
            
            using var connection = DatabaseManager.GetConnection();
            ReportService.GenerateStatisticsPdf(connection, startDate, endDate, tempPdfPath, chartImages);
            
            // Clean up temp image files
            foreach (var imgPath in chartImages.Values)
            {
                try
                {
                    if (File.Exists(imgPath))
                        File.Delete(imgPath);
                }
                catch { }
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
            MessageBox.Show($"Error printing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Exports a PlotModel to a PNG image file and returns the file path.
    /// Returns empty string if export fails.
    /// </summary>
    private string ExportChartToImage(PlotModel plotModel, string chartName)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"{chartName}_{Guid.NewGuid()}.png");
        
        try
        {
            using (var stream = File.Create(tempPath))
            {
                // PngExporter constructor: width, height, DPI
                var exporter = new OxyPlot.ImageSharp.PngExporter(800, 600, 96);
                exporter.Export(plotModel, stream);
            }
            return tempPath;
        }
        catch
        {
            // If export fails, clean up and return empty string
            if (File.Exists(tempPath))
            {
                try { File.Delete(tempPath); } catch { }
            }
            return string.Empty;
        }
    }
}
