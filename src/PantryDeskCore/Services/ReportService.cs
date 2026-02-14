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
        var raceBreakdown = StatisticsService.GetDemographicsByRace(connection, startDate, endDate);
        var veteranBreakdown = StatisticsService.GetDemographicsByVeteranStatus(connection, startDate, endDate);
        var disabilityBreakdown = StatisticsService.GetDemographicsByDisabledStatus(connection, startDate, endDate);
        var visitTypeBreakdown = StatisticsService.GetVisitTypeBreakdown(connection, startDate, endDate);
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

                    // Page 1: Cover page with Summary Totals
                    column.Item().Element(elem => RenderTotalsSection(elem, stats, startDate.Year));

                    // Page 2: City Breakdown
                    column.Item().PageBreak();
                    column.Item().Element(elem => RenderCityBreakdown(elem, cityBreakdown, chartImages));

                    // Page 3: Age Distribution
                    column.Item().PageBreak();
                    column.Item().Element(elem => RenderAgeDistributionSection(elem, composition, chartImages));

                    // Page 4: Race Distribution
                    column.Item().PageBreak();
                    column.Item().Element(elem => RenderRaceDistributionSection(elem, raceBreakdown, chartImages));

                    // Page 5: Veteran Status
                    column.Item().PageBreak();
                    column.Item().Element(elem => RenderVeteranStatusSection(elem, veteranBreakdown, chartImages));

                    // Page 6: Disability Status
                    column.Item().PageBreak();
                    column.Item().Element(elem => RenderDisabilityStatusSection(elem, disabilityBreakdown, chartImages));

                    // Page 7: Visit Type Breakdown
                    column.Item().PageBreak();
                    column.Item().Element(elem => RenderVisitTypeSection(elem, visitTypeBreakdown, chartImages));

                    // Page 8: Event Type Breakdown
                    column.Item().PageBreak();
                    column.Item().Element(elem => RenderEventTypeSection(elem, stats, chartImages));

                    // Page 9+: Pantry Day Breakdown (flows across pages as needed)
                    column.Item().PageBreak();
                    column.Item().Element(elem => RenderPantryDayBreakdown(elem, pantryDayBreakdown, chartImages));

                    // Next: Monthly Visits Trend
                    if (monthlyTrend.Count > 0)
                    {
                        column.Item().PageBreak();
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

    /// <summary>
    /// Generates the Monthly Activity Report PDF (one Letter-size page, landscape).
    /// Uses same statistics as Statistics Dashboard: all completed services in range. Deck-only averages added to duplicated individuals only when deck stats exist.
    /// </summary>
    /// <param name="cityBreakdownLine">Optional one-line text of city counts with " · " separators (e.g. "Winlock: 76 · Vader: 37"); if provided, shown as "Total Households (per city): ..." after Households Served.</param>
    /// <param name="raceLine">Optional one-line race distribution (e.g. "White: 100 · Hispanic: 50"); shown as "Race Distribution: ..." below Individuals table.</param>
    /// <param name="veteranLine">Optional one-line veteran status (includes derived "Disabled Veteran"); shown as "Veteran Status: ..." below Individuals table.</param>
    /// <param name="disabilityLine">Optional one-line disability status; shown as "Disability Status: ..." below Individuals table.</param>
    public static void GenerateMonthlyActivityReportPdf(SqliteConnection connection, int year, int month, MonthlyActivityReportHeader header, string filePath, string? cityBreakdownLine = null, string? raceLine = null, string? veteranLine = null, string? disabilityLine = null)
    {
        var monthName = new DateTime(year, month, 1).ToString("MMMM yyyy");
        var deckStats = DeckStatsRepository.Get(connection, year, month);
        var stats = StatisticsService.GetMonthlyActivityReportStats(connection, year, month, deckStats);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter.Landscape());
                page.Margin(40);

                page.Header().Element(h =>
                {
                    h.Column(c =>
                    {
                        c.Item().Text("MONTHLY ACTIVITY REPORT: FOOD BANKS").FontSize(15).Bold().AlignCenter();
                        c.Item().Text("This report is due by the 10th of each month.").FontSize(10).AlignCenter();
                        c.Item().PaddingTop(4);
                        c.Item().Row(r =>
                        {
                            r.RelativeItem().Column(left =>
                            {
                                left.Item().Text($"Food Bank: {header.FoodBankName}").FontSize(11);
                                left.Item().Text($"County: {header.County}").FontSize(11);
                                left.Item().Text($"Prepared by: {header.PreparedBy}").FontSize(11);
                            });
                            r.RelativeItem().Column(right =>
                            {
                                right.Item().AlignRight().Text($"Month/Year: {monthName}").FontSize(11);
                                right.Item().AlignRight().Text($"Phone: {header.Phone}").FontSize(11);
                            });
                        });
                        c.Item().PaddingBottom(6);
                    });
                });

                page.Content().Column(column =>
                {
                    column.Item().Row(r =>
                    {
                        r.RelativeItem().AlignLeft().Text($"Total number of days open for food distribution this month: {stats.TotalDaysOpen} days").FontSize(11);
                        r.RelativeItem().AlignRight().Text($"Total pounds of food distributed from all sources: {stats.TotalPounds:N0} lbs").FontSize(11);
                    });
                    column.Item().PaddingVertical(4);

                    column.Item().Text("Households Served:").FontSize(12).Bold();
                    column.Item().PaddingBottom(4);
                    column.Item().Row(r =>
                    {
                        r.RelativeItem().AlignLeft().Text($"Duplicated (returning): {stats.HouseholdsDuplicated}").FontSize(11);
                        r.RelativeItem().AlignCenter().Text($"Unduplicated (first visit this year): {stats.HouseholdsUnduplicated}").FontSize(11);
                        r.RelativeItem().AlignRight().Text($"Total Households Served: {stats.HouseholdsTotal}").FontSize(11);
                    });
                    if (!string.IsNullOrEmpty(cityBreakdownLine))
                    {
                        column.Item().PaddingBottom(6);
                        column.Item().Text($"Total Households (per city): {cityBreakdownLine}").FontSize(11);
                        column.Item().Height(6);
                    }
                    else
                    {
                        column.Item().Height(6);
                    }

                    column.Item().Text("Individuals Served:").FontSize(12).Bold();
                    column.Item().PaddingBottom(4);
                    column.Item().Table(t =>
                    {
                        t.ColumnsDefinition(cd =>
                        {
                            cd.ConstantColumn(120);
                            cd.RelativeColumn();
                            cd.RelativeColumn();
                            cd.RelativeColumn();
                        });
                        t.Header(h =>
                        {
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Text("Age Group").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Text("Duplicated (returning)").FontSize(11).Bold().AlignRight());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Text("Unduplicated (first visit this year)").FontSize(11).Bold().AlignRight());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Text("Total Individuals Served").FontSize(11).Bold().AlignRight());
                        });
                        RenderActivityReportRow(t, "Infant (0-2)", stats.InfantDuplicated, stats.InfantUnduplicated);
                        RenderActivityReportRow(t, "Child (2-18)", stats.ChildDuplicated, stats.ChildUnduplicated);
                        RenderActivityReportRow(t, "Adult (18-55)", stats.AdultDuplicated, stats.AdultUnduplicated);
                        RenderActivityReportRow(t, "Senior (55+)", stats.SeniorDuplicated, stats.SeniorUnduplicated);
                        t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(3).Text("Total").FontSize(11).Bold());
                        t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(3).Text(stats.IndividualsDuplicated.ToString("N0")).FontSize(11).AlignRight());
                        t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(3).Text(stats.IndividualsUnduplicated.ToString("N0")).FontSize(11).AlignRight());
                        t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(3).Text(stats.IndividualsTotal.ToString("N0")).FontSize(11).AlignRight());
                    });
                    column.Item().Height(6);
                    if (!string.IsNullOrEmpty(raceLine))
                    {
                        column.Item().Text($"Race Distribution: {raceLine}").FontSize(11);
                        column.Item().Height(6);
                    }
                    if (!string.IsNullOrEmpty(veteranLine))
                    {
                        column.Item().Text($"Veteran Status: {veteranLine}").FontSize(11);
                        column.Item().Height(6);
                    }
                    if (!string.IsNullOrEmpty(disabilityLine))
                    {
                        column.Item().Text($"Disability Status: {disabilityLine}").FontSize(11);
                        column.Item().Height(6);
                    }
                });

                page.Footer().Element(f =>
                {
                    f.AlignCenter().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}").FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });
        });

        document.GeneratePdf(filePath);
    }

    /// <summary>
    /// Generates a blank Registration & Shopper Designation form PDF (one page, portrait, Letter).
    /// No database connection required; form is for printing and hand-fill.
    /// </summary>
    public static void GenerateRegistrationFormPdf(string filePath)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(40);

                page.Content().Column(column =>
                {
                    column.Item().Text("Winlock-Vader Food Bank Registration & Shopper Designation Form")
                        .FontSize(16)
                        .Bold()
                        .AlignCenter();
                    column.Item().PaddingBottom(4);
                    column.Item().Text("Information below MUST MATCH your proof of residency information.")
                        .FontSize(12)
                        .Bold()
                        .AlignCenter();
                    column.Item().PaddingBottom(16);

                    // Contact Info
                    column.Item().Text("Contact Info:").FontSize(13).Bold();
                    column.Item().PaddingBottom(4);
                    // Line 1: Street Address
                    column.Item().Column(c =>
                    {
                        c.Item().Text("Street Address").FontSize(12);
                        c.Item().PaddingTop(2);
                        c.Item().LineHorizontal(1).LineColor(Colors.Black);
                    });
                    column.Item().PaddingBottom(20);
                    // Line 2: City, State, Zip
                    column.Item().Row(r =>
                    {
                        r.RelativeItem(2).Column(c =>
                        {
                            c.Item().Text("City").FontSize(12);
                            c.Item().PaddingTop(2);
                            c.Item().LineHorizontal(1).LineColor(Colors.Black);
                        });
                        r.ConstantItem(100).Column(c =>
                        {
                            c.Item().Text("State").FontSize(12);
                            c.Item().PaddingTop(2);
                            c.Item().LineHorizontal(1).LineColor(Colors.Black);
                        });
                        r.ConstantItem(100).Column(c =>
                        {
                            c.Item().Text("Zip").FontSize(12);
                            c.Item().PaddingTop(2);
                            c.Item().LineHorizontal(1).LineColor(Colors.Black);
                        });
                    });
                    column.Item().PaddingBottom(20);
                    // Line 3: Phone #, Email
                    column.Item().Row(r =>
                    {
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Phone #").FontSize(12);
                            c.Item().PaddingTop(2);
                            c.Item().LineHorizontal(1).LineColor(Colors.Black);
                        });
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Email").FontSize(12);
                            c.Item().PaddingTop(2);
                            c.Item().LineHorizontal(1).LineColor(Colors.Black);
                        });
                    });
                    column.Item().PaddingBottom(12);

                    // Household Members table
                    column.Item().Text("Household Members:").FontSize(13).Bold();
                    column.Item().PaddingBottom(4);
                    column.Item().Text("1 - First listed member should be the primary household member.").FontSize(11).Bold();
                    column.Item().PaddingBottom(10);
                    column.Item().Text("2 - Name and birthday are required for each household member.").FontSize(11).Bold();
                    column.Item().PaddingBottom(10);
                    column.Item().Text("3 - Race, veteran status, and disability status are optional, but providing them helps us receive more funding.").FontSize(11).Bold();
                    column.Item().PaddingBottom(10);
                    column.Item().Text("4 - For race, please choose the closest match from the following: White, Black, Hispanic, Native American.").FontSize(11).Bold();
                    column.Item().PaddingBottom(12);
                    column.Item().Table(t =>
                    {
                        t.ColumnsDefinition(cd =>
                        {
                            cd.RelativeColumn(2);
                            cd.RelativeColumn(1.5f);
                            cd.RelativeColumn(1.5f);
                            cd.ConstantColumn(50);
                            cd.ConstantColumn(50);
                        });
                        t.Header(h =>
                        {
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Height(30).AlignCenter().AlignMiddle().Text("Name (please print first and last)").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Height(30).AlignCenter().AlignMiddle().Text("Birthday (mm/dd/yyyy)").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Height(30).AlignCenter().AlignMiddle().Text("Race (see above)").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Height(30).AlignCenter().AlignMiddle().Text("Veteran").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(3).Height(30).AlignCenter().AlignMiddle().Text("Disabled").FontSize(11).Bold());
                        });
                        for (var i = 0; i < 10; i++)
                        {
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(2).Height(30));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(2).Height(30));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(2).Height(30));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(2).Height(30));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(2).Height(30));
                        }
                    });
                });
            });
        });

        document.GeneratePdf(filePath);
    }

    /// <summary>
    /// Generates a blank Deck Sign In form PDF (one page, landscape, Letter).
    /// Table has at least 21 blank rows for hand-fill. No database connection required.
    /// </summary>
    public static void GenerateDeckSignInFormPdf(string filePath)
    {
        const int rowCount = 20;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter.Landscape());
                page.Margin(30);

                page.Content().Column(column =>
                {
                    column.Item().Text("Winlock-Vader Food Bank Deck Sign In")
                        .FontSize(18)
                        .Bold()
                        .AlignCenter();
                    column.Item().PaddingBottom(8);

                    column.Item().Table(t =>
                    {
                        t.ColumnsDefinition(cd =>
                        {
                            cd.RelativeColumn(2);
                            cd.ConstantColumn(70);
                            cd.ConstantColumn(45);
                            cd.ConstantColumn(50);
                            cd.ConstantColumn(45);
                            cd.ConstantColumn(45);
                            cd.RelativeColumn();
                        });
                        t.Header(h =>
                        {
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(2).AlignCenter().AlignMiddle().Text("Name (please print first and last)").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(2).AlignCenter().AlignMiddle().Text("Household Size").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(2).AlignCenter().AlignMiddle().Text("Infants (0-2)").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(2).AlignCenter().AlignMiddle().Text("Children (2-18)").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(2).AlignCenter().AlignMiddle().Text("Adults (18-55)").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(2).AlignCenter().AlignMiddle().Text("Seniors (55+)").FontSize(11).Bold());
                            h.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Lighten3).Padding(2).AlignCenter().AlignMiddle().Text("Comment").FontSize(11).Bold());
                        });
                        for (var i = 0; i < rowCount; i++)
                        {
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(1).Height(22));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(1).Height(22));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(1).Height(22));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(1).Height(22));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(1).Height(22));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(1).Height(22));
                            t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(1).Height(22));
                        }
                    });
                });
            });
        });

        document.GeneratePdf(filePath);
    }

    private static void RenderActivityReportRow(TableDescriptor t, string label, int dup, int undup)
    {
        t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(3).Text(label).FontSize(11));
        t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(3).Text(dup.ToString("N0")).FontSize(11).AlignRight());
        t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(3).Text(undup.ToString("N0")).FontSize(11).AlignRight());
        t.Cell().Element(c => c.Border(1).BorderColor(Colors.Black).Padding(3).Text((dup + undup).ToString("N0")).FontSize(11).AlignRight());
    }

    private static string GetDateRangeLabel(DateTime startDate, DateTime endDate)
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

    /// <summary>
    /// Renders totals section for Monthly Summary (no cover page styling).
    /// </summary>
    private static void RenderTotalsSection(IContainer container, MonthlyStatistics stats)
    {
        container.Column(column =>
        {
            column.Item().Text("TOTALS").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);
            column.Item().Text($"Unique Households Served: {stats.UniqueHouseholdsServed:N0}");
            column.Item().Text($"Total People: {stats.TotalPeople:N0}");
            column.Item().Text($"Completed Services: {stats.CompletedServices:N0}");
            column.Item().Text($"Deck Total: {stats.DeckTotal:N0}");
            column.Item().PaddingBottom(10);
        });
    }

    /// <summary>
    /// Renders cover page with food bank title, year, and totals for Statistics/Yearly Report.
    /// </summary>
    private static void RenderTotalsSection(IContainer container, MonthlyStatistics stats, int year)
    {
        container.Column(column =>
        {
            column.Item().Text("WINLOCK-VADER FOOD BANK YEARLY REPORT").FontSize(56).Bold();
            column.Item().PaddingBottom(100);
            column.Item().Text(year.ToString()).FontSize(56).Bold();
            column.Item().PaddingBottom(100);
            column.Item().Text("TOTALS").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);
            column.Item().Text($"Unique Households Served: {stats.UniqueHouseholdsServed:N0}");
            column.Item().Text($"Total People: {stats.TotalPeople:N0}");
            column.Item().Text($"Completed Services: {stats.CompletedServices:N0}");
            column.Item().Text($"Deck Total: {stats.DeckTotal:N0}");
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

    /// <summary>
    /// Renders composition section for Monthly Summary (Age only, no demographics charts).
    /// </summary>
    private static void RenderCompositionSection(IContainer container, (int Infant, int Child, int Adult, int Senior) composition)
    {
        var total = composition.Infant + composition.Child + composition.Adult + composition.Senior;
        container.Column(column =>
        {
            column.Item().Text("HOUSEHOLD COMPOSITION SERVED").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);
            column.Item().Text($"Infant (0-2): {composition.Infant:N0}");
            column.Item().Text($"Child (2-18): {composition.Child:N0}");
            column.Item().Text($"Adult (18-55): {composition.Adult:N0}");
            column.Item().Text($"Senior (55+): {composition.Senior:N0}");
            column.Item().Text($"Total: {total:N0}").Bold();
            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderAgeDistributionSection(IContainer container, (int Infant, int Child, int Adult, int Senior) composition, Dictionary<string, string>? chartImages = null)
    {
        var total = composition.Infant + composition.Child + composition.Adult + composition.Senior;
        container.Column(column =>
        {
            column.Item().Text("AGE DISTRIBUTION").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            if (chartImages != null && chartImages.TryGetValue("AgeGroupDistribution", out var chartPath) && File.Exists(chartPath))
            {
                column.Item().Image(chartPath).FitWidth();
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

    private static void RenderRaceDistributionSection(IContainer container, List<DemographicsBreakdown> raceBreakdown, Dictionary<string, string>? chartImages = null)
    {
        container.Column(column =>
        {
            column.Item().Text("RACE DISTRIBUTION").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            if (chartImages != null && chartImages.TryGetValue("RaceDistribution", out var chartPath) && File.Exists(chartPath))
            {
                column.Item().Image(chartPath).FitWidth();
                column.Item().PaddingBottom(10);
            }
            RenderDemographicsBreakdownDetail(column, raceBreakdown);
            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderVeteranStatusSection(IContainer container, List<DemographicsBreakdown> veteranBreakdown, Dictionary<string, string>? chartImages = null)
    {
        container.Column(column =>
        {
            column.Item().Text("VETERAN STATUS").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            if (chartImages != null && chartImages.TryGetValue("VeteranDistribution", out var chartPath) && File.Exists(chartPath))
            {
                column.Item().Image(chartPath).FitWidth();
                column.Item().PaddingBottom(10);
            }
            RenderDemographicsBreakdownDetail(column, veteranBreakdown);
            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderDisabilityStatusSection(IContainer container, List<DemographicsBreakdown> disabilityBreakdown, Dictionary<string, string>? chartImages = null)
    {
        container.Column(column =>
        {
            column.Item().Text("DISABILITY STATUS").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            if (chartImages != null && chartImages.TryGetValue("DisabilityDistribution", out var chartPath) && File.Exists(chartPath))
            {
                column.Item().Image(chartPath).FitWidth();
                column.Item().PaddingBottom(10);
            }
            RenderDemographicsBreakdownDetail(column, disabilityBreakdown);
            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderDemographicsBreakdownDetail(ColumnDescriptor column, List<DemographicsBreakdown> breakdown)
    {
        if (breakdown.Count == 0)
        {
            column.Item().Text("No data for selected range.").FontColor(Colors.Grey.Medium);
            return;
        }
        var total = breakdown.Sum(b => b.Count);
        foreach (var item in breakdown)
        {
            var pct = total > 0 ? (100.0 * item.Count / total) : 0;
            column.Item().Text($"{item.Label}: {item.Count:N0} ({pct:0}%)");
        }
    }

    private static void RenderVisitTypeSection(IContainer container, List<DemographicsBreakdown> visitTypeBreakdown, Dictionary<string, string>? chartImages = null)
    {
        container.Column(column =>
        {
            column.Item().Text("VISIT TYPE BREAKDOWN").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            if (chartImages != null && chartImages.TryGetValue("VisitType", out var chartPath) && File.Exists(chartPath))
            {
                column.Item().Image(chartPath).FitWidth();
                column.Item().PaddingBottom(10);
            }

            if (visitTypeBreakdown.Count > 0)
            {
                var total = visitTypeBreakdown.Sum(b => b.Count);
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
                        header.Cell().Element(c => c.Background(Colors.Grey.Lighten3).Padding(5).Text("Visit Type").Bold());
                        header.Cell().Element(c => c.Background(Colors.Grey.Lighten3).Padding(5).Text("Count").Bold());
                        header.Cell().Element(c => c.Background(Colors.Grey.Lighten3).Padding(5).Text("%").Bold());
                    });
                    foreach (var item in visitTypeBreakdown)
                    {
                        var pct = total > 0 ? (100.0 * item.Count / total) : 0;
                        table.Cell().Element(c => c.Padding(5).Text(item.Label));
                        table.Cell().Element(c => c.Padding(5).Text(item.Count.ToString("N0")).AlignRight());
                        table.Cell().Element(c => c.Padding(5).Text($"{pct:0}%").AlignRight());
                    }
                });
            }
            else
            {
                column.Item().Text("No data for selected range.").FontColor(Colors.Grey.Medium);
            }

            column.Item().PaddingBottom(10);
        });
    }

    private static void RenderEventTypeSection(IContainer container, MonthlyStatistics stats, Dictionary<string, string>? chartImages = null)
    {
        var pantryCount = stats.PantryDayCompletions;
        var appointmentCount = stats.AppointmentCompletions;
        var total = pantryCount + appointmentCount;

        container.Column(column =>
        {
            column.Item().Text("EVENT TYPE BREAKDOWN").FontSize(14).Bold();
            column.Item().PaddingBottom(5);
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().PaddingTop(5);

            if (chartImages != null && chartImages.TryGetValue("EventType", out var chartPath) && File.Exists(chartPath))
            {
                column.Item().Image(chartPath).FitWidth();
                column.Item().PaddingBottom(10);
            }

            if (total > 0)
            {
                var pantryPct = 100.0 * pantryCount / total;
                var appointmentPct = 100.0 * appointmentCount / total;
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
                        header.Cell().Element(c => c.Background(Colors.Grey.Lighten3).Padding(5).Text("Event Type").Bold());
                        header.Cell().Element(c => c.Background(Colors.Grey.Lighten3).Padding(5).Text("Count").Bold());
                        header.Cell().Element(c => c.Background(Colors.Grey.Lighten3).Padding(5).Text("%").Bold());
                    });
                    table.Cell().Element(c => c.Padding(5).Text("Pantry Day"));
                    table.Cell().Element(c => c.Padding(5).Text(pantryCount.ToString("N0")).AlignRight());
                    table.Cell().Element(c => c.Padding(5).Text($"{pantryPct:0}%").AlignRight());
                    table.Cell().Element(c => c.Padding(5).Text("Appointment"));
                    table.Cell().Element(c => c.Padding(5).Text(appointmentCount.ToString("N0")).AlignRight());
                    table.Cell().Element(c => c.Padding(5).Text($"{appointmentPct:0}%").AlignRight());
                });
            }
            else
            {
                column.Item().Text("No data for selected range.").FontColor(Colors.Grey.Medium);
            }

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
