using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Linear.Runtime;
using Linear.Runtime.Deserializers;
using Linear.Runtime.Expressions;

namespace Linear.Format;

/// <summary>
/// Parsing utility for linear format.
/// </summary>
public static class FormatParser
{
    /// <summary>
    /// Load linear format from a <see cref="String"/>.
    /// </summary>
    /// <param name="input">Source.</param>
    /// <param name="filenameHint">Filename hint.</param>
    /// <param name="errorHandler">ANTLR error handler.</param>
    /// <param name="deserializers">Deserializers.</param>
    /// <param name="methods">methods.</param>
    /// <param name="createdDeserializers">Generated deserializers.</param>
    /// <param name="structures">Generated structures.</param>
    /// <exception cref="LynFormatException">Thrown for a parse error.</exception>
    public static void Load(string input, string? filenameHint, IAntlrErrorStrategy? errorHandler,
        IReadOnlyDictionary<string, IDeserializer> deserializers, IReadOnlyDictionary<string, MethodCallDelegate> methods,
        out List<KeyValuePair<string, IDeserializer>> createdDeserializers,
        out List<KeyValuePair<string, Structure>> structures)
    {
        var inputStream = new AntlrInputStream(input);
        var lexer = new LinearLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new LinearParser(tokens);
        Load(parser, filenameHint, errorHandler, deserializers, methods, out createdDeserializers, out structures);
    }

    /// <summary>
    /// Load linear format from a <see cref="TextReader"/>.
    /// </summary>
    /// <param name="input">Source.</param>
    /// <param name="filenameHint">Filename hint.</param>
    /// <param name="errorHandler">ANTLR error handler.</param>
    /// <param name="deserializers">Deserializers.</param>
    /// <param name="methods">methods.</param>
    /// <param name="createdDeserializers">Generated deserializers.</param>
    /// <param name="structures">Generated structures.</param>
    /// <exception cref="LynFormatException">Thrown for a parse error.</exception>
    public static void Load(TextReader input, string? filenameHint, IAntlrErrorStrategy? errorHandler,
        IReadOnlyDictionary<string, IDeserializer> deserializers, IReadOnlyDictionary<string, MethodCallDelegate> methods,
        out List<KeyValuePair<string, IDeserializer>> createdDeserializers,
        out List<KeyValuePair<string, Structure>> structures)
    {
        var inputStream = new AntlrInputStream(input);
        var lexer = new LinearLexer(inputStream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new LinearParser(tokens);
        Load(parser, filenameHint, errorHandler, deserializers, methods, out createdDeserializers, out structures);
    }

    /// <summary>
    /// Load lyn-format stream from a parser instance.
    /// </summary>
    /// <param name="parser">Source parser.</param>
    /// <param name="filenameHint">Filename hint.</param>
    /// <param name="errorHandler">ANTLR error handler.</param>
    /// <param name="deserializers">Deserializers.</param>
    /// <param name="methods">methods.</param>
    /// <param name="createdDeserializers">Generated deserializers.</param>
    /// <param name="structures">Generated structures.</param>
    /// <exception cref="LynFormatException">Thrown for a parse error.</exception>
    public static void Load(LinearParser parser, string? filenameHint, IAntlrErrorStrategy? errorHandler,
        IReadOnlyDictionary<string, IDeserializer> deserializers, IReadOnlyDictionary<string, MethodCallDelegate> methods,
        out List<KeyValuePair<string, IDeserializer>> createdDeserializers,
        out List<KeyValuePair<string, Structure>> structures)
    {
        var listenerPre = new LinearPreListener();
        if (errorHandler != null)
            parser.ErrorHandler = errorHandler;
        ParseTreeWalker.Default.Walk(listenerPre, parser.compilation_unit());
        if (listenerPre.Fail)
        {
            throw new LynFormatException("Failed to parse structure", Array.Empty<ParseError>());
        }
        createdDeserializers = listenerPre.GetStructureNames().Select(v => new KeyValuePair<string, IDeserializer>(v, new StructureDeserializer(v))).ToList();
        Dictionary<string, IDeserializer> deserializersTmp = new(deserializers.Concat(createdDeserializers));
        var listener = new LinearListener(deserializersTmp, methods, filenameHint);
        parser.Reset();
        ParseTreeWalker.Default.Walk(listener, parser.compilation_unit());
        if (listener.Fail)
        {
            throw new LynFormatException(listener.GetErrors());
        }
        structures = new List<KeyValuePair<string, Structure>>();
        foreach (StructureDefinition structure in listener.GetStructures())
        {
            var built = structure.Build();
            structures.Add(new KeyValuePair<string, Structure>(structure.Name, built));
        }
    }
}
