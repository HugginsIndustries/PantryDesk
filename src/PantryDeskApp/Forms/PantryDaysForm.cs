using PantryDeskCore.Data;
using PantryDeskCore.Models;
using PantryDeskCore.Security;

namespace PantryDeskApp.Forms;

/// <summary>
/// Form for managing pantry day calendar: generating pantry days for a year and editing existing ones.
/// Admin-only access.
/// </summary>
public partial class PantryDaysForm : Form
{
    private PantryDay? _selectedPantryDay;

    public PantryDaysForm()
    {
        InitializeComponent();
        PermissionChecker.RequireAdmin();
        SetupDataGridView();
        
        // Set form icon if available
        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            this.Icon = new Icon(iconPath);
        }
    }

    private void PantryDaysForm_Load(object? sender, EventArgs e)
    {
        numYear.Value = DateTime.Now.Year;
        LoadPantryDays();
    }

    private void SetupDataGridView()
    {
        dgvPantryDays.Columns.Clear();
        dgvPantryDays.Columns.Add("Date", "Date");
        dgvPantryDays.Columns.Add("Active", "Active");
        dgvPantryDays.Columns.Add("Notes", "Notes");
        dgvPantryDays.Columns.Add("Id", "Id");

        var idColumn = dgvPantryDays.Columns["Id"];
        if (idColumn != null)
        {
            idColumn.Visible = false;
        }

        var dateColumn = dgvPantryDays.Columns["Date"];
        var activeColumn = dgvPantryDays.Columns["Active"];
        var notesColumn = dgvPantryDays.Columns["Notes"];

        if (dateColumn != null) dateColumn.Width = 120;
        if (activeColumn != null) activeColumn.Width = 80;
        if (notesColumn != null) notesColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    private void LoadPantryDays()
    {
        dgvPantryDays.Rows.Clear();
        _selectedPantryDay = null;
        grpEdit.Enabled = false;

        try
        {
            using var connection = DatabaseManager.GetConnection();
            var pantryDays = PantryDayRepository.GetAll(connection);

            foreach (var pantryDay in pantryDays)
            {
                var notesText = string.IsNullOrWhiteSpace(pantryDay.Notes) ? "" : pantryDay.Notes;
                if (notesText.Length > 50)
                {
                    notesText = notesText.Substring(0, 47) + "...";
                }

                dgvPantryDays.Rows.Add(
                    pantryDay.PantryDate.ToString("yyyy-MM-dd"),
                    pantryDay.IsActive ? "Yes" : "No",
                    notesText,
                    pantryDay.Id
                );
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading pantry days: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvPantryDays_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvPantryDays.SelectedRows.Count == 0)
        {
            _selectedPantryDay = null;
            grpEdit.Enabled = false;
            return;
        }

        var selectedRow = dgvPantryDays.SelectedRows[0];
        var idValue = selectedRow.Cells["Id"].Value;
        if (idValue == null)
        {
            return;
        }

        var id = (int)idValue;

        try
        {
            using var connection = DatabaseManager.GetConnection();
            _selectedPantryDay = PantryDayRepository.GetById(connection, id);

            if (_selectedPantryDay != null)
            {
                dtpEditDate.Value = _selectedPantryDay.PantryDate;
                chkEditActive.Checked = _selectedPantryDay.IsActive;
                txtEditNotes.Text = _selectedPantryDay.Notes ?? "";
                grpEdit.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading pantry day: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnGenerate_Click(object? sender, EventArgs e)
    {
        var year = (int)numYear.Value;

        try
        {
            using var connection = DatabaseManager.GetConnection();
            var count = GeneratePantryDaysForYear(connection, year);

            MessageBox.Show($"Generated {count} pantry days for {year}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadPantryDays();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating pantry days: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private int GeneratePantryDaysForYear(Microsoft.Data.Sqlite.SqliteConnection connection, int year)
    {
        var createdCount = 0;

        // Jan-Oct: 2nd, 3rd, 4th Wednesday
        for (int month = 1; month <= 10; month++)
        {
            var dates = new[]
            {
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 2),
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 3),
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 4)
            };

            foreach (var date in dates)
            {
                // Check if pantry day already exists
                var existing = PantryDayRepository.GetByDate(connection, date);
                if (existing != null)
                {
                    continue; // Skip if already exists
                }

                var pantryDay = new PantryDay
                {
                    PantryDate = date,
                    IsActive = true,
                    Notes = null
                };

                PantryDayRepository.Create(connection, pantryDay);
                createdCount++;
            }
        }

        // Nov-Dec: 1st, 2nd, 3rd Wednesday
        for (int month = 11; month <= 12; month++)
        {
            var dates = new[]
            {
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 1),
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 2),
                GetNthWeekdayOfMonth(year, month, DayOfWeek.Wednesday, 3)
            };

            foreach (var date in dates)
            {
                // Check if pantry day already exists
                var existing = PantryDayRepository.GetByDate(connection, date);
                if (existing != null)
                {
                    continue; // Skip if already exists
                }

                var pantryDay = new PantryDay
                {
                    PantryDate = date,
                    IsActive = true,
                    Notes = null
                };

                PantryDayRepository.Create(connection, pantryDay);
                createdCount++;
            }
        }

        return createdCount;
    }

    private static DateTime GetNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int occurrence)
    {
        // Find the first occurrence of the weekday in the month
        var firstDay = new DateTime(year, month, 1);
        var firstDayOfWeek = (int)firstDay.DayOfWeek;
        var targetDayOfWeek = (int)dayOfWeek;

        // Calculate days to add to get to the first occurrence
        // If first day is the target day, daysToAdd = 0
        // Otherwise, calculate how many days forward to the first occurrence
        var daysToAdd = (targetDayOfWeek - firstDayOfWeek + 7) % 7;
        if (daysToAdd == 0 && firstDay.DayOfWeek != dayOfWeek)
        {
            // This shouldn't happen with the modulo, but handle edge case
            daysToAdd = 7;
        }

        var firstOccurrence = firstDay.AddDays(daysToAdd);

        // Add weeks for the nth occurrence (occurrence is 1-based)
        var targetDate = firstOccurrence.AddDays((occurrence - 1) * 7);

        // Verify we're still in the same month
        if (targetDate.Month != month)
        {
            throw new ArgumentException($"Month {month} does not have {occurrence} occurrences of {dayOfWeek}");
        }

        return targetDate;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_selectedPantryDay == null)
        {
            return;
        }

        var newDate = dtpEditDate.Value.Date;

        // Check for duplicate date (excluding current pantry day)
        try
        {
            using var connection = DatabaseManager.GetConnection();
            var existing = PantryDayRepository.GetByDate(connection, newDate);
            if (existing != null && existing.Id != _selectedPantryDay.Id)
            {
                MessageBox.Show("A pantry day already exists for this date. Please choose a different date.", "Duplicate Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update the pantry day
            _selectedPantryDay.PantryDate = newDate;
            _selectedPantryDay.IsActive = chkEditActive.Checked;
            _selectedPantryDay.Notes = string.IsNullOrWhiteSpace(txtEditNotes.Text) ? null : txtEditNotes.Text.Trim();

            PantryDayRepository.Update(connection, _selectedPantryDay);

            MessageBox.Show("Pantry day updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadPantryDays();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating pantry day: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        // Clear selection and reset edit panel
        dgvPantryDays.ClearSelection();
        _selectedPantryDay = null;
        grpEdit.Enabled = false;
    }
}
