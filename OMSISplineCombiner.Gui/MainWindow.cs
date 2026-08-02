using Microsoft.VisualBasic;
using OMSISplineCombiner.Common;
using OMSISplineCombiner.Common.Constants;
using OMSISplineCombiner.Common.Writers;
using System.Text.Json;

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
        openFileDialog1.FileName = "my-project.json";
        openFileDialog1.ShowDialog();
    }

    private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var file = openFileDialog1.FileName;

        try
        {
            var projects = ProjectsService.LoadProjects(file);

            //var project = _projects.FirstOrDefault();

            foreach (var project in projects)
            {

                if (project.OmsiDirectoryPath is not null && project.SplinesSourcePath is not null && project.SplinesOutputPath is not null)
                {
                    string? userFileName = Guid.NewGuid().ToString();
                    if(string.IsNullOrEmpty(project.FileName))
                    {
                        var inputBox = Interaction.InputBox("Enter file name without sli");
                        if(!string.IsNullOrEmpty(inputBox))
                        {
                            userFileName = inputBox;
                        }
                    }
                    else
                    {
                        userFileName = project.FileName;
                    }
                    
                    var completeSpline = ProjectsService.MakeCompleteSpline(project);
                    string newSplinePath = Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, (!string.IsNullOrWhiteSpace(userFileName) ? userFileName : Guid.NewGuid().ToString()) + ".sli");

                    FileService.EnsureDirectoryExists(newSplinePath);
                    if (File.Exists(newSplinePath))
                    {
                        var warningMessage = MessageBox.Show("FILE EXISTS! Do you want to overwrite?", "Warning", MessageBoxButtons.YesNo);
                        if (warningMessage != DialogResult.Yes)
                        {
                            continue;
                        }
                    }
                    if (completeSpline is not null)
                    {
                        SplineWriter.Write(newSplinePath, completeSpline);
                        MessageBox.Show($"Exported to {newSplinePath}");
                    }
                }

            }
        }
        catch (JsonException ex)
        {
            var messageBox = MessageBox.Show(
                "Invalid JSON file, please select the correct project file or generate one.",
                "Error",
                MessageBoxButtons.RetryCancel
           );

            if(messageBox == DialogResult.Retry)
            {
                openFileDialog1.ShowDialog();
            }
        }
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

    private void MainWindow_Load(object sender, EventArgs e)
    {

    }
}
