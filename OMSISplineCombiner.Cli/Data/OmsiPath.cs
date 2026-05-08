namespace OMSISplineCombiner.Cli.Data;

public class OmsiPath
{
    public OmsiPathType Type { get; init; } = OmsiPathType.RoadVehicles;
    public float PositionX { get; set; } = 0;
    public float PositionZ { get; init; } = 0;
    public float Width { get; init; } = 3;
    public OmsiPathDirection Direction { get; init; } = OmsiPathDirection.Forward;

    public string Output() => $"[path]\n{(int)Type}\n{PositionX}\n{PositionZ}\n{Width}\n{(int)Direction}\n";

}

public enum OmsiPathType
{
    RoadVehicles = 0,
    People = 1,
    Railway = 2,
    AirCraft = 3
}
public enum OmsiPathDirection
{
    Forward = 0,
    Backwards = 1,
    Both = 2
}