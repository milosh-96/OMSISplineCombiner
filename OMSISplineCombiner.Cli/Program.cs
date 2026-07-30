using OMSISplineCombiner.Cli.App;

namespace OMSISplineCombiner.Cli;

internal class Program
{
    static void Main(string[] args)
    {
        try
        { 
            var app = new OmsiSplineCombinerApp("test.json");
            app.Run();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Something went wrong. Sorry. {ex.Message}");
            File.AppendAllText("log.txt", $"{DateTime.Now}, Message: {ex.Message} {Environment.NewLine}");
            Console.ReadKey();
        }
    }
}
