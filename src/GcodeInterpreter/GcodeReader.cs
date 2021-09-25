using System;
using System.Collections.Generic;
using System.IO;

namespace GcodeInterpreter
{
    public class GcodeReader : StreamReader, IDisposable
    {
        private static readonly List<string> _allowedExtensions = new()
        {
            ".g",
            ".gco",
            ".gcode"
        };

        public GcodeReader(Stream stream)
            : base(stream)
        {
        }

        public GcodeReader(string path)
            : base(path)
        {
            string? extension = Path.GetExtension(path);

            if (!_allowedExtensions.Contains(extension.ToLower()))
            {
                throw new Exception("File must have extension 'gcode'.");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
        }

        /// <summary>
        /// Streams all lines in the gcode file.
        /// Each string returned represents one line in the file.
        /// </summary>
        /// <returns>Stream of strings.</returns>
        public async IAsyncEnumerable<string> ReadLinesAsync()
        {
            string? line;

            while ((line = await ReadLineAsync()) != null)
            {
                yield return line;
            }
        }
    }
}