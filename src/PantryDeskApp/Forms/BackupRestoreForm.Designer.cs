namespace PantryDeskApp.Forms;

partial class BackupRestoreForm
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
        panelInfo = new Panel();
        lblDbPath = new Label();
        lblAutoBackup = new Label();
        lblManualBackup = new Label();
        lblTitle = new Label();
        lblBackupPath = new Label();
        txtBackupPath = new TextBox();
        btnBrowse = new Button();
        lblBackupInfo = new Label();
        btnRestore = new Button();
        btnCancel = new Button();
        panelInfo.SuspendLayout();
        SuspendLayout();
        // 
        // panelInfo
        // 
        panelInfo.Controls.Add(lblDbPath);
        panelInfo.Controls.Add(lblAutoBackup);
        panelInfo.Controls.Add(lblManualBackup);
        panelInfo.Dock = DockStyle.Top;
        panelInfo.Location = new Point(0, 0);
        panelInfo.Name = "panelInfo";
        panelInfo.Padding = new Padding(12, 8, 12, 8);
        panelInfo.Size = new Size(565, 72);
        panelInfo.TabIndex = 7;
        // 
        // lblDbPath
        // 
        lblDbPath.AutoSize = true;
        lblDbPath.Location = new Point(12, 8);
        lblDbPath.MaximumSize = new Size(541, 0);
        lblDbPath.Name = "lblDbPath";
        lblDbPath.Size = new Size(85, 15);
        lblDbPath.TabIndex = 0;
        lblDbPath.Text = "Database path: ";
        // 
        // lblAutoBackup
        // 
        lblAutoBackup.AutoSize = true;
        lblAutoBackup.Location = new Point(12, 28);
        lblAutoBackup.Name = "lblAutoBackup";
        lblAutoBackup.Size = new Size(145, 15);
        lblAutoBackup.TabIndex = 1;
        lblAutoBackup.Text = "Last Auto Backup: No backup yet";
        // 
        // lblManualBackup
        // 
        lblManualBackup.AutoSize = true;
        lblManualBackup.Location = new Point(12, 48);
        lblManualBackup.Name = "lblManualBackup";
        lblManualBackup.Size = new Size(162, 15);
        lblManualBackup.TabIndex = 2;
        lblManualBackup.Text = "Last Manual Backup: No backup yet";
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(12, 81);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(168, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Restore from Backup";
        // 
        // lblBackupPath
        // 
        lblBackupPath.AutoSize = true;
        lblBackupPath.Location = new Point(12, 117);
        lblBackupPath.Name = "lblBackupPath";
        lblBackupPath.Size = new Size(75, 15);
        lblBackupPath.TabIndex = 1;
        lblBackupPath.Text = "Backup File:";
        // 
        // txtBackupPath
        // 
        txtBackupPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBackupPath.Location = new Point(12, 135);
        txtBackupPath.Name = "txtBackupPath";
        txtBackupPath.ReadOnly = true;
        txtBackupPath.Size = new Size(460, 23);
        txtBackupPath.TabIndex = 2;
        txtBackupPath.TextChanged += TxtBackupPath_TextChanged;
        // 
        // btnBrowse
        // 
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.Location = new Point(478, 134);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(75, 25);
        btnBrowse.TabIndex = 3;
        btnBrowse.Text = "Browse...";
        btnBrowse.UseVisualStyleBackColor = true;
        btnBrowse.Click += BtnBrowse_Click;
        // 
        // lblBackupInfo
        // 
        lblBackupInfo.AutoSize = true;
        lblBackupInfo.Location = new Point(12, 172);
        lblBackupInfo.Name = "lblBackupInfo";
        lblBackupInfo.Size = new Size(0, 15);
        lblBackupInfo.TabIndex = 4;
        // 
        // btnRestore
        // 
        btnRestore.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnRestore.Enabled = false;
        btnRestore.Location = new Point(397, 222);
        btnRestore.Name = "btnRestore";
        btnRestore.Size = new Size(75, 30);
        btnRestore.TabIndex = 5;
        btnRestore.Text = "Restore";
        btnRestore.UseVisualStyleBackColor = true;
        btnRestore.Click += BtnRestore_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.Location = new Point(478, 222);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 30);
        btnCancel.TabIndex = 6;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += BtnCancel_Click;
        // 
        // BackupRestoreForm
        // 
        AcceptButton = btnRestore;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(565, 264);
        Controls.Add(btnCancel);
        Controls.Add(btnRestore);
        Controls.Add(lblBackupInfo);
        Controls.Add(btnBrowse);
        Controls.Add(txtBackupPath);
        Controls.Add(lblBackupPath);
        Controls.Add(lblTitle);
        Controls.Add(panelInfo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "BackupRestoreForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Restore from Backup";
        panelInfo.ResumeLayout(false);
        panelInfo.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel panelInfo;
    private Label lblDbPath;
    private Label lblAutoBackup;
    private Label lblManualBackup;
    private Label lblTitle;
    private Label lblBackupPath;
    private TextBox txtBackupPath;
    private Button btnBrowse;
    private Label lblBackupInfo;
    private Button btnRestore;
    private Button btnCancel;

    #endregion
}
