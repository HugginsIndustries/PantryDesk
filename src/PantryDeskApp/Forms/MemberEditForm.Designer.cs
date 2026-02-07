namespace PantryDeskApp.Forms;

partial class MemberEditForm
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
        lblFirstName = new Label();
        txtFirstName = new TextBox();
        lblLastName = new Label();
        txtLastName = new TextBox();
        lblBirthday = new Label();
        dtpBirthday = new DateTimePicker();
        chkPrimary = new CheckBox();
        lblRace = new Label();
        cmbRace = new ComboBox();
        lblVeteran = new Label();
        cmbVeteran = new ComboBox();
        lblDisabled = new Label();
        cmbDisabled = new ComboBox();
        btnSave = new Button();
        btnCancel = new Button();
        lblError = new Label();
        SuspendLayout();
        //
        // lblFirstName
        //
        lblFirstName.AutoSize = true;
        lblFirstName.Location = new Point(12, 15);
        lblFirstName.Name = "lblFirstName";
        lblFirstName.Size = new Size(64, 15);
        lblFirstName.TabIndex = 0;
        lblFirstName.Text = "First Name:";
        //
        // txtFirstName
        //
        txtFirstName.Location = new Point(12, 33);
        txtFirstName.Name = "txtFirstName";
        txtFirstName.Size = new Size(250, 23);
        txtFirstName.TabIndex = 1;
        //
        // lblLastName
        //
        lblLastName.AutoSize = true;
        lblLastName.Location = new Point(12, 65);
        lblLastName.Name = "lblLastName";
        lblLastName.Size = new Size(63, 15);
        lblLastName.TabIndex = 2;
        lblLastName.Text = "Last Name:";
        //
        // txtLastName
        //
        txtLastName.Location = new Point(12, 83);
        txtLastName.Name = "txtLastName";
        txtLastName.Size = new Size(250, 23);
        txtLastName.TabIndex = 2;
        //
        // lblBirthday
        //
        lblBirthday.AutoSize = true;
        lblBirthday.Location = new Point(12, 115);
        lblBirthday.Name = "lblBirthday";
        lblBirthday.Size = new Size(51, 15);
        lblBirthday.TabIndex = 4;
        lblBirthday.Text = "Birthday:";
        //
        // dtpBirthday
        //
        dtpBirthday.Format = DateTimePickerFormat.Short;
        dtpBirthday.Location = new Point(12, 133);
        dtpBirthday.Name = "dtpBirthday";
        dtpBirthday.Size = new Size(120, 23);
        dtpBirthday.TabIndex = 3;
        //
        // chkPrimary
        //
        chkPrimary.AutoSize = true;
        chkPrimary.Location = new Point(12, 165);
        chkPrimary.Name = "chkPrimary";
        chkPrimary.Size = new Size(104, 19);
        chkPrimary.TabIndex = 4;
        chkPrimary.Text = "Primary contact";
        //
        // lblRace
        //
        lblRace.AutoSize = true;
        lblRace.Location = new Point(12, 195);
        lblRace.Name = "lblRace";
        lblRace.Size = new Size(33, 15);
        lblRace.TabIndex = 6;
        lblRace.Text = "Race:";
        //
        // cmbRace
        //
        cmbRace.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRace.FormattingEnabled = true;
        cmbRace.Items.AddRange(new object[] { "White", "Black", "Hispanic", "Native American", "Not Specified" });
        cmbRace.Location = new Point(12, 213);
        cmbRace.Name = "cmbRace";
        cmbRace.Size = new Size(150, 23);
        cmbRace.TabIndex = 5;
        cmbRace.SelectedIndex = 4;
        //
        // lblVeteran
        //
        lblVeteran.AutoSize = true;
        lblVeteran.Location = new Point(12, 245);
        lblVeteran.Name = "lblVeteran";
        lblVeteran.Size = new Size(83, 15);
        lblVeteran.TabIndex = 8;
        lblVeteran.Text = "Veteran status:";
        //
        // cmbVeteran
        //
        cmbVeteran.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbVeteran.FormattingEnabled = true;
        cmbVeteran.Items.AddRange(new object[] { "None", "Active Duty", "Reserve", "Veteran", "Unknown", "Prefer Not To Answer" });
        cmbVeteran.Location = new Point(12, 263);
        cmbVeteran.Name = "cmbVeteran";
        cmbVeteran.Size = new Size(150, 23);
        cmbVeteran.TabIndex = 6;
        cmbVeteran.SelectedIndex = 5;
        //
        // lblDisabled
        //
        lblDisabled.AutoSize = true;
        lblDisabled.Location = new Point(12, 295);
        lblDisabled.Name = "lblDisabled";
        lblDisabled.Size = new Size(87, 15);
        lblDisabled.TabIndex = 10;
        lblDisabled.Text = "Disabled status:";
        //
        // cmbDisabled
        //
        cmbDisabled.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDisabled.FormattingEnabled = true;
        cmbDisabled.Items.AddRange(new object[] { "Not Disabled", "Disabled", "Unknown", "Prefer Not To Answer" });
        cmbDisabled.Location = new Point(12, 313);
        cmbDisabled.Name = "cmbDisabled";
        cmbDisabled.Size = new Size(150, 23);
        cmbDisabled.TabIndex = 7;
        cmbDisabled.SelectedIndex = 3;
        //
        // btnSave
        //
        btnSave.Location = new Point(106, 355);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 8;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        //
        // btnCancel
        //
        btnCancel.Location = new Point(187, 355);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 9;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        //
        // lblError
        //
        lblError.AutoSize = true;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(12, 350);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 12;
        lblError.Visible = false;
        //
        // MemberEditForm
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(274, 397);
        Controls.Add(lblError);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(cmbDisabled);
        Controls.Add(lblDisabled);
        Controls.Add(cmbVeteran);
        Controls.Add(lblVeteran);
        Controls.Add(cmbRace);
        Controls.Add(lblRace);
        Controls.Add(chkPrimary);
        Controls.Add(dtpBirthday);
        Controls.Add(lblBirthday);
        Controls.Add(txtLastName);
        Controls.Add(lblLastName);
        Controls.Add(txtFirstName);
        Controls.Add(lblFirstName);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MemberEditForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Household Member";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblFirstName;
    private TextBox txtFirstName;
    private Label lblLastName;
    private TextBox txtLastName;
    private Label lblBirthday;
    private DateTimePicker dtpBirthday;
    private CheckBox chkPrimary;
    private Label lblRace;
    private ComboBox cmbRace;
    private Label lblVeteran;
    private ComboBox cmbVeteran;
    private Label lblDisabled;
    private ComboBox cmbDisabled;
    private Button btnSave;
    private Button btnCancel;
    private Label lblError;
}
