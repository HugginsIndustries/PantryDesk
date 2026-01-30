namespace PantryDeskApp.Forms;

partial class ChangePasswordForm
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
        lblRole = new Label();
        cmbRole = new ComboBox();
        lblCurrentPassword = new Label();
        txtCurrentPassword = new TextBox();
        lblNewPassword = new Label();
        txtNewPassword = new TextBox();
        lblConfirmPassword = new Label();
        txtConfirmPassword = new TextBox();
        btnChangePassword = new Button();
        btnCancel = new Button();
        lblError = new Label();
        lblSuccess = new Label();
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
        lblTitle.Text = "Change Role Password";
        // 
        // lblRole
        // 
        lblRole.AutoSize = true;
        lblRole.Location = new Point(20, 60);
        lblRole.Name = "lblRole";
        lblRole.Size = new Size(65, 15);
        lblRole.TabIndex = 1;
        lblRole.Text = "Select Role:";
        // 
        // cmbRole
        // 
        cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRole.FormattingEnabled = true;
        cmbRole.Items.AddRange(new object[] { "Entry", "Admin" });
        cmbRole.Location = new Point(20, 78);
        cmbRole.Name = "cmbRole";
        cmbRole.Size = new Size(250, 23);
        cmbRole.TabIndex = 2;
        // 
        // lblCurrentPassword
        // 
        lblCurrentPassword.AutoSize = true;
        lblCurrentPassword.Location = new Point(20, 120);
        lblCurrentPassword.Name = "lblCurrentPassword";
        lblCurrentPassword.Size = new Size(100, 15);
        lblCurrentPassword.TabIndex = 3;
        lblCurrentPassword.Text = "Current Password:";
        // 
        // txtCurrentPassword
        // 
        txtCurrentPassword.Location = new Point(20, 138);
        txtCurrentPassword.Name = "txtCurrentPassword";
        txtCurrentPassword.Size = new Size(250, 23);
        txtCurrentPassword.TabIndex = 4;
        txtCurrentPassword.UseSystemPasswordChar = true;
        txtCurrentPassword.KeyDown += TxtPassword_KeyDown;
        // 
        // lblNewPassword
        // 
        lblNewPassword.AutoSize = true;
        lblNewPassword.Location = new Point(20, 180);
        lblNewPassword.Name = "lblNewPassword";
        lblNewPassword.Size = new Size(78, 15);
        lblNewPassword.TabIndex = 5;
        lblNewPassword.Text = "New Password:";
        // 
        // txtNewPassword
        // 
        txtNewPassword.Location = new Point(20, 198);
        txtNewPassword.Name = "txtNewPassword";
        txtNewPassword.Size = new Size(250, 23);
        txtNewPassword.TabIndex = 6;
        txtNewPassword.UseSystemPasswordChar = true;
        txtNewPassword.KeyDown += TxtPassword_KeyDown;
        // 
        // lblConfirmPassword
        // 
        lblConfirmPassword.AutoSize = true;
        lblConfirmPassword.Location = new Point(20, 240);
        lblConfirmPassword.Name = "lblConfirmPassword";
        lblConfirmPassword.Size = new Size(123, 15);
        lblConfirmPassword.TabIndex = 7;
        lblConfirmPassword.Text = "Confirm New Password:";
        // 
        // txtConfirmPassword
        // 
        txtConfirmPassword.Location = new Point(20, 258);
        txtConfirmPassword.Name = "txtConfirmPassword";
        txtConfirmPassword.Size = new Size(250, 23);
        txtConfirmPassword.TabIndex = 8;
        txtConfirmPassword.UseSystemPasswordChar = true;
        txtConfirmPassword.KeyDown += TxtPassword_KeyDown;
        // 
        // btnChangePassword
        // 
        btnChangePassword.Location = new Point(195, 310);
        btnChangePassword.Name = "btnChangePassword";
        btnChangePassword.Size = new Size(75, 30);
        btnChangePassword.TabIndex = 9;
        btnChangePassword.Text = "Change Password";
        btnChangePassword.UseVisualStyleBackColor = true;
        btnChangePassword.Click += BtnChangePassword_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(114, 310);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 10;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // lblError
        // 
        lblError.AutoSize = true;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(20, 290);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 11;
        lblError.Visible = false;
        // 
        // lblSuccess
        // 
        lblSuccess.AutoSize = true;
        lblSuccess.ForeColor = Color.Green;
        lblSuccess.Location = new Point(20, 290);
        lblSuccess.Name = "lblSuccess";
        lblSuccess.Size = new Size(0, 15);
        lblSuccess.TabIndex = 12;
        lblSuccess.Visible = false;
        // 
        // ChangePasswordForm
        // 
        AcceptButton = btnChangePassword;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(290, 360);
        Controls.Add(lblSuccess);
        Controls.Add(lblError);
        Controls.Add(btnCancel);
        Controls.Add(btnChangePassword);
        Controls.Add(txtConfirmPassword);
        Controls.Add(lblConfirmPassword);
        Controls.Add(txtNewPassword);
        Controls.Add(lblNewPassword);
        Controls.Add(txtCurrentPassword);
        Controls.Add(lblCurrentPassword);
        Controls.Add(cmbRole);
        Controls.Add(lblRole);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ChangePasswordForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Change Role Password";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitle;
    private Label lblRole;
    private ComboBox cmbRole;
    private Label lblCurrentPassword;
    private TextBox txtCurrentPassword;
    private Label lblNewPassword;
    private TextBox txtNewPassword;
    private Label lblConfirmPassword;
    private TextBox txtConfirmPassword;
    private Button btnChangePassword;
    private Button btnCancel;
    private Label lblError;
    private Label lblSuccess;
}
