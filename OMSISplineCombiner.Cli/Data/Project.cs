using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Cli.Data;

internal class Project
{
    public string? OmsiDirectoryPath { get; set; } = @"C:\Program Files (x86)\Steam\steamapps\common\OMSI 2";
    public string? SplinesSourcePath { get; set; } = "Splines";
    public string? SplinesOutputPath { get; set; } = @"Splines\MySplines";

    public string? FileName { get; set; } = Guid.NewGuid().ToString();

    public List<SplineInput> SplinesInputs { get; set; } = new();
}
