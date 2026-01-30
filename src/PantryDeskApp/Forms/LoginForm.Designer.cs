namespace PantryDeskApp.Forms;

partial class LoginForm
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
        lblPassword = new Label();
        txtPassword = new TextBox();
        btnLogin = new Button();
        btnCancel = new Button();
        lblError = new Label();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(133, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "PantryDesk - Login";
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
        cmbRole.Size = new Size(200, 23);
        cmbRole.TabIndex = 2;
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(20, 120);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(60, 15);
        lblPassword.TabIndex = 3;
        lblPassword.Text = "Password:";
        // 
        // txtPassword
        // 
        txtPassword.Location = new Point(20, 138);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(200, 23);
        txtPassword.TabIndex = 4;
        txtPassword.UseSystemPasswordChar = true;
        txtPassword.KeyDown += TxtPassword_KeyDown;
        // 
        // btnLogin
        // 
        btnLogin.Location = new Point(145, 180);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(75, 30);
        btnLogin.TabIndex = 5;
        btnLogin.Text = "Login";
        btnLogin.UseVisualStyleBackColor = true;
        btnLogin.Click += BtnLogin_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(64, 180);
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
        lblError.Location = new Point(20, 165);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 7;
        lblError.Visible = false;
        // 
        // LoginForm
        // 
        AcceptButton = btnLogin;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(250, 230);
        Controls.Add(lblError);
        Controls.Add(btnCancel);
        Controls.Add(btnLogin);
        Controls.Add(txtPassword);
        Controls.Add(lblPassword);
        Controls.Add(cmbRole);
        Controls.Add(lblRole);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PantryDesk - Login";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitle;
    private Label lblRole;
    private ComboBox cmbRole;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnLogin;
    private Button btnCancel;
    private Label lblError;
}
