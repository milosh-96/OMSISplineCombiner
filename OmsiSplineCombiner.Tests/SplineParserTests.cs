using NUnit.Framework;
using OMSISplineCombiner.Cli.Data;
using OMSISplineCombiner.Cli.Parsers;

namespace OmsiSplineCombiner.Tests;

[TestFixture]
public class SplineParserTests
{
    [Test]
    public void PrepareSpline_ShouldThrowArgumentException_IfEmptyContents()
    {
        Assert.Throws<ArgumentException>(() => SplineParser.PrepareSpline([], "test-spline1.sli"));
    }

    [Test]
    public void ReadProfiles_ShouldParseProfilesCorrectly()
    {
        var data = @"[profile]
0

[profilepnt]
-4.150
0.100
0.000
0.167

[profilepnt]
-4.050
0.100
0.033
0.167";

        var actual = SplineParser.ReadProfiles(data.Split("\n"));
        var expected = new Profile()
        {
            Points = new() {
            new ProfilePoint() {
                PositionX = -4.150f,
                Height = 0.1f,
                TexturePositionX = 0,
                StretchFactor = 0.167f
            },
            new ProfilePoint() {
                PositionX = -4.050f,
                Height = 0.1f,
                TexturePositionX = 0.033f,
                StretchFactor = 0.167f
            }
        },
            TextureId = 0
        };
        Assert.AreEqual(expected, actual?.FirstOrDefault());
    }
    [Test]
    public void ReadHeightProfile_ShouldParseProfilesCorrectly()
    {
        var data = @"[heightprofile]
-4.150
4.150
0.100
0.100";

        var actual = SplineParser.ReadHeightProfile(data.Split("\n"));
        var expected = new HeightProfile()
        {
            FromX = -4.150f,
            ToX = 4.150f,
            FromZ = 0.1f,
            ToZ = 0.1f
        };
        Assert.AreEqual(expected, actual?.FirstOrDefault());
    }
    [Test]
    public void ReadTextures_ShouldParseTexturesCorrectly()
    {
        var data = @"[texture]
str_asphdrk.bmp

[texture]
str_asphdrk_1C.bmp

[texture]
str_asphdrk_1line.bmp";

        var actual = SplineParser.ReadTextures(data.Split("\n"));
        var expected = new List<Texture>() {
            new Texture() { Id = 0, Name = "str_asphdrk.bmp"},
            new Texture() { Id = 1, Name = "str_asphdrk_1C.bmp"},
            new Texture() { Id = 0, Name = "str_asphdrk_1line.bmp"},
        };
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ReadTextures_ShouldParseTexturesWithPatchworkChainCorrectly()
    {
        var data = @"[texture]
DDR_Asphalt_variety.bmp
[patchwork_chain]
5
AAAAAABBACAADDAAA
3111113111113111
1111111111100011

[texture]
str_asphdrk_1C.bmp

[texture]
str_asphdrk_1line.bmp";

        var actual = SplineParser.ReadTextures(data.Split("\n"));
        var expected = new List<Texture>() {
            new Texture() { Id = 0, Name = "DDR_Asphalt_variety.bmp", PatchworkChain = new PatchworkChain()
            {
                SegmentLength = 5,
                ChainOfTransitions = "AAAAAABBACAADDAAA",
                ChainOfWeightFactors = "3111113111113111",
                Invertable = "1111111111100011"
            } },
            new Texture() { Id = 1, Name = "str_asphdrk_1C.bmp"},
            new Texture() { Id = 0, Name = "str_asphdrk_1line.bmp"},
        };
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ReadPaths_ShouldParsePathsCorrectly()
    {
        var data = @"---------------------------
          Paths
---------------------------

[path]
0
-1.875
0.100
3.535
1

[path]
0
1.875
0.100
3.535
0
";

        var actual = SplineParser.ReadPaths(data.Split("\n"));
        var expected = new List<OmsiPath>() {
            new OmsiPath() { Type = 0, PositionX = -1.875f, PositionZ = 0.1f, Width = 3.535f, Direction = OmsiPathDirection.Backwards},
            new OmsiPath() { Type = 0, PositionX = 1.875f, PositionZ = 0.1f, Width = 3.535f, Direction = OmsiPathDirection.Forward}
        };
        Assert.AreEqual(expected, actual);
    }

    [TestCase("profile", new int[] { 6, 23 })]
    [TestCase("profilepnt", new int[] { 9, 15, 26, 32})]
    public void FetchPositionsOfAttributes_ShouldParsePositionsCorrectly(string attribute, int[] expected)
    {
        var data = @"---------------------------
     Graphical Lanes
---------------------------

Grass strip:

[profile]
0

[profilepnt]
-4.150
0.100
0.000
0.167

[profilepnt]
-4.050
0.100
0.033
0.167

Grass strip:

[profile]
0

[profilepnt]
-4.050
0.100
0.033
0.167

[profilepnt]
-3.950
0.100
0.000
0.167";

        var actual = SplineParser.FetchPositionsOfAttribute(attribute, data.Split("\n"));
        Assert.AreEqual(expected, actual);
    }
}
