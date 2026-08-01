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
        var expectedJson = "[\r\n  {\r\n    \"OmsiDirectoryPath\": \"C:\\\\Program Files (x86)\\\\Steam\\\\steamapps\\\\common\\\\OMSI 2\",\r\n    \"SplinesSourcePath\": \"Splines\",\r\n    \"SplinesOutputPath\": \"Splines\\\\MySplines\",\r\n    \"FileName\": \"my-spline1\",\r\n    \"SplinesInputs\": [\r\n      {\r\n        \"Path\": \"MyModularSplines/asphalt1.sli\",\r\n        \"Settings\": {\r\n          \"XOffset\": 0,\r\n          \"ZOffset\": 0,\r\n          \"Mirror\": false\r\n        }\r\n      },\r\n      {\r\n        \"Path\": \"MyModularSplines/sidewalk.sli\",\r\n        \"Settings\": {\r\n          \"XOffset\": -3,\r\n          \"ZOffset\": 0,\r\n          \"Mirror\": false\r\n        }\r\n      }\r\n    ]\r\n  }\r\n]";
        // Act
        var resultTask = ProjectJsonGenerator.Generate();
        var result = resultTask;
        // Assert
        Assert.AreEqual(expectedJson, result);
    }
}
