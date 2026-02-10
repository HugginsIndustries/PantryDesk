namespace PantryDeskApp.Forms;

partial class MonthlyActivityReportForm
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
        lblTitle = new Label();
        lblMonth = new Label();
        cmbMonth = new ComboBox();
        lblYear = new Label();
        cmbYear = new ComboBox();
        lblFoodBankName = new Label();
        txtFoodBankName = new TextBox();
        lblCounty = new Label();
        txtCounty = new TextBox();
        lblPreparedBy = new Label();
        txtPreparedBy = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        btnExportPdf = new Button();
        btnPrint = new Button();
        btnClose = new Button();
        SuspendLayout();
        //
        // lblTitle
        //
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(170, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Monthly Activity Report";
        //
        // lblMonth
        //
        lblMonth.AutoSize = true;
        lblMonth.Location = new Point(20, 55);
        lblMonth.Name = "lblMonth";
        lblMonth.Size = new Size(45, 15);
        lblMonth.TabIndex = 1;
        lblMonth.Text = "Month:";
        //
        // cmbMonth
        //
        cmbMonth.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMonth.FormattingEnabled = true;
        cmbMonth.Items.AddRange(new object[]
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        });
        cmbMonth.Location = new Point(20, 73);
        cmbMonth.Name = "cmbMonth";
        cmbMonth.Size = new Size(140, 23);
        cmbMonth.TabIndex = 2;
        //
        // lblYear
        //
        lblYear.AutoSize = true;
        lblYear.Location = new Point(180, 55);
        lblYear.Name = "lblYear";
        lblYear.Size = new Size(32, 15);
        lblYear.TabIndex = 3;
        lblYear.Text = "Year:";
        //
        // cmbYear
        //
        cmbYear.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbYear.FormattingEnabled = true;
        cmbYear.Location = new Point(180, 73);
        cmbYear.Name = "cmbYear";
        cmbYear.Size = new Size(90, 23);
        cmbYear.TabIndex = 4;
        //
        // lblFoodBankName
        //
        lblFoodBankName.AutoSize = true;
        lblFoodBankName.Location = new Point(20, 108);
        lblFoodBankName.Name = "lblFoodBankName";
        lblFoodBankName.Size = new Size(85, 15);
        lblFoodBankName.TabIndex = 5;
        lblFoodBankName.Text = "Food bank name:";
        //
        // txtFoodBankName
        //
        txtFoodBankName.Location = new Point(20, 126);
        txtFoodBankName.Name = "txtFoodBankName";
        txtFoodBankName.Size = new Size(350, 23);
        txtFoodBankName.TabIndex = 6;
        //
        // lblCounty
        //
        lblCounty.AutoSize = true;
        lblCounty.Location = new Point(20, 158);
        lblCounty.Name = "lblCounty";
        lblCounty.Size = new Size(44, 15);
        lblCounty.TabIndex = 7;
        lblCounty.Text = "County:";
        //
        // txtCounty
        //
        txtCounty.Location = new Point(20, 176);
        txtCounty.Name = "txtCounty";
        txtCounty.Size = new Size(200, 23);
        txtCounty.TabIndex = 8;
        //
        // lblPreparedBy
        //
        lblPreparedBy.AutoSize = true;
        lblPreparedBy.Location = new Point(20, 208);
        lblPreparedBy.Name = "lblPreparedBy";
        lblPreparedBy.Size = new Size(68, 15);
        lblPreparedBy.TabIndex = 9;
        lblPreparedBy.Text = "Prepared by:";
        //
        // txtPreparedBy
        //
        txtPreparedBy.Location = new Point(20, 226);
        txtPreparedBy.Name = "txtPreparedBy";
        txtPreparedBy.Size = new Size(250, 23);
        txtPreparedBy.TabIndex = 10;
        //
        // lblPhone
        //
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(20, 258);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(84, 15);
        lblPhone.TabIndex = 11;
        lblPhone.Text = "Phone number:";
        //
        // txtPhone
        //
        txtPhone.Location = new Point(20, 276);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(180, 23);
        txtPhone.TabIndex = 12;
        //
        // btnExportPdf
        //
        btnExportPdf.Location = new Point(20, 320);
        btnExportPdf.Name = "btnExportPdf";
        btnExportPdf.Size = new Size(100, 30);
        btnExportPdf.TabIndex = 13;
        btnExportPdf.Text = "Export PDF";
        btnExportPdf.UseVisualStyleBackColor = true;
        btnExportPdf.Click += BtnExportPdf_Click;
        //
        // btnPrint
        //
        btnPrint.Location = new Point(130, 320);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(100, 30);
        btnPrint.TabIndex = 14;
        btnPrint.Text = "Print";
        btnPrint.UseVisualStyleBackColor = true;
        btnPrint.Click += BtnPrint_Click;
        //
        // btnClose
        //
        btnClose.Location = new Point(240, 320);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 30);
        btnClose.TabIndex = 15;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += BtnClose_Click;
        //
        // MonthlyActivityReportForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnClose;
        ClientSize = new Size(394, 369);
        Controls.Add(btnClose);
        Controls.Add(btnPrint);
        Controls.Add(btnExportPdf);
        Controls.Add(txtPhone);
        Controls.Add(lblPhone);
        Controls.Add(txtPreparedBy);
        Controls.Add(lblPreparedBy);
        Controls.Add(txtCounty);
        Controls.Add(lblCounty);
        Controls.Add(txtFoodBankName);
        Controls.Add(lblFoodBankName);
        Controls.Add(cmbYear);
        Controls.Add(lblYear);
        Controls.Add(cmbMonth);
        Controls.Add(lblMonth);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MonthlyActivityReportForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Monthly Activity Report";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitle;
    private Label lblMonth;
    private ComboBox cmbMonth;
    private Label lblYear;
    private ComboBox cmbYear;
    private Label lblFoodBankName;
    private TextBox txtFoodBankName;
    private Label lblCounty;
    private TextBox txtCounty;
    private Label lblPreparedBy;
    private TextBox txtPreparedBy;
    private Label lblPhone;
    private TextBox txtPhone;
    private Button btnExportPdf;
    private Button btnPrint;
    private Button btnClose;
}
