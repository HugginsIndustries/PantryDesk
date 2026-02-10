namespace PantryDeskApp.Forms;

partial class DeckStatsEntryForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lblTitle = new Label();
        lblMonth = new Label();
        cmbMonth = new ComboBox();
        lblYear = new Label();
        cmbYear = new ComboBox();
        lblHouseholdTotal = new Label();
        numHouseholdTotal = new NumericUpDown();
        lblInfant = new Label();
        numInfant = new NumericUpDown();
        lblChild = new Label();
        numChild = new NumericUpDown();
        lblAdult = new Label();
        numAdult = new NumericUpDown();
        lblSenior = new Label();
        numSenior = new NumericUpDown();
        lblPages = new Label();
        numPages = new NumericUpDown();
        btnSave = new Button();
        btnCancel = new Button();
        lblError = new Label();
        ((System.ComponentModel.ISupportInitialize)numHouseholdTotal).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numInfant).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numChild).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numAdult).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSenior).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numPages).BeginInit();
        SuspendLayout();
        //
        // lblTitle
        //
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(20, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(120, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Enter Deck Stats";
        //
        // lblMonth
        //
        lblMonth.AutoSize = true;
        lblMonth.Location = new Point(20, 55);
        lblMonth.Name = "lblMonth";
        lblMonth.Size = new Size(45, 15);
        lblMonth.TabIndex = 1;
        lblMonth.Text = "Month:";
        //
        // cmbMonth
        //
        cmbMonth.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMonth.FormattingEnabled = true;
        cmbMonth.Items.AddRange(new object[]
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        });
        cmbMonth.Location = new Point(20, 73);
        cmbMonth.Name = "cmbMonth";
        cmbMonth.Size = new Size(140, 23);
        cmbMonth.TabIndex = 2;
        //
        // lblYear
        //
        lblYear.AutoSize = true;
        lblYear.Location = new Point(180, 55);
        lblYear.Name = "lblYear";
        lblYear.Size = new Size(32, 15);
        lblYear.TabIndex = 3;
        lblYear.Text = "Year:";
        //
        // cmbYear
        //
        cmbYear.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbYear.FormattingEnabled = true;
        cmbYear.Location = new Point(180, 73);
        cmbYear.Name = "cmbYear";
        cmbYear.Size = new Size(90, 23);
        cmbYear.TabIndex = 4;
        //
        // lblHouseholdTotal
        //
        lblHouseholdTotal.AutoSize = true;
        lblHouseholdTotal.Location = new Point(20, 110);
        lblHouseholdTotal.Name = "lblHouseholdTotal";
        lblHouseholdTotal.Size = new Size(200, 15);
        lblHouseholdTotal.TabIndex = 5;
        lblHouseholdTotal.Text = "Household total (summed across pages):";
        //
        // numHouseholdTotal
        //
        numHouseholdTotal.Location = new Point(20, 128);
        numHouseholdTotal.Maximum = 99999;
        numHouseholdTotal.Minimum = 0;
        numHouseholdTotal.Name = "numHouseholdTotal";
        numHouseholdTotal.Size = new Size(100, 23);
        numHouseholdTotal.TabIndex = 6;
        numHouseholdTotal.Value = 0;
        //
        // lblInfant
        //
        lblInfant.AutoSize = true;
        lblInfant.Location = new Point(20, 158);
        lblInfant.Name = "lblInfant";
        lblInfant.Size = new Size(180, 15);
        lblInfant.TabIndex = 7;
        lblInfant.Text = "Infant (0-2) total (summed):";
        //
        // numInfant
        //
        numInfant.Location = new Point(20, 176);
        numInfant.Maximum = 99999;
        numInfant.Minimum = 0;
        numInfant.Name = "numInfant";
        numInfant.Size = new Size(100, 23);
        numInfant.TabIndex = 8;
        numInfant.Value = 0;
        //
        // lblChild
        //
        lblChild.AutoSize = true;
        lblChild.Location = new Point(20, 206);
        lblChild.Name = "lblChild";
        lblChild.Size = new Size(190, 15);
        lblChild.TabIndex = 9;
        lblChild.Text = "Child (2-18) total (summed):";
        //
        // numChild
        //
        numChild.Location = new Point(20, 224);
        numChild.Maximum = 99999;
        numChild.Minimum = 0;
        numChild.Name = "numChild";
        numChild.Size = new Size(100, 23);
        numChild.TabIndex = 10;
        numChild.Value = 0;
        //
        // lblAdult
        //
        lblAdult.AutoSize = true;
        lblAdult.Location = new Point(20, 254);
        lblAdult.Name = "lblAdult";
        lblAdult.Size = new Size(200, 15);
        lblAdult.TabIndex = 11;
        lblAdult.Text = "Adult (18-55) total (summed):";
        //
        // numAdult
        //
        numAdult.Location = new Point(20, 272);
        numAdult.Maximum = 99999;
        numAdult.Minimum = 0;
        numAdult.Name = "numAdult";
        numAdult.Size = new Size(100, 23);
        numAdult.TabIndex = 12;
        numAdult.Value = 0;
        //
        // lblSenior
        //
        lblSenior.AutoSize = true;
        lblSenior.Location = new Point(20, 302);
        lblSenior.Name = "lblSenior";
        lblSenior.Size = new Size(180, 15);
        lblSenior.TabIndex = 13;
        lblSenior.Text = "Senior (55+) total (summed):";
        //
        // numSenior
        //
        numSenior.Location = new Point(20, 320);
        numSenior.Maximum = 99999;
        numSenior.Minimum = 0;
        numSenior.Name = "numSenior";
        numSenior.Size = new Size(100, 23);
        numSenior.TabIndex = 14;
        numSenior.Value = 0;
        //
        // lblPages
        //
        lblPages.AutoSize = true;
        lblPages.Location = new Point(20, 358);
        lblPages.Name = "lblPages";
        lblPages.Size = new Size(95, 15);
        lblPages.TabIndex = 15;
        lblPages.Text = "Number of pages:";
        //
        // numPages
        //
        numPages.Location = new Point(20, 376);
        numPages.Minimum = 1;
        numPages.Name = "numPages";
        numPages.Size = new Size(100, 23);
        numPages.TabIndex = 16;
        numPages.Value = 1;
        //
        // btnSave
        //
        btnSave.Location = new Point(195, 415);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 30);
        btnSave.TabIndex = 17;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += BtnSave_Click;
        //
        // btnCancel
        //
        btnCancel.Location = new Point(114, 415);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 18;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        //
        // lblError
        //
        lblError.AutoSize = true;
        lblError.ForeColor = Color.Red;
        lblError.Location = new Point(20, 395);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 19;
        lblError.Visible = false;
        //
        // DeckStatsEntryForm
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(284, 461);
        Controls.Add(lblError);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(numPages);
        Controls.Add(lblPages);
        Controls.Add(numSenior);
        Controls.Add(lblSenior);
        Controls.Add(numAdult);
        Controls.Add(lblAdult);
        Controls.Add(numChild);
        Controls.Add(lblChild);
        Controls.Add(numInfant);
        Controls.Add(lblInfant);
        Controls.Add(numHouseholdTotal);
        Controls.Add(lblHouseholdTotal);
        Controls.Add(cmbYear);
        Controls.Add(lblYear);
        Controls.Add(cmbMonth);
        Controls.Add(lblMonth);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "DeckStatsEntryForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Enter Deck Stats";
        ((System.ComponentModel.ISupportInitialize)numHouseholdTotal).EndInit();
        ((System.ComponentModel.ISupportInitialize)numInfant).EndInit();
        ((System.ComponentModel.ISupportInitialize)numChild).EndInit();
        ((System.ComponentModel.ISupportInitialize)numAdult).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSenior).EndInit();
        ((System.ComponentModel.ISupportInitialize)numPages).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTitle;
    private Label lblMonth;
    private ComboBox cmbMonth;
    private Label lblYear;
    private ComboBox cmbYear;
    private Label lblHouseholdTotal;
    private NumericUpDown numHouseholdTotal;
    private Label lblInfant;
    private NumericUpDown numInfant;
    private Label lblChild;
    private NumericUpDown numChild;
    private Label lblAdult;
    private NumericUpDown numAdult;
    private Label lblSenior;
    private NumericUpDown numSenior;
    private Label lblPages;
    private NumericUpDown numPages;
    private Button btnSave;
    private Button btnCancel;
    private Label lblError;
}
