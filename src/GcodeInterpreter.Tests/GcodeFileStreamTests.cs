using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GcodeInterpreter.Tests
{
    public class GcodeFileStreamTests
    {
        private readonly string _gcodeFile = AppContext.BaseDirectory + "/SampleData/10x10x10mm_cube.gcode";
        private readonly string _stlFile = AppContext.BaseDirectory + "/SampleData/10x10x10mm_cube.stl";

        [Fact]
        public async Task Reads_Lines()
        {
            List<string> lines = new();
            using GcodeReader gcode = new(_gcodeFile);

            await foreach (string line in gcode.ReadLinesAsync())
            {
                lines.Add(line);
            }

            Assert.Equal("M190 S60.000000", lines[0]);
            Assert.Equal("M109 S200.000000", lines[1]);
            Assert.StartsWith("G90", lines[1912]);
            Assert.Equal(1916, lines.Count);
        }

        [Fact]
        public void Bad_File_Extension()
        {
            Assert.Throws<Exception>(() =>
            {
                using GcodeReader gcode = new(_stlFile);
            });
        }
    }
}