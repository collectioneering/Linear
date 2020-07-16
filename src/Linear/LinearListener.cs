using System;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Linear.Runtime;
using Linear.Runtime.Elements;
using Linear.Runtime.Expressions;

namespace Linear
{
    /// <summary>
    /// ANTLR listener implementation
    /// </summary>
    internal class LinearListener : LinearBaseListener
    {
        /// <summary>
        /// Deserializers
        /// </summary>
        private readonly Dictionary<string, IDeserializer> _deserializers;

        private readonly List<StructureDefinition> _structures;

        /// <summary>
        /// Create new instance of <see cref="LinearListener"/>
        /// </summary>
        public LinearListener(Dictionary<string, IDeserializer> deserializers)
        {
            _deserializers = deserializers;
            _structures = new List<StructureDefinition>();
        }

        /// <summary>
        /// Get parsed structures
        /// </summary>
        /// <returns>Structures</returns>
        public List<StructureDefinition> GetStructures() => new List<StructureDefinition>(_structures);

        private StructureDefinition? _currentDefinition;

        public override void EnterStruct(LinearParser.StructContext context)
        {
            _currentDefinition = new StructureDefinition(context.IDENTIFIER().GetText());
            //Console.WriteLine(context.IDENTIFIER().GetText());
        }

        public override void ExitStruct(LinearParser.StructContext context)
        {
            _structures.Add(_currentDefinition!);
            _currentDefinition = null;
        }

        /*public override void EnterStruct_statement(LinearParser.Struct_statementContext context)
        {
            Console.WriteLine(context.GetText());
        }*/

        public override void EnterStruct_statement_define(LinearParser.Struct_statement_defineContext context)
        {
            ExpressionDefinition offsetExpression = GetExpression(context.expr());
            ITerminalNode[] ids = context.IDENTIFIER();
            string typeName = ids[0].GetText();
            string dataName = ids[1].GetText();
            (int _, bool littleEndian) = StringToPrimitiveInfo(typeName);
            ExpressionDefinition littleEndianExpression = new ConstantExpression<bool>(littleEndian);
            Element dataElement = new DataElement(dataName, offsetExpression, littleEndianExpression,
                StringToDeserializer(typeName), GetPropertyGroup(context.property_group()));

            _currentDefinition!.Members.Add((dataName, dataElement));
        }

        public override void EnterStruct_statement_define_array(
            LinearParser.Struct_statement_define_arrayContext context)
        {
            // TODO implement statement generator
        }

        public override void EnterStruct_statement_define_array_indirect(
            LinearParser.Struct_statement_define_array_indirectContext context)
        {
            // TODO implement statement generator
        }

        public override void EnterStruct_statement_define_value(LinearParser.Struct_statement_define_valueContext context)
        {
            /*ITerminalNode[] ids = context.IDENTIFIER();
            string typeName = ids[0].GetText();
            string dataName = ids[1].GetText();*/
            string dataName = context.IDENTIFIER().GetText();
            ExpressionDefinition expr = GetExpression(context.expr());
            Element dataElement = new ValueElement(dataName, expr);
            _currentDefinition!.Members.Add((dataName, dataElement));
        }

        public override void EnterStruct_statement_output(LinearParser.Struct_statement_outputContext context)
        {
            ExpressionDefinition formatExpression = new ConstantExpression<string>(context.IDENTIFIER().GetText());
            ExpressionDefinition nameExpression = GetExpression(context.expr(0));
            ExpressionDefinition rangeExpression = GetExpression(context.expr(1));
            Element outputElement = new OutputElement(formatExpression, rangeExpression, nameExpression,
                GetPropertyGroup(context.property_group()));

            _currentDefinition!.Members.Add((null, outputElement));
        }

        private static Dictionary<string, ExpressionDefinition> GetPropertyGroup(
            LinearParser.Property_groupContext? context)
        {
            Dictionary<string, ExpressionDefinition> res = new Dictionary<string, ExpressionDefinition>();
            if (context == null) return res;
            foreach (LinearParser.Property_statementContext x in context.property_statement())
                res.Add(x.IDENTIFIER().GetText(), GetExpression(x.expr()));
            return res;
        }

        private static (int size, bool littleEndian) StringToPrimitiveInfo(string identifier) => identifier switch
        {
            "byte" => (1, true),
            "sbyte" => (1, true),
            "ushort" => (2, true),
            "short" => (2, true),
            "uint" => (4, true),
            "int" => (4, true),
            "ulong" => (8, true),
            "long" => (8, true),
            "byteb" => (1, false),
            "sbyteb" => (1, false),
            "ushortb" => (2, false),
            "shortb" => (2, false),
            "uintb" => (4, false),
            "intb" => (4, false),
            "ulongb" => (8, false),
            "longb" => (8, false),
            "float" => (4, false),
            "double" => (8, false),
            _ => (0, false)
        };

        private IDeserializer StringToDeserializer(string identifier) =>
            _deserializers.TryGetValue(identifier, out IDeserializer deserializer)
                ? deserializer
                : throw new Exception($"Failed to find deserializer for type {identifier}");

        private static ExpressionDefinition GetExpression(LinearParser.ExprContext context)
        {
            //Console.WriteLine($"Rule ihndex {context.RuleIndex}");
            return context switch
            {
                LinearParser.ExprArrayAccessContext exprArrayAccessContext => new ArrayAccessExpression(
                    GetExpression(exprArrayAccessContext.expr(0)), GetExpression(exprArrayAccessContext.expr(1))),
                LinearParser.ExprMemberContext exprMemberContext => new ProxyMemberExpression(
                    exprMemberContext.IDENTIFIER().GetText(), GetExpression(exprMemberContext.expr())),
                LinearParser.ExprOpContext exprOpContext => new OperatorDualExpression(
                    GetExpression(exprOpContext.expr(0)), GetExpression(exprOpContext.expr(1)),
                    OperatorDualExpression.GetOperator(exprOpContext.op().GetText())),
                LinearParser.ExprRangeEndContext exprRangeEndContext => new RangeExpression(GetExpression(exprRangeEndContext.expr(0)), GetExpression(exprRangeEndContext.expr(1)), null),
                LinearParser.ExprRangeLengthContext exprRangeLengthContext => new RangeExpression(GetExpression(exprRangeLengthContext.expr(0)), null, GetExpression(exprRangeLengthContext.expr(1))),
                LinearParser.ExprTermContext exprTermContext => GetTerm(exprTermContext.term()),
                LinearParser.ExprUnOpContext exprUnOpContext => new OperatorUnaryExpression(
                    GetExpression(exprUnOpContext.expr()),
                    OperatorUnaryExpression.GetOperator(exprUnOpContext.un_op().GetText())),
                LinearParser.ExprWrappedContext exprWrappedContext => GetExpression(exprWrappedContext.expr()),
                _ => throw new ArgumentOutOfRangeException(nameof(context))
            };
        }

        private static ExpressionDefinition GetTerm(LinearParser.TermContext context)
        {
            return context switch
            {
                LinearParser.TermCharContext termCharContext => new ConstantExpression<char>(termCharContext.GetText()
                    .Trim(' ', '\'')[0]),
                LinearParser.TermHexContext termHexContext => new ConstantExpression<long>(
                    Convert.ToInt32(termHexContext.GetText(), 16)),
                LinearParser.TermIdentifierContext termIdentifierContext => new MemberExpression(termIdentifierContext
                    .GetText()),
                LinearParser.TermIntContext termIntContext => new ConstantExpression<long>(
                    long.Parse(termIntContext.GetText())),
                LinearParser.TermRealContext termRealContext => new ConstantExpression<double>(
                    double.Parse(termRealContext.GetText())),
                LinearParser.TermRepIContext _ => new StructureEvaluateExpression<long>(i => i.Index),
                LinearParser.TermRepLengthContext _ => new StructureEvaluateExpression<long>(i =>
                    i.Length),
                LinearParser.TermRepPContext _ => new StructureEvaluateExpression<StructureInstance?>(i =>
                    i.Parent),
                LinearParser.TermRepUContext _ => new StructureEvaluateExpression<int>(i =>
                    i.GetUniqueId()),
                LinearParser.TermStringContext termStringContext => new ConstantExpression<string>(termStringContext
                    .GetText()
                    .Trim(' ', '"')),
                LinearParser.TermStringVerbContext termStringVerbContext => new ConstantExpression<string>(
                    termStringVerbContext.GetText().Trim(' ', '"', '@')),
                _ => throw new ArgumentOutOfRangeException(nameof(context))
            };
        }
    }
}
