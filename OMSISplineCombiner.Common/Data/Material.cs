using System.Xml.Linq;

namespace OMSISplineCombiner.Common.Data;

public record Material
{
    public MaterialAlphaValues AlphaValue { get; set; } = MaterialAlphaValues.NoTransparency;
    public string? TextureName { get; set; }

    public string Output()
    {
        string output = $"[matl]\n{TextureName}\n\n";
        output += $"[matl_alpha]\n{(int)AlphaValue}\n";
        return output;
    }
}

public enum MaterialAlphaValues
{
    NoTransparency = 0,
    BinaryTransparency = 1,
    BlendedTransparency = 2
}