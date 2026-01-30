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
        lblTitle = new Label();
        lblBackupPath = new Label();
        txtBackupPath = new TextBox();
        btnBrowse = new Button();
        lblBackupInfo = new Label();
        btnRestore = new Button();
        btnCancel = new Button();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(12, 9);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(168, 21);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "Restore from Backup";
        // 
        // lblBackupPath
        // 
        lblBackupPath.AutoSize = true;
        lblBackupPath.Location = new Point(12, 45);
        lblBackupPath.Name = "lblBackupPath";
        lblBackupPath.Size = new Size(75, 15);
        lblBackupPath.TabIndex = 1;
        lblBackupPath.Text = "Backup File:";
        // 
        // txtBackupPath
        // 
        txtBackupPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBackupPath.Location = new Point(12, 63);
        txtBackupPath.Name = "txtBackupPath";
        txtBackupPath.ReadOnly = true;
        txtBackupPath.Size = new Size(460, 23);
        txtBackupPath.TabIndex = 2;
        txtBackupPath.TextChanged += TxtBackupPath_TextChanged;
        // 
        // btnBrowse
        // 
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.Location = new Point(478, 62);
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
        lblBackupInfo.Location = new Point(12, 100);
        lblBackupInfo.Name = "lblBackupInfo";
        lblBackupInfo.Size = new Size(0, 15);
        lblBackupInfo.TabIndex = 4;
        // 
        // btnRestore
        // 
        btnRestore.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnRestore.Enabled = false;
        btnRestore.Location = new Point(397, 150);
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
        btnCancel.Location = new Point(478, 150);
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
        ClientSize = new Size(565, 192);
        Controls.Add(btnCancel);
        Controls.Add(btnRestore);
        Controls.Add(lblBackupInfo);
        Controls.Add(btnBrowse);
        Controls.Add(txtBackupPath);
        Controls.Add(lblBackupPath);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "BackupRestoreForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Restore from Backup";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTitle;
    private Label lblBackupPath;
    private TextBox txtBackupPath;
    private Button btnBrowse;
    private Label lblBackupInfo;
    private Button btnRestore;
    private Button btnCancel;

    #endregion
}
