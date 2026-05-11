using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Cli.App;
public static class UserInput
{
    public static List<string> AskForFiles(string splinesSourceDirectory, string omsiDirectory)
    {
        List<string> files = new List<string>();
        string? input = "";
        string fullFilePath = omsiDirectory + "\\" + splinesSourceDirectory + "\\" + input;
        do
        {

            Console.WriteLine($"Enter path to your spline ({splinesSourceDirectory}/...). If you want to repeat one spline multiple times, add them again. Press 'f' if you finished adding splines.");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                fullFilePath = omsiDirectory + "\\" + splinesSourceDirectory + "\\" + input;
                if (input.ToLower() == "f")
                {
                    break;
                }
                if(!File.Exists(fullFilePath))
                {
                    Console.WriteLine($"{fullFilePath} doesn't exist.");
                }
                else
                {
                    files.Add(input);
                }
            }
            Console.Write($"****\nCurrent files: \n{string.Join('\n', files)}***\n");
        }
        while (string.IsNullOrEmpty(input) || !File.Exists(fullFilePath) ||  input.ToLower() != "f");
        return files;
    }
}
