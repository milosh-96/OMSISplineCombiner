using OMSISplineCombiner.Common.Data;
using System.Text.RegularExpressions;

namespace OMSISplineCombiner.Common.Parsers;

public static class SplineParser
{
    public static List<Spline> GetSplines(List<string> files, string? omsiDirectory, string? splinesSourceDirectory)
    {
        var splines = new List<Spline>();

        if(omsiDirectory is not null &&  splinesSourceDirectory is not null)
        {
            foreach (var file in files)
            {
                Spline spline = PrepareSpline(File.ReadAllLines(Path.Combine(omsiDirectory, splinesSourceDirectory,file)), file);
                splines.Add(spline);
            }
        }
        return splines;
    }

    public static Spline PrepareSpline(string[] fileContents, string file)
    {
        if(fileContents is null || fileContents.Length == 0) { throw new ArgumentException(); }
        fileContents = fileContents.ToArray();
        var spline = new Spline()
        {
            HeightProfiles = ReadHeightProfile(fileContents),
            Textures = ReadTextures(fileContents, Regex.Match(file, @".*(?=[\\/])").Value),
            Profiles = ReadProfiles(fileContents),
            Paths = ReadPaths(fileContents),
            Materials = ReadMaterials(fileContents)
        };
        return spline;
    }

    public static List<HeightProfile> ReadHeightProfile(string[] fileContents)
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

    public static List<Texture> ReadTextures(string[] fileContents, string splineFolderPath = "/")
    {
        List<int> positions = FetchPositionsOfAttribute("texture", fileContents);
        List<Texture> textures = new List<Texture>(positions.Count);

        foreach (int position in positions)
        {
            var textureContents = fileContents.Skip(position + 1);
            var data = textureContents.Take(1).ToList();
            PatchworkChain? patchworkChain = null;
            var patchworkChainPositions = FetchPositionsOfAttribute("patchwork_chain", textureContents.ToArray());
            int textureId = 0;
            foreach (int patchworkChainPosition in patchworkChainPositions)
            {
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
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
            //string name = Regex.Match(data[0], @"[^\\\/]+$").Value;
            string name = data[0].Trim();
            Texture texture = new Texture()
            {
                Id = textureId,
                Name = name,
                FolderPath = splineFolderPath,
                PatchworkChain = patchworkChain
            };
            textures.Add(texture);
            textureId++;
        }

        return textures;
    }
    public static List<OmsiPath> ReadPaths(string[] fileContents)
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

    public static List<Profile> ReadProfiles(string[] fileContents)
    {
        List<int> positions = FetchPositionsOfAttribute("profile", fileContents);
        List<Profile> profiles = new List<Profile>(positions.Count);
        foreach (int position in positions)
        {
            var data = fileContents.Skip(position + 1).Take(1).ToList();
            var profilePointContents = new List<string>();
            List<string> profileFileContents = new List<string>();

            foreach (string line in fileContents.Skip(position + 1).ToList())
            {
                if (line.Contains("[profile]"))
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

    public static List<int> FetchPositionsOfAttribute(string attribute, string[] fileContents)
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

    public static List<Material> ReadMaterials(string[] fileContents)
    {
        List<int> positions = FetchPositionsOfAttribute("matl", fileContents);
        if(!positions.Any())
        {
            positions = FetchPositionsOfAttribute("texture", fileContents);
        }

        List<Material> materials = new List<Material>(positions.Count);

        if (positions.Any())
        {
            foreach (int position in positions)
            {
                var data = fileContents.Skip(position + 1).ToList();
                var matlAlphaPositions = FetchPositionsOfAttribute("matl_alpha", fileContents);
                if(matlAlphaPositions.Any())
                {
                    var matlAlphaData = fileContents.Skip(matlAlphaPositions.First() + 1).Take(1).ToList();
                    Material material = new Material()
                    {
                        TextureName = data[0].Trim(),
                        AlphaValue = (MaterialAlphaValues)Enum.Parse(typeof(MaterialAlphaValues), matlAlphaData[0]),
                    };
                    materials.Add(material);
                }
            }
        }
        return materials;
    }
}
