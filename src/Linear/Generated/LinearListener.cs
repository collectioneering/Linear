//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\black\Documents\GitHub\Linear\scripts\\..\Linear.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="LinearParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface ILinearListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.compilation_unit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCompilation_unit([NotNull] LinearParser.Compilation_unitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.compilation_unit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCompilation_unit([NotNull] LinearParser.Compilation_unitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.struct"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStruct([NotNull] LinearParser.StructContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.struct"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStruct([NotNull] LinearParser.StructContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.struct_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStruct_statement([NotNull] LinearParser.Struct_statementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.struct_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStruct_statement([NotNull] LinearParser.Struct_statementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.struct_statement_define"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStruct_statement_define([NotNull] LinearParser.Struct_statement_defineContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.struct_statement_define"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStruct_statement_define([NotNull] LinearParser.Struct_statement_defineContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.struct_statement_define_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStruct_statement_define_value([NotNull] LinearParser.Struct_statement_define_valueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.struct_statement_define_value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStruct_statement_define_value([NotNull] LinearParser.Struct_statement_define_valueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.struct_statement_define_array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStruct_statement_define_array([NotNull] LinearParser.Struct_statement_define_arrayContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.struct_statement_define_array"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStruct_statement_define_array([NotNull] LinearParser.Struct_statement_define_arrayContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.struct_statement_define_array_indirect"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStruct_statement_define_array_indirect([NotNull] LinearParser.Struct_statement_define_array_indirectContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.struct_statement_define_array_indirect"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStruct_statement_define_array_indirect([NotNull] LinearParser.Struct_statement_define_array_indirectContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.struct_statement_output"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStruct_statement_output([NotNull] LinearParser.Struct_statement_outputContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.struct_statement_output"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStruct_statement_output([NotNull] LinearParser.Struct_statement_outputContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.struct_statement_comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStruct_statement_comment([NotNull] LinearParser.Struct_statement_commentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.struct_statement_comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStruct_statement_comment([NotNull] LinearParser.Struct_statement_commentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.property_group"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProperty_group([NotNull] LinearParser.Property_groupContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.property_group"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProperty_group([NotNull] LinearParser.Property_groupContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.property_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProperty_statement([NotNull] LinearParser.Property_statementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.property_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProperty_statement([NotNull] LinearParser.Property_statementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.term_replacement_length"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerm_replacement_length([NotNull] LinearParser.Term_replacement_lengthContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.term_replacement_length"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerm_replacement_length([NotNull] LinearParser.Term_replacement_lengthContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.term_replacement_i"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerm_replacement_i([NotNull] LinearParser.Term_replacement_iContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.term_replacement_i"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerm_replacement_i([NotNull] LinearParser.Term_replacement_iContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.term_replacement_p"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerm_replacement_p([NotNull] LinearParser.Term_replacement_pContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.term_replacement_p"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerm_replacement_p([NotNull] LinearParser.Term_replacement_pContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.term_replacement_u"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerm_replacement_u([NotNull] LinearParser.Term_replacement_uContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.term_replacement_u"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerm_replacement_u([NotNull] LinearParser.Term_replacement_uContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExprArrayAccess</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExprArrayAccess([NotNull] LinearParser.ExprArrayAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExprArrayAccess</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExprArrayAccess([NotNull] LinearParser.ExprArrayAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExprUnOp</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExprUnOp([NotNull] LinearParser.ExprUnOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExprUnOp</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExprUnOp([NotNull] LinearParser.ExprUnOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExprTerm</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExprTerm([NotNull] LinearParser.ExprTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExprTerm</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExprTerm([NotNull] LinearParser.ExprTermContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExprOp</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExprOp([NotNull] LinearParser.ExprOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExprOp</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExprOp([NotNull] LinearParser.ExprOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExprRangeLength</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExprRangeLength([NotNull] LinearParser.ExprRangeLengthContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExprRangeLength</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExprRangeLength([NotNull] LinearParser.ExprRangeLengthContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExprWrapped</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExprWrapped([NotNull] LinearParser.ExprWrappedContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExprWrapped</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExprWrapped([NotNull] LinearParser.ExprWrappedContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExprMember</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExprMember([NotNull] LinearParser.ExprMemberContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExprMember</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExprMember([NotNull] LinearParser.ExprMemberContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExprRangeEnd</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExprRangeEnd([NotNull] LinearParser.ExprRangeEndContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExprRangeEnd</c>
	/// labeled alternative in <see cref="LinearParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExprRangeEnd([NotNull] LinearParser.ExprRangeEndContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOp([NotNull] LinearParser.OpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOp([NotNull] LinearParser.OpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.un_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUn_op([NotNull] LinearParser.Un_opContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.un_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUn_op([NotNull] LinearParser.Un_opContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="LinearParser.bool_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBool_op([NotNull] LinearParser.Bool_opContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="LinearParser.bool_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBool_op([NotNull] LinearParser.Bool_opContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>StructSizeInt</c>
	/// labeled alternative in <see cref="LinearParser.struct_size"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStructSizeInt([NotNull] LinearParser.StructSizeIntContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>StructSizeInt</c>
	/// labeled alternative in <see cref="LinearParser.struct_size"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStructSizeInt([NotNull] LinearParser.StructSizeIntContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>StrictSizeHex</c>
	/// labeled alternative in <see cref="LinearParser.struct_size"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStrictSizeHex([NotNull] LinearParser.StrictSizeHexContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>StrictSizeHex</c>
	/// labeled alternative in <see cref="LinearParser.struct_size"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStrictSizeHex([NotNull] LinearParser.StrictSizeHexContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermRepLength</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermRepLength([NotNull] LinearParser.TermRepLengthContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermRepLength</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermRepLength([NotNull] LinearParser.TermRepLengthContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermRepI</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermRepI([NotNull] LinearParser.TermRepIContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermRepI</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermRepI([NotNull] LinearParser.TermRepIContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermRepP</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermRepP([NotNull] LinearParser.TermRepPContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermRepP</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermRepP([NotNull] LinearParser.TermRepPContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermRepU</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermRepU([NotNull] LinearParser.TermRepUContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermRepU</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermRepU([NotNull] LinearParser.TermRepUContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermIdentifier</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermIdentifier([NotNull] LinearParser.TermIdentifierContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermIdentifier</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermIdentifier([NotNull] LinearParser.TermIdentifierContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermInt</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermInt([NotNull] LinearParser.TermIntContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermInt</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermInt([NotNull] LinearParser.TermIntContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermHex</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermHex([NotNull] LinearParser.TermHexContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermHex</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermHex([NotNull] LinearParser.TermHexContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermReal</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermReal([NotNull] LinearParser.TermRealContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermReal</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermReal([NotNull] LinearParser.TermRealContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermChar</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermChar([NotNull] LinearParser.TermCharContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermChar</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermChar([NotNull] LinearParser.TermCharContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermString</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermString([NotNull] LinearParser.TermStringContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermString</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermString([NotNull] LinearParser.TermStringContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TermStringVerb</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermStringVerb([NotNull] LinearParser.TermStringVerbContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TermStringVerb</c>
	/// labeled alternative in <see cref="LinearParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermStringVerb([NotNull] LinearParser.TermStringVerbContext context);
}
