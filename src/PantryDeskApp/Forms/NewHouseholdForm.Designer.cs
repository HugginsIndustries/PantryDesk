namespace PantryDeskApp.Forms;

partial class NewHouseholdForm
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
        lblNotes = new Label();
        txtNotes = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        lblError = new Label();
        ((System.ComponentModel.ISupportInitialize)numChildren).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numAdults).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSeniors).BeginInit();
        SuspendLayout();
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
        txtPrimaryName.Size = new Size(350, 23);
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
        txtAddress1.Size = new Size(350, 23);
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
        txtState.Text = "WA";
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
        txtZip.Size = new Size(68, 23);
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
        lblEmail.Text = "Email:";
        // 
        // txtEmail
        // 
        txtEmail.Location = new Point(218, 198);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(144, 23);
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
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 290);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(41, 15);
        lblNotes.TabIndex = 20;
        lblNotes.Text = "Notes:";
        // 
        // txtNotes
        // 
        txtNotes.Location = new Point(12, 308);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(350, 80);
        txtNotes.TabIndex = 11;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(206, 400);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 12;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(287, 400);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 13;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // lblError
        // 
        lblError.AutoSize = true;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(12, 395);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 22;
        lblError.Visible = false;
        // 
        // NewHouseholdForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(374, 442);
        Controls.Add(lblError);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(txtNotes);
        Controls.Add(lblNotes);
        Controls.Add(numSeniors);
        Controls.Add(lblSeniors);
        Controls.Add(numAdults);
        Controls.Add(lblAdults);
        Controls.Add(numChildren);
        Controls.Add(lblChildren);
        Controls.Add(txtEmail);
        Controls.Add(lblEmail);
        Controls.Add(txtPhone);
        Controls.Add(lblPhone);
        Controls.Add(txtZip);
        Controls.Add(lblZip);
        Controls.Add(txtState);
        Controls.Add(lblState);
        Controls.Add(txtCity);
        Controls.Add(lblCity);
        Controls.Add(txtAddress1);
        Controls.Add(lblAddress1);
        Controls.Add(txtPrimaryName);
        Controls.Add(lblPrimaryName);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "NewHouseholdForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "New Household";
        ((System.ComponentModel.ISupportInitialize)numChildren).EndInit();
        ((System.ComponentModel.ISupportInitialize)numAdults).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSeniors).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

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
    private Label lblNotes;
    private TextBox txtNotes;
    private Button btnSave;
    private Button btnCancel;
    private Label lblError;
}
