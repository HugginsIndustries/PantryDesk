using Microsoft.Data.Sqlite;
using PantryDeskCore.Data;
using PantryDeskCore.Models;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form to enter or edit deck-only monthly bulk stats (totals and page count; averages are stored).
/// One record per (year, month). Default month is last month.
/// </summary>
public partial class DeckStatsEntryForm : Form
{
    private static readonly string[] MonthNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    private int _lastSelectedYear;
    private int _lastSelectedMonth;
    private bool _suppressMonthYearCheck;

    public DeckStatsEntryForm()
    {
        InitializeComponent();
        cmbYear.SelectedIndexChanged += OnMonthYearChanged;
        cmbMonth.SelectedIndexChanged += OnMonthYearChanged;
        LoadDefaultsAndCheckExisting();

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void LoadDefaultsAndCheckExisting()
    {
        _suppressMonthYearCheck = true;
        try
        {
            var now = DateTime.Now;
            var lastMonth = now.AddMonths(-1);

            // Year dropdown: current year and several past
            cmbYear.Items.Clear();
            for (var y = now.Year; y >= now.Year - 5; y--)
            {
                cmbYear.Items.Add(y);
            }
            cmbYear.SelectedItem = lastMonth.Year;

            cmbMonth.SelectedIndex = lastMonth.Month - 1;

            using var connection = DatabaseManager.GetConnection();
            if (DeckStatsRepository.Exists(connection, lastMonth.Year, lastMonth.Month))
            {
                var existing = DeckStatsRepository.Get(connection, lastMonth.Year, lastMonth.Month);
                if (existing != null)
                    LoadExistingForEdit(existing);
            }

            _lastSelectedYear = lastMonth.Year;
            _lastSelectedMonth = lastMonth.Month;
        }
        finally
        {
            _suppressMonthYearCheck = false;
        }
    }

    private void OnMonthYearChanged(object? sender, EventArgs e)
    {
        if (_suppressMonthYearCheck) return;

        var (year, month) = GetSelectedYearMonth();
        if (year == _lastSelectedYear && month == _lastSelectedMonth) return;

        using var connection = DatabaseManager.GetConnection();
        if (!DeckStatsRepository.Exists(connection, year, month))
        {
            _lastSelectedYear = year;
            _lastSelectedMonth = month;
            numHouseholdTotal.Value = 0;
            numInfant.Value = 0;
            numChild.Value = 0;
            numAdult.Value = 0;
            numSenior.Value = 0;
            numPages.Value = 1;
            return;
        }

        var existing = DeckStatsRepository.Get(connection, year, month);
        if (existing != null)
            LoadExistingForEdit(existing);
        _lastSelectedYear = year;
        _lastSelectedMonth = month;
    }

    private void LoadExistingForEdit(DeckStatsMonthly existing)
    {
        var pages = existing.PageCount ?? 1;
        if (pages < 1) pages = 1;

        numHouseholdTotal.Value = Math.Max(0, (decimal)Math.Round(existing.HouseholdTotalAvg * pages));
        numInfant.Value = Math.Max(0, (decimal)Math.Round(existing.InfantAvg * pages));
        numChild.Value = Math.Max(0, (decimal)Math.Round(existing.ChildAvg * pages));
        numAdult.Value = Math.Max(0, (decimal)Math.Round(existing.AdultAvg * pages));
        numSenior.Value = Math.Max(0, (decimal)Math.Round(existing.SeniorAvg * pages));
        numPages.Value = pages;
    }

    private (int year, int month) GetSelectedYearMonth()
    {
        var year = (int)(cmbYear.SelectedItem ?? DateTime.Now.Year);
        var month = cmbMonth.SelectedIndex + 1;
        return (year, month);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        lblError.Visible = false;

        var pages = (int)numPages.Value;
        if (pages < 1)
        {
            ShowError("Number of pages must be at least 1.");
            return;
        }

        var householdTotal = (double)numHouseholdTotal.Value;
        var infant = (double)numInfant.Value;
        var child = (double)numChild.Value;
        var adult = (double)numAdult.Value;
        var senior = (double)numSenior.Value;

        if (householdTotal < 0 || infant < 0 || child < 0 || adult < 0 || senior < 0)
        {
            ShowError("Totals cannot be negative.");
            return;
        }

        var (year, month) = GetSelectedYearMonth();

        using (var conn = DatabaseManager.GetConnection())
        {
            if (DeckStatsRepository.Exists(conn, year, month))
            {
                var monthName = MonthNames[month - 1];
                var overwrite = MessageBox.Show(
                    $"Deck stats for {monthName} {year} already exist. Overwrite?",
                    "Deck Stats",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (overwrite != DialogResult.Yes)
                    return;
            }
        }

        var entity = new DeckStatsMonthly
        {
            Year = year,
            Month = month,
            HouseholdTotalAvg = householdTotal / pages,
            InfantAvg = infant / pages,
            ChildAvg = child / pages,
            AdultAvg = adult / pages,
            SeniorAvg = senior / pages,
            PageCount = pages,
            UpdatedAt = string.Empty
        };

        try
        {
            using var connection = DatabaseManager.GetConnection();
            DeckStatsRepository.Upsert(connection, entity);
        }
        catch (Exception ex)
        {
            ShowError($"Error saving: {ex.Message}");
            return;
        }

        MessageBox.Show("Deck stats saved successfully.", "Deck Stats", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
    }
}
