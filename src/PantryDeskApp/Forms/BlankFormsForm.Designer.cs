namespace PantryDeskApp.Forms;

partial class BlankFormsForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lblFormType = new Label();
        btnExportPdf = new Button();
        btnPrint = new Button();
        btnClose = new Button();
        SuspendLayout();
        //
        // lblFormType
        //
        lblFormType.AutoSize = true;
        lblFormType.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblFormType.Location = new Point(20, 20);
        lblFormType.Name = "lblFormType";
        lblFormType.Size = new Size(120, 21);
        lblFormType.TabIndex = 0;
        lblFormType.Text = "Form: Registration";
        //
        // btnExportPdf
        //
        btnExportPdf.Location = new Point(20, 55);
        btnExportPdf.Name = "btnExportPdf";
        btnExportPdf.Size = new Size(100, 30);
        btnExportPdf.TabIndex = 1;
        btnExportPdf.Text = "Export PDF";
        btnExportPdf.UseVisualStyleBackColor = true;
        btnExportPdf.Click += BtnExportPdf_Click;
        //
        // btnPrint
        //
        btnPrint.Location = new Point(130, 55);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(100, 30);
        btnPrint.TabIndex = 2;
        btnPrint.Text = "Print";
        btnPrint.UseVisualStyleBackColor = true;
        btnPrint.Click += BtnPrint_Click;
        //
        // btnClose
        //
        btnClose.Location = new Point(240, 55);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 30);
        btnClose.TabIndex = 3;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += BtnClose_Click;
        //
        // BlankFormsForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnClose;
        ClientSize = new Size(334, 100);
        Controls.Add(btnClose);
        Controls.Add(btnPrint);
        Controls.Add(btnExportPdf);
        Controls.Add(lblFormType);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "BlankFormsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Forms";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblFormType;
    private Button btnExportPdf;
    private Button btnPrint;
    private Button btnClose;
}
