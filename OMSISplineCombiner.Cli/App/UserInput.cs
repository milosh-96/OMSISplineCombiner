using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Cli.App;
public static class UserInput
{
    public static List<string> AskForFiles(string splinesSourceDirectory)
    {
        List<string> files = new List<string>();
        string? input = "";
        do
        {

            Console.WriteLine($"Enter path to your spline ({splinesSourceDirectory}/...). If you want to repeat one spline multiple times, add them again. Press 'f' if you finished adding splines.");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                if (input.ToLower() != "f")
                {
                    files.Add(input);
                }
            }
        }
        while (input is null || input.ToLower() != "f");
        return files;
    }
}
