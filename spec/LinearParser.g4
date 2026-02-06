parser grammar LinearParser;

options {
	tokenVocab = LinearLexer;
}

compilation_unit: body_element* EOF;

body_element: struct;

// structureName optionalDefaultLength {}
struct:
	IDENTIFIER struct_size? OPEN struct_statement* CLOSE;

struct_statement:
	struct_statement_define
	| struct_statement_define_lambda
	| struct_statement_discard
	| struct_statement_call
	| struct_statement_length
//	| struct_statement_define_value
	| struct_statement_define_array
	| struct_statement_define_array_indirect
	| struct_statement_output;

// varType memberName locationExpr {};
struct_statement_define:
	IDENTIFIER IDENTIFIER expr ENDL;
	//IDENTIFIER IDENTIFIER expr property_group? ENDL;

struct_statement_define_lambda:
	LAMBDA IDENTIFIER expr ENDL;

// value memberName valueExpr;
//struct_statement_define_value:
	//	EXEC_VALUE IDENTIFIER IDENTIFIER expr ENDL;
//	EXEC_VALUE IDENTIFIER expr ENDL;

// discard evalExpr;
struct_statement_discard: EXEC_DISCARD expr ENDL;

// call methodExpr;
struct_statement_call: EXEC_CALL expr ENDL;

// length lengthExpr;
struct_statement_length: EXEC_SETLENGTH expr ENDL;

// elementType[lengthExpr] memberName locationExpr {};
struct_statement_define_array:
	IDENTIFIER OPENSQ expr CLOSESQ IDENTIFIER expr property_group? ENDL;

// elementType[lengthExpr] -> targetType[] memberName pointerArrayLocationExpr, relativeOffsetExpr {};
struct_statement_define_array_indirect:
	IDENTIFIER OPENSQ expr CLOSESQ LINK_ARRAY PLUS? IDENTIFIER OPENSQ CLOSESQ
		IDENTIFIER expr COMMA expr property_group? ENDL;

// output formatName rangeExpr nameExpr {};
struct_statement_output:
	EXEC_OUTPUT IDENTIFIER expr expr property_group? ENDL;
// maybe "outputvar" for expression-based format selection?

// { name=valueExpr; name2=valueExpr2; }
property_group: OPEN property_statement* CLOSE;
property_statement: IDENTIFIER ASSIGNMENT expr ENDL;

term_replacement_length: EXEC_LENGTH;
term_replacement_a: EXEC_ABSOLUTE_INDEX;
term_replacement_i: EXEC_INDEX;
term_replacement_p: EXEC_PARENT;
term_replacement_u: EXEC_UNIQUE;
term_literal_true: TRUE;
term_literal_false: FALSE;
expr:
	IDENTIFIER OPENPA expr? (COMMA expr)* CLOSEPA		# ExprMethodCall
	| EXEC_REPLACE IDENTIFIER												# ExprLambdaReplacement
	| term															# ExprTerm
	| DESERIALIZE expr property_group?									# ExprUnboundDeserialize
	| IDENTIFIER DESERIALIZE expr property_group?						# ExprDeserialize
	| OPENSQ expr RANGE expr CLOSESQ					# ExprRangeEnd
	| OPENSQ expr COMMA expr CLOSESQ					# ExprRangeLength
	| expr DOT IDENTIFIER											# ExprMember
	| expr BANG expr													# ExprSourceWithOffset
	| expr OPENSQ expr CLOSESQ										# ExprArrayAccess
	| OPENPA expr CLOSEPA											# ExprWrapped
	| un_op expr												# ExprUnOp
	| expr op_mul_div expr									# ExprOpMulDiv
	| expr op_add_sub expr									# ExprOpAddSub
	| expr op_shift expr									# ExprOpShift
	| expr op_rel expr										# ExprOpRel
	| expr op_eq expr										# ExprOpEq
	| expr AMP expr											# ExprOpAmp
	| expr CARET expr										# ExprOpCaret
	| expr BITWISE_OR expr									# ExprOpBitwiseOr
	| expr op_cond_and expr									# ExprOpCondAnd
	| expr op_cond_or expr									# ExprOpCondOr
	| expr INTERR expr COLON expr						# ExprOpTernary
;
//	| expr bool_op expr # ExprBoolOp

un_op: PLUS | MINUS | BANG | TILDE;
op_mul_div: STAR | DIV | PERCENT;
op_add_sub: PLUS | MINUS;
op_shift: RSHIFT | URSHIFT | LSHIFT;
op_rel: LT | GT | OP_LE | OP_GE;
op_eq: OP_EQ | OP_NE;
op_cond_and: OP_AND;
op_cond_or: OP_OR;
bool_op:
	LT
	| GT
	| OP_AND
	| OP_OR
	| OP_EQ
	| OP_NE
	| OP_LE
	| OP_GE;
struct_size:
	INTEGER_LITERAL			# StructSizeInt
	| HEX_INTEGER_LITERAL	# StrictSizeHex;
term:
	term_replacement_length	# TermRepLength
	| term_replacement_a	# TermRepA
	| term_replacement_i	# TermRepI
	| term_replacement_p	# TermRepP
	| term_replacement_u	# TermRepU
	| term_literal_true		# TermLiteralTrue
	| term_literal_false	# TermLiteralFalse
	| IDENTIFIER			# TermIdentifier
	| INTEGER_LITERAL		# TermInt
	| HEX_INTEGER_LITERAL	# TermHex
	| REAL_LITERAL			# TermReal
	| CHARACTER_LITERAL		# TermChar
	| REGULAR_STRING		# TermString
	| VERBATIM_STRING		# TermStringVerb;
