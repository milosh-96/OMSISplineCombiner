using OMSISplineCombiner.Cli.Data;
using OMSISplineCombiner.Cli.Writers;
using System.Diagnostics;

namespace OMSISplineCombiner.Cli.App;

public class OmsiSplineCombinerApp
{
    public bool FirstRun = true;
    public string OmsiDirectory { get; init; } = @"C:\Program Files (x86)\Steam\steamapps\common\OMSI 2\";
    public string SplinesSourceDirectory { get; init; } = @"Splines\";
    public string DestinationDirectory { get; init; } = @"CombinedSplines";

    //private List<string> _files = ["Chodnik_kraweznik_1,5m.sli", "Asfalt_3m.sli", "linia_przerywana.sli"];
    private List<string> _files = new();
    
    public void OMSISplineCombiner()
    {
        if(FirstRun)
        {
            // Get the config from the user
        }
        else
        {
            // Read from the config file
        }
    }

    
    public void Run()
    {
        string? input = "";
        do
        {

            Console.WriteLine($"Enter path to your spline ({SplinesSourceDirectory}/...)");
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
            };
            splines.Add(spline);
        }

        int texturesCount = splines.First().Textures.Count;
        for (int i = 0; i < splines.Count; i++)
        {
            var spline = splines[i];
            spline.Textures.ForEach(texture => { if (!textures.Contains(texture)) { textures.Add(texture); } });
            string? offsetInput = Console.ReadLine();
            if(offsetInput is null) { throw new ArgumentNullException(nameof(offsetInput)); }
            float offset = float.Parse(offsetInput);
            spline.HeightProfiles.ForEach(profile => { profile.FromX += offset; profile.ToX += offset; });
            spline.Profiles.ForEach(
                    profile =>
                    {
                        profile.TextureName = spline.Textures[profile.TextureId].Name;
                        Texture? texture = textures.FirstOrDefault(texture => texture.Name == profile.TextureName) ?? throw new NullReferenceException("Couldn't find a texture.");
                        profile.TextureId = textures.IndexOf(texture);
                        //if(i > 0)
                        //{
                        //    profile.TextureId += 1 + splines[i-1].Profiles.Max(profile=>profile.TextureId);
                        //}
                        profile.Points.ForEach(
                            point => point.PositionX += offset
                    );
                    });
            texturesCount = spline.Textures.Count;
        }

        var completeSpline = new Spline();
        foreach (var spline in splines)
        {
            completeSpline.HeightProfiles.AddRange(spline.HeightProfiles);
            completeSpline.Profiles.AddRange(spline.Profiles);

            Console.WriteLine('+' +
                string.Join(',', string.Join(',', spline.Profiles.Select(profile => profile.TextureName))));
        }
        completeSpline.Textures.AddRange(textures);


        stopwatch.Stop();

        //Console.WriteLine(string.Join(',',textures));
        SplineWriter.Write($"{OmsiDirectory + SplinesSourceDirectory}\\{Guid.NewGuid().ToString()}.sli", completeSpline);
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
            var data = fileContents.Skip(position + 1).Take(1).ToList();
            Texture texture = new Texture()
            {
                Id = 0,
                Name = data[0]
            };
            textures.Add(texture);
        }
        return textures;
    }

    private static List<Profile> ReadProfiles(string[] fileContents)
    {
        List<int> positions = FetchPositionsOfAttribute("profile", fileContents);
        List<Profile> profiles = new List<Profile>(positions.Count);
        foreach (int position in positions)
        {
            var data = fileContents.Skip(position + 1).Take(1).ToList();
            var profilePointContents = new List<string>();
            List<string> profileFileContents = fileContents.Skip(position + 1).ToList();

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
            if (fileContents[i].Contains($"[{attribute}]"))
            {
                positions.Add(i);
            }
        }

        return positions;
    }
}
