using OMSISplineCombiner.Cli.Constants;
using OMSISplineCombiner.Cli.Data;
using OMSISplineCombiner.Cli.Parsers;
using OMSISplineCombiner.Cli.Writers;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace OMSISplineCombiner.Cli.App;

public class OmsiSplineCombinerApp
{
    public string OmsiDirectory { get; set; } = @"C:\Program Files (x86)\Steam\steamapps\common\OMSI 2";
    public string SplinesSourceDirectory { get; set; } = @"Splines";
    public string DestinationDirectory { get; set; } = @"Splines\MySplines";

    //private List<string> _files = ["Chodnik_kraweznik_1,5m.sli", "Asfalt_3m.sli", "linia_przerywana.sli"];
    private List<string> _files = new();

    public void Run()
    {
        LoadConfiguration();

        _files = UserInput.AskForFiles(SplinesSourceDirectory, OmsiDirectory);

        List<Texture> textures = new List<Texture>();

        var splines = SplineParser.GetSplines(_files, OmsiDirectory, SplinesSourceDirectory);

        // todo: extract stuff from this main method
        for (int i = 0; i < splines.Count; i++)
        {
            var spline = splines[i];
            spline.Textures.ForEach(texture => { if (!textures.Contains(texture)) { textures.Add(texture); } });
            Console.WriteLine($"Enter the offset for the current spline (#{i + 1})");
            string? offsetInput = Console.ReadLine();
            if (offsetInput is null) { throw new ArgumentNullException(nameof(offsetInput)); }
            float offset = float.Parse(offsetInput);
            spline.HeightProfiles.ForEach(profile => { profile.FromX += offset; profile.ToX += offset; });
            spline.Profiles.ForEach(
                    profile =>
                    {
                        profile.TextureName = spline.Textures[profile.TextureId].Name;
                        Texture? texture = textures.FirstOrDefault(texture => texture.Name == profile.TextureName) ?? throw new InvalidOperationException("Couldn't find a texture.");
                        profile.TextureId = textures.IndexOf(texture);
                        //if(i > 0)
                        //{
                        //    profile.TextureId += 1 + splines[i-1].Profiles.Max(profile=>profile.TextureId);
                        //}
                        profile.Points.ForEach(
                            point => point.PositionX += offset
                    );
                    });
            spline.Paths.ForEach(
                    path =>
                    {
                        path.PositionX += offset;
                    });
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
        
        foreach(Texture texture in completeSpline.Textures)
        {
            string justFileName = Regex.Match(texture.Name, @"[^\\\/]+$").Value;
            EnsureDirectoryExists($"{OmsiDirectory}\\{DestinationDirectory}\\texture\\{justFileName}");
            EnsureDirectoryExists($"{OmsiDirectory}\\{DestinationDirectory}\\texture\\WinterSnow\\{justFileName}");
            EnsureDirectoryExists($"{OmsiDirectory}\\{DestinationDirectory}\\texture\\WinterSnowfall\\{justFileName}");

            CopyTextureFile($"{OmsiDirectory}\\{SplinesSourceDirectory}\\{texture.FolderPath}\\texture\\{texture}", $"{OmsiDirectory}\\{DestinationDirectory}\\texture\\{texture}");
            CopyTextureFile($"{OmsiDirectory}\\{SplinesSourceDirectory}\\{texture.FolderPath}\\texture\\WinterSnow\\{texture}", $"{OmsiDirectory}\\{DestinationDirectory}\\texture\\WinterSnow\\{texture}");
            CopyTextureFile($"{OmsiDirectory}\\{SplinesSourceDirectory}\\{texture.FolderPath}\\texture\\WinterSnowfall\\{texture}", $"{OmsiDirectory}\\{DestinationDirectory}\\texture\\WinterSnowfall\\{texture}");
        }

        //Console.WriteLine(string.Join(',',textures));
        string newSplinePath = $"{OmsiDirectory}\\{DestinationDirectory}\\{Guid.NewGuid().ToString()}.sli";
        EnsureDirectoryExists(newSplinePath);
        SplineWriter.Write(newSplinePath, completeSpline);
        Console.WriteLine($"Exported to {newSplinePath}");
        Console.WriteLine(new string('*',32));
        Console.WriteLine("Press N to create a new spline; Press E to exit");
        ConsoleKeyInfo userInput = Console.ReadKey();
        if(userInput.Key == ConsoleKey.N)
        {
            Run();
        }
        else
        {
            return;
        }
    }

    

    private void CopyTextureFile(string path, string destination)
    {
        if(File.Exists(path))
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

    private void LoadConfiguration()
    {
        if(File.Exists(AppInfo.ConfigFile))
        {
            var configContents = File.ReadAllLines(AppInfo.ConfigFile).ToArray();
            if (configContents.Length > 0 && configContents[0] is not null)
            {
                OmsiDirectory = configContents[0];
            }
            if (configContents.Length > 1 && configContents[1] is not null)
            {
                SplinesSourceDirectory = configContents[1];
            }
            if (configContents.Length > 2 && configContents[2] is not null)
            {
                DestinationDirectory = configContents[2];
            }
        }
        SetConfiguration();
    }

    private void SetConfiguration()
    {
        Console.WriteLine($"Enter path to your OMSI directory. Current: {OmsiDirectory}");
        string? omsiPath = Console.ReadLine();

        if(string.IsNullOrEmpty(omsiPath))
        {
            omsiPath = OmsiDirectory;
        }

        Console.WriteLine($"Enter path to your splines directory. Current: {SplinesSourceDirectory}");
        string? splinesSourceDirectory = Console.ReadLine() ?? SplinesSourceDirectory;

        if (string.IsNullOrEmpty(splinesSourceDirectory))
        {
            splinesSourceDirectory = SplinesSourceDirectory;
        }

        Console.WriteLine($"Enter path where new splines will be saved. Current: {DestinationDirectory}");
        string? destinationDirectory = Console.ReadLine() ?? DestinationDirectory;

        if (string.IsNullOrEmpty(destinationDirectory))
        {
            destinationDirectory = DestinationDirectory;
        }


        File.WriteAllText(AppInfo.ConfigFile, string.Empty);
        var file = File.OpenWrite(AppInfo.ConfigFile);
        
        using StreamWriter writer = new StreamWriter(file);
        writer.WriteLine(omsiPath);
        writer.WriteLine(splinesSourceDirectory);
        writer.WriteLine(destinationDirectory);

        OmsiDirectory = omsiPath;
        SplinesSourceDirectory = splinesSourceDirectory;
        DestinationDirectory = destinationDirectory;

    }
}
