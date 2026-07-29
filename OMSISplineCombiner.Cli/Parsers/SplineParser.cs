using OMSISplineCombiner.Cli.Constants;
using OMSISplineCombiner.Cli.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Cli.Parsers;
public static class SplineParser
{
    public static List<Spline> GetSplines(List<string> files, string omsiDirectory, string splinesSourceDirectory)
    {
        var splines = new List<Spline>();

        foreach (var file in files)
        {
            try
            {
                Spline spline = PrepareSpline(omsiDirectory, splinesSourceDirectory, file);
                splines.Add(spline);
            }
            catch
            {
                continue;
            }
        }
        return splines;
    }

    public static Spline PrepareSpline(string omsiDirectory, string splinesSourceDirectory, string file)
    {
        string filePath = omsiDirectory + "\\" + splinesSourceDirectory + '\\' + file;
        if(!File.Exists(filePath)) { throw new FileNotFoundException(); }
        var fileContents = File.ReadAllLines(file, AppInfo.GetDefaultEncoding()).ToArray();
        var spline = new Spline()
        {
            HeightProfiles = ReadHeightProfile(fileContents),
            Textures = ReadTextures(fileContents, Regex.Match(file, @".*(?=[\\/])").Value),
            Profiles = ReadProfiles(fileContents),
            Paths = ReadPaths(fileContents)
        };
        return spline;
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

    private static List<Texture> ReadTextures(string[] fileContents, string splineFolderPath = "/")
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
            string name = data[0];
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
}
