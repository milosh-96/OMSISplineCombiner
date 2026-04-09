using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Cli.Data;
public class Spline
{
    public List<Texture> Textures = new();
    public List<HeightProfile> HeightProfiles = new();
    public List<Profile> Profiles = new();
    public List<OmsiPath> Paths = new();


    public override string ToString()
    {
        string output = "";
        foreach(var profile in HeightProfiles)
        {
            output += profile + Environment.NewLine + "----" + Environment.NewLine;
        }
        return output;
    }
}
