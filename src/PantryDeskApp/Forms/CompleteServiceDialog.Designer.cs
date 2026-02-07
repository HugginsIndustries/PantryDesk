namespace PantryDeskApp.Forms;

partial class CompleteServiceDialog
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
        lblVisitType = new Label();
        cmbVisitType = new ComboBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        btnOK = new Button();
        btnCancel = new Button();
        SuspendLayout();
        //
        // lblVisitType
        //
        lblVisitType.AutoSize = true;
        lblVisitType.Location = new Point(12, 15);
        lblVisitType.Name = "lblVisitType";
        lblVisitType.Size = new Size(150, 15);
        lblVisitType.TabIndex = 0;
        lblVisitType.Text = "Visit Type:";
        //
        // cmbVisitType
        //
        cmbVisitType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbVisitType.FormattingEnabled = true;
        cmbVisitType.Items.AddRange(new object[] { "Shop with TEFAP", "Shop", "TEFAP Only", "Deck Only" });
        cmbVisitType.Location = new Point(12, 33);
        cmbVisitType.Name = "cmbVisitType";
        cmbVisitType.Size = new Size(350, 23);
        cmbVisitType.TabIndex = 1;
        cmbVisitType.SelectedIndex = 0;
        //
        // lblNotes
        //
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 70);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(38, 15);
        lblNotes.TabIndex = 2;
        lblNotes.Text = "Notes:";
        //
        // txtNotes
        //
        txtNotes.Location = new Point(12, 88);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(350, 100);
        txtNotes.TabIndex = 3;
        //
        // btnOK
        //
        btnOK.Location = new Point(206, 200);
        btnOK.Name = "btnOK";
        btnOK.Size = new Size(75, 30);
        btnOK.TabIndex = 4;
        btnOK.Text = "OK";
        btnOK.UseVisualStyleBackColor = true;
        btnOK.Click += BtnOK_Click;
        //
        // btnCancel
        //
        btnCancel.Location = new Point(287, 200);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 5;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        //
        // CompleteServiceDialog
        //
        AcceptButton = btnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(374, 242);
        Controls.Add(btnCancel);
        Controls.Add(btnOK);
        Controls.Add(txtNotes);
        Controls.Add(lblNotes);
        Controls.Add(cmbVisitType);
        Controls.Add(lblVisitType);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CompleteServiceDialog";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Complete Service";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblVisitType;
    private ComboBox cmbVisitType;
    private Label lblNotes;
    private TextBox txtNotes;
    private Button btnOK;
    private Button btnCancel;
}
