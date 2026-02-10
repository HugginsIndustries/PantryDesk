namespace PantryDeskApp.Forms;

partial class EditServiceEventDialog
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lblEventType = new Label();
        lblEventTypeValue = new Label();
        lblDate = new Label();
        dtpEventDate = new DateTimePicker();
        lblNotes = new Label();
        txtNotes = new TextBox();
        grpAppointment = new GroupBox();
        lblStatus = new Label();
        cmbStatus = new ComboBox();
        lblScheduledText = new Label();
        txtScheduledText = new TextBox();
        lblMember = new Label();
        cmbMember = new ComboBox();
        pnlVisitType = new Panel();
        lblVisitType = new Label();
        cmbVisitType = new ComboBox();
        pnlOverride = new Panel();
        lblOverrideReason = new Label();
        cmbOverrideReason = new ComboBox();
        btnSave = new Button();
        btnCancel = new Button();
        grpAppointment.SuspendLayout();
        pnlVisitType.SuspendLayout();
        pnlOverride.SuspendLayout();
        SuspendLayout();
        //
        // lblEventType
        //
        lblEventType.AutoSize = true;
        lblEventType.Location = new Point(12, 15);
        lblEventType.Name = "lblEventType";
        lblEventType.Size = new Size(65, 15);
        lblEventType.TabIndex = 0;
        lblEventType.Text = "Event Type:";
        //
        // lblEventTypeValue
        //
        lblEventTypeValue.AutoSize = true;
        lblEventTypeValue.Location = new Point(120, 15);
        lblEventTypeValue.Name = "lblEventTypeValue";
        lblEventTypeValue.Size = new Size(0, 15);
        lblEventTypeValue.TabIndex = 1;
        //
        // lblDate
        //
        lblDate.AutoSize = true;
        lblDate.Location = new Point(12, 45);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(34, 15);
        lblDate.TabIndex = 2;
        lblDate.Text = "Date:";
        //
        // dtpEventDate
        //
        dtpEventDate.Format = DateTimePickerFormat.Short;
        dtpEventDate.Location = new Point(12, 63);
        dtpEventDate.Name = "dtpEventDate";
        dtpEventDate.Size = new Size(200, 23);
        dtpEventDate.TabIndex = 3;
        //
        // lblNotes
        //
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 335);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(38, 15);
        lblNotes.TabIndex = 4;
        lblNotes.Text = "Notes:";
        //
        // txtNotes
        //
        txtNotes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtNotes.Location = new Point(12, 353);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(436, 70);
        txtNotes.TabIndex = 5;
        //
        // grpAppointment
        //
        grpAppointment.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpAppointment.Controls.Add(lblStatus);
        grpAppointment.Controls.Add(cmbStatus);
        grpAppointment.Controls.Add(lblScheduledText);
        grpAppointment.Controls.Add(txtScheduledText);
        grpAppointment.Controls.Add(lblMember);
        grpAppointment.Controls.Add(cmbMember);
        grpAppointment.Location = new Point(12, 95);
        grpAppointment.Name = "grpAppointment";
        grpAppointment.Size = new Size(436, 230);
        grpAppointment.TabIndex = 6;
        grpAppointment.TabStop = false;
        grpAppointment.Text = "Appointment Details";
        //
        // lblStatus
        //
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(12, 25);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(42, 15);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "Status:";
        //
        // cmbStatus
        //
        cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatus.FormattingEnabled = true;
        cmbStatus.Location = new Point(12, 43);
        cmbStatus.Name = "cmbStatus";
        cmbStatus.Size = new Size(180, 23);
        cmbStatus.TabIndex = 1;
        cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;
        //
        // lblScheduledText
        //
        lblScheduledText.AutoSize = true;
        lblScheduledText.Location = new Point(12, 75);
        lblScheduledText.Name = "lblScheduledText";
        lblScheduledText.Size = new Size(89, 15);
        lblScheduledText.TabIndex = 2;
        lblScheduledText.Text = "Scheduled Text (required):";
        //
        // txtScheduledText
        //
        txtScheduledText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtScheduledText.Location = new Point(12, 93);
        txtScheduledText.Multiline = true;
        txtScheduledText.Name = "txtScheduledText";
        txtScheduledText.Size = new Size(412, 45);
        txtScheduledText.TabIndex = 3;
        //
        // lblMember
        //
        lblMember.AutoSize = true;
        lblMember.Location = new Point(12, 145);
        lblMember.Name = "lblMember";
        lblMember.Size = new Size(53, 15);
        lblMember.TabIndex = 4;
        lblMember.Text = "Member:";
        //
        // cmbMember
        //
        cmbMember.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMember.FormattingEnabled = true;
        cmbMember.Location = new Point(12, 163);
        cmbMember.Name = "cmbMember";
        cmbMember.Size = new Size(300, 23);
        cmbMember.TabIndex = 5;
        //
        // pnlVisitType
        //
        pnlVisitType.Controls.Add(lblVisitType);
        pnlVisitType.Controls.Add(cmbVisitType);
        pnlVisitType.Location = new Point(12, 95);
        pnlVisitType.Name = "pnlVisitType";
        pnlVisitType.Size = new Size(436, 60);
        pnlVisitType.TabIndex = 7;
        //
        // lblVisitType
        //
        lblVisitType.AutoSize = true;
        lblVisitType.Location = new Point(0, 8);
        lblVisitType.Name = "lblVisitType";
        lblVisitType.Size = new Size(58, 15);
        lblVisitType.TabIndex = 0;
        lblVisitType.Text = "Visit Type:";
        //
        // cmbVisitType
        //
        cmbVisitType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbVisitType.FormattingEnabled = true;
        cmbVisitType.Location = new Point(0, 28);
        cmbVisitType.Name = "cmbVisitType";
        cmbVisitType.Size = new Size(250, 23);
        cmbVisitType.TabIndex = 1;
        //
        // pnlOverride
        //
        pnlOverride.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlOverride.Controls.Add(lblOverrideReason);
        pnlOverride.Controls.Add(cmbOverrideReason);
        pnlOverride.Location = new Point(12, 430);
        pnlOverride.Name = "pnlOverride";
        pnlOverride.Size = new Size(436, 55);
        pnlOverride.TabIndex = 8;
        //
        // lblOverrideReason
        //
        lblOverrideReason.AutoSize = true;
        lblOverrideReason.Location = new Point(0, 8);
        lblOverrideReason.Name = "lblOverrideReason";
        lblOverrideReason.Size = new Size(91, 15);
        lblOverrideReason.TabIndex = 0;
        lblOverrideReason.Text = "Override Reason:";
        //
        // cmbOverrideReason
        //
        cmbOverrideReason.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbOverrideReason.FormattingEnabled = true;
        cmbOverrideReason.Location = new Point(0, 28);
        cmbOverrideReason.Name = "cmbOverrideReason";
        cmbOverrideReason.Size = new Size(250, 23);
        cmbOverrideReason.TabIndex = 1;
        //
        // btnSave
        //
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.Location = new Point(316, 505);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 9;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        //
        // btnCancel
        //
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.Location = new Point(397, 505);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 10;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        //
        // EditServiceEventDialog
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(484, 547);
        Controls.Add(lblEventType);
        Controls.Add(lblEventTypeValue);
        Controls.Add(lblDate);
        Controls.Add(dtpEventDate);
        Controls.Add(grpAppointment);
        Controls.Add(pnlVisitType);
        Controls.Add(lblNotes);
        Controls.Add(txtNotes);
        Controls.Add(pnlOverride);
        Controls.Add(btnSave);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EditServiceEventDialog";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Edit";
        grpAppointment.ResumeLayout(false);
        grpAppointment.PerformLayout();
        pnlVisitType.ResumeLayout(false);
        pnlVisitType.PerformLayout();
        pnlOverride.ResumeLayout(false);
        pnlOverride.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblEventType;
    private Label lblEventTypeValue;
    private Label lblDate;
    private DateTimePicker dtpEventDate;
    private Label lblNotes;
    private TextBox txtNotes;
    private GroupBox grpAppointment;
    private Label lblStatus;
    private ComboBox cmbStatus;
    private Label lblScheduledText;
    private TextBox txtScheduledText;
    private Label lblMember;
    private ComboBox cmbMember;
    private Panel pnlVisitType;
    private Label lblVisitType;
    private ComboBox cmbVisitType;
    private Panel pnlOverride;
    private Label lblOverrideReason;
    private ComboBox cmbOverrideReason;
    private Button btnSave;
    private Button btnCancel;
}
