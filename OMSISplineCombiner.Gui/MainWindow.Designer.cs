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
        browseProjectFileButton.Location = new Point(3, 3);
        browseProjectFileButton.Name = "browseProjectFileButton";
        browseProjectFileButton.Size = new Size(242, 23);
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
        generateProjectFileButtons.Location = new Point(251, 3);
        generateProjectFileButtons.Name = "generateProjectFileButtons";
        generateProjectFileButtons.Size = new Size(139, 23);
        generateProjectFileButtons.TabIndex = 1;
        generateProjectFileButtons.Text = "Generate Project File";
        generateProjectFileButtons.UseVisualStyleBackColor = true;
        generateProjectFileButtons.Click += generateProjectFileButtons_Click;
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.Controls.Add(browseProjectFileButton);
        flowLayoutPanel1.Controls.Add(generateProjectFileButtons);
        flowLayoutPanel1.Location = new Point(115, 97);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new Size(612, 100);
        flowLayoutPanel1.TabIndex = 2;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(flowLayoutPanel1);
        Name = "MainWindow";
        Text = "Omsi";
        ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
        flowLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private OpenFileDialog openFileDialog1;
    private Button browseProjectFileButton;
    private FileSystemWatcher fileSystemWatcher1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button generateProjectFileButtons;
    private SaveFileDialog saveGeneratedProjectFileDialog;
}
