using System;

namespace GcodeInterpreter
{
    public class GcodeInterpreterException : Exception
    {
        public GcodeInterpreterException()
        {
        }

        public GcodeInterpreterException(string? message)
            : base(message)
        {
        }

        public GcodeInterpreterException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}