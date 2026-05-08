using OMSISplineCombiner.Cli.Data;
using OMSISplineCombiner.Cli.Writers;
using System.Diagnostics;

namespace OMSISplineCombiner.Cli.App;

public class OmsiSplineCombinerApp
{
    public bool FirstRun = true;
    public string OmsiDirectory { get; init; } = @"C:\Program Files (x86)\Steam\steamapps\common\OMSI 2\";
    public string SplinesSourceDirectory { get; init; } = @"Splines\Ruede";
    public string DestinationDirectory { get; init; } = @"Splines\MySplines";

    //private List<string> _files = ["Chodnik_kraweznik_1,5m.sli", "Asfalt_3m.sli", "linia_przerywana.sli"];
    private List<string> _files = new();


    public void Run()
    {
        LoadConfiguration();

        string? input = "";
        do
        {

            Console.WriteLine($"Enter path to your spline ({SplinesSourceDirectory}/...). If you want to repeat one spline multiple times, add them again. Press 'f' if you finished adding splines.");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                if (input.ToLower() != "f")
                {
                    _files.Add(input);
                }
            }
        }
        while (input is null || input.ToLower() != "f");
        Stopwatch stopwatch = Stopwatch.StartNew();
        var splines = new List<Spline>();
        List<Texture> textures = new List<Texture>();

        foreach (var _file in _files)
        {
            var fileContents = File.ReadAllLines(OmsiDirectory + SplinesSourceDirectory + '\\' + _file).ToArray();
            var spline = new Spline()
            {
                HeightProfiles = ReadHeightProfile(fileContents),
                Textures = ReadTextures(fileContents),
                Profiles = ReadProfiles(fileContents),
                Paths = ReadPaths(fileContents)
            };
            splines.Add(spline);
        }

        int texturesCount = splines.First().Textures.Count;
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

            texturesCount = spline.Textures.Count;
        }

        var completeSpline = new Spline();
        foreach (var spline in splines)
        {
            completeSpline.HeightProfiles.AddRange(spline.HeightProfiles);
            completeSpline.Profiles.AddRange(spline.Profiles);
            completeSpline.Paths.AddRange(spline.Paths);
            Console.WriteLine('+' +
                string.Join(',', string.Join(',', spline.Profiles.Select(profile => profile.TextureName))));
        }
        completeSpline.Textures.AddRange(textures);
        
        foreach(Texture texture in completeSpline.Textures)
        {
            EnsureDirectoryExists($"{OmsiDirectory}{DestinationDirectory}\\texture\\{texture}");
            File.Copy($"{OmsiDirectory}{SplinesSourceDirectory}\\texture\\{texture}", $"{OmsiDirectory}{DestinationDirectory}\\texture\\{texture}");
            File.Copy($"{OmsiDirectory}{SplinesSourceDirectory}\\texture\\{texture}.cfg", $"{OmsiDirectory}{DestinationDirectory}\\texture\\{texture}.cfg");
        }

        stopwatch.Stop();

        //Console.WriteLine(string.Join(',',textures));
        string newSplinePath = $"{OmsiDirectory}{DestinationDirectory}\\{Guid.NewGuid().ToString()}.sli";
        EnsureDirectoryExists(newSplinePath);
        SplineWriter.Write(newSplinePath, completeSpline);
        Console.WriteLine(newSplinePath);
        Console.ReadKey();

    }

    private static List<HeightProfile> ReadHeightProfile(string[] fileContents)
    {
        List<int> positions = FetchPositionsOfAttribute("heightprofile", fileContents);
        List<HeightProfile> heightProfiles = new List<HeightProfile>(positions.Count);
        foreach (int position in positions)
        {
            var profileData = fileContents.Skip(position + 1).Take(4).ToList();
            HeightProfile heightProfile = new HeightProfile()
            {
                FromX = float.Parse(profileData[0]),
                ToX = float.Parse(profileData[1]),
                FromZ = float.Parse(profileData[2]),
                ToZ = float.Parse(profileData[3]),
            };
            heightProfiles.Add(heightProfile);
        }
        return heightProfiles;
    }

    private static List<Texture> ReadTextures(string[] fileContents)
    {
        List<int> positions = FetchPositionsOfAttribute("texture", fileContents);
        List<Texture> textures = new List<Texture>(positions.Count);

        foreach (int position in positions)
        {
            var textureContents = fileContents.Skip(position + 1);
            var data = textureContents.Take(1).ToList();
            PatchworkChain? patchworkChain = null;
            var patchworkChainPositions = FetchPositionsOfAttribute("patchwork_chain", textureContents.ToArray());
            
            foreach(int patchworkChainPosition in patchworkChainPositions) { 
                var patchworkChainData = textureContents.Skip(patchworkChainPosition + 1).Take(4).ToList();
                try
                {
                    patchworkChain = new PatchworkChain()
                    {
                        SegmentLength = int.Parse(patchworkChainData[0]),
                        ChainOfTransitions = patchworkChainData[1],
                        ChainOfWeightFactors = patchworkChainData[2],
                        Invertable = patchworkChainData[3]
                    };
                }
                catch(FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
            Texture texture = new Texture()
            {
                Id = 0,
                Name = data[0],
                PatchworkChain = patchworkChain
            };
            textures.Add(texture);
        }

        return textures;
    }
    private static List<OmsiPath> ReadPaths(string[] fileContents)
    {
        List<int> positions = FetchPositionsOfAttribute("path", fileContents);
        List<OmsiPath> paths = new List<OmsiPath>(positions.Count);

        foreach (int position in positions)
        {
            var data = fileContents.Skip(position + 1).Take(5).ToList();
            OmsiPath path = new OmsiPath()
            {
                Type = (OmsiPathType)Enum.Parse(typeof(OmsiPathType), data[0]),
                PositionX = float.Parse(data[1]),
                PositionZ = float.Parse(data[2]),
                Width = float.Parse(data[3]),
                Direction = (OmsiPathDirection)Enum.Parse(typeof(OmsiPathDirection), data[4])
            };
            paths.Add(path);
        }
        return paths;
    }

    private static List<Profile> ReadProfiles(string[] fileContents)
    {
        List<int> positions = FetchPositionsOfAttribute("profile", fileContents);
        List<Profile> profiles = new List<Profile>(positions.Count);
        foreach (int position in positions)
        {
            var data = fileContents.Skip(position + 1).Take(1).ToList();
            var profilePointContents = new List<string>();
            List<string> profileFileContents = new List<string>();
            
            foreach(string line in fileContents.Skip(position + 1).ToList())
            {
                if(line.Contains("[profile]"))
                {
                    break;
                }
                profileFileContents.Add(line);
            }

            List<ProfilePoint> profilePoints = new List<ProfilePoint>();

            List<int> profilePointsPositions = FetchPositionsOfAttribute("profilepnt", profileFileContents.ToArray());

            foreach (var profilePointPosition in profilePointsPositions)
            {
                var profilePointData = profileFileContents.Skip(profilePointPosition + 1).Take(4).ToList();
                ProfilePoint profilePoint = new ProfilePoint()
                {
                    PositionX = float.Parse(profilePointData[0]),
                    Height = float.Parse(profilePointData[1]),
                    TexturePositionX = float.Parse(profilePointData[2]),
                    StretchFactor = float.Parse(profilePointData[3]),
                };
                profilePoints.Add(profilePoint);
            }


            Profile profile = new Profile()
            {
                TextureId = int.Parse(data[0]),
                Points = profilePoints
            };
            profiles.Add(profile);
        }
        return profiles;
    }

    private static List<int> FetchPositionsOfAttribute(string attribute, string[] fileContents)
    {
        var positions = new List<int>();

        for (int i = 0; i < fileContents.Count(); i++)
        {
            if (fileContents[i].Trim() == $"[{attribute}]")
            {
                positions.Add(i);
            }
        }

        return positions;
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
        if (FirstRun)
        {
            SetConfiguration();
        }
        else
        {
            // Read from the config file
        }
    }

    private void SetConfiguration()
    {
        Console.WriteLine($"Enter path to your OMSI directory. Default: {OmsiDirectory}");
        string? omsiPath = Console.ReadLine();

        Console.WriteLine($"Enter path to your splines directory. Default: {SplinesSourceDirectory}");
        string? splinesSourceDirectory = Console.ReadLine();

        Console.WriteLine($"Enter path where new splines will be saved. Default: {DestinationDirectory}");
        string? desinationDirectory = Console.ReadLine();
    }
}
