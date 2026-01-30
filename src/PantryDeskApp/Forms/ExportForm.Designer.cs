namespace PantryDeskApp.Forms;

partial class ExportForm
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
        lblTitle = new Label();
        radioCsv = new RadioButton();
        radioJson = new RadioButton();
        lblOutputPath = new Label();
        txtOutputPath = new TextBox();
        btnBrowse = new Button();
        btnExport = new Button();
        btnCancel = new Button();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(12, 9);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(100, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Export Data";
        // 
        // radioCsv
        // 
        radioCsv.AutoSize = true;
        radioCsv.Checked = true;
        radioCsv.Location = new Point(12, 45);
        radioCsv.Name = "radioCsv";
        radioCsv.Size = new Size(165, 19);
        radioCsv.TabIndex = 1;
        radioCsv.TabStop = true;
        radioCsv.Text = "CSV (Excel-compatible)";
        radioCsv.UseVisualStyleBackColor = true;
        radioCsv.CheckedChanged += RadioCsv_CheckedChanged;
        // 
        // radioJson
        // 
        radioJson.AutoSize = true;
        radioJson.Location = new Point(12, 70);
        radioJson.Name = "radioJson";
        radioJson.Size = new Size(120, 19);
        radioJson.TabIndex = 2;
        radioJson.Text = "JSON (Structured)";
        radioJson.UseVisualStyleBackColor = true;
        radioJson.CheckedChanged += RadioJson_CheckedChanged;
        // 
        // lblOutputPath
        // 
        lblOutputPath.AutoSize = true;
        lblOutputPath.Location = new Point(12, 105);
        lblOutputPath.Name = "lblOutputPath";
        lblOutputPath.Size = new Size(85, 15);
        lblOutputPath.TabIndex = 3;
        lblOutputPath.Text = "Output Folder:";
        // 
        // txtOutputPath
        // 
        txtOutputPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtOutputPath.Location = new Point(12, 123);
        txtOutputPath.Name = "txtOutputPath";
        txtOutputPath.ReadOnly = true;
        txtOutputPath.Size = new Size(460, 23);
        txtOutputPath.TabIndex = 4;
        // 
        // btnBrowse
        // 
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.Location = new Point(478, 122);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(75, 25);
        btnBrowse.TabIndex = 5;
        btnBrowse.Text = "Browse...";
        btnBrowse.UseVisualStyleBackColor = true;
        btnBrowse.Click += BtnBrowse_Click;
        // 
        // btnExport
        // 
        btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnExport.Location = new Point(397, 170);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(75, 30);
        btnExport.TabIndex = 6;
        btnExport.Text = "Export";
        btnExport.UseVisualStyleBackColor = true;
        btnExport.Click += BtnExport_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.Location = new Point(478, 170);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 7;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // ExportForm
        // 
        AcceptButton = btnExport;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(565, 212);
        Controls.Add(btnCancel);
        Controls.Add(btnExport);
        Controls.Add(btnBrowse);
        Controls.Add(txtOutputPath);
        Controls.Add(lblOutputPath);
        Controls.Add(radioJson);
        Controls.Add(radioCsv);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ExportForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Export Data";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitle;
    private RadioButton radioCsv;
    private RadioButton radioJson;
    private Label lblOutputPath;
    private TextBox txtOutputPath;
    private Button btnBrowse;
    private Button btnExport;
    private Button btnCancel;

    #endregion
}
