namespace PantryDeskApp.Forms;

partial class OverrideReasonForm
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
        lblMessage = new Label();
        lblReason = new Label();
        cmbReason = new ComboBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        btnOK = new Button();
        btnCancel = new Button();
        SuspendLayout();
        // 
        // lblMessage
        // 
        lblMessage.AutoSize = true;
        lblMessage.Location = new Point(12, 15);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(350, 15);
        lblMessage.TabIndex = 0;
        lblMessage.Text = "This household was already served this month. Reason for override:";
        // 
        // lblReason
        // 
        lblReason.AutoSize = true;
        lblReason.Location = new Point(12, 50);
        lblReason.Name = "lblReason";
        lblReason.Size = new Size(45, 15);
        lblReason.TabIndex = 1;
        lblReason.Text = "Reason:";
        // 
        // cmbReason
        // 
        cmbReason.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbReason.FormattingEnabled = true;
        cmbReason.Items.AddRange(new object[] { "Special Circumstance", "Emergency Need", "Admin Override", "Other" });
        cmbReason.Location = new Point(12, 68);
        cmbReason.Name = "cmbReason";
        cmbReason.Size = new Size(350, 23);
        cmbReason.TabIndex = 2;
        cmbReason.SelectedIndexChanged += CmbReason_SelectedIndexChanged;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(12, 110);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(38, 15);
        lblNotes.TabIndex = 3;
        lblNotes.Text = "Notes:";
        // 
        // txtNotes
        // 
        txtNotes.Location = new Point(12, 128);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(350, 100);
        txtNotes.TabIndex = 4;
        // 
        // btnOK
        // 
        btnOK.Enabled = false;
        btnOK.Location = new Point(206, 240);
        btnOK.Name = "btnOK";
        btnOK.Size = new Size(75, 30);
        btnOK.TabIndex = 5;
        btnOK.Text = "OK";
        btnOK.UseVisualStyleBackColor = true;
        btnOK.Click += BtnOK_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(287, 240);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 6;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // OverrideReasonForm
        // 
        AcceptButton = btnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(374, 282);
        Controls.Add(btnCancel);
        Controls.Add(btnOK);
        Controls.Add(txtNotes);
        Controls.Add(lblNotes);
        Controls.Add(cmbReason);
        Controls.Add(lblReason);
        Controls.Add(lblMessage);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "OverrideReasonForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Override Reason Required";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMessage;
    private Label lblReason;
    private ComboBox cmbReason;
    private Label lblNotes;
    private TextBox txtNotes;
    private Button btnOK;
    private Button btnCancel;
}
