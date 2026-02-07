namespace PantryDeskApp.Forms;

partial class ManualBackupReminderForm
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
        lblMessage = new Label();
        lblAdminNote = new Label();
        btnSnooze = new Button();
        btnBackupNow = new Button();
        SuspendLayout();
        // 
        // lblMessage
        // 
        lblMessage.AutoSize = true;
        lblMessage.Location = new Point(12, 12);
        lblMessage.MaximumSize = new Size(420, 0);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(420, 45);
        lblMessage.TabIndex = 0;
        lblMessage.Text = "No manual backup has been run in the last 7 days. Consider backing up to USB or another location.";
        lblMessage.UseMnemonic = false;
        // 
        // lblAdminNote
        // 
        lblAdminNote.AutoSize = true;
        lblAdminNote.ForeColor = System.Drawing.Color.Gray;
        lblAdminNote.Location = new Point(12, 68);
        lblAdminNote.Name = "lblAdminNote";
        lblAdminNote.Size = new Size(0, 15);
        lblAdminNote.TabIndex = 1;
        lblAdminNote.Visible = false;
        // 
        // btnSnooze
        // 
        btnSnooze.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSnooze.Location = new Point(257, 105);
        btnSnooze.Name = "btnSnooze";
        btnSnooze.Size = new Size(85, 28);
        btnSnooze.TabIndex = 2;
        btnSnooze.Text = "Snooze";
        btnSnooze.UseVisualStyleBackColor = true;
        btnSnooze.Click += BtnSnooze_Click;
        // 
        // btnBackupNow
        // 
        btnBackupNow.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnBackupNow.Location = new Point(348, 105);
        btnBackupNow.Name = "btnBackupNow";
        btnBackupNow.Size = new Size(84, 28);
        btnBackupNow.TabIndex = 3;
        btnBackupNow.Text = "Backup Now";
        btnBackupNow.UseVisualStyleBackColor = true;
        btnBackupNow.Click += BtnBackupNow_Click;
        // 
        // ManualBackupReminderForm
        // 
        AcceptButton = btnBackupNow;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnSnooze;
        ClientSize = new Size(444, 145);
        Controls.Add(btnBackupNow);
        Controls.Add(btnSnooze);
        Controls.Add(lblAdminNote);
        Controls.Add(lblMessage);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ManualBackupReminderForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Manual Backup Reminder";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMessage;
    private Label lblAdminNote;
    private Button btnSnooze;
    private Button btnBackupNow;
}
