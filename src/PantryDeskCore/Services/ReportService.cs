using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;
using PantryDeskCore.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PantryDeskCore.Services;

/// <summary>
/// Service for generating reports, including PDF exports.
/// </summary>
public static class ReportService
{
    static ReportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Generates a monthly summary PDF report and saves it to the specified file path.
    /// </summary>
    public static void GenerateMonthlySummaryPdf(SqliteConnection connection, int year, int month, string filePath)
    {
        var monthStart = new DateTime(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);
        var monthName = monthStart.ToString("MMMM yyyy");

        // Get all statistics
        var stats = StatisticsService.GetMonthlyStats(connection, year, month);
        var cityBreakdown = StatisticsService.GetStatsByCity(connection, monthStart, monthEnd);
        var overrideBreakdown = StatisticsService.GetOverrideBreakdown(connection, monthStart, monthEnd);
        var pantryDayBreakdown = StatisticsService.GetPantryDayBreakdown(connection, year, month);
        var composition = StatisticsService.GetCompositionServed(connection, monthStart, monthEnd);

        // Generate PDF
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(50);

                page.Header().Element(header =>
                {
                    header.Column(column =>
                    {
                        column.Item().Text($"MONTHLY SUMMARY REPORT - {monthName.ToUpper()}")
                            .FontSize(18)
                            .Bold()
                            .AlignCenter();
                        column.Item().PaddingBottom(10);
                    });
                });

                page.Content().Column(column =>
                {
                    column.Item().PaddingVertical(10);

                    // Section 1: Totals
                    column.Item().Element(elem => RenderTotalsSection(elem, stats));

                    // Section 2: Pantry Day Breakdown
                    column.Item().Element(elem => RenderPantryDayBreakdown(elem, pantryDayBreakdown));

                    // Section 3: Household Composition
                    column.Item().Element(elem => RenderCompositionSection(elem, composition));

                    // Section 4: Area Breakdown
                    column.Item().Element(elem => RenderAreaBreakdown(elem, cityBreakdown));
                });

                page.Footer().Element(footer =>
                {
                    footer.AlignCenter().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                        .FontSize(8)
                        .FontColor(Colors.Grey.Medium);
                });
            });
        });

        document.GeneratePdf(filePath);
    }

    /// <summary>
    /// Generates a statistics PDF report for a date range and saves it to the specified file path.
    /// </summary>
    /// <param name="connection">Database connection</param>
    /// <param name="startDate">Start date for statistics</param>
    /// <param name="endDate">End date for statistics</param>
    /// <param name="filePath">Output file path for PDF</param>
    /// <param name="chartImages">Optional dictionary of chart image file paths (key: chart name, value: file path)</param>
    public static void GenerateStatisticsPdf(SqliteConnection connection, DateTime startDate, DateTime endDate, string filePath, Dictionary<string, string>? chartImages = null)
    {
        var dateLabel = GetDateRangeLabel(startDate, endDate);

        // Get all statistics
        var stats = StatisticsService.GetStatsForDateRange(connection, startDate, endDate);
        var cityBreakdown = StatisticsService.GetStatsByCity(connection, startDate, endDate);
        var pantryDayBreakdown = StatisticsService.GetPantryDayVolumeByEvent(connection, startDate, endDate);
        var composition = StatisticsService.GetCompositionServed(connection, startDate, endDate);
        var monthlyTrend = StatisticsService.GetMonthlyVisitsTrend(connection, startDate, endDate);

        // Generate PDF
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(50);

                page.Header().Element(header =>
                {
                    header.Column(column =>
                    {
                        column.Item().Text($"STATISTICS REPORT - {dateLabel.ToUpper()}")
                            .FontSize(18)
                            .Bold()
                            .AlignCenter();
                        column.Item().PaddingBottom(10);
                    });
                });

                page.Content().Column(column =>
                {
                    column.Item().PaddingVertical(10);

                    // Section 1: Summary Totals
                    column.Item().Element(elem => RenderTotalsSection(elem, stats));

                    // Section 2: City Breakdown (with chart if available)
                    column.Item().Element(elem => RenderCityBreakdown(elem, cityBreakdown, chartImages));

                    // Section 3: Age Group Distribution (with chart if available)
                    column.Item().Element(elem => RenderCompositionSection(elem, composition, chartImages));

                    // Section 4: Pantry Day Breakdown (with chart if available)
                    column.Item().Element(elem => RenderPantryDayBreakdown(elem, pantryDayBreakdown, chartImages));

                    // Section 5: Monthly Trend (with chart if available)
                    if (monthlyTrend.Count > 0)
                    {
                        column.Item().Element(elem => RenderMonthlyTrendSection(elem, monthlyTrend, chartImages));
                    }
                });

                page.Footer().Element(footer =>
                {
                    footer.AlignCenter().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                        .FontSize(8)
                        .FontColor(Colors.Grey.Medium);
                });
            });
        });

        document.GeneratePdf(filePath);
    }

    private static string GetDateRangeLabel(DateTime startDate, DateTime endDate)
    {
        if (startDate.Year == endDate.Year && startDate.Month == endDate.Month)
        {
            return startDate.ToString("MMMM yyyy");
        }
        else if (startDate.Year == endDate.Year)
        {
            return $"{startDate:MMM} - {endDate:MMM} {startDate:yyyy}";
        }
        else
        {
            return $"{startDate:MMM yyyy} - {endDate:MMM yyyy}";
        }
    }

    private static void RenderTotalsSection(IContainer container, MonthlyStatistics stats)
    {
        container.Column(column =>
        {
            column.Item().Text("TOTALS").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);
            column.Item().Text($"Total Active Households: {stats.TotalActiveHouseholds:N0}");
            column.Item().Text($"Total People: {stats.TotalPeople:N0}");
            column.Item().Text($"Completed Services: {stats.CompletedServices:N0}");
            column.Item().Text($"Unique Households Served: {stats.UniqueHouseholdsServed:N0}");
            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderPantryDayBreakdown(IContainer container, List<PantryDayBreakdown> pantryDayBreakdown, Dictionary<string, string>? chartImages = null)
    {
        container.Column(column =>
        {
            column.Item().Text("PANTRY DAY BREAKDOWN").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            // Chart first (if available) - scale to fit width
            if (chartImages != null && chartImages.ContainsKey("PantryDayVolume") && File.Exists(chartImages["PantryDayVolume"]))
            {
                column.Item().Image(chartImages["PantryDayVolume"]).FitWidth();
                column.Item().PaddingBottom(10);
            }

            if (pantryDayBreakdown.Count > 0)
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("Date").Bold());
                        header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("Completed Services").Bold());
                    });

                    foreach (var pd in pantryDayBreakdown)
                    {
                        table.Cell().Element(cell => cell.Padding(5).Text(pd.PantryDate.ToString("yyyy-MM-dd")));
                        table.Cell().Element(cell => cell.Padding(5).Text(pd.CompletedServices.ToString("N0")).AlignRight());
                    }
                });
            }
            else
            {
                column.Item().Text("No pantry day services completed this month.").FontColor(Colors.Grey.Medium);
            }

            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderCompositionSection(IContainer container, (int Infant, int Child, int Adult, int Senior) composition, Dictionary<string, string>? chartImages = null)
    {
        var total = composition.Infant + composition.Child + composition.Adult + composition.Senior;

        container.Column(column =>
        {
            column.Item().Text("HOUSEHOLD COMPOSITION SERVED").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            if (chartImages != null && chartImages.ContainsKey("AgeGroupDistribution") && File.Exists(chartImages["AgeGroupDistribution"]))
            {
                column.Item().Image(chartImages["AgeGroupDistribution"]).FitArea();
                column.Item().PaddingBottom(10);
            }

            column.Item().Text($"Infant (0-2): {composition.Infant:N0}");
            column.Item().Text($"Child (2-18): {composition.Child:N0}");
            column.Item().Text($"Adult (18-55): {composition.Adult:N0}");
            column.Item().Text($"Senior (55+): {composition.Senior:N0}");
            column.Item().Text($"Total: {total:N0}").Bold();
            column.Item().PaddingTop(2);
            column.Item().Text("(Totals across unique households served in date range)").FontSize(8).FontColor(Colors.Grey.Medium);
            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderAreaBreakdown(IContainer container, List<CityBreakdown> cityBreakdown)
    {
        container.Column(column =>
        {
            column.Item().Text("AREA BREAKDOWN").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            if (cityBreakdown.Count > 0)
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("City").Bold());
                        header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("Households Served").Bold());
                        header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("Services Completed").Bold());
                    });

                    foreach (var city in cityBreakdown)
                    {
                        table.Cell().Element(cell => cell.Padding(5).Text(city.City));
                        table.Cell().Element(cell => cell.Padding(5).Text(city.HouseholdsServed.ToString("N0")).AlignRight());
                        table.Cell().Element(cell => cell.Padding(5).Text(city.ServicesCompleted.ToString("N0")).AlignRight());
                    }
                });
            }
            else
            {
                column.Item().Text("No services completed this month.").FontColor(Colors.Grey.Medium);
            }

            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderCityBreakdown(IContainer container, List<CityBreakdown> cityBreakdown, Dictionary<string, string>? chartImages = null)
    {
        container.Column(column =>
        {
            column.Item().Text("CITY BREAKDOWN").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            // Chart first (if available)
            if (chartImages != null && chartImages.ContainsKey("CityDistribution") && File.Exists(chartImages["CityDistribution"]))
            {
                column.Item().Image(chartImages["CityDistribution"]).FitArea();
                column.Item().PaddingBottom(10);
            }

            if (cityBreakdown.Count > 0)
            {
                // Table
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("City").Bold());
                        header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("Households Served").Bold());
                        header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("Services Completed").Bold());
                    });

                    foreach (var city in cityBreakdown)
                    {
                        table.Cell().Element(cell => cell.Padding(5).Text(city.City));
                        table.Cell().Element(cell => cell.Padding(5).Text(city.HouseholdsServed.ToString("N0")).AlignRight());
                        table.Cell().Element(cell => cell.Padding(5).Text(city.ServicesCompleted.ToString("N0")).AlignRight());
                    }
                });
            }
            else
            {
                column.Item().Text("No services completed in this date range.").FontColor(Colors.Grey.Medium);
            }

            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderMonthlyTrendSection(IContainer container, List<MonthlyVisitsTrend> monthlyTrend, Dictionary<string, string>? chartImages = null)
    {
        container.Column(column =>
        {
            column.Item().Text("MONTHLY VISITS TREND").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            // Chart first (if available)
            if (chartImages != null && chartImages.ContainsKey("MonthlyVisitsTrend") && File.Exists(chartImages["MonthlyVisitsTrend"]))
            {
                column.Item().Image(chartImages["MonthlyVisitsTrend"]).FitArea();
                column.Item().PaddingBottom(10);
            }

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("Month").Bold());
                    header.Cell().Element(cell => cell.Background(Colors.Grey.Lighten3).Padding(5).Text("Completed Services").Bold());
                });

                foreach (var trend in monthlyTrend.OrderBy(t => t.Month))
                {
                    var monthLabel = DateTime.TryParseExact(trend.Month + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthDate)
                        ? monthDate.ToString("MMMM yyyy")
                        : trend.Month;
                    table.Cell().Element(cell => cell.Padding(5).Text(monthLabel));
                    table.Cell().Element(cell => cell.Padding(5).Text(trend.Count.ToString("N0")).AlignRight());
                }
            });

            column.Item().PaddingBottom(10);
        });
    }
}
