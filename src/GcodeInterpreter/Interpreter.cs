using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GcodeInterpreter
{
    public interface IGcodeInterpreter
    {
        /// <summary>
        /// Generates a <see cref="GcodeProgram"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The G-codes stream.</param>
        /// <returns>The generated <see cref="GcodeProgram"/>.</returns>
        Task<GcodeProgram> ParseAsync(Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a <see cref="GcodeProgram"/> from a G-code file.
        /// </summary>
        /// <param name="path">The G-codes file path.</param>
        /// <returns>The generated <see cref="GcodeProgram"/>.</returns>
        Task<GcodeProgram> ParseAsync(string path, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Used to parse a text file into Gcode blocks.
    /// </summary>
    public class Interpreter : IGcodeInterpreter
    {
        // The regex parses as follows
        // 1 letter, lower case or upper case
        // A + or - character, optional
        // 1 to unlimited numbers
        // A . character, optional
        // 0 to unlimited numbers
        // https://regex101.com/r/5nYrVz/3
        private const string ParseFieldsRegex = @"[a-zA-Z][\+\-]?[0-9]+[\.]?[0-9]*";
        private readonly ILogger<Interpreter> _logger;
        private readonly IEnumerable<ICommentRemover> _commentRemovers;

        public Interpreter(
            ILogger<Interpreter> logger,
            IEnumerable<ICommentRemover> commentRemovers)
        {
            _logger = logger;
            _commentRemovers = commentRemovers;
        }

        public async Task<GcodeProgram> ParseAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            using GcodeReader gcode = new(stream);
            return await ParseAsync(gcode, cancellationToken);
        }

        public async Task<GcodeProgram> ParseAsync(string path, CancellationToken cancellationToken = default)
        {
            using GcodeReader gcode = new(path);
            return await ParseAsync(gcode, cancellationToken);
        }

        internal async Task<GcodeProgram> ParseAsync(GcodeReader gcodeReader, CancellationToken cancellationToken = default)
        {
            List<Line> fields = new();

            await foreach (string line in gcodeReader.ReadLinesAsync(cancellationToken))
            {
                // If the line is empty, continue to the next line
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string? temp = line;
                foreach (ICommentRemover? commentRemover in _commentRemovers)
                {
                    temp = commentRemover.RemoveComment(temp);
                }
                temp = temp.Trim();

                // Only include if not empty after striping the comments
                if (!string.IsNullOrWhiteSpace(temp))
                {
                    fields.Add(ParseLine(temp));
                }
            }

            return new GcodeProgram(fields.AsReadOnly());
        }

        internal static Line ParseLine(string line)
        {
            Field? command = null;
            List<Field> paremters = new();

            foreach (Match match in Regex.Matches(line, ParseFieldsRegex))
            {
                FieldLetter letter = new(match.Value[0]);
                string code = match.Value[1..match.Length];

                if (letter.IsCommand)
                {
                    if (command is not null)
                    {
                        throw new GcodeInterpreterException($"Found multiple commands on line {line}");
                    }
                    command = new Field(letter, code);
                }

                if (letter.IsParameter)
                {
                    paremters.Add(new Field(letter, code));
                }
            }

            if (command is null)
            {
                throw new GcodeInterpreterException($"No command found, line:{line}");
            }

            return new Line(command, paremters);
        }
    }
}