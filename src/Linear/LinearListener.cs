using System;
using System.Collections.Generic;
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

        // TODO complete listener

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

        public override void EnterStruct_statement(LinearParser.Struct_statementContext context)
        {
            Console.WriteLine(context.GetText());
            // TODO
        }

        public override void EnterStruct_statement_define(LinearParser.Struct_statement_defineContext context)
        {
            //Console.WriteLine(context.GetText());
            ExpressionDefinition offsetExpression = GetExpression(context.expr());
            string typeName = context.IDENTIFIER()[0].GetText();
            string dataName = context.IDENTIFIER()[1].GetText();
            Element dataElement;
            (Type? targetType, bool littleEndian) = StringToType(typeName);
            ExpressionDefinition littleEndianExpression = new ConstantExpression<bool>(littleEndian);
            if (targetType != null)
                dataElement = new DataElement(dataName, targetType, offsetExpression, littleEndianExpression);
            else
            {
                dataElement = new DataElement(dataName, offsetExpression, littleEndianExpression,
                    StringToDeserializer(typeName), GetPropertyGroup(context.property_group()));
            }

            _currentDefinition!.Members.Add((dataName, dataElement));
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

        private static (Type? type, bool littleEndian) StringToType(string identifier) => identifier switch
        {
            "byte" => (typeof(byte), true),
            "sbyte" => (typeof(sbyte), true),
            "ushort" => (typeof(ushort), true),
            "short" => (typeof(short), true),
            "uint" => (typeof(uint), true),
            "int" => (typeof(int), true),
            "ulong" => (typeof(ulong), true),
            "long" => (typeof(long), true),
            "byteb" => (typeof(byte), false),
            "sbyteb" => (typeof(sbyte), false),
            "ushortb" => (typeof(ushort), false),
            "shortb" => (typeof(short), false),
            "uintb" => (typeof(uint), false),
            "intb" => (typeof(int), false),
            "ulongb" => (typeof(ulong), false),
            "longb" => (typeof(long), false),
            "float" => (typeof(float), false),
            "double" => (typeof(double), false),
            "range" => (typeof(ValueTuple<long, long>), false),
            _ => (null, false)
        };

        private IDeserializer StringToDeserializer(string identifier) =>
            _deserializers.TryGetValue(identifier, out IDeserializer deserializer)
                ? deserializer
                : throw new Exception($"Failed to find deserializer for type {identifier}");

        private static ExpressionDefinition GetExpression(LinearParser.ExprContext context)
        {
            //Console.WriteLine($"Rule ihndex {context.RuleIndex}");
            // TODO implement cases
            switch (context)
            {
                case LinearParser.ExprArrayAccessContext exprArrayAccessContext:
                    break;
                case LinearParser.ExprBoolOpContext exprBoolOpContext:
                    break;
                case LinearParser.ExprMemberContext exprMemberContext:
                    break;
                case LinearParser.ExprOpContext exprOpContext:
                    break;
                case LinearParser.ExprTermContext exprTermContext:
                    return GetTerm(exprTermContext.term());
                case LinearParser.ExprUnOpContext exprUnOpContext:
                    break;
                case LinearParser.ExprWrappedContext exprWrappedContext:
                    return GetExpression(exprWrappedContext.expr());
                default:
                    throw new ArgumentOutOfRangeException(nameof(context));
            }

            return new ConstantExpression<int>(0);
        }

        private static ExpressionDefinition GetTerm(LinearParser.TermContext context)
        {
            return context switch
            {
                LinearParser.TermCharContext termCharContext => new ConstantExpression<char>(termCharContext.GetText()
                    .Trim(' ', '\'')[0]),
                LinearParser.TermHexContext termHexContext => new ConstantExpression<long>(
                    Convert.ToInt32(termHexContext.GetText(), 16)),
                LinearParser.TermIdentifierContext termIdentifierContext => new StructureEvaluateExpression<object>(i =>
                    i[termIdentifierContext.GetText()]),
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
