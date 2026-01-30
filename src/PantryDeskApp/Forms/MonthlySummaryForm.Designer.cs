namespace PantryDeskApp.Forms;

partial class MonthlySummaryForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        grpMonthPicker = new GroupBox();
        lblMonth = new Label();
        dtpMonth = new DateTimePicker();
        grpPreview = new GroupBox();
        txtPreview = new TextBox();
        btnExportPdf = new Button();
        btnPrint = new Button();
        btnClose = new Button();
        grpMonthPicker.SuspendLayout();
        grpPreview.SuspendLayout();
        SuspendLayout();
        // 
        // grpMonthPicker
        // 
        grpMonthPicker.Controls.Add(lblMonth);
        grpMonthPicker.Controls.Add(dtpMonth);
        grpMonthPicker.Location = new Point(12, 12);
        grpMonthPicker.Name = "grpMonthPicker";
        grpMonthPicker.Size = new Size(776, 60);
        grpMonthPicker.TabIndex = 0;
        grpMonthPicker.TabStop = false;
        grpMonthPicker.Text = "Select Month";
        grpMonthPicker.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // lblMonth
        // 
        lblMonth.AutoSize = true;
        lblMonth.Location = new Point(12, 25);
        lblMonth.Name = "lblMonth";
        lblMonth.Size = new Size(46, 15);
        lblMonth.TabIndex = 0;
        lblMonth.Text = "Month:";
        // 
        // dtpMonth
        // 
        dtpMonth.Format = DateTimePickerFormat.Custom;
        dtpMonth.CustomFormat = "MMMM yyyy";
        dtpMonth.ShowUpDown = true;
        dtpMonth.Location = new Point(64, 23);
        dtpMonth.Name = "dtpMonth";
        dtpMonth.Size = new Size(200, 23);
        dtpMonth.TabIndex = 1;
        dtpMonth.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        dtpMonth.ValueChanged += DtpMonth_ValueChanged;
        // 
        // grpPreview
        // 
        grpPreview.Controls.Add(txtPreview);
        grpPreview.Location = new Point(12, 78);
        grpPreview.Name = "grpPreview";
        grpPreview.Size = new Size(776, 350);
        grpPreview.TabIndex = 1;
        grpPreview.TabStop = false;
        grpPreview.Text = "Preview";
        grpPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // txtPreview
        // 
        txtPreview.Dock = DockStyle.Fill;
        txtPreview.Font = new Font("Courier New", 9F);
        txtPreview.Location = new Point(3, 19);
        txtPreview.Multiline = true;
        txtPreview.Name = "txtPreview";
        txtPreview.ReadOnly = true;
        txtPreview.ScrollBars = ScrollBars.Both;
        txtPreview.Size = new Size(770, 328);
        txtPreview.TabIndex = 0;
        txtPreview.WordWrap = false;
        // 
        // btnExportPdf
        // 
        btnExportPdf.Location = new Point(12, 434);
        btnExportPdf.Name = "btnExportPdf";
        btnExportPdf.Size = new Size(150, 35);
        btnExportPdf.TabIndex = 2;
        btnExportPdf.Text = "Export PDF";
        btnExportPdf.UseVisualStyleBackColor = true;
        btnExportPdf.Click += BtnExportPdf_Click;
        btnExportPdf.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        // 
        // btnPrint
        // 
        btnPrint.Location = new Point(168, 434);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(150, 35);
        btnPrint.TabIndex = 3;
        btnPrint.Text = "Print";
        btnPrint.UseVisualStyleBackColor = true;
        btnPrint.Click += BtnPrint_Click;
        btnPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        // 
        // btnClose
        // 
        btnClose.Location = new Point(638, 434);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(150, 35);
        btnClose.TabIndex = 4;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += BtnClose_Click;
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        // 
        // MonthlySummaryForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 481);
        MinimumSize = new Size(600, 400);
        Controls.Add(btnClose);
        Controls.Add(btnPrint);
        Controls.Add(btnExportPdf);
        Controls.Add(grpPreview);
        Controls.Add(grpMonthPicker);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MonthlySummaryForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Monthly Summary Report";
        Load += MonthlySummaryForm_Load;
        grpMonthPicker.ResumeLayout(false);
        grpMonthPicker.PerformLayout();
        grpPreview.ResumeLayout(false);
        grpPreview.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpMonthPicker;
    private Label lblMonth;
    private DateTimePicker dtpMonth;
    private GroupBox grpPreview;
    private TextBox txtPreview;
    private Button btnExportPdf;
    private Button btnPrint;
    private Button btnClose;
}
