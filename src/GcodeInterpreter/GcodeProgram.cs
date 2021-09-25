using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GcodeInterpreter
{
    public record GcodeProgram
    {
        internal GcodeProgram(IReadOnlyList<Line> lines)
        {
            Lines = lines;
        }

        public IReadOnlyList<Line> Lines { get; }
    }

    public record Line(Field Command, List<Field> Parameters)
    {
        public override string ToString()
        {
            var fields = new List<string>() { Command.ToString() };
            var parameters = Parameters.Select(o => o.ToString());
            fields.AddRange(parameters);
            return string.Join(' ', fields);
        }
    }

    public record Field(FieldLetter FieldLetter, string Code)
    {
        public override string ToString() => FieldLetter.Letter + Code;
    }

    public record FieldLetter
    {
        public char Letter { get; }

        internal FieldLetter(char letter)
        {
            Letter = letter.ToString().ToUpper()[0];
        }

        public bool IsCommand => Enum.TryParse<Commands>(Letter.ToString(), out _);

        public bool IsParameter => Enum.TryParse<Parameters>(Letter.ToString(), out _);

        public enum Commands
        {
            [Description("General command")]
            G = 6,

            [Description("Miscellaneous command")]
            M = 12,

            [Description("Line number")]
            N = 13,
        }

        public enum Parameters
        {
            [Description("Length of extrudate")]
            E = 4,

            [Description("Feedrate in mm per minute")]
            F = 5,

            [Description("Speed parameter")]
            S = 17,

            [Description("X axis coordinate")]
            X = 23,

            [Description("Y axis coordinate")]
            Y = 24,

            [Description("Z axis coordinate")]
            Z = 25,
        }
    }
}