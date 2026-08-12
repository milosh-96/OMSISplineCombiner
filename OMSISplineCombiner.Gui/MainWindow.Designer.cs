namespace OMSISplineCombiner.Gui;

partial class MainWindow
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
        openFileDialog1 = new OpenFileDialog();
        browseProjectFileButton = new Button();
        fileSystemWatcher1 = new FileSystemWatcher();
        generateProjectFileButtons = new Button();
        flowLayoutPanel1 = new FlowLayoutPanel();
        saveGeneratedProjectFileDialog = new SaveFileDialog();
        authorLinkLabel = new LinkLabel();
        versionLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
        flowLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // openFileDialog1
        // 
        openFileDialog1.FileName = "openFileDialog1";
        openFileDialog1.FileOk += openFileDialog1_FileOk;
        // 
        // browseProjectFileButton
        // 
        browseProjectFileButton.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        browseProjectFileButton.Location = new Point(3, 4);
        browseProjectFileButton.Margin = new Padding(3, 4, 3, 4);
        browseProjectFileButton.Name = "browseProjectFileButton";
        browseProjectFileButton.Size = new Size(485, 76);
        browseProjectFileButton.TabIndex = 0;
        browseProjectFileButton.Text = "Browse Project File";
        browseProjectFileButton.UseVisualStyleBackColor = true;
        browseProjectFileButton.Click += browseProjectFileButton_Click;
        // 
        // fileSystemWatcher1
        // 
        fileSystemWatcher1.EnableRaisingEvents = true;
        fileSystemWatcher1.SynchronizingObject = this;
        // 
        // generateProjectFileButtons
        // 
        generateProjectFileButtons.Location = new Point(3, 88);
        generateProjectFileButtons.Margin = new Padding(3, 4, 3, 4);
        generateProjectFileButtons.Name = "generateProjectFileButtons";
        generateProjectFileButtons.Size = new Size(485, 31);
        generateProjectFileButtons.TabIndex = 1;
        generateProjectFileButtons.Text = "Generate Project File";
        generateProjectFileButtons.UseVisualStyleBackColor = true;
        generateProjectFileButtons.Click += generateProjectFileButtons_Click;
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.Controls.Add(browseProjectFileButton);
        flowLayoutPanel1.Controls.Add(generateProjectFileButtons);
        flowLayoutPanel1.Location = new Point(12, 13);
        flowLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new Size(488, 144);
        flowLayoutPanel1.TabIndex = 2;
        // 
        // authorLinkLabel
        // 
        authorLinkLabel.AutoSize = true;
        authorLinkLabel.Location = new Point(15, 264);
        authorLinkLabel.Name = "authorLinkLabel";
        authorLinkLabel.Size = new Size(213, 20);
        authorLinkLabel.TabIndex = 3;
        authorLinkLabel.TabStop = true;
        authorLinkLabel.Text = "By Miloš Jovanović - milosh-96";
        authorLinkLabel.LinkClicked += authorLinkLabel_LinkClicked;
        // 
        // versionLabel
        // 
        versionLabel.AutoSize = true;
        versionLabel.ImageAlign = ContentAlignment.TopLeft;
        versionLabel.Location = new Point(416, 264);
        versionLabel.Name = "versionLabel";
        versionLabel.Size = new Size(50, 20);
        versionLabel.TabIndex = 4;
        versionLabel.Text = "label1";
        versionLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(512, 293);
        Controls.Add(versionLabel);
        Controls.Add(authorLinkLabel);
        Controls.Add(flowLayoutPanel1);
        Margin = new Padding(3, 4, 3, 4);
        Name = "MainWindow";
        Text = "Omsi Spline Combiner";
        ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
        flowLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private OpenFileDialog openFileDialog1;
    private Button browseProjectFileButton;
    private FileSystemWatcher fileSystemWatcher1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button generateProjectFileButtons;
    private SaveFileDialog saveGeneratedProjectFileDialog;
    private LinkLabel authorLinkLabel;
    private Label versionLabel;
}
