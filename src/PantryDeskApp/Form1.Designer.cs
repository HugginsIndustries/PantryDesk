namespace PantryDeskApp;

partial class Form1
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
        btnTest = new Button();
        SuspendLayout();
        // 
        // btnTest
        // 
        btnTest.Location = new Point(12, 12);
        btnTest.Name = "btnTest";
        btnTest.Size = new Size(200, 30);
        btnTest.TabIndex = 0;
        btnTest.Text = "Test Database (Phase 1)";
        btnTest.UseVisualStyleBackColor = true;
        btnTest.Click += BtnTest_Click;
        // 
        // Form1
        // 
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(btnTest);
        Text = "PantryDesk";
        ResumeLayout(false);
    }

    private Button btnTest;

    #endregion
}
