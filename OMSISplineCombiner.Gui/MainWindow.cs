using OMSISplineCombiner.Common;

namespace OMSISplineCombiner.Gui;

public partial class MainWindow : Form
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public string Message { get; set; } = "";
    private void browseProjectFileButton_Click(object sender, EventArgs e)
    {
        openFileDialog1.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        openFileDialog1.ShowDialog();
    }

    private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Message = openFileDialog1.FileName;
    }

    private void generateProjectFileButtons_Click(object sender, EventArgs e)
    {
        saveGeneratedProjectFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        var json = ProjectJsonGenerator.Generate();
        if (saveGeneratedProjectFileDialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(saveGeneratedProjectFileDialog.FileName, json);
            MessageBox.Show("File has been saved.", "Success");
        }
    }
}
