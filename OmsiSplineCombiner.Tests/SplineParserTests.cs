using NUnit.Framework;
using OMSISplineCombiner.Cli.Parsers;

namespace OmsiSplineCombiner.Tests;

[TestFixture]
public class SplineParserTests
{
    [Test]
    public void PrepareSpline_ShouldThrowFileNotFoundException_IfFileDoesntExist()
    {
        Assert.Throws<FileNotFoundException>(()=>SplineParser.PrepareSpline("ss", "ss", "ss"));
    }
}
