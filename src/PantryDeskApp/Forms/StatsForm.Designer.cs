namespace PantryDeskApp.Forms;

partial class StatsForm
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
        grpMainStats = new GroupBox();
        lblTotalActiveHouseholds = new Label();
        txtTotalActiveHouseholds = new TextBox();
        lblTotalPeople = new Label();
        txtTotalPeople = new TextBox();
        lblCompletedServices = new Label();
        txtCompletedServices = new TextBox();
        lblUniqueHouseholdsServed = new Label();
        txtUniqueHouseholdsServed = new TextBox();
        lblPantryDayCompletions = new Label();
        txtPantryDayCompletions = new TextBox();
        lblAppointmentCompletions = new Label();
        txtAppointmentCompletions = new TextBox();
        lblOverridesCount = new Label();
        txtOverridesCount = new TextBox();
        grpCityBreakdown = new GroupBox();
        dgvCityBreakdown = new DataGridView();
        grpOverrideBreakdown = new GroupBox();
        dgvOverrideBreakdown = new DataGridView();
        btnRefresh = new Button();
        btnMonthlySummary = new Button();
        grpMainStats.SuspendLayout();
        grpCityBreakdown.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCityBreakdown).BeginInit();
        grpOverrideBreakdown.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvOverrideBreakdown).BeginInit();
        SuspendLayout();
        // 
        // grpMainStats
        // 
        grpMainStats.Controls.Add(lblTotalActiveHouseholds);
        grpMainStats.Controls.Add(txtTotalActiveHouseholds);
        grpMainStats.Controls.Add(lblTotalPeople);
        grpMainStats.Controls.Add(txtTotalPeople);
        grpMainStats.Controls.Add(lblCompletedServices);
        grpMainStats.Controls.Add(txtCompletedServices);
        grpMainStats.Controls.Add(lblUniqueHouseholdsServed);
        grpMainStats.Controls.Add(txtUniqueHouseholdsServed);
        grpMainStats.Controls.Add(lblPantryDayCompletions);
        grpMainStats.Controls.Add(txtPantryDayCompletions);
        grpMainStats.Controls.Add(lblAppointmentCompletions);
        grpMainStats.Controls.Add(txtAppointmentCompletions);
        grpMainStats.Controls.Add(lblOverridesCount);
        grpMainStats.Controls.Add(txtOverridesCount);
        grpMainStats.Location = new Point(12, 12);
        grpMainStats.Name = "grpMainStats";
        grpMainStats.Size = new Size(400, 280);
        grpMainStats.TabIndex = 0;
        grpMainStats.TabStop = false;
        grpMainStats.Text = "Current Month Statistics";
        // 
        // lblTotalActiveHouseholds
        // 
        lblTotalActiveHouseholds.AutoSize = true;
        lblTotalActiveHouseholds.Location = new Point(12, 25);
        lblTotalActiveHouseholds.Name = "lblTotalActiveHouseholds";
        lblTotalActiveHouseholds.Size = new Size(140, 15);
        lblTotalActiveHouseholds.TabIndex = 0;
        lblTotalActiveHouseholds.Text = "Total Active Households:";
        // 
        // txtTotalActiveHouseholds
        // 
        txtTotalActiveHouseholds.Location = new Point(158, 22);
        txtTotalActiveHouseholds.Name = "txtTotalActiveHouseholds";
        txtTotalActiveHouseholds.ReadOnly = true;
        txtTotalActiveHouseholds.Size = new Size(150, 23);
        txtTotalActiveHouseholds.TabIndex = 1;
        txtTotalActiveHouseholds.TextAlign = HorizontalAlignment.Right;
        // 
        // lblTotalPeople
        // 
        lblTotalPeople.AutoSize = true;
        lblTotalPeople.Location = new Point(12, 55);
        lblTotalPeople.Name = "lblTotalPeople";
        lblTotalPeople.Size = new Size(80, 15);
        lblTotalPeople.TabIndex = 2;
        lblTotalPeople.Text = "Total People:";
        // 
        // txtTotalPeople
        // 
        txtTotalPeople.Location = new Point(158, 52);
        txtTotalPeople.Name = "txtTotalPeople";
        txtTotalPeople.ReadOnly = true;
        txtTotalPeople.Size = new Size(150, 23);
        txtTotalPeople.TabIndex = 3;
        txtTotalPeople.TextAlign = HorizontalAlignment.Right;
        // 
        // lblCompletedServices
        // 
        lblCompletedServices.AutoSize = true;
        lblCompletedServices.Location = new Point(12, 85);
        lblCompletedServices.Name = "lblCompletedServices";
        lblCompletedServices.Size = new Size(115, 15);
        lblCompletedServices.TabIndex = 4;
        lblCompletedServices.Text = "Completed Services:";
        // 
        // txtCompletedServices
        // 
        txtCompletedServices.Location = new Point(158, 82);
        txtCompletedServices.Name = "txtCompletedServices";
        txtCompletedServices.ReadOnly = true;
        txtCompletedServices.Size = new Size(150, 23);
        txtCompletedServices.TabIndex = 5;
        txtCompletedServices.TextAlign = HorizontalAlignment.Right;
        // 
        // lblUniqueHouseholdsServed
        // 
        lblUniqueHouseholdsServed.AutoSize = true;
        lblUniqueHouseholdsServed.Location = new Point(12, 115);
        lblUniqueHouseholdsServed.Name = "lblUniqueHouseholdsServed";
        lblUniqueHouseholdsServed.Size = new Size(150, 15);
        lblUniqueHouseholdsServed.TabIndex = 6;
        lblUniqueHouseholdsServed.Text = "Unique Households Served:";
        // 
        // txtUniqueHouseholdsServed
        // 
        txtUniqueHouseholdsServed.Location = new Point(168, 112);
        txtUniqueHouseholdsServed.Name = "txtUniqueHouseholdsServed";
        txtUniqueHouseholdsServed.ReadOnly = true;
        txtUniqueHouseholdsServed.Size = new Size(140, 23);
        txtUniqueHouseholdsServed.TabIndex = 7;
        txtUniqueHouseholdsServed.TextAlign = HorizontalAlignment.Right;
        // 
        // lblPantryDayCompletions
        // 
        lblPantryDayCompletions.AutoSize = true;
        lblPantryDayCompletions.Location = new Point(12, 145);
        lblPantryDayCompletions.Name = "lblPantryDayCompletions";
        lblPantryDayCompletions.Size = new Size(135, 15);
        lblPantryDayCompletions.TabIndex = 8;
        lblPantryDayCompletions.Text = "PantryDay Completions:";
        // 
        // txtPantryDayCompletions
        // 
        txtPantryDayCompletions.Location = new Point(158, 142);
        txtPantryDayCompletions.Name = "txtPantryDayCompletions";
        txtPantryDayCompletions.ReadOnly = true;
        txtPantryDayCompletions.Size = new Size(150, 23);
        txtPantryDayCompletions.TabIndex = 9;
        txtPantryDayCompletions.TextAlign = HorizontalAlignment.Right;
        // 
        // lblAppointmentCompletions
        // 
        lblAppointmentCompletions.AutoSize = true;
        lblAppointmentCompletions.Location = new Point(12, 175);
        lblAppointmentCompletions.Name = "lblAppointmentCompletions";
        lblAppointmentCompletions.Size = new Size(145, 15);
        lblAppointmentCompletions.TabIndex = 10;
        lblAppointmentCompletions.Text = "Appointment Completions:";
        // 
        // txtAppointmentCompletions
        // 
        txtAppointmentCompletions.Location = new Point(163, 172);
        txtAppointmentCompletions.Name = "txtAppointmentCompletions";
        txtAppointmentCompletions.ReadOnly = true;
        txtAppointmentCompletions.Size = new Size(145, 23);
        txtAppointmentCompletions.TabIndex = 11;
        txtAppointmentCompletions.TextAlign = HorizontalAlignment.Right;
        // 
        // lblOverridesCount
        // 
        lblOverridesCount.AutoSize = true;
        lblOverridesCount.Location = new Point(12, 205);
        lblOverridesCount.Name = "lblOverridesCount";
        lblOverridesCount.Size = new Size(95, 15);
        lblOverridesCount.TabIndex = 12;
        lblOverridesCount.Text = "Overrides Count:";
        // 
        // txtOverridesCount
        // 
        txtOverridesCount.Location = new Point(158, 202);
        txtOverridesCount.Name = "txtOverridesCount";
        txtOverridesCount.ReadOnly = true;
        txtOverridesCount.Size = new Size(150, 23);
        txtOverridesCount.TabIndex = 13;
        txtOverridesCount.TextAlign = HorizontalAlignment.Right;
        // 
        // grpCityBreakdown
        // 
        grpCityBreakdown.Controls.Add(dgvCityBreakdown);
        grpCityBreakdown.Location = new Point(418, 12);
        grpCityBreakdown.Name = "grpCityBreakdown";
        grpCityBreakdown.Size = new Size(370, 200);
        grpCityBreakdown.TabIndex = 1;
        grpCityBreakdown.TabStop = false;
        grpCityBreakdown.Text = "By City";
        // 
        // dgvCityBreakdown
        // 
        dgvCityBreakdown.AllowUserToAddRows = false;
        dgvCityBreakdown.AllowUserToDeleteRows = false;
        dgvCityBreakdown.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCityBreakdown.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCityBreakdown.Dock = DockStyle.Fill;
        dgvCityBreakdown.Location = new Point(3, 19);
        dgvCityBreakdown.MultiSelect = false;
        dgvCityBreakdown.Name = "dgvCityBreakdown";
        dgvCityBreakdown.ReadOnly = true;
        dgvCityBreakdown.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCityBreakdown.Size = new Size(364, 178);
        dgvCityBreakdown.TabIndex = 0;
        // 
        // grpOverrideBreakdown
        // 
        grpOverrideBreakdown.Controls.Add(dgvOverrideBreakdown);
        grpOverrideBreakdown.Location = new Point(418, 218);
        grpOverrideBreakdown.Name = "grpOverrideBreakdown";
        grpOverrideBreakdown.Size = new Size(370, 200);
        grpOverrideBreakdown.TabIndex = 2;
        grpOverrideBreakdown.TabStop = false;
        grpOverrideBreakdown.Text = "Override Breakdown";
        // 
        // dgvOverrideBreakdown
        // 
        dgvOverrideBreakdown.AllowUserToAddRows = false;
        dgvOverrideBreakdown.AllowUserToDeleteRows = false;
        dgvOverrideBreakdown.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvOverrideBreakdown.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvOverrideBreakdown.Dock = DockStyle.Fill;
        dgvOverrideBreakdown.Location = new Point(3, 19);
        dgvOverrideBreakdown.MultiSelect = false;
        dgvOverrideBreakdown.Name = "dgvOverrideBreakdown";
        dgvOverrideBreakdown.ReadOnly = true;
        dgvOverrideBreakdown.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvOverrideBreakdown.Size = new Size(364, 178);
        dgvOverrideBreakdown.TabIndex = 0;
        // 
        // btnRefresh
        // 
        btnRefresh.Location = new Point(12, 300);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(150, 35);
        btnRefresh.TabIndex = 3;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += BtnRefresh_Click;
        // 
        // btnMonthlySummary
        // 
        btnMonthlySummary.Location = new Point(168, 300);
        btnMonthlySummary.Name = "btnMonthlySummary";
        btnMonthlySummary.Size = new Size(150, 35);
        btnMonthlySummary.TabIndex = 4;
        btnMonthlySummary.Text = "Monthly Summary";
        btnMonthlySummary.UseVisualStyleBackColor = true;
        btnMonthlySummary.Click += BtnMonthlySummary_Click;
        // 
        // StatsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 350);
        MinimumSize = new Size(600, 350);
        Controls.Add(btnMonthlySummary);
        Controls.Add(btnRefresh);
        Controls.Add(grpOverrideBreakdown);
        Controls.Add(grpCityBreakdown);
        Controls.Add(grpMainStats);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "StatsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Statistics Dashboard";
        Load += StatsForm_Load;
        grpMainStats.ResumeLayout(false);
        grpMainStats.PerformLayout();
        grpCityBreakdown.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvCityBreakdown).EndInit();
        grpOverrideBreakdown.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvOverrideBreakdown).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpMainStats;
    private Label lblTotalActiveHouseholds;
    private TextBox txtTotalActiveHouseholds;
    private Label lblTotalPeople;
    private TextBox txtTotalPeople;
    private Label lblCompletedServices;
    private TextBox txtCompletedServices;
    private Label lblUniqueHouseholdsServed;
    private TextBox txtUniqueHouseholdsServed;
    private Label lblPantryDayCompletions;
    private TextBox txtPantryDayCompletions;
    private Label lblAppointmentCompletions;
    private TextBox txtAppointmentCompletions;
    private Label lblOverridesCount;
    private TextBox txtOverridesCount;
    private GroupBox grpCityBreakdown;
    private DataGridView dgvCityBreakdown;
    private GroupBox grpOverrideBreakdown;
    private DataGridView dgvOverrideBreakdown;
    private Button btnRefresh;
    private Button btnMonthlySummary;
}
