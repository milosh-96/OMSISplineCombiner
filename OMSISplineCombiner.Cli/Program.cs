using OMSISplineCombiner.Cli.App;

namespace OMSISplineCombiner.Cli;

internal class Program
{
    static void Main(string[] args)
    {
        try
        { 
            var app = new OmsiSplineCombinerApp() { SplinesSourceDirectory = @"Splines\Test_skrzyzowania" };
            app.Run();
        }
        catch(Exception ex)
        {
            Console.WriteLine("Something went wrong. Sorry.");
            File.AppendAllText("log.txt", $"{DateTime.Now}, Message: {ex.Message}, stack trace: {ex.StackTrace} {Environment.NewLine}");
        }
    }
}
