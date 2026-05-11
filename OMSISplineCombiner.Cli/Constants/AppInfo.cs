using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Cli.Constants;
public static class AppInfo
{
    public const string Name = "OMSI Spline Combiner";
    public const string Author = "Miloš Jovanović";
    public const string AppHeader = $"Generated with {Name} by M96";

    public const string ConfigFile = "config.txt";
    public static Encoding GetDefaultEncoding() => Encoding.GetEncoding("iso-8859-1");
}
