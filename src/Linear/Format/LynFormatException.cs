using System.Collections.Generic;
using System.IO;

namespace Linear.Format;

internal class LynFormatException : IOException
{
    public IReadOnlyList<ParseError> Errors { get; }

    public LynFormatException(IReadOnlyList<ParseError> errors) : base("Errors occurred while parsing format")
    {
        Errors = errors;
    }

    public LynFormatException(string message, IReadOnlyList<ParseError> errors) : base(message)
    {
        Errors = errors;
    }
}
