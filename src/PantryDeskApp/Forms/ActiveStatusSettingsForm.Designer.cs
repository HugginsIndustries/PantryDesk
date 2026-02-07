namespace PantryDeskApp.Forms;

partial class ActiveStatusSettingsForm
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
        lblDay = new Label();
        numDay = new NumericUpDown();
        btnSave = new Button();
        btnCancel = new Button();
        lblError = new Label();
        ((System.ComponentModel.ISupportInitialize)numDay).BeginInit();
        SuspendLayout();
        //
        // lblTitle
        //
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(240, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Active Status Reset Date";
        //
        // lblMonth
        //
        lblMonth.AutoSize = true;
        lblMonth.Location = new Point(20, 60);
        lblMonth.Name = "lblMonth";
        lblMonth.Size = new Size(112, 15);
        lblMonth.TabIndex = 1;
        lblMonth.Text = "Reset Month (1-12):";
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
        cmbMonth.Location = new Point(20, 78);
        cmbMonth.Name = "cmbMonth";
        cmbMonth.Size = new Size(180, 23);
        cmbMonth.TabIndex = 2;
        //
        // lblDay
        //
        lblDay.AutoSize = true;
        lblDay.Location = new Point(20, 120);
        lblDay.Name = "lblDay";
        lblDay.Size = new Size(93, 15);
        lblDay.TabIndex = 3;
        lblDay.Text = "Reset Day (1-31):";
        //
        // numDay
        //
        numDay.Location = new Point(20, 138);
        numDay.Maximum = 31;
        numDay.Minimum = 1;
        numDay.Name = "numDay";
        numDay.Size = new Size(80, 23);
        numDay.TabIndex = 4;
        numDay.Value = 1;
        //
        // btnSave
        //
        btnSave.Location = new Point(120, 190);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 5;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        //
        // btnCancel
        //
        btnCancel.Location = new Point(39, 190);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 6;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        //
        // lblError
        //
        lblError.AutoSize = true;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(20, 170);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 7;
        lblError.Visible = false;
        //
        // ActiveStatusSettingsForm
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(234, 241);
        Controls.Add(lblError);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(numDay);
        Controls.Add(lblDay);
        Controls.Add(cmbMonth);
        Controls.Add(lblMonth);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ActiveStatusSettingsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Active Status Reset Date";
        ((System.ComponentModel.ISupportInitialize)numDay).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitle;
    private Label lblMonth;
    private ComboBox cmbMonth;
    private Label lblDay;
    private NumericUpDown numDay;
    private Button btnSave;
    private Button btnCancel;
    private Label lblError;
}
