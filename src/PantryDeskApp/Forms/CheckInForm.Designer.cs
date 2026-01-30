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
        menuAdmin = new ToolStripMenuItem();
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
        menuStrip.Items.AddRange(new ToolStripItem[] { menuAdmin, menuItemLogout });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(800, 24);
        menuStrip.TabIndex = 6;
        menuStrip.Text = "menuStrip";
        // 
        // menuAdmin
        // 
        menuAdmin.DropDownItems.AddRange(new ToolStripItem[] { menuItemChangePasswords, menuItemPantryDays });
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
    private ToolStripMenuItem menuAdmin;
    private ToolStripMenuItem menuItemChangePasswords;
    private ToolStripMenuItem menuItemPantryDays;
    private ToolStripMenuItem menuItemLogout;
}
