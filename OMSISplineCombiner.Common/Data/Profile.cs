namespace OMSISplineCombiner.Common.Data;

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
    public override string ToString()
    {
        string[] pointsOutput = Points.Select(point => point.Output()).ToArray();

        return $"[profile]\n{TextureId}\n{TextureName}\n{string.Join('\n', pointsOutput)}";
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(TextureId);

        if (Points != null)
        {
            foreach (var point in Points)
            {
                hash.Add(point);
            }
        }
        return hash.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Profile other) return false;

        return TextureId == other.TextureId && (Points?.SequenceEqual(other.Points) ?? other.Points == null);
    }
}