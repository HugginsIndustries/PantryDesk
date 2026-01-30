namespace PantryDeskApp;

partial class Form1
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
        btnTest = new Button();
        menuStrip = new MenuStrip();
        menuAdmin = new ToolStripMenuItem();
        menuItemChangePasswords = new ToolStripMenuItem();
        menuItemLogout = new ToolStripMenuItem();
        menuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip
        // 
        menuStrip.Items.AddRange(new ToolStripItem[] { menuAdmin, menuItemLogout });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(800, 24);
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip1";
        // 
        // menuAdmin
        // 
        menuAdmin.DropDownItems.AddRange(new ToolStripItem[] { menuItemChangePasswords });
        menuAdmin.Name = "menuAdmin";
        menuAdmin.Size = new Size(55, 20);
        menuAdmin.Text = "Admin";
        // 
        // menuItemChangePasswords
        // 
        menuItemChangePasswords.Name = "menuItemChangePasswords";
        menuItemChangePasswords.Size = new Size(195, 22);
        menuItemChangePasswords.Text = "Change Role Passwords";
        menuItemChangePasswords.Click += MenuItemChangePasswords_Click;
        // 
        // menuItemLogout
        // 
        menuItemLogout.Name = "menuItemLogout";
        menuItemLogout.Size = new Size(57, 20);
        menuItemLogout.Text = "Logout";
        menuItemLogout.Click += MenuItemLogout_Click;
        // 
        // btnTest
        // 
        btnTest.Location = new Point(12, 35);
        btnTest.Name = "btnTest";
        btnTest.Size = new Size(200, 30);
        btnTest.TabIndex = 1;
        btnTest.Text = "Test Database (Phase 1)";
        btnTest.UseVisualStyleBackColor = true;
        btnTest.Click += BtnTest_Click;
        // 
        // Form1
        // 
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(btnTest);
        Controls.Add(menuStrip);
        MainMenuStrip = menuStrip;
        Text = "PantryDesk";
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private MenuStrip menuStrip;
    private ToolStripMenuItem menuAdmin;
    private ToolStripMenuItem menuItemChangePasswords;
    private ToolStripMenuItem menuItemLogout;
    private Button btnTest;

    #endregion
}
