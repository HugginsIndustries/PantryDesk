using PantryDeskCore.Services;

namespace PantryDeskApp.Forms;

/// <summary>
/// Shared dialog for printing/exporting blank forms (Registration, Deck Sign In).
/// </summary>
public enum BlankFormKind
{
    Registration,
    DeckSignIn
}

/// <summary>
/// Form to export or print a blank Registration or Deck Sign In PDF.
/// </summary>
public partial class BlankFormsForm : Form
{
    private readonly BlankFormKind _formKind;

    public BlankFormsForm(BlankFormKind formKind)
    {
        _formKind = formKind;
        InitializeComponent();
        lblFormType.Text = _formKind == BlankFormKind.Registration ? "Form: Registration" : "Form: Deck Sign In";

        var iconPath = Path.Combine(AppContext.BaseDirectory, "icon.ico");
        if (File.Exists(iconPath))
        {
            Icon = new Icon(iconPath);
        }
    }

    private void BtnExportPdf_Click(object? sender, EventArgs e)
    {
        var defaultFileName = _formKind == BlankFormKind.Registration ? "RegistrationForm.pdf" : "DeckSignInForm.pdf";
        using var saveDialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            FileName = defaultFileName,
            DefaultExt = "pdf"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                if (_formKind == BlankFormKind.Registration)
                {
                    ReportService.GenerateRegistrationFormPdf(saveDialog.FileName);
                }
                else
                {
                    ReportService.GenerateDeckSignInFormPdf(saveDialog.FileName);
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
        try
        {
            var tempBaseName = _formKind == BlankFormKind.Registration ? "RegistrationForm" : "DeckSignInForm";
            var tempPdfPath = Path.Combine(Path.GetTempPath(), $"{tempBaseName}_{Guid.NewGuid()}.pdf");
            if (_formKind == BlankFormKind.Registration)
            {
                ReportService.GenerateRegistrationFormPdf(tempPdfPath);
            }
            else
            {
                ReportService.GenerateDeckSignInFormPdf(tempPdfPath);
            }

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
                // Print verb failed, fall through to open normally
            }

            if (!printSuccess)
            {
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

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
