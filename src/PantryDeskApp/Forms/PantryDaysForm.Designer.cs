namespace PantryDeskApp.Forms;

partial class PantryDaysForm
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
        grpGenerate = new GroupBox();
        lblYear = new Label();
        numYear = new NumericUpDown();
        btnGenerate = new Button();
        dgvPantryDays = new DataGridView();
        grpEdit = new GroupBox();
        lblEditDate = new Label();
        dtpEditDate = new DateTimePicker();
        chkEditActive = new CheckBox();
        lblEditNotes = new Label();
        txtEditNotes = new TextBox();
        btnSave = new Button();
        btnCancel = new Button();
        grpGenerate.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numYear).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPantryDays).BeginInit();
        grpEdit.SuspendLayout();
        SuspendLayout();
        // 
        // grpGenerate
        // 
        grpGenerate.Controls.Add(lblYear);
        grpGenerate.Controls.Add(numYear);
        grpGenerate.Controls.Add(btnGenerate);
        grpGenerate.Location = new Point(12, 12);
        grpGenerate.Name = "grpGenerate";
        grpGenerate.Size = new Size(776, 60);
        grpGenerate.TabIndex = 0;
        grpGenerate.TabStop = false;
        grpGenerate.Text = "Generate for Year";
        grpGenerate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // lblYear
        // 
        lblYear.AutoSize = true;
        lblYear.Location = new Point(12, 25);
        lblYear.Name = "lblYear";
        lblYear.Size = new Size(32, 15);
        lblYear.TabIndex = 0;
        lblYear.Text = "Year:";
        // 
        // numYear
        // 
        numYear.Location = new Point(50, 23);
        numYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
        numYear.Minimum = new decimal(new int[] { 2020, 0, 0, 0 });
        numYear.Name = "numYear";
        numYear.Size = new Size(100, 23);
        numYear.TabIndex = 1;
        numYear.Value = new decimal(new int[] { 2026, 0, 0, 0 });
        // 
        // btnGenerate
        // 
        btnGenerate.Location = new Point(156, 22);
        btnGenerate.Name = "btnGenerate";
        btnGenerate.Size = new Size(150, 25);
        btnGenerate.TabIndex = 2;
        btnGenerate.Text = "Generate Pantry Days";
        btnGenerate.UseVisualStyleBackColor = true;
        btnGenerate.Click += BtnGenerate_Click;
        // 
        // dgvPantryDays
        // 
        dgvPantryDays.AllowUserToAddRows = false;
        dgvPantryDays.AllowUserToDeleteRows = false;
        dgvPantryDays.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPantryDays.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPantryDays.Location = new Point(12, 78);
        dgvPantryDays.MultiSelect = false;
        dgvPantryDays.Name = "dgvPantryDays";
        dgvPantryDays.ReadOnly = true;
        dgvPantryDays.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPantryDays.Size = new Size(776, 300);
        dgvPantryDays.TabIndex = 1;
        dgvPantryDays.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPantryDays.SelectionChanged += DgvPantryDays_SelectionChanged;
        // 
        // grpEdit
        // 
        grpEdit.Controls.Add(lblEditDate);
        grpEdit.Controls.Add(dtpEditDate);
        grpEdit.Controls.Add(chkEditActive);
        grpEdit.Controls.Add(lblEditNotes);
        grpEdit.Controls.Add(txtEditNotes);
        grpEdit.Controls.Add(btnSave);
        grpEdit.Controls.Add(btnCancel);
        grpEdit.Enabled = false;
        grpEdit.Location = new Point(12, 384);
        grpEdit.Name = "grpEdit";
        grpEdit.Size = new Size(776, 120);
        grpEdit.TabIndex = 2;
        grpEdit.TabStop = false;
        grpEdit.Text = "Edit Selected Pantry Day";
        grpEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // lblEditDate
        // 
        lblEditDate.AutoSize = true;
        lblEditDate.Location = new Point(12, 25);
        lblEditDate.Name = "lblEditDate";
        lblEditDate.Size = new Size(34, 15);
        lblEditDate.TabIndex = 0;
        lblEditDate.Text = "Date:";
        // 
        // dtpEditDate
        // 
        dtpEditDate.Format = DateTimePickerFormat.Short;
        dtpEditDate.Location = new Point(52, 23);
        dtpEditDate.Name = "dtpEditDate";
        dtpEditDate.Size = new Size(150, 23);
        dtpEditDate.TabIndex = 1;
        // 
        // chkEditActive
        // 
        chkEditActive.AutoSize = true;
        chkEditActive.Location = new Point(220, 25);
        chkEditActive.Name = "chkEditActive";
        chkEditActive.Size = new Size(60, 19);
        chkEditActive.TabIndex = 2;
        chkEditActive.Text = "Active";
        // 
        // lblEditNotes
        // 
        lblEditNotes.AutoSize = true;
        lblEditNotes.Location = new Point(12, 55);
        lblEditNotes.Name = "lblEditNotes";
        lblEditNotes.Size = new Size(41, 15);
        lblEditNotes.TabIndex = 3;
        lblEditNotes.Text = "Notes:";
        // 
        // txtEditNotes
        // 
        txtEditNotes.Location = new Point(59, 52);
        txtEditNotes.Name = "txtEditNotes";
        txtEditNotes.Size = new Size(500, 23);
        txtEditNotes.TabIndex = 4;
        txtEditNotes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(620, 52);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(70, 25);
        btnSave.TabIndex = 5;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(696, 52);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(70, 25);
        btnCancel.TabIndex = 6;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // PantryDaysForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 516);
        MinimumSize = new Size(600, 400);
        Controls.Add(grpEdit);
        Controls.Add(dgvPantryDays);
        Controls.Add(grpGenerate);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PantryDaysForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Pantry Days Calendar";
        Load += PantryDaysForm_Load;
        grpGenerate.ResumeLayout(false);
        grpGenerate.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numYear).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPantryDays).EndInit();
        grpEdit.ResumeLayout(false);
        grpEdit.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpGenerate;
    private Label lblYear;
    private NumericUpDown numYear;
    private Button btnGenerate;
    private DataGridView dgvPantryDays;
    private GroupBox grpEdit;
    private Label lblEditDate;
    private DateTimePicker dtpEditDate;
    private CheckBox chkEditActive;
    private Label lblEditNotes;
    private TextBox txtEditNotes;
    private Button btnSave;
    private Button btnCancel;
}
