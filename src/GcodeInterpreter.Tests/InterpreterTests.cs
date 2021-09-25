using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace GcodeInterpreter.Tests
{
    public class InterpreterTests
    {
        private readonly string _gcodeFile = AppContext.BaseDirectory + "/SampleData/10x10x10mm_cube.gcode";

        [Fact]
        public async Task Test()
        {
            IGcodeInterpreter interpreter = CreateInterpreter();

            GcodeProgram program = await interpreter.ParseAsync(_gcodeFile);

            Assert.Equal("M190", program.Lines[0].Command.ToString()); //M190 S60.000000
            Assert.Equal('M', program.Lines[0].Command.FieldLetter.Letter); //M190 S60.000000
            Assert.Equal('G', program.Lines.Skip(8).First().Command.FieldLetter.Letter); //G1 Z15.0 F4800 ;move the platform down 15mm
        }

        private static IGcodeInterpreter CreateInterpreter()
        {
            ILogger<Interpreter> logger = NullLogger<Interpreter>.Instance;
            IEnumerable<ICommentRemover> commentRemovers = new List<ICommentRemover> { new AfterSemiColonCommentRemover(), new InPerenthesesisCommentRemover() };
            return new Interpreter(logger, commentRemovers);
        }
    }
}