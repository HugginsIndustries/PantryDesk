namespace PantryDeskApp.Forms;

partial class NewHouseholdForm
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
        grpContact = new GroupBox();
        txtEmail = new TextBox();
        lblEmail = new Label();
        txtPhone = new TextBox();
        lblPhone = new Label();
        txtZip = new TextBox();
        txtState = new TextBox();
        txtCity = new TextBox();
        lblZip = new Label();
        lblState = new Label();
        lblCity = new Label();
        txtAddress1 = new TextBox();
        lblAddress1 = new Label();
        grpMembers = new GroupBox();
        btnSetPrimary = new Button();
        btnRemoveMember = new Button();
        btnEditMember = new Button();
        btnAddMember = new Button();
        grdMembers = new DataGridView();
        lblNotes = new Label();
        txtNotes = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        lblError = new Label();
        grpContact.SuspendLayout();
        grpMembers.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)grdMembers).BeginInit();
        SuspendLayout();
        //
        // grpContact
        //
        grpContact.Controls.Add(txtEmail);
        grpContact.Controls.Add(lblEmail);
        grpContact.Controls.Add(txtPhone);
        grpContact.Controls.Add(lblPhone);
        grpContact.Controls.Add(txtZip);
        grpContact.Controls.Add(txtState);
        grpContact.Controls.Add(txtCity);
        grpContact.Controls.Add(lblZip);
        grpContact.Controls.Add(lblState);
        grpContact.Controls.Add(lblCity);
        grpContact.Controls.Add(txtAddress1);
        grpContact.Controls.Add(lblAddress1);
        grpContact.Location = new Point(12, 12);
        grpContact.Name = "grpContact";
        grpContact.Size = new Size(450, 150);
        grpContact.TabIndex = 0;
        grpContact.TabStop = false;
        grpContact.Text = "Contact Information";
        //
        // lblAddress1
        //
        lblAddress1.AutoSize = true;
        lblAddress1.Location = new Point(10, 25);
        lblAddress1.Name = "lblAddress1";
        lblAddress1.Size = new Size(58, 15);
        lblAddress1.TabIndex = 0;
        lblAddress1.Text = "Address:";
        //
        // txtAddress1
        //
        txtAddress1.Location = new Point(10, 43);
        txtAddress1.Name = "txtAddress1";
        txtAddress1.Size = new Size(420, 23);
        txtAddress1.TabIndex = 1;
        //
        // lblCity
        //
        lblCity.AutoSize = true;
        lblCity.Location = new Point(10, 75);
        lblCity.Name = "lblCity";
        lblCity.Size = new Size(31, 15);
        lblCity.TabIndex = 2;
        lblCity.Text = "City:";
        //
        // txtCity
        //
        txtCity.Location = new Point(10, 93);
        txtCity.Name = "txtCity";
        txtCity.Size = new Size(150, 23);
        txtCity.TabIndex = 2;
        //
        // lblState
        //
        lblState.AutoSize = true;
        lblState.Location = new Point(170, 75);
        lblState.Name = "lblState";
        lblState.Size = new Size(36, 15);
        lblState.TabIndex = 4;
        lblState.Text = "State:";
        //
        // txtState
        //
        txtState.Location = new Point(170, 93);
        txtState.Name = "txtState";
        txtState.Size = new Size(50, 23);
        txtState.TabIndex = 3;
        txtState.Text = "WA";
        //
        // lblZip
        //
        lblZip.AutoSize = true;
        lblZip.Location = new Point(230, 75);
        lblZip.Name = "lblZip";
        lblZip.Size = new Size(27, 15);
        lblZip.TabIndex = 6;
        lblZip.Text = "Zip:";
        //
        // txtZip
        //
        txtZip.Location = new Point(230, 93);
        txtZip.Name = "txtZip";
        txtZip.Size = new Size(80, 23);
        txtZip.TabIndex = 4;
        //
        // lblPhone
        //
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(320, 75);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(44, 15);
        lblPhone.TabIndex = 8;
        lblPhone.Text = "Phone:";
        //
        // txtPhone
        //
        txtPhone.Location = new Point(320, 93);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(110, 23);
        txtPhone.TabIndex = 5;
        //
        // lblEmail
        //
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(10, 120);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(39, 15);
        lblEmail.TabIndex = 10;
        lblEmail.Text = "Email:";
        //
        // txtEmail
        //
        txtEmail.Location = new Point(55, 117);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(375, 23);
        txtEmail.TabIndex = 6;
        //
        // grpMembers
        //
        grpMembers.Controls.Add(btnSetPrimary);
        grpMembers.Controls.Add(btnRemoveMember);
        grpMembers.Controls.Add(btnEditMember);
        grpMembers.Controls.Add(btnAddMember);
        grpMembers.Controls.Add(grdMembers);
        grpMembers.Location = new Point(12, 168);
        grpMembers.Name = "grpMembers";
        grpMembers.Size = new Size(450, 230);
        grpMembers.TabIndex = 1;
        grpMembers.TabStop = false;
        grpMembers.Text = "Household Members (at least one; one must be primary)";
        //
        // grdMembers
        //
        grdMembers.AllowUserToAddRows = false;
        grdMembers.AllowUserToDeleteRows = false;
        grdMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        grdMembers.Location = new Point(10, 25);
        grdMembers.MultiSelect = false;
        grdMembers.Name = "grdMembers";
        grdMembers.ReadOnly = true;
        grdMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grdMembers.Size = new Size(430, 150);
        grdMembers.TabIndex = 0;
        //
        // btnAddMember
        //
        btnAddMember.Location = new Point(10, 185);
        btnAddMember.Name = "btnAddMember";
        btnAddMember.Size = new Size(90, 30);
        btnAddMember.TabIndex = 1;
        btnAddMember.Text = "Add Member";
        btnAddMember.UseVisualStyleBackColor = true;
        btnAddMember.Click += BtnAddMember_Click;
        //
        // btnEditMember
        //
        btnEditMember.Location = new Point(106, 185);
        btnEditMember.Name = "btnEditMember";
        btnEditMember.Size = new Size(90, 30);
        btnEditMember.TabIndex = 2;
        btnEditMember.Text = "Edit";
        btnEditMember.UseVisualStyleBackColor = true;
        btnEditMember.Click += BtnEditMember_Click;
        //
        // btnRemoveMember
        //
        btnRemoveMember.Location = new Point(202, 185);
        btnRemoveMember.Name = "btnRemoveMember";
        btnRemoveMember.Size = new Size(90, 30);
        btnRemoveMember.TabIndex = 3;
        btnRemoveMember.Text = "Remove";
        btnRemoveMember.UseVisualStyleBackColor = true;
        btnRemoveMember.Click += BtnRemoveMember_Click;
        //
        // btnSetPrimary
        //
        btnSetPrimary.Location = new Point(298, 185);
        btnSetPrimary.Name = "btnSetPrimary";
        btnSetPrimary.Size = new Size(90, 30);
        btnSetPrimary.TabIndex = 4;
        btnSetPrimary.Text = "Set Primary";
        btnSetPrimary.UseVisualStyleBackColor = true;
        btnSetPrimary.Click += BtnSetPrimary_Click;
        //
        // lblNotes
        //
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 405);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(41, 15);
        lblNotes.TabIndex = 2;
        lblNotes.Text = "Notes:";
        //
        // txtNotes
        //
        txtNotes.Location = new Point(12, 423);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(450, 60);
        txtNotes.TabIndex = 2;
        //
        // btnSave
        //
        btnSave.Location = new Point(306, 495);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 4;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        //
        // btnCancel
        //
        btnCancel.Location = new Point(387, 495);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 5;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        //
        // lblError
        //
        lblError.AutoSize = true;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(12, 492);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 6;
        lblError.Visible = false;
        //
        // NewHouseholdForm
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(474, 537);
        Controls.Add(lblError);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(txtNotes);
        Controls.Add(lblNotes);
        Controls.Add(grpMembers);
        Controls.Add(grpContact);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "NewHouseholdForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "New Household";
        grpContact.ResumeLayout(false);
        grpContact.PerformLayout();
        grpMembers.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)grdMembers).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox grpContact;
    private TextBox txtAddress1;
    private Label lblAddress1;
    private TextBox txtCity;
    private Label lblCity;
    private TextBox txtState;
    private Label lblState;
    private TextBox txtZip;
    private Label lblZip;
    private TextBox txtPhone;
    private Label lblPhone;
    private TextBox txtEmail;
    private Label lblEmail;
    private GroupBox grpMembers;
    private DataGridView grdMembers;
    private Button btnAddMember;
    private Button btnEditMember;
    private Button btnRemoveMember;
    private Button btnSetPrimary;
    private Label lblNotes;
    private TextBox txtNotes;
    private Button btnSave;
    private Button btnCancel;
    private Label lblError;
}
