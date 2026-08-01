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

            if (points.Count == 3)
            {
                var temp = points[0].ShallowCopy();

                points[0].PositionX = points[0].PositionX;
                points[0].Height = points[0].Height;
                points[0].TexturePositionX = points[2].TexturePositionX;

                points[1].PositionX = -points[1].PositionX;
                points[1].TexturePositionX = points[1].TexturePositionX;
                points[1].Height = points[1].Height;
                
                points[2].PositionX = points[2].PositionX;
                points[2].TexturePositionX = temp.TexturePositionX;
                points[2].Height = points[2].Height;
            }
            else
            {
                ProfilePoint temp = null;
                for (int i = 0; i < points.Count; i++)
                {
                    try
                    {
                        temp = points[i].ShallowCopy();
                        points[i].PositionX = -points[i + 1].PositionX;
                        points[i].TexturePositionX = points[i + 1].TexturePositionX;
                        points[i].Height = points[i + 1].Height;

                        points[i + 1].PositionX = -temp.PositionX;
                        points[i + 1].TexturePositionX = temp.TexturePositionX;
                        points[i + 1].Height = temp.Height;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        continue;
                    }
                }
            }
        }

        foreach(var path in spline.Paths)
        {
            path.PositionX = -path.PositionX;
            if(path.Direction == OmsiPathDirection.Forward || path.Direction == OmsiPathDirection.Backwards)
            {
                path.Direction = path.Direction == OmsiPathDirection.Forward ? OmsiPathDirection.Backwards : OmsiPathDirection.Forward;
            }   
        }

        return spline;
    }
}
