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
        dgvResults = new DataGridView();
        btnAppointments = new Button();
        btnCompleteService = new Button();
        btnNewHousehold = new Button();
        btnOpenProfile = new Button();
        panelTopRow = new TableLayoutPanel();
        menuStrip = new MenuStrip();
        menuForms = new ToolStripMenuItem();
        menuItemRegistration = new ToolStripMenuItem();
        menuItemDeckSignIn = new ToolStripMenuItem();
        menuReports = new ToolStripMenuItem();
        menuItemEnterDeckStats = new ToolStripMenuItem();
        menuItemMonthlyActivityReport = new ToolStripMenuItem();
        menuItemStatisticsDashboard = new ToolStripMenuItem();
        menuAdmin = new ToolStripMenuItem();
        menuItemBackupToUsb = new ToolStripMenuItem();
        menuItemRestore = new ToolStripMenuItem();
        menuItemExport = new ToolStripMenuItem();
        menuSeparator1 = new ToolStripSeparator();
        menuSeparator2 = new ToolStripSeparator();
        menuSeparator3 = new ToolStripSeparator();
        menuItemChangePasswords = new ToolStripMenuItem();
        menuItemPantryDays = new ToolStripMenuItem();
        menuItemActiveStatusSettings = new ToolStripMenuItem();
        menuItemLogout = new ToolStripMenuItem();
        statusStrip = new StatusStrip();
        statusLabelRole = new ToolStripStatusLabel();
        statusLabelBackup = new ToolStripStatusLabel();
        ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
        menuStrip.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // panelTopRow
        // 
        panelTopRow.ColumnCount = 5;
        panelTopRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        panelTopRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        panelTopRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        panelTopRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        panelTopRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        panelTopRow.Controls.Add(txtSearch, 0, 0);
        panelTopRow.Controls.Add(btnAppointments, 1, 0);
        panelTopRow.Controls.Add(btnCompleteService, 2, 0);
        panelTopRow.Controls.Add(btnNewHousehold, 3, 0);
        panelTopRow.Controls.Add(btnOpenProfile, 4, 0);
        panelTopRow.Dock = DockStyle.Top;
        panelTopRow.Location = new Point(0, 24);
        panelTopRow.Name = "panelTopRow";
        panelTopRow.Padding = new Padding(12, 8, 12, 4);
        panelTopRow.RowCount = 1;
        panelTopRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        panelTopRow.Size = new Size(800, 50);
        panelTopRow.TabIndex = 0;
        // 
        // txtSearch
        // 
        txtSearch.Dock = DockStyle.Fill;
        txtSearch.Font = new Font("Segoe UI", 12F);
        txtSearch.Location = new Point(15, 11);
        txtSearch.Margin = new Padding(3, 3, 6, 3);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search by name...";
        txtSearch.Size = new Size(614, 29);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += TxtSearch_TextChanged;
        txtSearch.KeyDown += TxtSearch_KeyDown;
        // 
        // btnAppointments
        // 
        btnAppointments.Dock = DockStyle.Fill;
        btnAppointments.Location = new Point(520, 11);
        btnAppointments.Margin = new Padding(6, 3, 3, 3);
        btnAppointments.Name = "btnAppointments";
        btnAppointments.Size = new Size(109, 32);
        btnAppointments.TabIndex = 1;
        btnAppointments.Text = "Appointments";
        btnAppointments.UseVisualStyleBackColor = true;
        btnAppointments.Click += BtnAppointments_Click;
        // 
        // btnCompleteService
        // 
        btnCompleteService.Dock = DockStyle.Fill;
        btnCompleteService.Enabled = false;
        btnCompleteService.Location = new Point(635, 11);
        btnCompleteService.Margin = new Padding(6, 3, 3, 3);
        btnCompleteService.Name = "btnCompleteService";
        btnCompleteService.Size = new Size(144, 32);
        btnCompleteService.TabIndex = 1;
        btnCompleteService.Text = "Complete Service";
        btnCompleteService.UseVisualStyleBackColor = true;
        btnCompleteService.Click += BtnCompleteService_Click;
        // 
        // btnNewHousehold
        // 
        btnNewHousehold.Dock = DockStyle.Fill;
        btnNewHousehold.Location = new Point(788, 11);
        btnNewHousehold.Margin = new Padding(6, 3, 3, 3);
        btnNewHousehold.Name = "btnNewHousehold";
        btnNewHousehold.Size = new Size(144, 32);
        btnNewHousehold.TabIndex = 2;
        btnNewHousehold.Text = "New Household";
        btnNewHousehold.UseVisualStyleBackColor = true;
        btnNewHousehold.Click += BtnNewHousehold_Click;
        // 
        // btnOpenProfile
        // 
        btnOpenProfile.Dock = DockStyle.Fill;
        btnOpenProfile.Enabled = false;
        btnOpenProfile.Location = new Point(941, 11);
        btnOpenProfile.Margin = new Padding(6, 3, 3, 3);
        btnOpenProfile.Name = "btnOpenProfile";
        btnOpenProfile.Size = new Size(144, 32);
        btnOpenProfile.TabIndex = 3;
        btnOpenProfile.Text = "Open Profile";
        btnOpenProfile.UseVisualStyleBackColor = true;
        btnOpenProfile.Click += BtnOpenProfile_Click;
        // 
        // dgvResults
        // 
        dgvResults.AllowUserToAddRows = false;
        dgvResults.AllowUserToDeleteRows = false;
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvResults.Dock = DockStyle.Fill;
        dgvResults.Font = new Font("Segoe UI", 12F);
        dgvResults.MultiSelect = false;
        dgvResults.Name = "dgvResults";
        dgvResults.ReadOnly = true;
        dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvResults.TabIndex = 2;
        dgvResults.SelectionChanged += DgvResults_SelectionChanged;
        // 
        // menuStrip
        // 
        menuStrip.Items.AddRange(new ToolStripItem[] { menuForms, menuReports, menuAdmin, menuItemLogout });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(800, 24);
        menuStrip.TabIndex = 6;
        menuStrip.Text = "menuStrip";
        // 
        // menuForms
        // 
        menuForms.DropDownItems.AddRange(new ToolStripItem[] { menuItemRegistration, menuItemDeckSignIn });
        menuForms.Name = "menuForms";
        menuForms.Size = new Size(50, 20);
        menuForms.Text = "Forms";
        // 
        // menuItemRegistration
        // 
        menuItemRegistration.Name = "menuItemRegistration";
        menuItemRegistration.Size = new Size(150, 22);
        menuItemRegistration.Text = "Registration";
        menuItemRegistration.Click += MenuItemRegistration_Click;
        // 
        // menuItemDeckSignIn
        // 
        menuItemDeckSignIn.Name = "menuItemDeckSignIn";
        menuItemDeckSignIn.Size = new Size(150, 22);
        menuItemDeckSignIn.Text = "Deck Sign In";
        menuItemDeckSignIn.Click += MenuItemDeckSignIn_Click;
        // 
        // menuReports
        // 
        menuReports.DropDownItems.AddRange(new ToolStripItem[] { menuItemEnterDeckStats, menuItemMonthlyActivityReport, menuItemStatisticsDashboard });
        menuReports.Name = "menuReports";
        menuReports.Size = new Size(59, 20);
        menuReports.Text = "Reports";
        // 
        // menuItemEnterDeckStats
        // 
        menuItemEnterDeckStats.Name = "menuItemEnterDeckStats";
        menuItemEnterDeckStats.Size = new Size(195, 22);
        menuItemEnterDeckStats.Text = "Enter Deck Stats";
        menuItemEnterDeckStats.Click += MenuItemEnterDeckStats_Click;
        // 
        // menuItemMonthlyActivityReport
        // 
        menuItemMonthlyActivityReport.Name = "menuItemMonthlyActivityReport";
        menuItemMonthlyActivityReport.Size = new Size(195, 22);
        menuItemMonthlyActivityReport.Text = "Monthly Activity Report";
        menuItemMonthlyActivityReport.Click += MenuItemMonthlyActivityReport_Click;
        // 
        // menuItemStatisticsDashboard
        // 
        menuItemStatisticsDashboard.Name = "menuItemStatisticsDashboard";
        menuItemStatisticsDashboard.Size = new Size(195, 22);
        menuItemStatisticsDashboard.Text = "Yearly Statistics";
        menuItemStatisticsDashboard.Click += MenuItemStatisticsDashboard_Click;
        // 
        // menuAdmin
        // 
        menuAdmin.DropDownItems.AddRange(new ToolStripItem[] { menuSeparator1, menuItemBackupToUsb, menuSeparator2, menuItemRestore, menuSeparator3, menuItemExport, menuItemChangePasswords, menuItemPantryDays, menuItemActiveStatusSettings });
        menuAdmin.Name = "menuAdmin";
        menuAdmin.Size = new Size(55, 20);
        menuAdmin.Text = "Admin";
        // 
        // menuSeparator1
        // 
        menuSeparator1.Name = "menuSeparator1";
        menuSeparator1.Size = new Size(177, 6);
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
        // menuItemActiveStatusSettings
        // 
        menuItemActiveStatusSettings.Name = "menuItemActiveStatusSettings";
        menuItemActiveStatusSettings.Size = new Size(195, 22);
        menuItemActiveStatusSettings.Text = "Active Status Reset Date...";
        menuItemActiveStatusSettings.Click += MenuItemActiveStatusSettings_Click;
        // 
        // menuItemLogout
        // 
        menuItemLogout.Name = "menuItemLogout";
        menuItemLogout.Size = new Size(57, 20);
        menuItemLogout.Text = "Switch Role";
        menuItemLogout.Click += MenuItemSwitchRole_Click;
        // 
        // statusStrip
        // 
        statusStrip.Dock = DockStyle.Bottom;
        statusStrip.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabelRole, statusLabelBackup });
        statusStrip.Location = new Point(0, 478);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(800, 22);
        statusStrip.TabIndex = 7;
        statusStrip.Text = "statusStrip";
        // 
        // statusLabelRole
        // 
        statusLabelRole.Name = "statusLabelRole";
        statusLabelRole.Size = new Size(42, 17);
        statusLabelRole.Text = "Entry";
        // 
        // statusLabelBackup
        // 
        statusLabelBackup.Name = "statusLabelBackup";
        statusLabelBackup.Size = new Size(743, 17);
        statusLabelBackup.Spring = true;
        statusLabelBackup.Text = "Last Auto Backup: No backup yet  Last Manual Backup: No backup yet";
        statusLabelBackup.TextAlign = ContentAlignment.MiddleRight;
        // 
        // CheckInForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1600, 750);
        MinimumSize = new Size(600, 400);
        Controls.Add(dgvResults);
        Controls.Add(panelTopRow);
        Controls.Add(menuStrip);
        Controls.Add(statusStrip);
        MainMenuStrip = menuStrip;
        Name = "CheckInForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PantryDesk - Check-In";
        Load += CheckInForm_Load;
        ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel panelTopRow;
    private TextBox txtSearch;
    private DataGridView dgvResults;
    private Button btnAppointments;
    private Button btnCompleteService;
    private Button btnNewHousehold;
    private Button btnOpenProfile;
    private MenuStrip menuStrip;
    private ToolStripMenuItem menuForms;
    private ToolStripMenuItem menuItemRegistration;
    private ToolStripMenuItem menuItemDeckSignIn;
    private ToolStripMenuItem menuReports;
    private ToolStripMenuItem menuItemEnterDeckStats;
    private ToolStripMenuItem menuItemMonthlyActivityReport;
    private ToolStripMenuItem menuItemStatisticsDashboard;
    private ToolStripMenuItem menuAdmin;
    private ToolStripMenuItem menuItemBackupToUsb;
    private ToolStripMenuItem menuItemRestore;
    private ToolStripMenuItem menuItemExport;
    private ToolStripSeparator menuSeparator1;
    private ToolStripSeparator menuSeparator2;
    private ToolStripSeparator menuSeparator3;
    private ToolStripMenuItem menuItemChangePasswords;
    private ToolStripMenuItem menuItemPantryDays;
    private ToolStripMenuItem menuItemActiveStatusSettings;
    private ToolStripMenuItem menuItemLogout;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabelRole;
    private ToolStripStatusLabel statusLabelBackup;
}
