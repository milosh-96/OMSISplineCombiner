namespace OMSISplineCombiner.Cli.Data;

public record ProfilePoint
{
    public float PositionX { get; set; }
    public float Height { get; set; }
    public float TexturePositionX { get; init; }
    public float StretchFactor { get; init; }

    public string Output() => $"[profilepnt]\n{PositionX}\n{Height}\n{TexturePositionX}\n{StretchFactor}\n";
}