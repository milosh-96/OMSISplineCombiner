namespace OMSISplineCombiner.Cli.Data;

public class Texture : IEquatable<Texture>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public override string ToString()
    {
        return Name ?? nameof(Texture);
    }
    public string Output()
    {
        return $"[texture]\n{Name}";
    }

    public override bool Equals(object? obj) => obj is not null ? Name == ((Texture)obj).Name : false;
    public bool Equals(Texture? other) => other is not null ? Name == other.Name : false;

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}