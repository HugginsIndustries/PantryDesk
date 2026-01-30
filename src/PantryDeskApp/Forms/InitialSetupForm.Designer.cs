namespace PantryDeskApp.Forms;

partial class InitialSetupForm
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
        lblEntryPassword = new Label();
        txtEntryPassword = new TextBox();
        lblEntryPasswordConfirm = new Label();
        txtEntryPasswordConfirm = new TextBox();
        lblAdminPassword = new Label();
        txtAdminPassword = new TextBox();
        lblAdminPasswordConfirm = new Label();
        txtAdminPasswordConfirm = new TextBox();
        btnCompleteSetup = new Button();
        lblError = new Label();
        lblWarning = new Label();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(300, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "PantryDesk - Initial Setup";
        // 
        // lblEntryPassword
        // 
        lblEntryPassword.AutoSize = true;
        lblEntryPassword.Location = new Point(20, 60);
        lblEntryPassword.Name = "lblEntryPassword";
        lblEntryPassword.Size = new Size(88, 15);
        lblEntryPassword.TabIndex = 1;
        lblEntryPassword.Text = "Entry Password:";
        // 
        // txtEntryPassword
        // 
        txtEntryPassword.Location = new Point(20, 78);
        txtEntryPassword.Name = "txtEntryPassword";
        txtEntryPassword.Size = new Size(300, 23);
        txtEntryPassword.TabIndex = 2;
        txtEntryPassword.UseSystemPasswordChar = true;
        txtEntryPassword.KeyDown += TxtPassword_KeyDown;
        // 
        // lblEntryPasswordConfirm
        // 
        lblEntryPasswordConfirm.AutoSize = true;
        lblEntryPasswordConfirm.Location = new Point(20, 110);
        lblEntryPasswordConfirm.Name = "lblEntryPasswordConfirm";
        lblEntryPasswordConfirm.Size = new Size(133, 15);
        lblEntryPasswordConfirm.TabIndex = 3;
        lblEntryPasswordConfirm.Text = "Confirm Entry Password:";
        // 
        // txtEntryPasswordConfirm
        // 
        txtEntryPasswordConfirm.Location = new Point(20, 128);
        txtEntryPasswordConfirm.Name = "txtEntryPasswordConfirm";
        txtEntryPasswordConfirm.Size = new Size(300, 23);
        txtEntryPasswordConfirm.TabIndex = 4;
        txtEntryPasswordConfirm.UseSystemPasswordChar = true;
        txtEntryPasswordConfirm.KeyDown += TxtPassword_KeyDown;
        // 
        // lblAdminPassword
        // 
        lblAdminPassword.AutoSize = true;
        lblAdminPassword.Location = new Point(20, 170);
        lblAdminPassword.Name = "lblAdminPassword";
        lblAdminPassword.Size = new Size(95, 15);
        lblAdminPassword.TabIndex = 5;
        lblAdminPassword.Text = "Admin Password:";
        // 
        // txtAdminPassword
        // 
        txtAdminPassword.Location = new Point(20, 188);
        txtAdminPassword.Name = "txtAdminPassword";
        txtAdminPassword.Size = new Size(300, 23);
        txtAdminPassword.TabIndex = 6;
        txtAdminPassword.UseSystemPasswordChar = true;
        txtAdminPassword.KeyDown += TxtPassword_KeyDown;
        // 
        // lblAdminPasswordConfirm
        // 
        lblAdminPasswordConfirm.AutoSize = true;
        lblAdminPasswordConfirm.Location = new Point(20, 220);
        lblAdminPasswordConfirm.Name = "lblAdminPasswordConfirm";
        lblAdminPasswordConfirm.Size = new Size(140, 15);
        lblAdminPasswordConfirm.TabIndex = 7;
        lblAdminPasswordConfirm.Text = "Confirm Admin Password:";
        // 
        // txtAdminPasswordConfirm
        // 
        txtAdminPasswordConfirm.Location = new Point(20, 238);
        txtAdminPasswordConfirm.Name = "txtAdminPasswordConfirm";
        txtAdminPasswordConfirm.Size = new Size(300, 23);
        txtAdminPasswordConfirm.TabIndex = 8;
        txtAdminPasswordConfirm.UseSystemPasswordChar = true;
        txtAdminPasswordConfirm.KeyDown += TxtPassword_KeyDown;
        // 
        // btnCompleteSetup
        // 
        btnCompleteSetup.Location = new Point(245, 290);
        btnCompleteSetup.Name = "btnCompleteSetup";
        btnCompleteSetup.Size = new Size(75, 30);
        btnCompleteSetup.TabIndex = 9;
        btnCompleteSetup.Text = "Complete Setup";
        btnCompleteSetup.UseVisualStyleBackColor = true;
        btnCompleteSetup.Click += BtnCompleteSetup_Click;
        // 
        // lblError
        // 
        lblError.AutoSize = true;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(20, 270);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 10;
        lblError.Visible = false;
        // 
        // lblWarning
        // 
        lblWarning.AutoSize = true;
        lblWarning.ForeColor = Color.Orange;
        lblWarning.Location = new Point(20, 270);
        lblWarning.Name = "lblWarning";
        lblWarning.Size = new Size(0, 15);
        lblWarning.TabIndex = 11;
        lblWarning.Visible = false;
        // 
        // InitialSetupForm
        // 
        AcceptButton = btnCompleteSetup;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(340, 340);
        Controls.Add(lblWarning);
        Controls.Add(lblError);
        Controls.Add(btnCompleteSetup);
        Controls.Add(txtAdminPasswordConfirm);
        Controls.Add(lblAdminPasswordConfirm);
        Controls.Add(txtAdminPassword);
        Controls.Add(lblAdminPassword);
        Controls.Add(txtEntryPasswordConfirm);
        Controls.Add(lblEntryPasswordConfirm);
        Controls.Add(txtEntryPassword);
        Controls.Add(lblEntryPassword);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "InitialSetupForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PantryDesk - Initial Setup";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitle;
    private Label lblEntryPassword;
    private TextBox txtEntryPassword;
    private Label lblEntryPasswordConfirm;
    private TextBox txtEntryPasswordConfirm;
    private Label lblAdminPassword;
    private TextBox txtAdminPassword;
    private Label lblAdminPasswordConfirm;
    private TextBox txtAdminPasswordConfirm;
    private Button btnCompleteSetup;
    private Label lblError;
    private Label lblWarning;
}
