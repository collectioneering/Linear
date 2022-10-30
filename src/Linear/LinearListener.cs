using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Antlr4.Runtime;
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
    internal bool Fail { get; private set; }

    private readonly IReadOnlyDictionary<string, IDeserializer> _deserializers;
    private readonly IReadOnlyDictionary<string, MethodCallDelegate> _methods;
    private readonly string? _filenameHint;
    private readonly List<StructureDefinition> _structures;
    private readonly List<ParseError> _errors;
    private readonly HashSet<string> _currentNames;
    private StructureDefinition? _currentDefinition;

    /// <summary>
    /// Create new instance of <see cref="LinearListener"/>
    /// </summary>
    public LinearListener(IReadOnlyDictionary<string, IDeserializer> deserializers, IReadOnlyDictionary<string, MethodCallDelegate> methods, string? filenameHint = null)
    {
        _deserializers = deserializers;
        _methods = methods;
        _filenameHint = filenameHint;
        _structures = new List<StructureDefinition>();
        _errors = new List<ParseError>();
        _currentNames = new HashSet<string>();
    }

    /// <summary>
    /// Gets errors.
    /// </summary>
    /// <returns>Errors.</returns>
    public List<ParseError> GetErrors() => new(_errors);

    /// <summary>
    /// Gets parsed structures.
    /// </summary>
    /// <returns>Structures.</returns>
    public List<StructureDefinition> GetStructures() => new(_structures);

    public override void EnterStruct(LinearParser.StructContext context)
    {
        int defaultLength = context.struct_size() switch
        {
            LinearParser.StrictSizeHexContext strictSizeHexContext => Convert.ToInt32(strictSizeHexContext.GetText(), 16),
            LinearParser.StructSizeIntContext structSizeIntContext => int.Parse(structSizeIntContext.GetText(), CultureInfo.InvariantCulture),
            _ => 0
        };
        _currentDefinition = new StructureDefinition(context.IDENTIFIER().GetText(), defaultLength);
    }

    public override void ExitStruct(LinearParser.StructContext context)
    {
        _structures.Add(_currentDefinition!);
        _currentDefinition = null;
        _currentNames.Clear();
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
        var e = GetExpression(context.expr(), typeName);
        if (!_currentNames.Add(dataName))
        {
            AddError($"Duplicate name {dataName}", ids[1].Symbol);
            return;
        }
        if (e == null)
        {
            return;
        }
        Element dataElement = new ValueElement(dataName, e);
        _currentDefinition!.Members.Add(new StructureDefinitionMember(dataName, dataElement));
    }

    public override void EnterStruct_statement_define_array(
        LinearParser.Struct_statement_define_arrayContext context)
    {
        LinearParser.ExprContext[] exprs = context.expr();
        ExpressionDefinition? countExpression = GetExpression(exprs[0]);
        ExpressionDefinition? offsetExpression = GetExpression(exprs[1]);
        ITerminalNode[] ids = context.IDENTIFIER();
        string typeName = ids[0].GetText();
        string dataName = ids[1].GetText();
        (int _, bool littleEndian) = StringToPrimitiveInfo(typeName);
        ExpressionDefinition littleEndianExpression = new ConstantExpression<bool>(littleEndian);
        IDeserializer? deserializer = StringToDeserializer(typeName, context);
        var propGroup = GetPropertyGroup(context.property_group());
        if (!_currentNames.Add(dataName))
        {
            AddError($"Duplicate name {dataName}", ids[1].Symbol);
            return;
        }
        if (countExpression == null)
        {
            return;
        }
        if (offsetExpression == null)
        {
            return;
        }
        if (deserializer == null)
        {
            return;
        }
        if (propGroup == null)
        {
            return;
        }
        Dictionary<StandardProperty, ExpressionDefinition> standardProperties = new();
        standardProperties.Add(StandardProperty.ArrayLengthProperty, countExpression);
        // "Should" add some other way for length instead of routing through a dictionary...
        // "Should" add efficient primitive array deserialization...
        Element dataElement = new ValueElement(dataName, new DeserializeExpression(offsetExpression, littleEndianExpression, new ArrayDeserializer(deserializer), propGroup, standardProperties));
        _currentDefinition!.Members.Add(new StructureDefinitionMember(dataName, dataElement));
    }

    public override void EnterStruct_statement_define_array_indirect(
        LinearParser.Struct_statement_define_array_indirectContext context)
    {
        LinearParser.ExprContext[] exprs = context.expr();
        ExpressionDefinition? countExpression = GetExpression(exprs[0]);
        ExpressionDefinition? offsetExpression = GetExpression(exprs[1]);
        ExpressionDefinition? pointerOffsetExpression = GetExpression(exprs[2]);
        ITerminalNode[] ids = context.IDENTIFIER();
        string typeName = ids[0].GetText();
        string targetTypeName = ids[1].GetText();
        string dataName = ids[2].GetText();
        IDeserializer? deserializer = StringToDeserializer(typeName, context);
        IDeserializer? targetDeserializer = StringToDeserializer(targetTypeName, context);
        var propGroup = GetPropertyGroup(context.property_group());
        if (!_currentNames.Add(dataName))
        {
            AddError($"Duplicate name {dataName}", ids[1].Symbol);
            return;
        }
        if (countExpression == null)
        {
            return;
        }
        if (offsetExpression == null)
        {
            return;
        }
        if (pointerOffsetExpression == null)
        {
            return;
        }
        if (deserializer == null)
        {
            return;
        }
        if (targetDeserializer == null)
        {
            return;
        }
        if (propGroup == null)
        {
            return;
        }
        (int _, bool littleEndian) = StringToPrimitiveInfo(typeName);
        ExpressionDefinition littleEndianExpression = new ConstantExpression<bool>(littleEndian);
        Dictionary<StandardProperty, ExpressionDefinition> standardProperties = new();
        bool lenFinder = context.PLUS() != null;
        standardProperties.Add(StandardProperty.ArrayLengthProperty,
            lenFinder
                ? new OperatorDualExpression(countExpression, new ConstantExpression<int>(1), BinaryOperator.Add)
                : countExpression);
        standardProperties.Add(StandardProperty.PointerOffsetProperty, pointerOffsetExpression);
        standardProperties.Add(StandardProperty.PointerArrayLengthProperty, countExpression);
        ArrayDeserializer arrayDeserializer = new(deserializer);
        Element dataElement = new ValueElement(dataName,
            new DeserializeExpression(offsetExpression, littleEndianExpression,
                new PointerArrayDeserializer(arrayDeserializer, targetDeserializer, lenFinder), propGroup, standardProperties));
        _currentDefinition!.Members.Add(new StructureDefinitionMember(dataName, dataElement));
    }

    public override void EnterStruct_statement_call(LinearParser.Struct_statement_callContext context)
    {
        ExpressionDefinition? expr = GetExpression(context.expr());
        if (expr == null)
        {
            return;
        }
        Element dataElement = new MethodCallElement(expr);
        _currentDefinition!.Members.Add(new StructureDefinitionMember(null, dataElement));
    }

    public override void EnterStruct_statement_length(LinearParser.Struct_statement_lengthContext context)
    {
        ExpressionDefinition? expr = GetExpression(context.expr());
        if (expr == null)
        {
            return;
        }
        Element dataElement = new LengthElement(expr);
        _currentDefinition!.Members.Add(new StructureDefinitionMember(null, dataElement));
    }

    public override void EnterStruct_statement_output(LinearParser.Struct_statement_outputContext context)
    {
        ExpressionDefinition formatExpression = new ConstantExpression<string>(context.IDENTIFIER().GetText());
        ExpressionDefinition? rangeExpression = GetExpression(context.expr(0));
        ExpressionDefinition? nameExpression = GetExpression(context.expr(1));
        if (rangeExpression == null)
        {
            return;
        }
        if (nameExpression == null)
        {
            return;
        }
        Element outputElement = new OutputElement(formatExpression, rangeExpression, nameExpression,
            GetPropertyGroup(context.property_group()));

        _currentDefinition!.Members.Add(new StructureDefinitionMember(null, outputElement));
    }

    private Dictionary<string, ExpressionDefinition>? GetPropertyGroup(
        LinearParser.Property_groupContext? context)
    {
        if (context == null)
        {
            // null context defaults to empty prop group
            return new Dictionary<string, ExpressionDefinition>();
        }
        Dictionary<string, ExpressionDefinition> dict = new();
        bool fail = false;
        foreach (var entry in context.property_statement())
        {
            string? key = entry.IDENTIFIER().GetText();
            var e = GetExpression(entry.expr());
            if (key == null && e == null)
            {
                AddError("Null key/value pair in property group", context.Start);
                fail = true;
                continue;
            }
            if (key == null)
            {
                AddError($"Null key for expression {e}", context.Start);
                fail = true;
                continue;
            }
            if (e == null)
            {
                AddError($"Null expression for key {key}", context.Start);
                fail = true;
                continue;
            }
            dict[key] = e;
        }
        return fail ? null : dict;
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

    private IDeserializer? StringToDeserializer(string identifier, ParserRuleContext context)
    {
        if (_deserializers.TryGetValue(identifier, out IDeserializer? deserializer))
            return deserializer;
        AddError($"Failed to find deserializer for type {identifier}", context.Start);
        return null;
    }

    private ExpressionDefinition? GetExpression(LinearParser.ExprContext context, string? activeType = null)
    {
        //Console.WriteLine($"Rule ihndex {context.RuleIndex}");
        return context switch
        {
            LinearParser.ExprArrayAccessContext exprArrayAccessContext =>
                RequireNonNull(GetExpression(exprArrayAccessContext.expr(0)), GetExpression(exprArrayAccessContext.expr(1)), out var e0, out var e1)
                    ? new ArrayAccessExpression(e0, e1)
                    : null,
            LinearParser.ExprMemberContext exprMemberContext => RequireNonNull(GetExpression(exprMemberContext.expr()), out var e0)
                ? new ProxyMemberExpression(exprMemberContext.IDENTIFIER().GetText(), e0)
                : null,
            LinearParser.ExprMethodCallContext exprMethodCallContext =>
                RequireNonNull(RequireNonNull(exprMethodCallContext.expr().Select(e => GetExpression(e))), out var list)
                    ? new MethodCallExpression(_methods[exprMethodCallContext.IDENTIFIER().GetText()], list)
                    : null,
            LinearParser.ExprOpAddSubContext exprOpAddSubContext =>
                RequireNonNull(GetExpression(exprOpAddSubContext.expr(0)), GetExpression(exprOpAddSubContext.expr(1)), out var e0, out var e1)
                    ? new OperatorDualExpression(e0, e1, OperatorDualExpression.GetOperator(exprOpAddSubContext.op_add_sub().GetText()))
                    : null,
            LinearParser.ExprOpAmpContext exprOpAmpContext =>
                RequireNonNull(GetExpression(exprOpAmpContext.expr(0)), GetExpression(exprOpAmpContext.expr(1)), out var e0, out var e1)
                    ? new OperatorDualExpression(e0, e1, OperatorDualExpression.GetOperator(exprOpAmpContext.AMP().GetText()))
                    : null,
            LinearParser.ExprOpBitwiseOrContext exprOpBitwiseOrContext =>
                RequireNonNull(GetExpression(exprOpBitwiseOrContext.expr(0)), GetExpression(exprOpBitwiseOrContext.expr(1)), out var e0, out var e1)
                    ? new OperatorDualExpression(e0, e1, OperatorDualExpression.GetOperator(exprOpBitwiseOrContext.BITWISE_OR().GetText()))
                    : null,
            LinearParser.ExprOpCaretContext exprOpCaretContext =>
                RequireNonNull(GetExpression(exprOpCaretContext.expr(0)), GetExpression(exprOpCaretContext.expr(1)), out var e0, out var e1)
                    ? new OperatorDualExpression(e0, e1, OperatorDualExpression.GetOperator(exprOpCaretContext.CARET().GetText()))
                    : null,
            LinearParser.ExprOpMulDivContext exprOpMulDivContext =>
                RequireNonNull(GetExpression(exprOpMulDivContext.expr(0)), GetExpression(exprOpMulDivContext.expr(1)), out var e0, out var e1)
                    ? new OperatorDualExpression(e0, e1, OperatorDualExpression.GetOperator(exprOpMulDivContext.op_mul_div().GetText()))
                    : null,
            LinearParser.ExprRangeEndContext exprRangeEndContext =>
                RequireNonNull(GetExpression(exprRangeEndContext.expr(0)), out var e0)
                    ? new RangeExpression(e0, GetExpression(exprRangeEndContext.expr(1)), null)
                    : null,
            LinearParser.ExprRangeLengthContext exprRangeLengthContext =>
                RequireNonNull(GetExpression(exprRangeLengthContext.expr(0)), out var e0)
                    ? new RangeExpression(e0, null, GetExpression(exprRangeLengthContext.expr(1)))
                    : null,
            LinearParser.ExprTermContext exprTermContext => GetTerm(exprTermContext.term()),
            LinearParser.ExprDeserializeContext exprDeserializeContext =>
                RequireNonNull(GetExpression(exprDeserializeContext.expr()), GetPropertyGroup(exprDeserializeContext.property_group()), out var e0, out var propGroup)
                    ? GetDeserializeExpression(exprDeserializeContext.IDENTIFIER().GetText(), e0, propGroup, new Dictionary<StandardProperty, ExpressionDefinition>(), exprDeserializeContext)
                    : null,
            LinearParser.ExprUnboundDeserializeContext exprUnboundDeserializeContext =>
                RequireNonNull(GetActiveType(activeType, nameof(DeserializeExpression), exprUnboundDeserializeContext), GetExpression(exprUnboundDeserializeContext.expr()), GetPropertyGroup(exprUnboundDeserializeContext.property_group()), out var name, out var e0, out var propGroup)
                    ? GetDeserializeExpression(name, e0, propGroup, new Dictionary<StandardProperty, ExpressionDefinition>(), exprUnboundDeserializeContext)
                    : null,
            LinearParser.ExprUnOpContext exprUnOpContext => GetExpression(exprUnOpContext.expr()) is { } e0 ? new OperatorUnaryExpression(e0, OperatorUnaryExpression.GetOperator(exprUnOpContext.un_op().GetText())) : null,
            LinearParser.ExprWrappedContext exprWrappedContext => GetExpression(exprWrappedContext.expr()),
            _ => throw new ArgumentOutOfRangeException(nameof(context))
        };
    }

    internal static Dictionary<TKey, TValue>? RequireNonNull<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue?>> sequence) where TKey : notnull
    {
        Dictionary<TKey, TValue> dict = new();
        foreach (var e in sequence)
        {
            if (e.Value == null)
            {
                return null;
            }
            dict[e.Key] = e.Value;
        }
        return dict;
    }

    internal static List<T>? RequireNonNull<T>(IEnumerable<T?> sequence)
    {
        List<T> list = new();
        foreach (var e in sequence)
        {
            if (e == null)
            {
                return null;
            }
            list.Add(e);
        }
        return list;
    }

    internal static bool RequireNonNull<T1>(T1? v1, [NotNullWhen(true)] out T1? r1) where T1 : class
    {
        if (v1 != null)
        {
            r1 = v1;
            return true;
        }
        r1 = null;
        return false;
    }

    internal static bool RequireNonNull<T1, T2>(T1? v1, T2? v2,
        [NotNullWhen(true)] out T1? r1, [NotNullWhen(true)] out T2? r2)
        where T1 : class where T2 : class
    {
        if (v1 != null && v2 != null)
        {
            r1 = v1;
            r2 = v2;
            return true;
        }
        r1 = null;
        r2 = null;
        return false;
    }

    internal static bool RequireNonNull<T1, T2, T3>(T1? v1, T2? v2, T3? v3,
        [NotNullWhen(true)] out T1? r1, [NotNullWhen(true)] out T2? r2, [NotNullWhen(true)] out T3? r3)
        where T1 : class where T2 : class where T3 : class
    {
        if (v1 != null && v2 != null && v3 != null)
        {
            r1 = v1;
            r2 = v2;
            r3 = v3;
            return true;
        }
        r1 = null;
        r2 = null;
        r3 = null;
        return false;
    }

    internal static bool RequireNonNull<T1, T2, T3, T4>(T1? v1, T2? v2, T3? v3, T4? v4,
        [NotNullWhen(true)] out T1? r1, [NotNullWhen(true)] out T2? r2, [NotNullWhen(true)] out T3? r3, [NotNullWhen(true)] out T4? r4)
        where T1 : class where T2 : class where T3 : class where T4 : class
    {
        if (v1 != null && v2 != null && v3 != null && v4 != null)
        {
            r1 = v1;
            r2 = v2;
            r3 = v3;
            r4 = v4;
            return true;
        }
        r1 = null;
        r2 = null;
        r3 = null;
        r4 = null;
        return false;
    }

    private string? GetActiveType(string? activeType, string expressionType, ParserRuleContext context)
    {
        if (activeType != null)
        {
            return activeType;
        }
        AddError($"{expressionType} cannot be used without type name", context.Start);
        return null;
    }

    private ExpressionDefinition? GetDeserializeExpression(string typeName, ExpressionDefinition offsetDefinition,
        Dictionary<string, ExpressionDefinition> deserializerParams,
        Dictionary<StandardProperty, ExpressionDefinition> standardProperties, ParserRuleContext context)
    {
        (int _, bool littleEndian) = StringToPrimitiveInfo(typeName);
        IDeserializer? deserializer = StringToDeserializer(typeName, context);
        if (deserializer == null)
        {
            return null;
        }
        return new DeserializeExpression(offsetDefinition, new ConstantExpression<bool>(littleEndian), deserializer, deserializerParams, standardProperties);
    }

    private static ExpressionDefinition GetTerm(LinearParser.TermContext context)
    {
        return context switch
        {
            LinearParser.TermCharContext termCharContext => new ConstantExpression<char>(termCharContext.GetText().Trim(' ', '\'')[0]),
            LinearParser.TermHexContext termHexContext => new ConstantNumberExpression<long>(Convert.ToInt32(termHexContext.GetText(), 16)),
            LinearParser.TermIdentifierContext termIdentifierContext => new MemberExpression(termIdentifierContext.GetText()),
            LinearParser.TermIntContext termIntContext => new ConstantNumberExpression<long>(long.Parse(termIntContext.GetText(), CultureInfo.InvariantCulture)),
            LinearParser.TermRealContext termRealContext => new ConstantNumberExpression<double>(double.Parse(termRealContext.GetText(), CultureInfo.InvariantCulture)),
            LinearParser.TermRepAContext => new StructureEvaluateExpression<long>(i => i.AbsoluteOffset),
            LinearParser.TermRepIContext => new StructureEvaluateExpression<long>(i => i.Index),
            LinearParser.TermRepLengthContext => new StructureEvaluateExpression<long>(i => i.Length),
            LinearParser.TermRepPContext => new StructureEvaluateExpression<StructureInstance?>(i => i.Parent),
            LinearParser.TermRepUContext => new StructureEvaluateExpression<int>(i => i.GetUniqueId()),
            LinearParser.TermStringContext termStringContext => new ConstantExpression<string>(termStringContext.GetText().Trim(' ', '"')),
            LinearParser.TermStringVerbContext termStringVerbContext => new ConstantExpression<string>(termStringVerbContext.GetText().Trim(' ', '"', '@')),
            _ => throw new ArgumentOutOfRangeException(nameof(context))
        };
    }

    private void AddError(string error, IToken token)
    {
        AddError(error, token.Line, token.Column);
    }

    private void AddError(string error, int l, int c)
    {
        Fail = true;
        _errors.Add(new ParseError(new SourceLocation(_filenameHint, l, c), error));
    }
}
