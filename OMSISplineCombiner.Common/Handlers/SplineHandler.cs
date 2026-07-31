using OMSISplineCombiner.Common.Data;
using System.Numerics;

namespace OMSISplineCombiner.Common.Handlers;
public static class SplineHandler
{
    public static Spline ApplyXOffset(Spline spline, float offset)
    {
        spline.HeightProfiles.ForEach(profile => { profile.FromX += offset; profile.ToX += offset; });
        spline.Profiles.ForEach(
                profile =>
                {
                    profile.Points.ForEach(
                    point => point.PositionX += offset
                );
                });
        spline.Paths.ForEach(
                path =>
                {
                    path.PositionX += offset;
                });
        return spline;
    }
    public static Spline ApplyZOffset(Spline spline, float offset)
    {
        spline.HeightProfiles.ForEach(profile => { profile.FromZ += offset; profile.ToZ += offset; });
        spline.Profiles.ForEach(
                profile =>
                {
                    profile.Points.ForEach(
                    point => point.Height += offset
                );
                });
        spline.Paths.ForEach(
                path =>
                {
                    path.PositionZ += offset;
                });
        return spline;
    }

    public static Spline ApplyMirror(Spline spline)
    { 
        foreach(var profile in spline.Profiles)
        {
            var points = profile.Points;
            ProfilePoint temp = points[0].ShallowCopy();

            points[0].PositionX = -points[1].PositionX;
            points[0].TexturePositionX = points[1].TexturePositionX;
            points[0].Height = points[1].Height;

            points[1].PositionX = -temp.PositionX;
            points[1].TexturePositionX = temp.TexturePositionX;
            points[1].Height = temp.Height;
        }
        return spline;
    }
}
