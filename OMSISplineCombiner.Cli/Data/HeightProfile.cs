namespace OMSISplineCombiner.Cli.Data;

public class HeightProfile
{
    public float FromX { get; set; }
    public float ToX { get; set; }
    public float FromZ { get; set; }
    public float ToZ { get; set; }

    public override string ToString()
    {
        return $"[heightprofile]\n{FromX}\n{ToX}\n{FromZ}\n{ToZ}";
    }
    public string Output()
    {
        return $"[heightprofile]\n{FromX}\n{ToX}\n{FromZ}\n{ToZ}";
    }
}