using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using Linear.Runtime;
using Linear.Runtime.Deserializers;
using Linear.Runtime.Elements;
using Linear.Runtime.Expressions;

namespace Linear;

/// <summary>
/// ANTLR listener implementation
/// </summary>
internal class LinearListener : LinearBaseListener
{
    private readonly Dictionary<string, IDeserializer> _deserializers;
    private readonly Dictionary<string, MethodCallExpression.MethodCallDelegate> _methods;
    private readonly List<StructureDefinition> _structures;
    private readonly Action<string> _logTarget;

    /// <summary>
    /// Create new instance of <see cref="LinearListener"/>
    /// </summary>
    public LinearListener(Dictionary<string, IDeserializer> deserializers,
        Dictionary<string, MethodCallExpression.MethodCallDelegate> methods, Action<string> logTarget)
    {
        _deserializers = deserializers;
        _methods = methods;
        _structures = new List<StructureDefinition>();
        _logTarget = logTarget;
    }

    /// <summary>
    /// Get parsed structures
    /// </summary>
    /// <returns>Structures</returns>
    public List<StructureDefinition> GetStructures() => new List<StructureDefinition>(_structures);

    private StructureDefinition? _currentDefinition;

    public override void EnterStruct(LinearParser.StructContext context)
    {
        int defaultLength = context.struct_size() switch
        {
            LinearParser.StrictSizeHexContext strictSizeHexContext => Convert.ToInt32(
                strictSizeHexContext.GetText(), 16),
            LinearParser.StructSizeIntContext structSizeIntContext => int.Parse(structSizeIntContext.GetText()),
            _ => 0
        };
        _currentDefinition = new StructureDefinition(context.IDENTIFIER().GetText(), defaultLength);
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
        ITerminalNode[] ids = context.IDENTIFIER();
        string typeName = ids[0].GetText();
        string dataName = ids[1].GetText();
        Element dataElement = new ValueElement(dataName, GetExpression(context.expr(), typeName));
        _currentDefinition!.Members.Add(new StructureDefinitionMember(dataName, dataElement));
    }

    public override void EnterStruct_statement_define_array(
        LinearParser.Struct_statement_define_arrayContext context)
    {
        LinearParser.ExprContext[] exprs = context.expr();
        ExpressionDefinition countExpression = GetExpression(exprs[0]);
        ExpressionDefinition offsetExpression = GetExpression(exprs[1]);
        ITerminalNode[] ids = context.IDENTIFIER();
        string typeName = ids[0].GetText();
        string dataName = ids[1].GetText();
        (int _, bool littleEndian) = StringToPrimitiveInfo(typeName);
        ExpressionDefinition littleEndianExpression = new ConstantExpression<bool>(littleEndian);
        Dictionary<LinearCommon.StandardProperty, ExpressionDefinition> standardProperties =
            new Dictionary<LinearCommon.StandardProperty, ExpressionDefinition>();
        standardProperties.Add(LinearCommon.StandardProperty.ArrayLengthProperty, countExpression);
        // "Should" add some other way for length instead of routing through a dictionary...
        // "Should" add efficient primitive array deserialization...
        IDeserializer? deserializer = StringToDeserializer(typeName);
        if (deserializer == null) return;
        Element dataElement = new ValueElement(dataName, new DeserializeExpression(offsetExpression,
            littleEndianExpression, new ArrayDeserializer(deserializer), GetPropertyGroup(context.property_group()),
            standardProperties));
        _currentDefinition!.Members.Add(new StructureDefinitionMember(dataName, dataElement));
    }

    public override void EnterStruct_statement_define_array_indirect(
        LinearParser.Struct_statement_define_array_indirectContext context)
    {
        LinearParser.ExprContext[] exprs = context.expr();
        ExpressionDefinition countExpression = GetExpression(exprs[0]);
        ExpressionDefinition offsetExpression = GetExpression(exprs[1]);
        ExpressionDefinition pointerOffsetExpression = GetExpression(exprs[2]);
        ITerminalNode[] ids = context.IDENTIFIER();
        string typeName = ids[0].GetText();
        string targetTypeName = ids[1].GetText();
        string dataName = ids[2].GetText();
        (int _, bool littleEndian) = StringToPrimitiveInfo(typeName);
        ExpressionDefinition littleEndianExpression = new ConstantExpression<bool>(littleEndian);
        Dictionary<LinearCommon.StandardProperty, ExpressionDefinition> standardProperties =
            new Dictionary<LinearCommon.StandardProperty, ExpressionDefinition>();
        bool lenFinder = context.PLUS() != null;
        standardProperties.Add(LinearCommon.StandardProperty.ArrayLengthProperty,
            lenFinder
                ? new OperatorDualExpression(countExpression, new ConstantExpression<int>(1),
                    OperatorDualExpression.Operator.Add)
                : countExpression);
        standardProperties.Add(LinearCommon.StandardProperty.PointerOffsetProperty, pointerOffsetExpression);
        standardProperties.Add(LinearCommon.StandardProperty.PointerArrayLengthProperty, countExpression);
        IDeserializer? deserializer = StringToDeserializer(typeName);
        if (deserializer == null) return;
        IDeserializer? targetDeserializer = StringToDeserializer(targetTypeName);
        if (targetDeserializer == null) return;
        ArrayDeserializer arrayDeserializer = new ArrayDeserializer(deserializer);
        Element dataElement = new ValueElement(dataName,
            new DeserializeExpression(offsetExpression, littleEndianExpression,
                new PointerArrayDeserializer(arrayDeserializer, targetDeserializer, lenFinder),
                GetPropertyGroup(context.property_group()), standardProperties));
        _currentDefinition!.Members.Add(new StructureDefinitionMember(dataName, dataElement));
    }

    /*public override void EnterStruct_statement_define_value(
        LinearParser.Struct_statement_define_valueContext context)
    {
        /*ITerminalNode[] ids = context.IDENTIFIER();
        string typeName = ids[0].GetText();
        string dataName = ids[1].GetText();#1#
        string dataName = context.IDENTIFIER().GetText();
        ExpressionDefinition expr = GetExpression(context.expr());
        Element dataElement = new ValueElement(dataName, expr);
        _currentDefinition!.Members.Add((dataName, dataElement));
    }*/

    public override void EnterStruct_statement_call(LinearParser.Struct_statement_callContext context)
    {
        ExpressionDefinition expr = GetExpression(context.expr());
        Element dataElement = new MethodCallElement(expr);
        _currentDefinition!.Members.Add(new StructureDefinitionMember(null, dataElement));
    }

    public override void EnterStruct_statement_length(LinearParser.Struct_statement_lengthContext context)
    {
        ExpressionDefinition expr = GetExpression(context.expr());
        Element dataElement = new LengthElement(expr);
        _currentDefinition!.Members.Add(new StructureDefinitionMember(null, dataElement));
    }

    public override void EnterStruct_statement_output(LinearParser.Struct_statement_outputContext context)
    {
        ExpressionDefinition formatExpression = new ConstantExpression<string>(context.IDENTIFIER().GetText());
        ExpressionDefinition rangeExpression = GetExpression(context.expr(0));
        ExpressionDefinition nameExpression = GetExpression(context.expr(1));
        Element outputElement = new OutputElement(formatExpression, rangeExpression, nameExpression,
            GetPropertyGroup(context.property_group()));

        _currentDefinition!.Members.Add(new StructureDefinitionMember(null, outputElement));
    }

    private Dictionary<string, ExpressionDefinition> GetPropertyGroup(
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

    private IDeserializer? StringToDeserializer(string identifier)
    {
        if (_deserializers.TryGetValue(identifier, out IDeserializer deserializer))
            return deserializer;
        _logTarget($"Failed to find deserializer for type {identifier}");
        return null;
    }

    private ExpressionDefinition GetExpression(LinearParser.ExprContext context, string? activeType = null)
    {
        //Console.WriteLine($"Rule ihndex {context.RuleIndex}");
        return context switch
        {
            LinearParser.ExprArrayAccessContext exprArrayAccessContext => new ArrayAccessExpression(
                GetExpression(exprArrayAccessContext.expr(0)), GetExpression(exprArrayAccessContext.expr(1))),
            LinearParser.ExprMemberContext exprMemberContext => new ProxyMemberExpression(
                exprMemberContext.IDENTIFIER().GetText(), GetExpression(exprMemberContext.expr())),
            LinearParser.ExprMethodCallContext exprMethodCallContext => new MethodCallExpression(
                _methods[exprMethodCallContext.IDENTIFIER().GetText()],
                exprMethodCallContext.expr().Select(e => GetExpression(e)).ToList()),
            LinearParser.ExprOpAddSubContext exprOpAddSubContext => new OperatorDualExpression(
                GetExpression(exprOpAddSubContext.expr(0)), GetExpression(exprOpAddSubContext.expr(1)),
                OperatorDualExpression.GetOperator(exprOpAddSubContext.op_add_sub().GetText())),
            LinearParser.ExprOpAmpContext exprOpAmpContext => new OperatorDualExpression(
                GetExpression(exprOpAmpContext.expr(0)), GetExpression(exprOpAmpContext.expr(1)),
                OperatorDualExpression.GetOperator(exprOpAmpContext.AMP().GetText())),
            LinearParser.ExprOpBitwiseOrContext exprOpBitwiseOrContext => new OperatorDualExpression(
                GetExpression(exprOpBitwiseOrContext.expr(0)), GetExpression(exprOpBitwiseOrContext.expr(1)),
                OperatorDualExpression.GetOperator(exprOpBitwiseOrContext.BITWISE_OR().GetText())),
            LinearParser.ExprOpCaretContext exprOpCaretContext => new OperatorDualExpression(
                GetExpression(exprOpCaretContext.expr(0)), GetExpression(exprOpCaretContext.expr(1)),
                OperatorDualExpression.GetOperator(exprOpCaretContext.CARET().GetText())),
            LinearParser.ExprOpMulDivContext exprOpMulDivContext => new OperatorDualExpression(
                GetExpression(exprOpMulDivContext.expr(0)), GetExpression(exprOpMulDivContext.expr(1)),
                OperatorDualExpression.GetOperator(exprOpMulDivContext.op_mul_div().GetText())),
            LinearParser.ExprRangeEndContext exprRangeEndContext => new RangeExpression(
                GetExpression(exprRangeEndContext.expr(0)), GetExpression(exprRangeEndContext.expr(1)), null),
            LinearParser.ExprRangeLengthContext exprRangeLengthContext => new RangeExpression(
                GetExpression(exprRangeLengthContext.expr(0)), null, GetExpression(exprRangeLengthContext.expr(1))),
            LinearParser.ExprTermContext exprTermContext => GetTerm(exprTermContext.term()),
            LinearParser.ExprDeserializeContext exprDeserializeContext => GetDeserializeExpression(
                exprDeserializeContext.IDENTIFIER().GetText(),
                GetExpression(exprDeserializeContext.expr()), GetPropertyGroup(exprDeserializeContext.property_group()),
                new Dictionary<LinearCommon.StandardProperty, ExpressionDefinition>()),
            LinearParser.ExprUnboundDeserializeContext exprUnboundDeserializeContext => GetDeserializeExpression(
                activeType ??
                throw new ApplicationException(
                    $"{nameof(DeserializeExpression)} cannot be used without type name"),
                GetExpression(exprUnboundDeserializeContext.expr()),
                GetPropertyGroup(exprUnboundDeserializeContext.property_group()),
                new Dictionary<LinearCommon.StandardProperty, ExpressionDefinition>()),
            LinearParser.ExprUnOpContext exprUnOpContext => new OperatorUnaryExpression(
                GetExpression(exprUnOpContext.expr()),
                OperatorUnaryExpression.GetOperator(exprUnOpContext.un_op().GetText())),
            LinearParser.ExprWrappedContext exprWrappedContext => GetExpression(exprWrappedContext.expr()),
            _ => throw new ArgumentOutOfRangeException(nameof(context))
        };
    }

    private ExpressionDefinition GetDeserializeExpression(string typeName, ExpressionDefinition offsetDefinition,
        Dictionary<string, ExpressionDefinition> deserializerParams,
        Dictionary<LinearCommon.StandardProperty, ExpressionDefinition> standardProperties)
    {
        (int _, bool littleEndian) = StringToPrimitiveInfo(typeName);
        IDeserializer? deserializer = StringToDeserializer(typeName);
        if (deserializer == null) throw new ArgumentException($"Unknown deserializer \"{typeName}\" referenced.");
        return new DeserializeExpression(offsetDefinition, new ConstantExpression<bool>(littleEndian), deserializer,
            deserializerParams, standardProperties);
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
            LinearParser.TermRepAContext _ => new StructureEvaluateExpression<long>(i => i.AbsoluteOffset),
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
