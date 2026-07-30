namespace OMSISplineCombiner.Common;

public class FileService
{
    public static void CopyTextureFile(string path, string destination)
    {
        if (File.Exists(path))
        {
            File.Copy(path, destination, true);

            var cfgFile = path + ".cfg";

            if (File.Exists(cfgFile))
            {
                File.Copy(cfgFile, destination + ".cfg", true);
            }
        }
    }

    public static void EnsureDirectoryExists(string filePath)
    {
        FileInfo fi = new FileInfo(filePath);
        if (fi.Directory == null || !fi.Directory.Exists)
        {
            Directory.CreateDirectory(fi.DirectoryName!);
        }
    }
}
