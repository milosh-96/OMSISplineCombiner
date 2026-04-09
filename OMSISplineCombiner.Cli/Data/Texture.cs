namespace OMSISplineCombiner.Cli.Data;

public class Texture : IEquatable<Texture>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
    public string Output()
    {
        return $"[texture]\n{Name}";
    }

    public override bool Equals(object? obj) => Name == ((Texture)obj).Name;
    public bool Equals(Texture? other) => Name == other.Name;

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}