using NUnit.Framework;
using OMSISplineCombiner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmsiSplineCombiner.Tests;

public class ProjectJsonGeneratorTests
{
    [Test]
    public void Generate_ShouldReturnCorrectJsonString()
    {
        // Arrange
        var expectedJson = "[{\"OmsiDirectoryPath\":\"C:\\\\Program Files (x86)\\\\Steam\\\\steamapps\\\\common\\\\OMSI 2\",\"SplinesSourcePath\":\"Splines\",\"SplinesOutputPath\":\"Splines\\\\MySplines\",\"FileName\":\"my-spline1.sli\",\"SplinesInputs\":[{\"Path\":\"MyModularSplines/asphalt1.sli\",\"Settings\":{\"XOffset\":0,\"ZOffset\":0}},{\"Path\":\"MyModularSplines/sidewalk.sli\",\"Settings\":{\"XOffset\":-3,\"ZOffset\":0}}]}]";
        // Act
        var resultTask = ProjectJsonGenerator.Generate();
        var result = resultTask;
        // Assert
        Assert.AreEqual(expectedJson, result);
    }
}
