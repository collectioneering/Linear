using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Linear
{
    /// <summary>
    /// Lyn processing utility
    /// </summary>
    public static class LinearUtil
    {
        /// <summary>
        /// Generate processor
        /// </summary>
        /// <param name="input">Lyn format stream</param>
        public static void Generate(Stream input)
        {
            var inputStream = new AntlrInputStream(input);
            var lexer = new LinearLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new LinearParser(tokens);
            var listener = new LinearListener();
            parser.ErrorHandler = new BailErrorStrategy();
            ParseTreeWalker.Default.Walk(listener, parser.compilation_unit());
            // TODO generate / return output object
        }
    }
}
