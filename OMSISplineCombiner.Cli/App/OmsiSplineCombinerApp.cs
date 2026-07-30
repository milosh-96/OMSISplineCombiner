using OMSISplineCombiner.Cli.Data;
using OMSISplineCombiner.Cli.Handlers;
using OMSISplineCombiner.Cli.Parsers;
using OMSISplineCombiner.Cli.Writers;
using System.Numerics;
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
        _projects = LoadProjects();

        //var project = _projects.FirstOrDefault();



        foreach(var project in _projects)
        {

            //Console.WriteLine(string.Join(',',textures));
            string? userFileName = project.FileName ?? UserInput.GetFileName();
            if (project.OmsiDirectoryPath is not null && project.SplinesSourcePath is not null && project.SplinesOutputPath is not null)
            {
                var completeSpline = MakeCompleteSpline(project);
                string newSplinePath = Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, (!string.IsNullOrWhiteSpace(userFileName) ? userFileName : Guid.NewGuid().ToString()) + ".sli");

                EnsureDirectoryExists(newSplinePath);
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

    private static Spline? MakeCompleteSpline(Project project)
    {
        {
            List<Texture> textures = new List<Texture>();

            var files = new List<string>();
            var splines = new List<Spline>();

            if (project.OmsiDirectoryPath is not null && project.SplinesSourcePath is not null && project.SplinesOutputPath is not null)
            {
                files.AddRange(project.SplinesInputs.Select(input => input.Path));
                //splines.AddRange(SplineParser.GetSplines(files, project.OmsiDirectoryPath, project.SplinesSourcePath));

                foreach (var splineInput in project.SplinesInputs)
                {
                    var filePath = Path.Combine(project.OmsiDirectoryPath!, project.SplinesSourcePath!, splineInput.Path);
                    var spline = SplineParser.PrepareSpline(File.ReadAllLines(filePath), splineInput.Path);
                    spline.Textures.ForEach(texture => { if (!textures.Contains(texture)) { textures.Add(texture); } });

                    float xOffset = splineInput.Settings.XOffset;
                    float zOffset = splineInput.Settings.ZOffset;

                    spline.Profiles.ForEach(
                            profile =>
                            {
                                profile.TextureName = spline.Textures[profile.TextureId].Name;
                                Texture? texture = textures.FirstOrDefault(texture => texture.Name == profile.TextureName) ?? throw new InvalidOperationException("Couldn't find a texture.");
                                profile.TextureId = textures.IndexOf(texture);
                            }
                   );
                    spline = SplineHandler.ApplyXOffset(spline, xOffset);
                    spline = SplineHandler.ApplyZOffset(spline, zOffset);
                    splines.Add(spline);
                }

                var completeSpline = new Spline();
                foreach (var spline in splines)
                {
                    completeSpline.HeightProfiles.AddRange(spline.HeightProfiles);
                    completeSpline.Profiles.AddRange(spline.Profiles);
                    completeSpline.Paths.AddRange(spline.Paths);
                    //Console.WriteLine('+' + string.Join(',', string.Join(',', spline.Profiles.Select(profile => profile.TextureName))));
                }
                completeSpline.Textures.AddRange(textures);

                foreach (Texture texture in completeSpline.Textures)
                {
                    string justFileName = Regex.Match(texture.Name, @"[^\\\/]+$").Value;
                    EnsureDirectoryExists(Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture", justFileName));
                    EnsureDirectoryExists(Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture\\WinterSnow", justFileName));
                    EnsureDirectoryExists(Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture\\WinterSnowfall", justFileName));

                    CopyTextureFile(Path.Combine(project.OmsiDirectoryPath, project.SplinesSourcePath, "texture", justFileName), Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture", texture.ToString()));
                    CopyTextureFile(Path.Combine(project.OmsiDirectoryPath, project.SplinesSourcePath, texture.FolderPath, "texture\\WinterSnow", texture.ToString()), Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture\\WinterSnow", texture.ToString()));
                    CopyTextureFile(Path.Combine(project.OmsiDirectoryPath, project.SplinesSourcePath, texture.FolderPath, "texture\\WinterSnowfall", texture.ToString()), Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture\\WinterSnowfall", texture.ToString()));
                }
                return completeSpline;
            }
        }
        return null;
    }

    private List<Project> LoadProjects()
    {
        var result = new List<Project>();
        List<Project>? projects = JsonSerializer.Deserialize<List<Project>>(
            File.ReadAllText(ProjectsFilePath!));
        if (projects is not null && projects.Count > 0) { result.AddRange(projects); }
        return result;
    }

    private static void CopyTextureFile(string path, string destination)
    {
        if (File.Exists(path))
        {
            File.Copy(path, destination, true);

            var cfgFile = path + ".cfg";

            if (File.Exists(cfgFile))
            {
                File.Copy(cfgFile, destination + ".cfg", true);
            }
        }
    }

    private static void EnsureDirectoryExists(string filePath)
    {
        FileInfo fi = new FileInfo(filePath);
        if (fi.Directory == null || !fi.Directory.Exists)
        {
            System.IO.Directory.CreateDirectory(fi.DirectoryName!);
        }
    }
}
