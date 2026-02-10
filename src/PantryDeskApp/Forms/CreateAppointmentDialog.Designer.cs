namespace PantryDeskApp.Forms;

partial class CreateAppointmentDialog
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
        lblSearch = new Label();
        txtSearch = new TextBox();
        dgvResults = new DataGridView();
        lblScheduledDate = new Label();
        dtpScheduledDate = new DateTimePicker();
        lblScheduledText = new Label();
        txtScheduledText = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        grpAppointment = new GroupBox();
        ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
        grpAppointment.SuspendLayout();
        SuspendLayout();
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(12, 15);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(180, 15);
        lblSearch.TabIndex = 0;
        lblSearch.Text = "Search by household member name:";
        // 
        // txtSearch
        // 
        txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtSearch.Location = new Point(12, 33);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Type to search...";
        txtSearch.Size = new Size(460, 23);
        txtSearch.TabIndex = 1;
        txtSearch.TextChanged += TxtSearch_TextChanged;
        txtSearch.KeyDown += TxtSearch_KeyDown;
        // 
        // dgvResults
        // 
        dgvResults.AllowUserToAddRows = false;
        dgvResults.AllowUserToDeleteRows = false;
        dgvResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvResults.Location = new Point(12, 62);
        dgvResults.MultiSelect = false;
        dgvResults.Name = "dgvResults";
        dgvResults.ReadOnly = true;
        dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvResults.Size = new Size(460, 150);
        dgvResults.TabIndex = 2;
        dgvResults.SelectionChanged += DgvResults_SelectionChanged;
        // 
        // grpAppointment
        // 
        grpAppointment.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpAppointment.Controls.Add(lblScheduledDate);
        grpAppointment.Controls.Add(dtpScheduledDate);
        grpAppointment.Controls.Add(lblScheduledText);
        grpAppointment.Controls.Add(txtScheduledText);
        grpAppointment.Controls.Add(lblNotes);
        grpAppointment.Controls.Add(txtNotes);
        grpAppointment.Enabled = false;
        grpAppointment.Location = new Point(12, 218);
        grpAppointment.Name = "grpAppointment";
        grpAppointment.Size = new Size(460, 250);
        grpAppointment.TabIndex = 3;
        grpAppointment.TabStop = false;
        grpAppointment.Text = "Appointment Details";
        // 
        // lblScheduledDate
        // 
        lblScheduledDate.AutoSize = true;
        lblScheduledDate.Location = new Point(12, 25);
        lblScheduledDate.Name = "lblScheduledDate";
        lblScheduledDate.Size = new Size(88, 15);
        lblScheduledDate.TabIndex = 0;
        lblScheduledDate.Text = "Scheduled Date:";
        // 
        // dtpScheduledDate
        // 
        dtpScheduledDate.Format = DateTimePickerFormat.Short;
        dtpScheduledDate.Location = new Point(12, 43);
        dtpScheduledDate.Name = "dtpScheduledDate";
        dtpScheduledDate.Size = new Size(200, 23);
        dtpScheduledDate.TabIndex = 1;
        // 
        // lblScheduledText
        // 
        lblScheduledText.AutoSize = true;
        lblScheduledText.Location = new Point(12, 75);
        lblScheduledText.Name = "lblScheduledText";
        lblScheduledText.Size = new Size(89, 15);
        lblScheduledText.TabIndex = 2;
        lblScheduledText.Text = "Scheduled Text (required):";
        // 
        // txtScheduledText
        // 
        txtScheduledText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtScheduledText.Location = new Point(12, 93);
        txtScheduledText.Multiline = true;
        txtScheduledText.Name = "txtScheduledText";
        txtScheduledText.PlaceholderText = "e.g., 10:00 AM - Food pickup";
        txtScheduledText.ScrollBars = ScrollBars.Vertical;
        txtScheduledText.Size = new Size(436, 50);
        txtScheduledText.TabIndex = 3;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 150);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(41, 15);
        lblNotes.TabIndex = 4;
        lblNotes.Text = "Notes (optional):";
        // 
        // txtNotes
        // 
        txtNotes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtNotes.Location = new Point(12, 168);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(436, 70);
        txtNotes.TabIndex = 5;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.Location = new Point(316, 478);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 4;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.Location = new Point(397, 478);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 5;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // CreateAppointmentDialog
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(484, 530);
        Controls.Add(lblSearch);
        Controls.Add(txtSearch);
        Controls.Add(dgvResults);
        Controls.Add(grpAppointment);
        Controls.Add(btnSave);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CreateAppointmentDialog";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Create New Appointment";
        ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
        grpAppointment.ResumeLayout(false);
        grpAppointment.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblSearch;
    private TextBox txtSearch;
    private DataGridView dgvResults;
    private GroupBox grpAppointment;
    private Label lblScheduledDate;
    private DateTimePicker dtpScheduledDate;
    private Label lblScheduledText;
    private TextBox txtScheduledText;
    private Label lblNotes;
    private TextBox txtNotes;
    private Button btnSave;
    private Button btnCancel;
}
