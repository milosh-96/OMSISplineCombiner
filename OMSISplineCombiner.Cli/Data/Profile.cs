namespace OMSISplineCombiner.Cli.Data;

public class Profile
{
    public int TextureId { get; set; }
    public string? TextureName { get; set; }

    public List<ProfilePoint> Points { get; init; } = new List<ProfilePoint>();

    public string Output()
    {
        string[] pointsOutput = Points.Select(point => point.Output()).ToArray();

        return $"[profile]\n{TextureId}\n{string.Join('\n', pointsOutput)}";
    }
}