using NUnit.Framework;
using OMSISplineCombiner.Common.Data;
using OMSISplineCombiner.Common.Handlers;

namespace OmsiSplineCombiner.Tests;

internal class SplineHandlerTests
{
    [Test]
    public void ApplyMirror_ShouldMirrorProfilePoints()
    {
        // Arrange
        var spline = new Spline
        {
            Profiles = new List<Profile>
            {
                new Profile
                {
                    Points = new List<ProfilePoint>
                    {
                        new ProfilePoint { PositionX = 0.1f, Height = 0.1f, TexturePositionX = 0.005f, StretchFactor = 0.167f },
                        new ProfilePoint { PositionX = 0.1f, Height = 0.25f, TexturePositionX = 0.995f, StretchFactor = 0.167f }
                    }
                }
            }
        };
        // Act
        var mirroredSpline = SplineHandler.ApplyMirror(spline);
        // Assert
        Assert.AreEqual(-0.1f, mirroredSpline.Profiles[0].Points[0].PositionX);
        Assert.AreEqual(0.995f, mirroredSpline.Profiles[0].Points[0].TexturePositionX);
        Assert.AreEqual(0.25f, mirroredSpline.Profiles[0].Points[0].Height);
        Assert.AreEqual(-0.1f, mirroredSpline.Profiles[0].Points[1].PositionX);
        Assert.AreEqual(0.005f, mirroredSpline.Profiles[0].Points[1].TexturePositionX);
        Assert.AreEqual(0.1f, mirroredSpline.Profiles[0].Points[1].Height);
    }

    [Test]
    public void ApplyMirror_WithThreeProfilePoints_ShouldMirrorCorrectly()
    {
        // Arrange
        var spline = new Spline
        {
            Profiles = new List<Profile>
            {
                new Profile
                {
                    Points = new List<ProfilePoint>
                    {
                        new ProfilePoint { PositionX = -3f, Height = 0.1f, TexturePositionX = 0.005f, StretchFactor = 0.167f },
                        new ProfilePoint { PositionX = 0.5f, Height = 0.25f, TexturePositionX = 0.995f, StretchFactor = 0.167f },
                        new ProfilePoint { PositionX = 3f, Height = 0.1f, TexturePositionX = 0.995f, StretchFactor = 0.167f }
                    }
                }
            }
        };
        // Act
        var mirroredSpline = SplineHandler.ApplyMirror(spline);
        // Assert
        Assert.AreEqual(-3f, mirroredSpline.Profiles[0].Points[0].PositionX);
        Assert.AreEqual(0.995f, mirroredSpline.Profiles[0].Points[0].TexturePositionX);
        Assert.AreEqual(0.1f, mirroredSpline.Profiles[0].Points[0].Height);

        Assert.AreEqual(-0.5f, mirroredSpline.Profiles[0].Points[1].PositionX);
        Assert.AreEqual(0.995f, mirroredSpline.Profiles[0].Points[1].TexturePositionX);
        Assert.AreEqual(0.25f, mirroredSpline.Profiles[0].Points[1].Height);

        Assert.AreEqual(3f, mirroredSpline.Profiles[0].Points[2].PositionX);
        Assert.AreEqual(0.005f, mirroredSpline.Profiles[0].Points[2].TexturePositionX);
        Assert.AreEqual(0.1f, mirroredSpline.Profiles[0].Points[2].Height);
    }

    [Test]
    public void ApplyMirror_ShouldMirrorPaths()
    {
        // Arrange
        var spline = new Spline
        {
            Paths = new List<OmsiPath>
            {
                new OmsiPath
                {
                    PositionZ = 0.1f,
                    PositionX = -2.5f,
                    Direction = OmsiPathDirection.Forward,
                    Type = OmsiPathType.RoadVehicles,
                    Width = 3.5f
                }
            }
        };
        // Act
        var mirroredSpline = SplineHandler.ApplyMirror(spline);
        // Assert
        Assert.AreEqual(2.5f, mirroredSpline.Paths[0].PositionX);
        Assert.AreEqual(OmsiPathDirection.Backwards, mirroredSpline.Paths[0].Direction);
    }
}
