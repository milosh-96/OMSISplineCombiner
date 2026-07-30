using OMSISplineCombiner.Common;
using OMSISplineCombiner.Common.Data;
using OMSISplineCombiner.Common.Handlers;
using OMSISplineCombiner.Common.Parsers;
using OMSISplineCombiner.Common.Writers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OMSISplineCombiner.Cli.App;

public class OmsiSplineCombinerApp
{
    private List<Project> _projects = new();
    private string? ProjectsFilePath { get; set; }

    public OmsiSplineCombinerApp(string? projectsFilePath)
    {
        ProjectsFilePath = projectsFilePath ?? throw new ArgumentNullException(nameof(projectsFilePath));
    }

    public void Run()
    {
        //LoadConfiguration();
        _projects = ProjectsService.LoadProjects(ProjectsFilePath!);

        //var project = _projects.FirstOrDefault();



        foreach(var project in _projects)
        {

            //Console.WriteLine(string.Join(',',textures));
            string? userFileName = project.FileName ?? UserInput.GetFileName();
            if (project.OmsiDirectoryPath is not null && project.SplinesSourcePath is not null && project.SplinesOutputPath is not null)
            {
                var completeSpline = ProjectsService.MakeCompleteSpline(project);
                string newSplinePath = Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, (!string.IsNullOrWhiteSpace(userFileName) ? userFileName : Guid.NewGuid().ToString()) + ".sli");

                FileService.EnsureDirectoryExists(newSplinePath);
                if (File.Exists(newSplinePath))
                {
                    Console.WriteLine("FILE EXISTS! Do you want to overwrite?");
                    if (Console.ReadLine()?.ToLower() != "y")
                    {
                        continue;
                    }
                }
                if (completeSpline is not null) {
                    SplineWriter.Write(newSplinePath, completeSpline);
                    Console.WriteLine($"Exported to {newSplinePath}");
                    Console.WriteLine(new string('*', 32));
                }
            }

        }



        Console.WriteLine("Press N to create a new spline; Press E to exit");
        ConsoleKeyInfo userInput = Console.ReadKey();
        Console.WriteLine();
        if (userInput.Key == ConsoleKey.N)
        {
            Console.WriteLine(new string('*', 25));
            Run();
        }
        else
        {
            return;
        }
    }
}
