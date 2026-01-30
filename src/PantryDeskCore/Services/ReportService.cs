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

    private static void RenderPantryDayBreakdown(IContainer container, List<PantryDayBreakdown> pantryDayBreakdown)
    {
        container.Column(column =>
        {
            column.Item().Text("PANTRY DAY BREAKDOWN").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

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

    private static void RenderCompositionSection(IContainer container, (int Children, int Adults, int Seniors) composition)
    {
        var total = composition.Children + composition.Adults + composition.Seniors;

        container.Column(column =>
        {
            column.Item().Text("HOUSEHOLD COMPOSITION SERVED").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);
            column.Item().Text($"Children: {composition.Children:N0}");
            column.Item().Text($"Adults: {composition.Adults:N0}");
            column.Item().Text($"Seniors: {composition.Seniors:N0}");
            column.Item().Text($"Total: {total:N0}").Bold();
            column.Item().PaddingTop(2);
            column.Item().Text("(Totals across unique households served this month)").FontSize(8).FontColor(Colors.Grey.Medium);
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
}
