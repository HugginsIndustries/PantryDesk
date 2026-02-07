namespace PantryDeskApp.Forms;

partial class HouseholdProfileForm
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
        tabControl = new TabControl();
        tabProfile = new TabPage();
        lblPrimaryName = new Label();
        txtPrimaryName = new TextBox();
        lblAddress1 = new Label();
        txtAddress1 = new TextBox();
        lblCity = new Label();
        txtCity = new TextBox();
        lblState = new Label();
        txtState = new TextBox();
        lblZip = new Label();
        txtZip = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblChildren = new Label();
        numChildren = new NumericUpDown();
        lblAdults = new Label();
        numAdults = new NumericUpDown();
        lblSeniors = new Label();
        numSeniors = new NumericUpDown();
        lblTotalSize = new Label();
        lblTotalSizeValue = new Label();
        lblNotes = new Label();
        txtNotes = new TextBox();
        lblStatus = new Label();
        lblStatusValue = new Label();
        tabServiceHistory = new TabPage();
        lblFilterStatus = new Label();
        cmbFilterStatus = new ComboBox();
        lblFilterType = new Label();
        cmbFilterType = new ComboBox();
        dgvServiceHistory = new DataGridView();
        tabAppointments = new TabPage();
        grpScheduleAppointment = new GroupBox();
        lblScheduledDate = new Label();
        dtpScheduledDate = new DateTimePicker();
        lblScheduledText = new Label();
        txtScheduledText = new TextBox();
        lblAppointmentNotes = new Label();
        txtAppointmentNotes = new TextBox();
        btnSchedule = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        lblError = new Label();
        tabControl.SuspendLayout();
        tabProfile.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numChildren).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numAdults).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSeniors).BeginInit();
        tabServiceHistory.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvServiceHistory).BeginInit();
        tabAppointments.SuspendLayout();
        grpScheduleAppointment.SuspendLayout();
        SuspendLayout();
        // 
        // tabControl
        // 
        tabControl.Controls.Add(tabProfile);
        tabControl.Controls.Add(tabServiceHistory);
        tabControl.Controls.Add(tabAppointments);
        tabControl.Dock = DockStyle.Fill;
        tabControl.Location = new Point(0, 0);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(800, 450);
        tabControl.TabIndex = 0;
        // 
        // tabProfile
        // 
        tabProfile.Controls.Add(lblPrimaryName);
        tabProfile.Controls.Add(txtPrimaryName);
        tabProfile.Controls.Add(lblAddress1);
        tabProfile.Controls.Add(txtAddress1);
        tabProfile.Controls.Add(lblCity);
        tabProfile.Controls.Add(txtCity);
        tabProfile.Controls.Add(lblState);
        tabProfile.Controls.Add(txtState);
        tabProfile.Controls.Add(lblZip);
        tabProfile.Controls.Add(txtZip);
        tabProfile.Controls.Add(lblPhone);
        tabProfile.Controls.Add(txtPhone);
        tabProfile.Controls.Add(lblEmail);
        tabProfile.Controls.Add(txtEmail);
        tabProfile.Controls.Add(lblChildren);
        tabProfile.Controls.Add(numChildren);
        tabProfile.Controls.Add(lblAdults);
        tabProfile.Controls.Add(numAdults);
        tabProfile.Controls.Add(lblSeniors);
        tabProfile.Controls.Add(numSeniors);
        tabProfile.Controls.Add(lblTotalSize);
        tabProfile.Controls.Add(lblTotalSizeValue);
        tabProfile.Controls.Add(lblNotes);
        tabProfile.Controls.Add(txtNotes);
        tabProfile.Controls.Add(lblStatus);
        tabProfile.Controls.Add(lblStatusValue);
        tabProfile.Location = new Point(4, 24);
        tabProfile.Name = "tabProfile";
        tabProfile.Padding = new Padding(3);
        tabProfile.Size = new Size(792, 422);
        tabProfile.TabIndex = 0;
        tabProfile.Text = "Profile";
        tabProfile.UseVisualStyleBackColor = true;
        // 
        // lblPrimaryName
        // 
        lblPrimaryName.AutoSize = true;
        lblPrimaryName.Location = new Point(12, 15);
        lblPrimaryName.Name = "lblPrimaryName";
        lblPrimaryName.Size = new Size(78, 15);
        lblPrimaryName.TabIndex = 0;
        lblPrimaryName.Text = "Primary Name:";
        // 
        // txtPrimaryName
        // 
        txtPrimaryName.Location = new Point(12, 33);
        txtPrimaryName.Name = "txtPrimaryName";
        txtPrimaryName.Size = new Size(400, 23);
        txtPrimaryName.TabIndex = 1;
        // 
        // lblAddress1
        // 
        lblAddress1.AutoSize = true;
        lblAddress1.Location = new Point(12, 70);
        lblAddress1.Name = "lblAddress1";
        lblAddress1.Size = new Size(58, 15);
        lblAddress1.TabIndex = 2;
        lblAddress1.Text = "Address:";
        // 
        // txtAddress1
        // 
        txtAddress1.Location = new Point(12, 88);
        txtAddress1.Name = "txtAddress1";
        txtAddress1.Size = new Size(400, 23);
        txtAddress1.TabIndex = 2;
        // 
        // lblCity
        // 
        lblCity.AutoSize = true;
        lblCity.Location = new Point(12, 125);
        lblCity.Name = "lblCity";
        lblCity.Size = new Size(31, 15);
        lblCity.TabIndex = 4;
        lblCity.Text = "City:";
        // 
        // txtCity
        // 
        txtCity.Location = new Point(12, 143);
        txtCity.Name = "txtCity";
        txtCity.Size = new Size(200, 23);
        txtCity.TabIndex = 3;
        // 
        // lblState
        // 
        lblState.AutoSize = true;
        lblState.Location = new Point(218, 125);
        lblState.Name = "lblState";
        lblState.Size = new Size(36, 15);
        lblState.TabIndex = 6;
        lblState.Text = "State:";
        // 
        // txtState
        // 
        txtState.Location = new Point(218, 143);
        txtState.Name = "txtState";
        txtState.Size = new Size(70, 23);
        txtState.TabIndex = 4;
        // 
        // lblZip
        // 
        lblZip.AutoSize = true;
        lblZip.Location = new Point(294, 125);
        lblZip.Name = "lblZip";
        lblZip.Size = new Size(27, 15);
        lblZip.TabIndex = 8;
        lblZip.Text = "Zip:";
        // 
        // txtZip
        // 
        txtZip.Location = new Point(294, 143);
        txtZip.Name = "txtZip";
        txtZip.Size = new Size(118, 23);
        txtZip.TabIndex = 5;
        // 
        // lblPhone
        // 
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(12, 180);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(44, 15);
        lblPhone.TabIndex = 10;
        lblPhone.Text = "Phone:";
        // 
        // txtPhone
        // 
        txtPhone.Location = new Point(12, 198);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(200, 23);
        txtPhone.TabIndex = 6;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(218, 180);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(39, 15);
        lblEmail.TabIndex = 12;
        lblEmail.Text = "Email (Optional):";
        // 
        // txtEmail
        // 
        txtEmail.Location = new Point(218, 198);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(194, 23);
        txtEmail.TabIndex = 7;
        // 
        // lblChildren
        // 
        lblChildren.AutoSize = true;
        lblChildren.Location = new Point(12, 235);
        lblChildren.Name = "lblChildren";
        lblChildren.Size = new Size(58, 15);
        lblChildren.TabIndex = 14;
        lblChildren.Text = "Children:";
        // 
        // numChildren
        // 
        numChildren.Location = new Point(12, 253);
        numChildren.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        numChildren.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
        numChildren.Name = "numChildren";
        numChildren.Size = new Size(100, 23);
        numChildren.TabIndex = 8;
        numChildren.ValueChanged += NumCount_ValueChanged;
        // 
        // lblAdults
        // 
        lblAdults.AutoSize = true;
        lblAdults.Location = new Point(118, 235);
        lblAdults.Name = "lblAdults";
        lblAdults.Size = new Size(45, 15);
        lblAdults.TabIndex = 16;
        lblAdults.Text = "Adults:";
        // 
        // numAdults
        // 
        numAdults.Location = new Point(118, 253);
        numAdults.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        numAdults.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
        numAdults.Name = "numAdults";
        numAdults.Size = new Size(100, 23);
        numAdults.TabIndex = 9;
        numAdults.ValueChanged += NumCount_ValueChanged;
        // 
        // lblSeniors
        // 
        lblSeniors.AutoSize = true;
        lblSeniors.Location = new Point(224, 235);
        lblSeniors.Name = "lblSeniors";
        lblSeniors.Size = new Size(49, 15);
        lblSeniors.TabIndex = 18;
        lblSeniors.Text = "Seniors:";
        // 
        // numSeniors
        // 
        numSeniors.Location = new Point(224, 253);
        numSeniors.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        numSeniors.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
        numSeniors.Name = "numSeniors";
        numSeniors.Size = new Size(100, 23);
        numSeniors.TabIndex = 10;
        numSeniors.ValueChanged += NumCount_ValueChanged;
        // 
        // lblTotalSize
        // 
        lblTotalSize.AutoSize = true;
        lblTotalSize.Location = new Point(330, 235);
        lblTotalSize.Name = "lblTotalSize";
        lblTotalSize.Size = new Size(60, 15);
        lblTotalSize.TabIndex = 20;
        lblTotalSize.Text = "Total Size:";
        // 
        // lblTotalSizeValue
        // 
        lblTotalSizeValue.AutoSize = true;
        lblTotalSizeValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTotalSizeValue.Location = new Point(330, 253);
        lblTotalSizeValue.Name = "lblTotalSizeValue";
        lblTotalSizeValue.Size = new Size(14, 15);
        lblTotalSizeValue.TabIndex = 21;
        lblTotalSizeValue.Text = "0";
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 290);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(41, 15);
        lblNotes.TabIndex = 22;
        lblNotes.Text = "Notes:";
        // 
        // txtNotes
        // 
        txtNotes.Location = new Point(12, 308);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(400, 80);
        txtNotes.TabIndex = 11;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(12, 400);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(42, 15);
        lblStatus.TabIndex = 12;
        lblStatus.Text = "Status:";
        // 
        // lblStatusValue
        // 
        lblStatusValue.AutoSize = true;
        lblStatusValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStatusValue.Location = new Point(60, 400);
        lblStatusValue.Name = "lblStatusValue";
        lblStatusValue.Size = new Size(40, 15);
        lblStatusValue.TabIndex = 13;
        lblStatusValue.Text = "Active";
        // 
        // tabServiceHistory
        // 
        tabServiceHistory.Controls.Add(lblFilterStatus);
        tabServiceHistory.Controls.Add(cmbFilterStatus);
        tabServiceHistory.Controls.Add(lblFilterType);
        tabServiceHistory.Controls.Add(cmbFilterType);
        tabServiceHistory.Controls.Add(dgvServiceHistory);
        tabServiceHistory.Location = new Point(4, 24);
        tabServiceHistory.Name = "tabServiceHistory";
        tabServiceHistory.Padding = new Padding(3);
        tabServiceHistory.Size = new Size(792, 422);
        tabServiceHistory.TabIndex = 1;
        tabServiceHistory.Text = "Service History";
        tabServiceHistory.UseVisualStyleBackColor = true;
        // 
        // lblFilterStatus
        // 
        lblFilterStatus.AutoSize = true;
        lblFilterStatus.Location = new Point(12, 15);
        lblFilterStatus.Name = "lblFilterStatus";
        lblFilterStatus.Size = new Size(42, 15);
        lblFilterStatus.TabIndex = 0;
        lblFilterStatus.Text = "Status:";
        // 
        // cmbFilterStatus
        // 
        cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterStatus.FormattingEnabled = true;
        cmbFilterStatus.Location = new Point(60, 12);
        cmbFilterStatus.Name = "cmbFilterStatus";
        cmbFilterStatus.Size = new Size(150, 23);
        cmbFilterStatus.TabIndex = 1;
        cmbFilterStatus.SelectedIndexChanged += Filter_SelectedIndexChanged;
        // 
        // lblFilterType
        // 
        lblFilterType.AutoSize = true;
        lblFilterType.Location = new Point(220, 15);
        lblFilterType.Name = "lblFilterType";
        lblFilterType.Size = new Size(35, 15);
        lblFilterType.TabIndex = 2;
        lblFilterType.Text = "Type:";
        // 
        // cmbFilterType
        // 
        cmbFilterType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterType.FormattingEnabled = true;
        cmbFilterType.Location = new Point(261, 12);
        cmbFilterType.Name = "cmbFilterType";
        cmbFilterType.Size = new Size(150, 23);
        cmbFilterType.TabIndex = 3;
        cmbFilterType.SelectedIndexChanged += Filter_SelectedIndexChanged;
        // 
        // dgvServiceHistory
        // 
        dgvServiceHistory.AllowUserToAddRows = false;
        dgvServiceHistory.AllowUserToDeleteRows = false;
        dgvServiceHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvServiceHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvServiceHistory.Location = new Point(12, 45);
        dgvServiceHistory.MultiSelect = false;
        dgvServiceHistory.Name = "dgvServiceHistory";
        dgvServiceHistory.ReadOnly = true;
        dgvServiceHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvServiceHistory.Size = new Size(774, 371);
        dgvServiceHistory.TabIndex = 4;
        dgvServiceHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // tabAppointments
        // 
        tabAppointments.Controls.Add(grpScheduleAppointment);
        tabAppointments.Location = new Point(4, 24);
        tabAppointments.Name = "tabAppointments";
        tabAppointments.Padding = new Padding(3);
        tabAppointments.Size = new Size(792, 422);
        tabAppointments.TabIndex = 2;
        tabAppointments.Text = "Appointments";
        tabAppointments.UseVisualStyleBackColor = true;
        // 
        // grpScheduleAppointment
        // 
        grpScheduleAppointment.Controls.Add(lblScheduledDate);
        grpScheduleAppointment.Controls.Add(dtpScheduledDate);
        grpScheduleAppointment.Controls.Add(lblScheduledText);
        grpScheduleAppointment.Controls.Add(txtScheduledText);
        grpScheduleAppointment.Controls.Add(lblAppointmentNotes);
        grpScheduleAppointment.Controls.Add(txtAppointmentNotes);
        grpScheduleAppointment.Controls.Add(btnSchedule);
        grpScheduleAppointment.Location = new Point(12, 15);
        grpScheduleAppointment.Name = "grpScheduleAppointment";
        grpScheduleAppointment.Size = new Size(500, 300);
        grpScheduleAppointment.TabIndex = 0;
        grpScheduleAppointment.TabStop = false;
        grpScheduleAppointment.Text = "Schedule New Appointment";
        // 
        // lblScheduledDate
        // 
        lblScheduledDate.AutoSize = true;
        lblScheduledDate.Location = new Point(12, 25);
        lblScheduledDate.Name = "lblScheduledDate";
        lblScheduledDate.Size = new Size(88, 15);
        lblScheduledDate.TabIndex = 0;
        lblScheduledDate.Text = "Scheduled Date:";
        // 
        // dtpScheduledDate
        // 
        dtpScheduledDate.Location = new Point(12, 43);
        dtpScheduledDate.Name = "dtpScheduledDate";
        dtpScheduledDate.Size = new Size(200, 23);
        dtpScheduledDate.TabIndex = 1;
        // 
        // lblScheduledText
        // 
        lblScheduledText.AutoSize = true;
        lblScheduledText.Location = new Point(12, 80);
        lblScheduledText.Name = "lblScheduledText";
        lblScheduledText.Size = new Size(89, 15);
        lblScheduledText.TabIndex = 2;
        lblScheduledText.Text = "Scheduled Text:";
        // 
        // txtScheduledText
        // 
        txtScheduledText.Location = new Point(12, 98);
        txtScheduledText.Multiline = true;
        txtScheduledText.Name = "txtScheduledText";
        txtScheduledText.PlaceholderText = "e.g., 10:00 AM - Food pickup";
        txtScheduledText.ScrollBars = ScrollBars.Vertical;
        txtScheduledText.Size = new Size(470, 80);
        txtScheduledText.TabIndex = 3;
        // 
        // lblAppointmentNotes
        // 
        lblAppointmentNotes.AutoSize = true;
        lblAppointmentNotes.Location = new Point(12, 190);
        lblAppointmentNotes.Name = "lblAppointmentNotes";
        lblAppointmentNotes.Size = new Size(41, 15);
        lblAppointmentNotes.TabIndex = 4;
        lblAppointmentNotes.Text = "Notes (Optional):";
        // 
        // txtAppointmentNotes
        // 
        txtAppointmentNotes.Location = new Point(12, 208);
        txtAppointmentNotes.Multiline = true;
        txtAppointmentNotes.Name = "txtAppointmentNotes";
        txtAppointmentNotes.ScrollBars = ScrollBars.Vertical;
        txtAppointmentNotes.Size = new Size(470, 60);
        txtAppointmentNotes.TabIndex = 5;
        // 
        // btnSchedule
        // 
        btnSchedule.Location = new Point(407, 274);
        btnSchedule.Name = "btnSchedule";
        btnSchedule.Size = new Size(75, 30);
        btnSchedule.TabIndex = 6;
        btnSchedule.Text = "Schedule";
        btnSchedule.UseVisualStyleBackColor = true;
        btnSchedule.Click += BtnSchedule_Click;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(624, 456);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 1;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(705, 456);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 2;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // lblError
        // 
        lblError.AutoSize = true;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(12, 463);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 3;
        lblError.Visible = false;
        // 
        // HouseholdProfileForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(800, 500);
        Controls.Add(lblError);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(tabControl);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "HouseholdProfileForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Household Profile";
        Load += HouseholdProfileForm_Load;
        tabControl.ResumeLayout(false);
        tabProfile.ResumeLayout(false);
        tabProfile.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numChildren).EndInit();
        ((System.ComponentModel.ISupportInitialize)numAdults).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSeniors).EndInit();
        tabServiceHistory.ResumeLayout(false);
        tabServiceHistory.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvServiceHistory).EndInit();
        tabAppointments.ResumeLayout(false);
        grpScheduleAppointment.ResumeLayout(false);
        grpScheduleAppointment.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TabControl tabControl;
    private TabPage tabProfile;
    private TabPage tabServiceHistory;
    private TabPage tabAppointments;
    private Label lblPrimaryName;
    private TextBox txtPrimaryName;
    private Label lblAddress1;
    private TextBox txtAddress1;
    private Label lblCity;
    private TextBox txtCity;
    private Label lblState;
    private TextBox txtState;
    private Label lblZip;
    private TextBox txtZip;
    private Label lblPhone;
    private TextBox txtPhone;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblChildren;
    private NumericUpDown numChildren;
    private Label lblAdults;
    private NumericUpDown numAdults;
    private Label lblSeniors;
    private NumericUpDown numSeniors;
    private Label lblTotalSize;
    private Label lblTotalSizeValue;
    private Label lblNotes;
    private TextBox txtNotes;
    private Label lblStatus;
    private Label lblStatusValue;
    private Label lblFilterStatus;
    private ComboBox cmbFilterStatus;
    private Label lblFilterType;
    private ComboBox cmbFilterType;
    private DataGridView dgvServiceHistory;
    private GroupBox grpScheduleAppointment;
    private Label lblScheduledDate;
    private DateTimePicker dtpScheduledDate;
    private Label lblScheduledText;
    private TextBox txtScheduledText;
    private Label lblAppointmentNotes;
    private TextBox txtAppointmentNotes;
    private Button btnSchedule;
    private Button btnSave;
    private Button btnCancel;
    private Label lblError;
}
