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
        grpMembers = new GroupBox();
        btnSetPrimary = new Button();
        btnRemoveMember = new Button();
        btnEditMember = new Button();
        btnAddMember = new Button();
        grdMembers = new DataGridView();
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
        btnSave = new Button();
        btnCancel = new Button();
        lblError = new Label();
        tabControl.SuspendLayout();
        tabProfile.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)grdMembers).BeginInit();
        tabServiceHistory.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvServiceHistory).BeginInit();
        SuspendLayout();
        // 
        // tabControl
        // 
        tabControl.Controls.Add(tabProfile);
        tabControl.Controls.Add(tabServiceHistory);
        tabControl.Dock = DockStyle.Fill;
        tabControl.Location = new Point(0, 0);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(800, 550);
        tabControl.TabIndex = 0;
        // 
        // tabProfile
        // 
        tabProfile.Controls.Add(lblPrimaryName);
        tabProfile.Controls.Add(txtPrimaryName);
        tabProfile.Controls.Add(grpMembers);
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
        tabProfile.Controls.Add(lblNotes);
        tabProfile.Controls.Add(txtNotes);
        tabProfile.Controls.Add(lblStatus);
        tabProfile.Controls.Add(lblStatusValue);
        tabProfile.Location = new Point(4, 24);
        tabProfile.Name = "tabProfile";
        tabProfile.Padding = new Padding(3);
        tabProfile.Size = new Size(792, 550);
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
        txtPrimaryName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtPrimaryName.Location = new Point(12, 33);
        txtPrimaryName.Name = "txtPrimaryName";
        txtPrimaryName.ReadOnly = true;
        txtPrimaryName.Size = new Size(768, 23);
        txtPrimaryName.TabIndex = 1;
        // 
        // grpMembers
        // 
        grpMembers.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpMembers.Controls.Add(btnSetPrimary);
        grpMembers.Controls.Add(btnRemoveMember);
        grpMembers.Controls.Add(btnEditMember);
        grpMembers.Controls.Add(btnAddMember);
        grpMembers.Controls.Add(grdMembers);
        grpMembers.Location = new Point(12, 62);
        grpMembers.Name = "grpMembers";
        grpMembers.Size = new Size(768, 185);
        grpMembers.TabIndex = 2;
        grpMembers.TabStop = false;
        grpMembers.Text = "Household Members";
        // 
        // grdMembers
        // 
        grdMembers.AllowUserToAddRows = false;
        grdMembers.AllowUserToDeleteRows = false;
        grdMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grdMembers.Location = new Point(10, 22);
        grdMembers.MultiSelect = false;
        grdMembers.Name = "grdMembers";
        grdMembers.ReadOnly = true;
        grdMembers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grdMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grdMembers.Size = new Size(748, 120);
        grdMembers.TabIndex = 0;
        // 
        // btnAddMember
        // 
        btnAddMember.Location = new Point(10, 150);
        btnAddMember.Name = "btnAddMember";
        btnAddMember.Size = new Size(85, 28);
        btnAddMember.TabIndex = 1;
        btnAddMember.Text = "Add Member";
        btnAddMember.UseVisualStyleBackColor = true;
        btnAddMember.Click += BtnAddMember_Click;
        // 
        // btnEditMember
        // 
        btnEditMember.Location = new Point(101, 150);
        btnEditMember.Name = "btnEditMember";
        btnEditMember.Size = new Size(75, 28);
        btnEditMember.TabIndex = 2;
        btnEditMember.Text = "Edit";
        btnEditMember.UseVisualStyleBackColor = true;
        btnEditMember.Click += BtnEditMember_Click;
        // 
        // btnRemoveMember
        // 
        btnRemoveMember.Location = new Point(182, 150);
        btnRemoveMember.Name = "btnRemoveMember";
        btnRemoveMember.Size = new Size(75, 28);
        btnRemoveMember.TabIndex = 3;
        btnRemoveMember.Text = "Remove";
        btnRemoveMember.UseVisualStyleBackColor = true;
        btnRemoveMember.Click += BtnRemoveMember_Click;
        // 
        // btnSetPrimary
        // 
        btnSetPrimary.Location = new Point(263, 150);
        btnSetPrimary.Name = "btnSetPrimary";
        btnSetPrimary.Size = new Size(85, 28);
        btnSetPrimary.TabIndex = 4;
        btnSetPrimary.Text = "Set Primary";
        btnSetPrimary.UseVisualStyleBackColor = true;
        btnSetPrimary.Click += BtnSetPrimary_Click;
        // 
        // lblAddress1
        // 
        lblAddress1.AutoSize = true;
        lblAddress1.Location = new Point(12, 253);
        lblAddress1.Name = "lblAddress1";
        lblAddress1.Size = new Size(58, 15);
        lblAddress1.TabIndex = 3;
        lblAddress1.Text = "Address:";
        // 
        // txtAddress1
        // 
        txtAddress1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtAddress1.Location = new Point(12, 271);
        txtAddress1.Name = "txtAddress1";
        txtAddress1.Size = new Size(768, 23);
        txtAddress1.TabIndex = 2;
        // 
        // lblCity
        // 
        lblCity.AutoSize = true;
        lblCity.Location = new Point(12, 308);
        lblCity.Name = "lblCity";
        lblCity.Size = new Size(31, 15);
        lblCity.TabIndex = 4;
        lblCity.Text = "City:";
        // 
        // txtCity
        // 
        txtCity.Location = new Point(12, 326);
        txtCity.Name = "txtCity";
        txtCity.Size = new Size(200, 23);
        txtCity.TabIndex = 3;
        // 
        // lblState
        // 
        lblState.AutoSize = true;
        lblState.Location = new Point(218, 308);
        lblState.Name = "lblState";
        lblState.Size = new Size(36, 15);
        lblState.TabIndex = 6;
        lblState.Text = "State:";
        // 
        // txtState
        // 
        txtState.Location = new Point(218, 326);
        txtState.Name = "txtState";
        txtState.Size = new Size(70, 23);
        txtState.TabIndex = 4;
        // 
        // lblZip
        // 
        lblZip.AutoSize = true;
        lblZip.Location = new Point(294, 308);
        lblZip.Name = "lblZip";
        lblZip.Size = new Size(27, 15);
        lblZip.TabIndex = 8;
        lblZip.Text = "Zip:";
        // 
        // txtZip
        // 
        txtZip.Location = new Point(294, 326);
        txtZip.Name = "txtZip";
        txtZip.Size = new Size(118, 23);
        txtZip.TabIndex = 5;
        // 
        // lblPhone
        // 
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(12, 363);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(44, 15);
        lblPhone.TabIndex = 10;
        lblPhone.Text = "Phone:";
        // 
        // txtPhone
        // 
        txtPhone.Location = new Point(12, 381);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(200, 23);
        txtPhone.TabIndex = 6;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(218, 363);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(39, 15);
        lblEmail.TabIndex = 12;
        lblEmail.Text = "Email (Optional):";
        // 
        // txtEmail
        // 
        txtEmail.Location = new Point(218, 381);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(194, 23);
        txtEmail.TabIndex = 7;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 418);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(41, 15);
        lblNotes.TabIndex = 22;
        lblNotes.Text = "Notes:";
        // 
        // txtNotes
        // 
        txtNotes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtNotes.Location = new Point(12, 436);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(768, 80);
        txtNotes.TabIndex = 11;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(12, 525);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(42, 15);
        lblStatus.TabIndex = 12;
        lblStatus.Text = "Status:";
        // 
        // lblStatusValue
        // 
        lblStatusValue.AutoSize = true;
        lblStatusValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStatusValue.Location = new Point(60, 525);
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
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.Location = new Point(624, 556);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 1;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.Location = new Point(705, 556);
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
        lblError.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblError.Location = new Point(12, 563);
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
        ClientSize = new Size(800, 600);
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
        ((System.ComponentModel.ISupportInitialize)grdMembers).EndInit();
        tabServiceHistory.ResumeLayout(false);
        tabServiceHistory.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvServiceHistory).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TabControl tabControl;
    private TabPage tabProfile;
    private TabPage tabServiceHistory;
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
    private GroupBox grpMembers;
    private DataGridView grdMembers;
    private Button btnAddMember;
    private Button btnEditMember;
    private Button btnRemoveMember;
    private Button btnSetPrimary;
    private Label lblNotes;
    private TextBox txtNotes;
    private Label lblStatus;
    private Label lblStatusValue;
    private Label lblFilterStatus;
    private ComboBox cmbFilterStatus;
    private Label lblFilterType;
    private ComboBox cmbFilterType;
    private DataGridView dgvServiceHistory;
    private Button btnSave;
    private Button btnCancel;
    private Label lblError;
}
