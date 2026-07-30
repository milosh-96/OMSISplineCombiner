using OMSISplineCombiner.Common.Data;
using OMSISplineCombiner.Common.Handlers;
using OMSISplineCombiner.Common.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Common;

public static class ProjectsService
{
    public static Spline? MakeCompleteSpline(Project project)
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
                    FileService.EnsureDirectoryExists(Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture", justFileName));
                    FileService.EnsureDirectoryExists(Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture\\WinterSnow", justFileName));
                    FileService.EnsureDirectoryExists(Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture\\WinterSnowfall", justFileName));

                    FileService.CopyTextureFile(Path.Combine(project.OmsiDirectoryPath, project.SplinesSourcePath, texture.FolderPath, "texture", justFileName), Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture", texture.ToString()));
                    FileService.CopyTextureFile(Path.Combine(project.OmsiDirectoryPath, project.SplinesSourcePath, texture.FolderPath, "texture\\WinterSnow", texture.ToString()), Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture\\WinterSnow", texture.ToString()));
                    FileService.CopyTextureFile(Path.Combine(project.OmsiDirectoryPath, project.SplinesSourcePath, texture.FolderPath, "texture\\WinterSnowfall", texture.ToString()), Path.Combine(project.OmsiDirectoryPath, project.SplinesOutputPath, "texture\\WinterSnowfall", texture.ToString()));
                }
                return completeSpline;
            }
        }
        return null;
    }

    public static List<Project> LoadProjects(string projectsFilePath)
    {
        var result = new List<Project>();
        List<Project>? projects = JsonSerializer.Deserialize<List<Project>>(
            File.ReadAllText(projectsFilePath));
        if (projects is not null && projects.Count > 0) { result.AddRange(projects); }
        return result;
    }
}
