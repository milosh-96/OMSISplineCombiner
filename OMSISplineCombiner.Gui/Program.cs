namespace OMSISplineCombiner.Gui;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        // Check if the DLL exists in the execution folder
        string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OMSISplineCombiner.Common.dll");

        if (!File.Exists(dllPath))
        {
            MessageBox.Show(
                "Critical dependency missing: OMSISplineCombiner.Common.dll. The application will now close.",
                "Error Loading Component",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return; // Exit the application cleanly
        }

        Application.Run(new MainWindow());
    }
}