using System.Drawing;

namespace PantryDeskApp.Forms;

partial class CheckInForm
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
        txtSearch = new TextBox();
        lblSearch = new Label();
        dgvResults = new DataGridView();
        btnCompleteService = new Button();
        btnNewHousehold = new Button();
        btnOpenProfile = new Button();
        menuStrip = new MenuStrip();
        menuReports = new ToolStripMenuItem();
        menuItemStatisticsDashboard = new ToolStripMenuItem();
        menuAdmin = new ToolStripMenuItem();
        menuItemBackupNow = new ToolStripMenuItem();
        menuItemBackupToUsb = new ToolStripMenuItem();
        menuItemRestore = new ToolStripMenuItem();
        menuItemExport = new ToolStripMenuItem();
        menuSeparator1 = new ToolStripSeparator();
        menuSeparator2 = new ToolStripSeparator();
        menuSeparator3 = new ToolStripSeparator();
        menuItemChangePasswords = new ToolStripMenuItem();
        menuItemPantryDays = new ToolStripMenuItem();
        menuItemLogout = new ToolStripMenuItem();
        ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
        menuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(12, 35);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(79, 15);
        lblSearch.TabIndex = 0;
        lblSearch.Text = "Search Name:";
        lblSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        // 
        // txtSearch
        // 
        txtSearch.Font = new Font("Segoe UI", 12F);
        txtSearch.Location = new Point(12, 53);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search by name...";
        txtSearch.Size = new Size(776, 29);
        txtSearch.TabIndex = 1;
        txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtSearch.TextChanged += TxtSearch_TextChanged;
        txtSearch.KeyDown += TxtSearch_KeyDown;
        // 
        // dgvResults
        // 
        dgvResults.AllowUserToAddRows = false;
        dgvResults.AllowUserToDeleteRows = false;
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvResults.Font = new Font("Segoe UI", 12F);
        dgvResults.Location = new Point(12, 88);
        dgvResults.MultiSelect = false;
        dgvResults.Name = "dgvResults";
        dgvResults.ReadOnly = true;
        dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvResults.Size = new Size(776, 350);
        dgvResults.TabIndex = 2;
        dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvResults.SelectionChanged += DgvResults_SelectionChanged;
        // 
        // btnCompleteService
        // 
        btnCompleteService.Enabled = false;
        btnCompleteService.Location = new Point(12, 444);
        btnCompleteService.Name = "btnCompleteService";
        btnCompleteService.Size = new Size(150, 35);
        btnCompleteService.TabIndex = 3;
        btnCompleteService.Text = "Complete Service";
        btnCompleteService.UseVisualStyleBackColor = true;
        btnCompleteService.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCompleteService.Click += BtnCompleteService_Click;
        // 
        // btnNewHousehold
        // 
        btnNewHousehold.Location = new Point(168, 444);
        btnNewHousehold.Name = "btnNewHousehold";
        btnNewHousehold.Size = new Size(150, 35);
        btnNewHousehold.TabIndex = 4;
        btnNewHousehold.Text = "New Household";
        btnNewHousehold.UseVisualStyleBackColor = true;
        btnNewHousehold.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnNewHousehold.Click += BtnNewHousehold_Click;
        // 
        // btnOpenProfile
        // 
        btnOpenProfile.Enabled = false;
        btnOpenProfile.Location = new Point(324, 444);
        btnOpenProfile.Name = "btnOpenProfile";
        btnOpenProfile.Size = new Size(150, 35);
        btnOpenProfile.TabIndex = 5;
        btnOpenProfile.Text = "Open Profile";
        btnOpenProfile.UseVisualStyleBackColor = true;
        btnOpenProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnOpenProfile.Click += BtnOpenProfile_Click;
        // 
        // menuStrip
        // 
        menuStrip.Items.AddRange(new ToolStripItem[] { menuReports, menuAdmin, menuItemLogout });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(800, 24);
        menuStrip.TabIndex = 6;
        menuStrip.Text = "menuStrip";
        // 
        // menuReports
        // 
        menuReports.DropDownItems.AddRange(new ToolStripItem[] { menuItemStatisticsDashboard });
        menuReports.Name = "menuReports";
        menuReports.Size = new Size(59, 20);
        menuReports.Text = "Reports";
        // 
        // menuItemStatisticsDashboard
        // 
        menuItemStatisticsDashboard.Name = "menuItemStatisticsDashboard";
        menuItemStatisticsDashboard.Size = new Size(195, 22);
        menuItemStatisticsDashboard.Text = "Statistics Dashboard";
        menuItemStatisticsDashboard.Click += MenuItemStatisticsDashboard_Click;
        // 
        // menuAdmin
        // 
        menuAdmin.DropDownItems.AddRange(new ToolStripItem[] { menuSeparator1, menuItemBackupNow, menuItemBackupToUsb, menuSeparator2, menuItemRestore, menuSeparator3, menuItemExport, menuItemChangePasswords, menuItemPantryDays });
        menuAdmin.Name = "menuAdmin";
        menuAdmin.Size = new Size(55, 20);
        menuAdmin.Text = "Admin";
        // 
        // menuSeparator1
        // 
        menuSeparator1.Name = "menuSeparator1";
        menuSeparator1.Size = new Size(177, 6);
        // 
        // menuItemBackupNow
        // 
        menuItemBackupNow.Name = "menuItemBackupNow";
        menuItemBackupNow.Size = new Size(180, 22);
        menuItemBackupNow.Text = "Backup Now";
        menuItemBackupNow.Click += MenuItemBackupNow_Click;
        // 
        // menuItemBackupToUsb
        // 
        menuItemBackupToUsb.Name = "menuItemBackupToUsb";
        menuItemBackupToUsb.Size = new Size(180, 22);
        menuItemBackupToUsb.Text = "Backup to USB...";
        menuItemBackupToUsb.Click += MenuItemBackupToUsb_Click;
        // 
        // menuSeparator2
        // 
        menuSeparator2.Name = "menuSeparator2";
        menuSeparator2.Size = new Size(177, 6);
        // 
        // menuItemRestore
        // 
        menuItemRestore.Name = "menuItemRestore";
        menuItemRestore.Size = new Size(180, 22);
        menuItemRestore.Text = "Restore from Backup...";
        menuItemRestore.Click += MenuItemRestore_Click;
        // 
        // menuSeparator3
        // 
        menuSeparator3.Name = "menuSeparator3";
        menuSeparator3.Size = new Size(177, 6);
        // 
        // menuItemExport
        // 
        menuItemExport.Name = "menuItemExport";
        menuItemExport.Size = new Size(180, 22);
        menuItemExport.Text = "Export Data...";
        menuItemExport.Click += MenuItemExport_Click;
        // 
        // menuItemChangePasswords
        // 
        menuItemChangePasswords.Name = "menuItemChangePasswords";
        menuItemChangePasswords.Size = new Size(195, 22);
        menuItemChangePasswords.Text = "Change Role Passwords";
        menuItemChangePasswords.Click += MenuItemChangePasswords_Click;
        // 
        // menuItemPantryDays
        // 
        menuItemPantryDays.Name = "menuItemPantryDays";
        menuItemPantryDays.Size = new Size(195, 22);
        menuItemPantryDays.Text = "Pantry Days";
        menuItemPantryDays.Click += MenuItemPantryDays_Click;
        // 
        // menuItemLogout
        // 
        menuItemLogout.Name = "menuItemLogout";
        menuItemLogout.Size = new Size(57, 20);
        menuItemLogout.Text = "Logout";
        menuItemLogout.Click += MenuItemLogout_Click;
        // 
        // CheckInForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 500);
        MinimumSize = new Size(600, 400);
        Controls.Add(btnOpenProfile);
        Controls.Add(btnNewHousehold);
        Controls.Add(btnCompleteService);
        Controls.Add(dgvResults);
        Controls.Add(txtSearch);
        Controls.Add(lblSearch);
        Controls.Add(menuStrip);
        MainMenuStrip = menuStrip;
        Name = "CheckInForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PantryDesk - Check-In";
        Load += CheckInForm_Load;
        ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblSearch;
    private TextBox txtSearch;
    private DataGridView dgvResults;
    private Button btnCompleteService;
    private Button btnNewHousehold;
    private Button btnOpenProfile;
    private MenuStrip menuStrip;
    private ToolStripMenuItem menuReports;
    private ToolStripMenuItem menuItemStatisticsDashboard;
    private ToolStripMenuItem menuAdmin;
    private ToolStripMenuItem menuItemBackupNow;
    private ToolStripMenuItem menuItemBackupToUsb;
    private ToolStripMenuItem menuItemRestore;
    private ToolStripMenuItem menuItemExport;
    private ToolStripSeparator menuSeparator1;
    private ToolStripSeparator menuSeparator2;
    private ToolStripSeparator menuSeparator3;
    private ToolStripMenuItem menuItemChangePasswords;
    private ToolStripMenuItem menuItemPantryDays;
    private ToolStripMenuItem menuItemLogout;
}
