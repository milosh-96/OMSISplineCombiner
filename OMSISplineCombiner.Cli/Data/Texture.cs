namespace OMSISplineCombiner.Cli.Data;

public class Texture : IEquatable<Texture>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public PatchworkChain? PatchworkChain { get; set; }

    public override string ToString()
    {
        return Name ?? nameof(Texture);
    }
    public string Output()
    {
        string output = $"[texture]\n{Name}\n";

        if(PatchworkChain is not null)
        {
            output += $"[patchwork_chain]\n{PatchworkChain.SegmentLength}\n{PatchworkChain.ChainOfTransitions}\n{PatchworkChain.ChainOfWeightFactors}\n{PatchworkChain.Invertable}\n";
        }
        return output;
    }

    public override bool Equals(object? obj) => obj is not null ? Name == ((Texture)obj).Name : false;
    public bool Equals(Texture? other) => other is not null ? Name == other.Name : false;

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}

public class PatchworkChain
{
    public int SegmentLength { get; init; }
    public string ChainOfTransitions { get; init; } = "A";
    public string ChainOfWeightFactors { get; init; } = "1";
    public string Invertable { get; init; } = "1";
}