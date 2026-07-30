using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSISplineCombiner.Cli.Data;

internal class SplineInput
{
    public string Path { get; set; } = string.Empty;
    public SplineInputSettings Settings { get; set; } = new();
}

internal class SplineInputSettings
{
    public float XOffset { get; set; } = 0;
    public float ZOffset { get; set; } = 0;
}