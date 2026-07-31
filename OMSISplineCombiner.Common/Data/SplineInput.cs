namespace OMSISplineCombiner.Common.Data;

public class SplineInput
{
    public string Path { get; set; } = string.Empty;
    public SplineInputSettings Settings { get; set; } = new();
}

public class SplineInputSettings
{
    public float XOffset { get; set; } = 0;
    public float ZOffset { get; set; } = 0;
    public bool Mirror { get; set; } = false;
}