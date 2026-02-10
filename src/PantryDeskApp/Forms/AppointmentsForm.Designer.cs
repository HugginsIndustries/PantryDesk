namespace PantryDeskApp.Forms;

partial class AppointmentsForm
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
        splitContainer = new SplitContainer();
        pnlPast = new Panel();
        lblPast = new Label();
        lblDateFrom = new Label();
        dtpDateFrom = new DateTimePicker();
        lblDateTo = new Label();
        dtpDateTo = new DateTimePicker();
        lblFilterStatus = new Label();
        cmbFilterStatus = new ComboBox();
        dgvPast = new DataGridView();
        pnlFuture = new Panel();
        lblFuture = new Label();
        btnCreateNew = new Button();
        dgvFuture = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        pnlPast.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPast).BeginInit();
        pnlFuture.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvFuture).BeginInit();
        SuspendLayout();
        // 
        // splitContainer
        // 
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 0);
        splitContainer.Name = "splitContainer";
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.Controls.Add(pnlPast);
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(pnlFuture);
        splitContainer.Size = new Size(900, 500);
        splitContainer.SplitterDistance = 450;
        splitContainer.TabIndex = 0;
        // 
        // pnlPast
        // 
        pnlPast.Controls.Add(lblPast);
        pnlPast.Controls.Add(lblDateFrom);
        pnlPast.Controls.Add(dtpDateFrom);
        pnlPast.Controls.Add(lblDateTo);
        pnlPast.Controls.Add(dtpDateTo);
        pnlPast.Controls.Add(lblFilterStatus);
        pnlPast.Controls.Add(cmbFilterStatus);
        pnlPast.Controls.Add(dgvPast);
        pnlPast.Dock = DockStyle.Fill;
        pnlPast.Location = new Point(0, 0);
        pnlPast.Name = "pnlPast";
        pnlPast.Padding = new Padding(8);
        pnlPast.Size = new Size(450, 500);
        pnlPast.TabIndex = 0;
        // 
        // lblPast
        // 
        lblPast.AutoSize = true;
        lblPast.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPast.Location = new Point(11, 12);
        lblPast.Name = "lblPast";
        lblPast.Size = new Size(101, 15);
        lblPast.TabIndex = 0;
        lblPast.Text = "Past (Completed / Cancelled / NoShow)";
        // 
        // lblDateFrom
        // 
        lblDateFrom.AutoSize = true;
        lblDateFrom.Location = new Point(11, 40);
        lblDateFrom.Name = "lblDateFrom";
        lblDateFrom.Size = new Size(34, 15);
        lblDateFrom.TabIndex = 1;
        lblDateFrom.Text = "From:";
        // 
        // dtpDateFrom
        // 
        dtpDateFrom.Format = DateTimePickerFormat.Short;
        dtpDateFrom.Location = new Point(51, 37);
        dtpDateFrom.Name = "dtpDateFrom";
        dtpDateFrom.Size = new Size(110, 23);
        dtpDateFrom.TabIndex = 2;
        dtpDateFrom.ValueChanged += Filter_Changed;
        // 
        // lblDateTo
        // 
        lblDateTo.AutoSize = true;
        lblDateTo.Location = new Point(167, 40);
        lblDateTo.Name = "lblDateTo";
        lblDateTo.Size = new Size(22, 15);
        lblDateTo.TabIndex = 3;
        lblDateTo.Text = "To:";
        // 
        // dtpDateTo
        // 
        dtpDateTo.Format = DateTimePickerFormat.Short;
        dtpDateTo.Location = new Point(195, 37);
        dtpDateTo.Name = "dtpDateTo";
        dtpDateTo.Size = new Size(110, 23);
        dtpDateTo.TabIndex = 4;
        dtpDateTo.ValueChanged += Filter_Changed;
        // 
        // lblFilterStatus
        // 
        lblFilterStatus.AutoSize = true;
        lblFilterStatus.Location = new Point(311, 40);
        lblFilterStatus.Name = "lblFilterStatus";
        lblFilterStatus.Size = new Size(42, 15);
        lblFilterStatus.TabIndex = 5;
        lblFilterStatus.Text = "Status:";
        // 
        // cmbFilterStatus
        // 
        cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFilterStatus.FormattingEnabled = true;
        cmbFilterStatus.Location = new Point(359, 37);
        cmbFilterStatus.Name = "cmbFilterStatus";
        cmbFilterStatus.Size = new Size(120, 23);
        cmbFilterStatus.TabIndex = 6;
        cmbFilterStatus.SelectedIndexChanged += Filter_Changed;
        // 
        // dgvPast
        // 
        dgvPast.AllowUserToAddRows = false;
        dgvPast.AllowUserToDeleteRows = false;
        dgvPast.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPast.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPast.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPast.Location = new Point(11, 70);
        dgvPast.MultiSelect = false;
        dgvPast.Name = "dgvPast";
        dgvPast.ReadOnly = true;
        dgvPast.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPast.Size = new Size(428, 419);
        dgvPast.TabIndex = 7;
        // 
        // pnlFuture
        // 
        btnMarkComplete = new Button();
        btnMarkCancelled = new Button();
        btnMarkNoShow = new Button();
        pnlFuture.Controls.Add(lblFuture);
        pnlFuture.Controls.Add(btnMarkComplete);
        pnlFuture.Controls.Add(btnMarkCancelled);
        pnlFuture.Controls.Add(btnMarkNoShow);
        pnlFuture.Controls.Add(btnCreateNew);
        pnlFuture.Controls.Add(dgvFuture);
        pnlFuture.Dock = DockStyle.Fill;
        pnlFuture.Location = new Point(0, 0);
        pnlFuture.Name = "pnlFuture";
        pnlFuture.Padding = new Padding(8);
        pnlFuture.Size = new Size(446, 500);
        pnlFuture.TabIndex = 0;
        // 
        // lblFuture
        // 
        lblFuture.AutoSize = true;
        lblFuture.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblFuture.Location = new Point(11, 12);
        lblFuture.Name = "lblFuture";
        lblFuture.Size = new Size(110, 15);
        lblFuture.TabIndex = 0;
        lblFuture.Text = "Future (Scheduled)";
        // 
        // btnMarkComplete
        // 
        btnMarkComplete.Location = new Point(11, 35);
        btnMarkComplete.Name = "btnMarkComplete";
        btnMarkComplete.Size = new Size(100, 28);
        btnMarkComplete.TabIndex = 1;
        btnMarkComplete.Text = "Mark Complete";
        btnMarkComplete.UseVisualStyleBackColor = true;
        btnMarkComplete.Click += BtnMarkComplete_Click;
        // 
        // btnMarkCancelled
        // 
        btnMarkCancelled.Location = new Point(117, 35);
        btnMarkCancelled.Name = "btnMarkCancelled";
        btnMarkCancelled.Size = new Size(100, 28);
        btnMarkCancelled.TabIndex = 2;
        btnMarkCancelled.Text = "Mark Cancelled";
        btnMarkCancelled.UseVisualStyleBackColor = true;
        btnMarkCancelled.Click += BtnMarkCancelled_Click;
        // 
        // btnMarkNoShow
        // 
        btnMarkNoShow.Location = new Point(223, 35);
        btnMarkNoShow.Name = "btnMarkNoShow";
        btnMarkNoShow.Size = new Size(100, 28);
        btnMarkNoShow.TabIndex = 3;
        btnMarkNoShow.Text = "Mark NoShow";
        btnMarkNoShow.UseVisualStyleBackColor = true;
        btnMarkNoShow.Click += BtnMarkNoShow_Click;
        // 
        // btnCreateNew
        // 
        btnCreateNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCreateNew.Location = new Point(278, 35);
        btnCreateNew.Name = "btnCreateNew";
        btnCreateNew.Size = new Size(156, 28);
        btnCreateNew.TabIndex = 4;
        btnCreateNew.Text = "Create New Appointment";
        btnCreateNew.UseVisualStyleBackColor = true;
        btnCreateNew.Click += BtnCreateNew_Click;
        // 
        // dgvFuture
        // 
        dgvFuture.AllowUserToAddRows = false;
        dgvFuture.AllowUserToDeleteRows = false;
        dgvFuture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvFuture.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvFuture.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvFuture.Location = new Point(11, 70);
        dgvFuture.MultiSelect = false;
        dgvFuture.Name = "dgvFuture";
        dgvFuture.ReadOnly = true;
        dgvFuture.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvFuture.Size = new Size(424, 419);
        dgvFuture.TabIndex = 5;
        // 
        // AppointmentsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1500, 750);
        MinimumSize = new Size(1100, 500);
        Controls.Add(splitContainer);
        Name = "AppointmentsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Appointments";
        Load += AppointmentsForm_Load;
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        pnlPast.ResumeLayout(false);
        pnlPast.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPast).EndInit();
        pnlFuture.ResumeLayout(false);
        pnlFuture.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvFuture).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer;
    private Panel pnlPast;
    private Label lblPast;
    private Label lblDateFrom;
    private DateTimePicker dtpDateFrom;
    private Label lblDateTo;
    private DateTimePicker dtpDateTo;
    private Label lblFilterStatus;
    private ComboBox cmbFilterStatus;
    private DataGridView dgvPast;
    private Panel pnlFuture;
    private Label lblFuture;
    private Button btnMarkComplete;
    private Button btnMarkCancelled;
    private Button btnMarkNoShow;
    private Button btnCreateNew;
    private DataGridView dgvFuture;
}
