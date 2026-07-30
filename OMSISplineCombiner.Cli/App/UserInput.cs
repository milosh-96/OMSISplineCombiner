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
            if (!string.IsNullOrWhiteSpace(input))
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
        while (string.IsNullOrWhiteSpace(input) || !File.Exists(fullFilePath) ||  input.ToLower() != "f");
        return files;
    }
    public static float GetXOffset(int splineNumber)
    {
        Console.WriteLine($"Enter the X offset for the current spline (#{splineNumber})");
        string? offsetInput = Console.ReadLine();
        if (offsetInput is null) { throw new ArgumentNullException(nameof(offsetInput)); }
        float offset = 0;
        float.TryParse(offsetInput, out offset);
        return offset;
    }
    public static float GetZOffset(int splineNumber)
    {
        Console.WriteLine($"Enter the Z offset for the current spline (#{splineNumber})");
        string? offsetInput = Console.ReadLine();
        if (offsetInput is null) { throw new ArgumentNullException(nameof(offsetInput)); }
        float offset = 0; 
        float.TryParse(offsetInput, out offset);
        return offset;
    }

    public static string? GetFileName() {
        Console.WriteLine("Enter the file name, leave empty for random name (without .sli):");
        return Console.ReadLine();
    }
}
