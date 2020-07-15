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

        // TODO implement listener

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
            // TODO endianness support
            ExpressionDefinition littleEndianExpression = new ConstantExpression<bool>(true);
            string typeName = context.IDENTIFIER()[0].GetText();
            string dataName = context.IDENTIFIER()[1].GetText();
            Element dataElement;
            Type? targetType = StringToType(typeName);
            if (targetType != null)
                dataElement = new DataElement(dataName, targetType, offsetExpression, littleEndianExpression);
            else
            {
                // TODO deserializer param support
                Dictionary<string, ExpressionDefinition> deserializerParams =
                    new Dictionary<string, ExpressionDefinition>();
                dataElement = new DataElement(dataName, offsetExpression, littleEndianExpression,
                    StringToDeserializer(typeName), deserializerParams);
            }

            _currentDefinition!.Members.Add((dataName, dataElement));
        }

        private static Type? StringToType(string identifier) => identifier switch
        {
            "byte" => typeof(byte),
            "sbyte" => typeof(sbyte),
            "ushort" => typeof(ushort),
            "short" => typeof(short),
            "uint" => typeof(uint),
            "int" => typeof(int),
            "ulong" => typeof(ulong),
            "long" => typeof(long),
            "float" => typeof(float),
            "double" => typeof(double),
            "range" => typeof(ValueTuple<long, long>),
            _ => typeof(StructureInstance)
        };

        private IDeserializer StringToDeserializer(string identifier) =>
            _deserializers.TryGetValue(identifier, out IDeserializer deserializer)
                ? deserializer
                : throw new Exception($"Failed to find deserializer for type {identifier}");

        private static ExpressionDefinition GetExpression(LinearParser.ExprContext context)
        {
            // TODO implement recursive expression packing
            return new ConstantExpression<int>(0);
        }
    }
}
