//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.11.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /Users/Shared/gabura/git/Linear/scripts/../spec/Linear.g4 by ANTLR 4.11.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.11.1")]
[System.CLSCompliant(false)]
public partial class LinearLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, COMMENT=23, COMENT_BLOCK=24, 
		OPEN=25, CLOSE=26, OPENSQ=27, CLOSESQ=28, ENDL=29, IDENTIFIER=30, WS=31, 
		PLUS=32, MINUS=33, STAR=34, DIV=35, PERCENT=36, AMP=37, BITWISE_OR=38, 
		CARET=39, BANG=40, TILDE=41, ASSIGNMENT=42, LT=43, GT=44, INTERR=45, OP_AND=46, 
		OP_OR=47, OP_EQ=48, OP_NE=49, OP_LE=50, OP_GE=51, INTEGER_LITERAL=52, 
		HEX_INTEGER_LITERAL=53, REAL_LITERAL=54, CHARACTER_LITERAL=55, REGULAR_STRING=56, 
		VERBATIM_STRING=57;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "T__20", "T__21", "COMMENT", "COMENT_BLOCK", 
		"OPEN", "CLOSE", "OPENSQ", "CLOSESQ", "ENDL", "IDENTIFIER", "WS", "PLUS", 
		"MINUS", "STAR", "DIV", "PERCENT", "AMP", "BITWISE_OR", "CARET", "BANG", 
		"TILDE", "ASSIGNMENT", "LT", "GT", "INTERR", "OP_AND", "OP_OR", "OP_EQ", 
		"OP_NE", "OP_LE", "OP_GE", "INTEGER_LITERAL", "HEX_INTEGER_LITERAL", "REAL_LITERAL", 
		"CHARACTER_LITERAL", "REGULAR_STRING", "VERBATIM_STRING", "IntegerTypeSuffix", 
		"HexDigit", "CommonCharacter", "SimpleEscapeSequence", "HexEscapeSequence", 
		"UnicodeEscapeSequence", "ExponentPart", "Whitespace", "UnicodeClassZS", 
		"NewLine", "IdentifierOrKeyword", "IdentifierStartCharacter", "IdentifierPartCharacter", 
		"LetterCharacter", "DecimalDigitCharacter", "ConnectingCharacter", "CombiningCharacter", 
		"FormattingCharacter", "UnicodeClassLU", "UnicodeClassLL", "UnicodeClassLT", 
		"UnicodeClassLM", "UnicodeClassLO", "UnicodeClassNL", "UnicodeClassMN", 
		"UnicodeClassMC", "UnicodeClassCF", "UnicodeClassPC", "UnicodeClassND"
	};


	public LinearLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public LinearLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'lambda'", "'$discard'", "'$call'", "'$setlength'", "'->'", "','", 
		"'$output'", "'$length'", "'$a'", "'$i'", "'$p'", "'$parent'", "'$u'", 
		"'$unique'", "'true'", "'false'", "'('", "')'", "'$$'", "'`'", "'..'", 
		"'.'", null, null, "'{'", "'}'", "'['", "']'", "';'", null, null, "'+'", 
		"'-'", "'*'", "'/'", "'%'", "'&'", "'|'", "'^'", "'!'", "'~'", "'='", 
		"'<'", "'>'", "'?'", "'&&'", "'||'", "'=='", "'!='", "'<='", "'>='"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, "COMMENT", 
		"COMENT_BLOCK", "OPEN", "CLOSE", "OPENSQ", "CLOSESQ", "ENDL", "IDENTIFIER", 
		"WS", "PLUS", "MINUS", "STAR", "DIV", "PERCENT", "AMP", "BITWISE_OR", 
		"CARET", "BANG", "TILDE", "ASSIGNMENT", "LT", "GT", "INTERR", "OP_AND", 
		"OP_OR", "OP_EQ", "OP_NE", "OP_LE", "OP_GE", "INTEGER_LITERAL", "HEX_INTEGER_LITERAL", 
		"REAL_LITERAL", "CHARACTER_LITERAL", "REGULAR_STRING", "VERBATIM_STRING"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Linear.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static LinearLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,57,625,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,
		7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,2,61,7,61,2,62,7,62,2,63,
		7,63,2,64,7,64,2,65,7,65,2,66,7,66,2,67,7,67,2,68,7,68,2,69,7,69,2,70,
		7,70,2,71,7,71,2,72,7,72,2,73,7,73,2,74,7,74,2,75,7,75,2,76,7,76,2,77,
		7,77,2,78,7,78,2,79,7,79,2,80,7,80,2,81,7,81,2,82,7,82,2,83,7,83,2,84,
		7,84,2,85,7,85,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
		1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,
		3,1,3,1,4,1,4,1,4,1,5,1,5,1,6,1,6,1,6,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,
		1,7,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,9,1,9,1,9,1,10,1,10,1,10,1,11,1,11,1,
		11,1,11,1,11,1,11,1,11,1,11,1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,
		13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,15,1,15,1,15,1,
		16,1,16,1,17,1,17,1,18,1,18,1,18,1,19,1,19,1,20,1,20,1,20,1,21,1,21,1,
		22,1,22,1,22,1,22,5,22,285,8,22,10,22,12,22,288,9,22,1,23,1,23,1,23,1,
		23,5,23,294,8,23,10,23,12,23,297,9,23,1,23,1,23,1,23,1,24,1,24,1,25,1,
		25,1,26,1,26,1,27,1,27,1,28,1,28,1,29,3,29,313,8,29,1,29,1,29,1,30,1,30,
		4,30,319,8,30,11,30,12,30,320,1,31,1,31,1,32,1,32,1,33,1,33,1,34,1,34,
		1,35,1,35,1,36,1,36,1,37,1,37,1,38,1,38,1,39,1,39,1,40,1,40,1,41,1,41,
		1,42,1,42,1,43,1,43,1,44,1,44,1,45,1,45,1,45,1,46,1,46,1,46,1,47,1,47,
		1,47,1,48,1,48,1,48,1,49,1,49,1,49,1,50,1,50,1,50,1,51,4,51,370,8,51,11,
		51,12,51,371,1,51,3,51,375,8,51,1,52,1,52,1,52,4,52,380,8,52,11,52,12,
		52,381,1,52,3,52,385,8,52,1,53,5,53,388,8,53,10,53,12,53,391,9,53,1,53,
		1,53,4,53,395,8,53,11,53,12,53,396,1,53,3,53,400,8,53,1,53,3,53,403,8,
		53,1,53,4,53,406,8,53,11,53,12,53,407,1,53,1,53,1,53,3,53,413,8,53,3,53,
		415,8,53,3,53,417,8,53,1,54,1,54,1,54,3,54,422,8,54,1,54,1,54,1,55,1,55,
		1,55,5,55,429,8,55,10,55,12,55,432,9,55,1,55,1,55,1,56,1,56,1,56,1,56,
		1,56,1,56,5,56,442,8,56,10,56,12,56,445,9,56,1,56,1,56,1,57,3,57,450,8,
		57,1,57,1,57,3,57,454,8,57,1,57,3,57,457,8,57,1,58,3,58,460,8,58,1,59,
		1,59,1,59,3,59,465,8,59,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,
		60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,3,60,489,
		8,60,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,
		1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,3,61,516,8,
		61,1,62,1,62,1,62,1,62,1,62,1,62,1,62,1,62,1,62,1,62,1,62,1,62,1,62,1,
		62,1,62,1,62,1,62,1,62,1,62,1,62,3,62,538,8,62,1,63,1,63,3,63,542,8,63,
		1,63,4,63,545,8,63,11,63,12,63,546,1,64,1,64,3,64,551,8,64,1,65,1,65,1,
		66,1,66,1,66,3,66,558,8,66,1,67,1,67,5,67,562,8,67,10,67,12,67,565,9,67,
		1,68,1,68,3,68,569,8,68,1,69,1,69,1,69,1,69,1,69,3,69,576,8,69,1,70,1,
		70,1,70,1,70,1,70,1,70,1,70,3,70,585,8,70,1,71,1,71,3,71,589,8,71,1,72,
		1,72,3,72,593,8,72,1,73,1,73,1,73,3,73,598,8,73,1,74,1,74,3,74,602,8,74,
		1,75,1,75,1,76,1,76,1,77,1,77,1,78,1,78,1,79,1,79,1,80,1,80,1,81,1,81,
		1,82,1,82,1,83,1,83,1,84,1,84,1,85,1,85,1,295,0,86,1,1,3,2,5,3,7,4,9,5,
		11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,
		18,37,19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,
		30,61,31,63,32,65,33,67,34,69,35,71,36,73,37,75,38,77,39,79,40,81,41,83,
		42,85,43,87,44,89,45,91,46,93,47,95,48,97,49,99,50,101,51,103,52,105,53,
		107,54,109,55,111,56,113,57,115,0,117,0,119,0,121,0,123,0,125,0,127,0,
		129,0,131,0,133,0,135,0,137,0,139,0,141,0,143,0,145,0,147,0,149,0,151,
		0,153,0,155,0,157,0,159,0,161,0,163,0,165,0,167,0,169,0,171,0,1,0,25,2,
		0,10,10,13,13,1,0,48,57,2,0,88,88,120,120,6,0,68,68,70,70,77,77,100,100,
		102,102,109,109,6,0,10,10,13,13,39,39,92,92,133,133,8232,8233,6,0,10,10,
		13,13,34,34,92,92,133,133,8232,8233,1,0,34,34,2,0,76,76,108,108,2,0,85,
		85,117,117,3,0,48,57,65,70,97,102,2,0,69,69,101,101,2,0,43,43,45,45,2,
		0,9,9,11,12,9,0,32,32,160,160,5760,5760,6158,6158,8192,8198,8200,8202,
		8239,8239,8287,8287,12288,12288,4,0,10,10,13,13,133,133,8232,8233,82,0,
		65,90,192,214,216,222,256,310,313,327,330,381,385,386,388,395,398,401,
		403,404,406,408,412,413,415,416,418,425,428,435,437,444,452,461,463,475,
		478,494,497,500,502,504,506,562,570,571,573,574,577,582,584,590,880,882,
		886,895,902,906,908,929,931,939,975,980,984,1006,1012,1015,1017,1018,1021,
		1071,1120,1152,1162,1229,1232,1326,1329,1366,4256,4293,4295,4301,7680,
		7828,7838,7934,7944,7951,7960,7965,7976,7983,7992,7999,8008,8013,8025,
		8031,8040,8047,8120,8123,8136,8139,8152,8155,8168,8172,8184,8187,8450,
		8455,8459,8461,8464,8466,8469,8477,8484,8493,8496,8499,8510,8511,8517,
		8579,11264,11310,11360,11364,11367,11376,11378,11381,11390,11392,11394,
		11490,11499,11501,11506,42560,42562,42604,42624,42650,42786,42798,42802,
		42862,42873,42886,42891,42893,42896,42898,42902,42925,42928,42929,65313,
		65338,81,0,97,122,181,246,248,255,257,375,378,384,387,389,392,402,405,
		411,414,417,419,421,424,429,432,436,438,447,454,460,462,499,501,505,507,
		569,572,578,583,659,661,687,881,883,887,893,912,974,976,977,981,983,985,
		1011,1013,1119,1121,1153,1163,1215,1218,1327,1377,1415,7424,7467,7531,
		7543,7545,7578,7681,7837,7839,7943,7952,7957,7968,7975,7984,7991,8000,
		8005,8016,8023,8032,8039,8048,8061,8064,8071,8080,8087,8096,8103,8112,
		8116,8118,8119,8126,8132,8134,8135,8144,8147,8150,8151,8160,8167,8178,
		8180,8182,8183,8458,8467,8495,8505,8508,8509,8518,8521,8526,8580,11312,
		11358,11361,11372,11377,11387,11393,11500,11502,11507,11520,11557,11559,
		11565,42561,42605,42625,42651,42787,42801,42803,42872,42874,42876,42879,
		42887,42892,42894,42897,42901,42903,42921,43002,43866,43876,43877,64256,
		64262,64275,64279,65345,65370,6,0,453,459,498,8079,8088,8095,8104,8111,
		8124,8140,8188,8188,33,0,688,705,710,721,736,740,748,750,884,890,1369,
		1600,1765,1766,2036,2037,2042,2074,2084,2088,2417,3654,3782,4348,6103,
		6211,6823,7293,7468,7530,7544,7615,8305,8319,8336,8348,11388,11389,11631,
		11823,12293,12341,12347,12542,40981,42237,42508,42623,42652,42653,42775,
		42783,42864,42888,43000,43001,43471,43494,43632,43741,43763,43764,43868,
		43871,65392,65439,234,0,170,186,443,451,660,1514,1520,1522,1568,1599,1601,
		1610,1646,1647,1649,1747,1749,1788,1791,1808,1810,1839,1869,1957,1969,
		2026,2048,2069,2112,2136,2208,2226,2308,2361,2365,2384,2392,2401,2418,
		2432,2437,2444,2447,2448,2451,2472,2474,2480,2482,2489,2493,2510,2524,
		2525,2527,2529,2544,2545,2565,2570,2575,2576,2579,2600,2602,2608,2610,
		2611,2613,2614,2616,2617,2649,2652,2654,2676,2693,2701,2703,2705,2707,
		2728,2730,2736,2738,2739,2741,2745,2749,2768,2784,2785,2821,2828,2831,
		2832,2835,2856,2858,2864,2866,2867,2869,2873,2877,2913,2929,2947,2949,
		2954,2958,2960,2962,2965,2969,2970,2972,2986,2990,3001,3024,3084,3086,
		3088,3090,3112,3114,3129,3133,3212,3214,3216,3218,3240,3242,3251,3253,
		3257,3261,3294,3296,3297,3313,3314,3333,3340,3342,3344,3346,3386,3389,
		3406,3424,3425,3450,3455,3461,3478,3482,3505,3507,3515,3517,3526,3585,
		3632,3634,3635,3648,3653,3713,3714,3716,3722,3725,3735,3737,3743,3745,
		3747,3749,3751,3754,3755,3757,3760,3762,3763,3773,3780,3804,3807,3840,
		3911,3913,3948,3976,3980,4096,4138,4159,4181,4186,4189,4193,4208,4213,
		4225,4238,4346,4349,4680,4682,4685,4688,4694,4696,4701,4704,4744,4746,
		4749,4752,4784,4786,4789,4792,4798,4800,4805,4808,4822,4824,4880,4882,
		4885,4888,4954,4992,5007,5024,5108,5121,5740,5743,5759,5761,5786,5792,
		5866,5873,5880,5888,5900,5902,5905,5920,5937,5952,5969,5984,5996,5998,
		6000,6016,6067,6108,6210,6212,6263,6272,6312,6314,6389,6400,6430,6480,
		6509,6512,6516,6528,6571,6593,6599,6656,6678,6688,6740,6917,6963,6981,
		6987,7043,7072,7086,7087,7098,7141,7168,7203,7245,7247,7258,7287,7401,
		7404,7406,7409,7413,7414,8501,8504,11568,11623,11648,11670,11680,11686,
		11688,11694,11696,11702,11704,11710,11712,11718,11720,11726,11728,11734,
		11736,11742,12294,12348,12353,12438,12447,12538,12543,12589,12593,12686,
		12704,12730,12784,12799,13312,19893,19968,40908,40960,40980,40982,42124,
		42192,42231,42240,42507,42512,42527,42538,42539,42606,42725,42999,43009,
		43011,43013,43015,43018,43020,43042,43072,43123,43138,43187,43250,43255,
		43259,43301,43312,43334,43360,43388,43396,43442,43488,43492,43495,43503,
		43514,43518,43520,43560,43584,43586,43588,43595,43616,43631,43633,43638,
		43642,43695,43697,43709,43712,43714,43739,43740,43744,43754,43762,43782,
		43785,43790,43793,43798,43808,43814,43816,43822,43968,44002,44032,55203,
		55216,55238,55243,55291,63744,64109,64112,64217,64285,64296,64298,64310,
		64312,64316,64318,64433,64467,64829,64848,64911,64914,64967,65008,65019,
		65136,65140,65142,65276,65382,65391,65393,65437,65440,65470,65474,65479,
		65482,65487,65490,65495,65498,65500,2,0,5870,5872,8544,8559,3,0,2307,2307,
		2366,2368,2377,2380,3,0,173,173,1536,1539,1757,1757,6,0,95,95,8255,8256,
		8276,8276,65075,65076,65101,65103,65343,65343,37,0,48,57,1632,1641,1776,
		1785,1984,1993,2406,2415,2534,2543,2662,2671,2790,2799,2918,2927,3046,
		3055,3174,3183,3302,3311,3430,3439,3558,3567,3664,3673,3792,3801,3872,
		3881,4160,4169,4240,4249,6112,6121,6160,6169,6470,6479,6608,6617,6784,
		6793,6800,6809,6992,7001,7088,7097,7232,7241,7248,7257,42528,42537,43216,
		43225,43264,43273,43472,43481,43504,43513,43600,43609,44016,44025,65296,
		65305,657,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,
		0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,
		1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,
		0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,
		1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,
		0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,
		1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,
		0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,
		1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,
		0,0,99,1,0,0,0,0,101,1,0,0,0,0,103,1,0,0,0,0,105,1,0,0,0,0,107,1,0,0,0,
		0,109,1,0,0,0,0,111,1,0,0,0,0,113,1,0,0,0,1,173,1,0,0,0,3,180,1,0,0,0,
		5,189,1,0,0,0,7,195,1,0,0,0,9,206,1,0,0,0,11,209,1,0,0,0,13,211,1,0,0,
		0,15,219,1,0,0,0,17,227,1,0,0,0,19,230,1,0,0,0,21,233,1,0,0,0,23,236,1,
		0,0,0,25,244,1,0,0,0,27,247,1,0,0,0,29,255,1,0,0,0,31,260,1,0,0,0,33,266,
		1,0,0,0,35,268,1,0,0,0,37,270,1,0,0,0,39,273,1,0,0,0,41,275,1,0,0,0,43,
		278,1,0,0,0,45,280,1,0,0,0,47,289,1,0,0,0,49,301,1,0,0,0,51,303,1,0,0,
		0,53,305,1,0,0,0,55,307,1,0,0,0,57,309,1,0,0,0,59,312,1,0,0,0,61,318,1,
		0,0,0,63,322,1,0,0,0,65,324,1,0,0,0,67,326,1,0,0,0,69,328,1,0,0,0,71,330,
		1,0,0,0,73,332,1,0,0,0,75,334,1,0,0,0,77,336,1,0,0,0,79,338,1,0,0,0,81,
		340,1,0,0,0,83,342,1,0,0,0,85,344,1,0,0,0,87,346,1,0,0,0,89,348,1,0,0,
		0,91,350,1,0,0,0,93,353,1,0,0,0,95,356,1,0,0,0,97,359,1,0,0,0,99,362,1,
		0,0,0,101,365,1,0,0,0,103,369,1,0,0,0,105,376,1,0,0,0,107,416,1,0,0,0,
		109,418,1,0,0,0,111,425,1,0,0,0,113,435,1,0,0,0,115,456,1,0,0,0,117,459,
		1,0,0,0,119,464,1,0,0,0,121,488,1,0,0,0,123,515,1,0,0,0,125,537,1,0,0,
		0,127,539,1,0,0,0,129,550,1,0,0,0,131,552,1,0,0,0,133,557,1,0,0,0,135,
		559,1,0,0,0,137,568,1,0,0,0,139,575,1,0,0,0,141,584,1,0,0,0,143,588,1,
		0,0,0,145,592,1,0,0,0,147,597,1,0,0,0,149,601,1,0,0,0,151,603,1,0,0,0,
		153,605,1,0,0,0,155,607,1,0,0,0,157,609,1,0,0,0,159,611,1,0,0,0,161,613,
		1,0,0,0,163,615,1,0,0,0,165,617,1,0,0,0,167,619,1,0,0,0,169,621,1,0,0,
		0,171,623,1,0,0,0,173,174,5,108,0,0,174,175,5,97,0,0,175,176,5,109,0,0,
		176,177,5,98,0,0,177,178,5,100,0,0,178,179,5,97,0,0,179,2,1,0,0,0,180,
		181,5,36,0,0,181,182,5,100,0,0,182,183,5,105,0,0,183,184,5,115,0,0,184,
		185,5,99,0,0,185,186,5,97,0,0,186,187,5,114,0,0,187,188,5,100,0,0,188,
		4,1,0,0,0,189,190,5,36,0,0,190,191,5,99,0,0,191,192,5,97,0,0,192,193,5,
		108,0,0,193,194,5,108,0,0,194,6,1,0,0,0,195,196,5,36,0,0,196,197,5,115,
		0,0,197,198,5,101,0,0,198,199,5,116,0,0,199,200,5,108,0,0,200,201,5,101,
		0,0,201,202,5,110,0,0,202,203,5,103,0,0,203,204,5,116,0,0,204,205,5,104,
		0,0,205,8,1,0,0,0,206,207,5,45,0,0,207,208,5,62,0,0,208,10,1,0,0,0,209,
		210,5,44,0,0,210,12,1,0,0,0,211,212,5,36,0,0,212,213,5,111,0,0,213,214,
		5,117,0,0,214,215,5,116,0,0,215,216,5,112,0,0,216,217,5,117,0,0,217,218,
		5,116,0,0,218,14,1,0,0,0,219,220,5,36,0,0,220,221,5,108,0,0,221,222,5,
		101,0,0,222,223,5,110,0,0,223,224,5,103,0,0,224,225,5,116,0,0,225,226,
		5,104,0,0,226,16,1,0,0,0,227,228,5,36,0,0,228,229,5,97,0,0,229,18,1,0,
		0,0,230,231,5,36,0,0,231,232,5,105,0,0,232,20,1,0,0,0,233,234,5,36,0,0,
		234,235,5,112,0,0,235,22,1,0,0,0,236,237,5,36,0,0,237,238,5,112,0,0,238,
		239,5,97,0,0,239,240,5,114,0,0,240,241,5,101,0,0,241,242,5,110,0,0,242,
		243,5,116,0,0,243,24,1,0,0,0,244,245,5,36,0,0,245,246,5,117,0,0,246,26,
		1,0,0,0,247,248,5,36,0,0,248,249,5,117,0,0,249,250,5,110,0,0,250,251,5,
		105,0,0,251,252,5,113,0,0,252,253,5,117,0,0,253,254,5,101,0,0,254,28,1,
		0,0,0,255,256,5,116,0,0,256,257,5,114,0,0,257,258,5,117,0,0,258,259,5,
		101,0,0,259,30,1,0,0,0,260,261,5,102,0,0,261,262,5,97,0,0,262,263,5,108,
		0,0,263,264,5,115,0,0,264,265,5,101,0,0,265,32,1,0,0,0,266,267,5,40,0,
		0,267,34,1,0,0,0,268,269,5,41,0,0,269,36,1,0,0,0,270,271,5,36,0,0,271,
		272,5,36,0,0,272,38,1,0,0,0,273,274,5,96,0,0,274,40,1,0,0,0,275,276,5,
		46,0,0,276,277,5,46,0,0,277,42,1,0,0,0,278,279,5,46,0,0,279,44,1,0,0,0,
		280,281,5,47,0,0,281,282,5,47,0,0,282,286,1,0,0,0,283,285,8,0,0,0,284,
		283,1,0,0,0,285,288,1,0,0,0,286,284,1,0,0,0,286,287,1,0,0,0,287,46,1,0,
		0,0,288,286,1,0,0,0,289,290,5,47,0,0,290,291,5,42,0,0,291,295,1,0,0,0,
		292,294,9,0,0,0,293,292,1,0,0,0,294,297,1,0,0,0,295,296,1,0,0,0,295,293,
		1,0,0,0,296,298,1,0,0,0,297,295,1,0,0,0,298,299,5,42,0,0,299,300,5,47,
		0,0,300,48,1,0,0,0,301,302,5,123,0,0,302,50,1,0,0,0,303,304,5,125,0,0,
		304,52,1,0,0,0,305,306,5,91,0,0,306,54,1,0,0,0,307,308,5,93,0,0,308,56,
		1,0,0,0,309,310,5,59,0,0,310,58,1,0,0,0,311,313,5,64,0,0,312,311,1,0,0,
		0,312,313,1,0,0,0,313,314,1,0,0,0,314,315,3,135,67,0,315,60,1,0,0,0,316,
		319,3,129,64,0,317,319,3,133,66,0,318,316,1,0,0,0,318,317,1,0,0,0,319,
		320,1,0,0,0,320,318,1,0,0,0,320,321,1,0,0,0,321,62,1,0,0,0,322,323,5,43,
		0,0,323,64,1,0,0,0,324,325,5,45,0,0,325,66,1,0,0,0,326,327,5,42,0,0,327,
		68,1,0,0,0,328,329,5,47,0,0,329,70,1,0,0,0,330,331,5,37,0,0,331,72,1,0,
		0,0,332,333,5,38,0,0,333,74,1,0,0,0,334,335,5,124,0,0,335,76,1,0,0,0,336,
		337,5,94,0,0,337,78,1,0,0,0,338,339,5,33,0,0,339,80,1,0,0,0,340,341,5,
		126,0,0,341,82,1,0,0,0,342,343,5,61,0,0,343,84,1,0,0,0,344,345,5,60,0,
		0,345,86,1,0,0,0,346,347,5,62,0,0,347,88,1,0,0,0,348,349,5,63,0,0,349,
		90,1,0,0,0,350,351,5,38,0,0,351,352,5,38,0,0,352,92,1,0,0,0,353,354,5,
		124,0,0,354,355,5,124,0,0,355,94,1,0,0,0,356,357,5,61,0,0,357,358,5,61,
		0,0,358,96,1,0,0,0,359,360,5,33,0,0,360,361,5,61,0,0,361,98,1,0,0,0,362,
		363,5,60,0,0,363,364,5,61,0,0,364,100,1,0,0,0,365,366,5,62,0,0,366,367,
		5,61,0,0,367,102,1,0,0,0,368,370,7,1,0,0,369,368,1,0,0,0,370,371,1,0,0,
		0,371,369,1,0,0,0,371,372,1,0,0,0,372,374,1,0,0,0,373,375,3,115,57,0,374,
		373,1,0,0,0,374,375,1,0,0,0,375,104,1,0,0,0,376,377,5,48,0,0,377,379,7,
		2,0,0,378,380,3,117,58,0,379,378,1,0,0,0,380,381,1,0,0,0,381,379,1,0,0,
		0,381,382,1,0,0,0,382,384,1,0,0,0,383,385,3,115,57,0,384,383,1,0,0,0,384,
		385,1,0,0,0,385,106,1,0,0,0,386,388,7,1,0,0,387,386,1,0,0,0,388,391,1,
		0,0,0,389,387,1,0,0,0,389,390,1,0,0,0,390,392,1,0,0,0,391,389,1,0,0,0,
		392,394,5,46,0,0,393,395,7,1,0,0,394,393,1,0,0,0,395,396,1,0,0,0,396,394,
		1,0,0,0,396,397,1,0,0,0,397,399,1,0,0,0,398,400,3,127,63,0,399,398,1,0,
		0,0,399,400,1,0,0,0,400,402,1,0,0,0,401,403,7,3,0,0,402,401,1,0,0,0,402,
		403,1,0,0,0,403,417,1,0,0,0,404,406,7,1,0,0,405,404,1,0,0,0,406,407,1,
		0,0,0,407,405,1,0,0,0,407,408,1,0,0,0,408,414,1,0,0,0,409,415,7,3,0,0,
		410,412,3,127,63,0,411,413,7,3,0,0,412,411,1,0,0,0,412,413,1,0,0,0,413,
		415,1,0,0,0,414,409,1,0,0,0,414,410,1,0,0,0,415,417,1,0,0,0,416,389,1,
		0,0,0,416,405,1,0,0,0,417,108,1,0,0,0,418,421,5,39,0,0,419,422,8,4,0,0,
		420,422,3,119,59,0,421,419,1,0,0,0,421,420,1,0,0,0,422,423,1,0,0,0,423,
		424,5,39,0,0,424,110,1,0,0,0,425,430,5,34,0,0,426,429,8,5,0,0,427,429,
		3,119,59,0,428,426,1,0,0,0,428,427,1,0,0,0,429,432,1,0,0,0,430,428,1,0,
		0,0,430,431,1,0,0,0,431,433,1,0,0,0,432,430,1,0,0,0,433,434,5,34,0,0,434,
		112,1,0,0,0,435,436,5,64,0,0,436,437,5,34,0,0,437,443,1,0,0,0,438,442,
		8,6,0,0,439,440,5,34,0,0,440,442,5,34,0,0,441,438,1,0,0,0,441,439,1,0,
		0,0,442,445,1,0,0,0,443,441,1,0,0,0,443,444,1,0,0,0,444,446,1,0,0,0,445,
		443,1,0,0,0,446,447,5,34,0,0,447,114,1,0,0,0,448,450,7,7,0,0,449,448,1,
		0,0,0,449,450,1,0,0,0,450,451,1,0,0,0,451,457,7,8,0,0,452,454,7,8,0,0,
		453,452,1,0,0,0,453,454,1,0,0,0,454,455,1,0,0,0,455,457,7,7,0,0,456,449,
		1,0,0,0,456,453,1,0,0,0,457,116,1,0,0,0,458,460,7,9,0,0,459,458,1,0,0,
		0,460,118,1,0,0,0,461,465,3,121,60,0,462,465,3,123,61,0,463,465,3,125,
		62,0,464,461,1,0,0,0,464,462,1,0,0,0,464,463,1,0,0,0,465,120,1,0,0,0,466,
		467,5,92,0,0,467,489,5,39,0,0,468,469,5,92,0,0,469,489,5,34,0,0,470,471,
		5,92,0,0,471,489,5,92,0,0,472,473,5,92,0,0,473,489,5,48,0,0,474,475,5,
		92,0,0,475,489,5,97,0,0,476,477,5,92,0,0,477,489,5,98,0,0,478,479,5,92,
		0,0,479,489,5,102,0,0,480,481,5,92,0,0,481,489,5,110,0,0,482,483,5,92,
		0,0,483,489,5,114,0,0,484,485,5,92,0,0,485,489,5,116,0,0,486,487,5,92,
		0,0,487,489,5,118,0,0,488,466,1,0,0,0,488,468,1,0,0,0,488,470,1,0,0,0,
		488,472,1,0,0,0,488,474,1,0,0,0,488,476,1,0,0,0,488,478,1,0,0,0,488,480,
		1,0,0,0,488,482,1,0,0,0,488,484,1,0,0,0,488,486,1,0,0,0,489,122,1,0,0,
		0,490,491,5,92,0,0,491,492,5,120,0,0,492,493,1,0,0,0,493,516,3,117,58,
		0,494,495,5,92,0,0,495,496,5,120,0,0,496,497,1,0,0,0,497,498,3,117,58,
		0,498,499,3,117,58,0,499,516,1,0,0,0,500,501,5,92,0,0,501,502,5,120,0,
		0,502,503,1,0,0,0,503,504,3,117,58,0,504,505,3,117,58,0,505,506,3,117,
		58,0,506,516,1,0,0,0,507,508,5,92,0,0,508,509,5,120,0,0,509,510,1,0,0,
		0,510,511,3,117,58,0,511,512,3,117,58,0,512,513,3,117,58,0,513,514,3,117,
		58,0,514,516,1,0,0,0,515,490,1,0,0,0,515,494,1,0,0,0,515,500,1,0,0,0,515,
		507,1,0,0,0,516,124,1,0,0,0,517,518,5,92,0,0,518,519,5,117,0,0,519,520,
		1,0,0,0,520,521,3,117,58,0,521,522,3,117,58,0,522,523,3,117,58,0,523,524,
		3,117,58,0,524,538,1,0,0,0,525,526,5,92,0,0,526,527,5,85,0,0,527,528,1,
		0,0,0,528,529,3,117,58,0,529,530,3,117,58,0,530,531,3,117,58,0,531,532,
		3,117,58,0,532,533,3,117,58,0,533,534,3,117,58,0,534,535,3,117,58,0,535,
		536,3,117,58,0,536,538,1,0,0,0,537,517,1,0,0,0,537,525,1,0,0,0,538,126,
		1,0,0,0,539,541,7,10,0,0,540,542,7,11,0,0,541,540,1,0,0,0,541,542,1,0,
		0,0,542,544,1,0,0,0,543,545,7,1,0,0,544,543,1,0,0,0,545,546,1,0,0,0,546,
		544,1,0,0,0,546,547,1,0,0,0,547,128,1,0,0,0,548,551,3,131,65,0,549,551,
		7,12,0,0,550,548,1,0,0,0,550,549,1,0,0,0,551,130,1,0,0,0,552,553,7,13,
		0,0,553,132,1,0,0,0,554,555,5,13,0,0,555,558,5,10,0,0,556,558,7,14,0,0,
		557,554,1,0,0,0,557,556,1,0,0,0,558,134,1,0,0,0,559,563,3,137,68,0,560,
		562,3,139,69,0,561,560,1,0,0,0,562,565,1,0,0,0,563,561,1,0,0,0,563,564,
		1,0,0,0,564,136,1,0,0,0,565,563,1,0,0,0,566,569,3,141,70,0,567,569,5,95,
		0,0,568,566,1,0,0,0,568,567,1,0,0,0,569,138,1,0,0,0,570,576,3,141,70,0,
		571,576,3,143,71,0,572,576,3,145,72,0,573,576,3,147,73,0,574,576,3,149,
		74,0,575,570,1,0,0,0,575,571,1,0,0,0,575,572,1,0,0,0,575,573,1,0,0,0,575,
		574,1,0,0,0,576,140,1,0,0,0,577,585,3,151,75,0,578,585,3,153,76,0,579,
		585,3,155,77,0,580,585,3,157,78,0,581,585,3,159,79,0,582,585,3,161,80,
		0,583,585,3,125,62,0,584,577,1,0,0,0,584,578,1,0,0,0,584,579,1,0,0,0,584,
		580,1,0,0,0,584,581,1,0,0,0,584,582,1,0,0,0,584,583,1,0,0,0,585,142,1,
		0,0,0,586,589,3,171,85,0,587,589,3,125,62,0,588,586,1,0,0,0,588,587,1,
		0,0,0,589,144,1,0,0,0,590,593,3,169,84,0,591,593,3,125,62,0,592,590,1,
		0,0,0,592,591,1,0,0,0,593,146,1,0,0,0,594,598,3,163,81,0,595,598,3,165,
		82,0,596,598,3,125,62,0,597,594,1,0,0,0,597,595,1,0,0,0,597,596,1,0,0,
		0,598,148,1,0,0,0,599,602,3,167,83,0,600,602,3,125,62,0,601,599,1,0,0,
		0,601,600,1,0,0,0,602,150,1,0,0,0,603,604,7,15,0,0,604,152,1,0,0,0,605,
		606,7,16,0,0,606,154,1,0,0,0,607,608,7,17,0,0,608,156,1,0,0,0,609,610,
		7,18,0,0,610,158,1,0,0,0,611,612,7,19,0,0,612,160,1,0,0,0,613,614,7,20,
		0,0,614,162,1,0,0,0,615,616,2,768,784,0,616,164,1,0,0,0,617,618,7,21,0,
		0,618,166,1,0,0,0,619,620,7,22,0,0,620,168,1,0,0,0,621,622,7,23,0,0,622,
		170,1,0,0,0,623,624,7,24,0,0,624,172,1,0,0,0,43,0,286,295,312,318,320,
		371,374,381,384,389,396,399,402,407,412,414,416,421,428,430,441,443,449,
		453,456,459,464,488,515,537,541,546,550,557,563,568,575,584,588,592,597,
		601,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
