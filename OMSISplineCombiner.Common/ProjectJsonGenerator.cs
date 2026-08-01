using OMSISplineCombiner.Common.Data;
using System.Text.Json;

namespace OMSISplineCombiner.Common;

public static class ProjectJsonGenerator
{
    public static string Generate()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        string content = JsonSerializer.Serialize(new List<Project>()
        {
            new()
            {
                FileName = "my-spline1.sli",
                SplinesInputs = new List<SplineInput>()
                {
                    new SplineInput()
                    {
                        Path = "MyModularSplines/asphalt1.sli",
                        Settings = new()
                        {
                            XOffset = 0,
                            ZOffset = 0
                        }
                    },
                    new SplineInput()
                    {
                        Path = "MyModularSplines/sidewalk.sli",
                        Settings = new()
                        {
                            XOffset = -3,
                            ZOffset = 0
                        }
                    }
                }
            }
        }, options);
        return content;
    }
}
