using OMSISplineCombiner.Cli.Constants;
using OMSISplineCombiner.Cli.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Cli.Writers;
public static class SplineWriter
{
    public static void Write(string path, Spline spline)
    {
        var file = File.OpenWrite(path);
        using StreamWriter writer = new StreamWriter(file);
        writer.WriteLine($"{new string('-', AppInfo.AppHeader.Length)}\n{AppInfo.AppHeader}\n{new string('-', AppInfo.AppHeader.Length)}\n\n");
        writer.WriteLine(
       string.Join('\n', spline.HeightProfiles.Select(heightProfile => heightProfile.Output())) + '\n'
       );


        writer.WriteLine(
          string.Join('\n', spline.Textures.Select(texture => texture.Output())) + '\n'
          );
        writer.WriteLine(
            string.Join('\n', spline.Profiles.Select(profile => profile.Output())) + '\n'
            );
    }

    [Obsolete]
    public static void WriteSplines(string path, List<Spline> splines)
    {
        var file = File.OpenWrite(path);
        using StreamWriter writer = new StreamWriter(file);
        writer.WriteLine($"{new string('-', AppInfo.AppHeader.Length)}\n{AppInfo.AppHeader}\n{new string('-', AppInfo.AppHeader.Length)}\n\n");

        foreach (var spline in splines)
        {
            writer.WriteLine(
           string.Join('\n', spline.HeightProfiles.Select(heightProfile => heightProfile.Output())) + '\n'
           );
        }
        foreach (var spline in splines)
        {
            writer.WriteLine(
              string.Join('\n', spline.Textures.Select(texture => texture.Output())) + '\n'
              );
        }
        foreach (var spline in splines)
        {
            writer.WriteLine(
                string.Join('\n', spline.Profiles.Select(profile => profile.Output())) + '\n'
                );
        }

    }
    //File.AppendAllLines(path, spline.HeightProfiles.Select(heightProfile => heightProfile.Output()));
    //File.AppendAllLines(path, spline.Textures.Select(texture => texture.Output()));
    //File.AppendAllLines(path, spline.Profiles.Select(profile => profile.Output()));
}
