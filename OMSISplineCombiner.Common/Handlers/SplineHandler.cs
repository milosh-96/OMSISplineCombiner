using OMSISplineCombiner.Common.Data;

namespace OMSISplineCombiner.Common.Handlers;
internal static class SplineHandler
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
}
