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
		T__17=18, T__18=19, T__19=20, COMMENT=21, COMENT_BLOCK=22, OPEN=23, CLOSE=24, 
		OPENSQ=25, CLOSESQ=26, ENDL=27, IDENTIFIER=28, WS=29, PLUS=30, MINUS=31, 
		STAR=32, DIV=33, PERCENT=34, AMP=35, BITWISE_OR=36, CARET=37, BANG=38, 
		TILDE=39, ASSIGNMENT=40, LT=41, GT=42, INTERR=43, OP_AND=44, OP_OR=45, 
		OP_EQ=46, OP_NE=47, OP_LE=48, OP_GE=49, INTEGER_LITERAL=50, HEX_INTEGER_LITERAL=51, 
		REAL_LITERAL=52, CHARACTER_LITERAL=53, REGULAR_STRING=54, VERBATIM_STRING=55;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "COMMENT", "COMENT_BLOCK", "OPEN", "CLOSE", 
		"OPENSQ", "CLOSESQ", "ENDL", "IDENTIFIER", "WS", "PLUS", "MINUS", "STAR", 
		"DIV", "PERCENT", "AMP", "BITWISE_OR", "CARET", "BANG", "TILDE", "ASSIGNMENT", 
		"LT", "GT", "INTERR", "OP_AND", "OP_OR", "OP_EQ", "OP_NE", "OP_LE", "OP_GE", 
		"INTEGER_LITERAL", "HEX_INTEGER_LITERAL", "REAL_LITERAL", "CHARACTER_LITERAL", 
		"REGULAR_STRING", "VERBATIM_STRING", "IntegerTypeSuffix", "HexDigit", 
		"CommonCharacter", "SimpleEscapeSequence", "HexEscapeSequence", "UnicodeEscapeSequence", 
		"ExponentPart", "Whitespace", "UnicodeClassZS", "NewLine", "IdentifierOrKeyword", 
		"IdentifierStartCharacter", "IdentifierPartCharacter", "LetterCharacter", 
		"DecimalDigitCharacter", "ConnectingCharacter", "CombiningCharacter", 
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
		null, "'lambda'", "'$call'", "'$setlength'", "'->'", "','", "'$output'", 
		"'$length'", "'$a'", "'$i'", "'$p'", "'$parent'", "'$u'", "'$unique'", 
		"'('", "')'", "'$$'", "'`'", "'end:'", "'length:'", "'.'", null, null, 
		"'{'", "'}'", "'['", "']'", "';'", null, null, "'+'", "'-'", "'*'", "'/'", 
		"'%'", "'&'", "'|'", "'^'", "'!'", "'~'", "'='", "'<'", "'>'", "'?'", 
		"'&&'", "'||'", "'=='", "'!='", "'<='", "'>='"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, "COMMENT", "COMENT_BLOCK", 
		"OPEN", "CLOSE", "OPENSQ", "CLOSESQ", "ENDL", "IDENTIFIER", "WS", "PLUS", 
		"MINUS", "STAR", "DIV", "PERCENT", "AMP", "BITWISE_OR", "CARET", "BANG", 
		"TILDE", "ASSIGNMENT", "LT", "GT", "INTERR", "OP_AND", "OP_OR", "OP_EQ", 
		"OP_NE", "OP_LE", "OP_GE", "INTEGER_LITERAL", "HEX_INTEGER_LITERAL", "REAL_LITERAL", 
		"CHARACTER_LITERAL", "REGULAR_STRING", "VERBATIM_STRING"
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
		4,0,55,611,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
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
		7,77,2,78,7,78,2,79,7,79,2,80,7,80,2,81,7,81,2,82,7,82,2,83,7,83,1,0,1,
		0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,
		1,2,1,2,1,2,1,2,1,2,1,3,1,3,1,3,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,5,1,5,1,
		5,1,6,1,6,1,6,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,8,1,8,1,8,1,9,1,9,1,9,
		1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,12,1,12,1,12,
		1,12,1,12,1,12,1,12,1,12,1,13,1,13,1,14,1,14,1,15,1,15,1,15,1,16,1,16,
		1,17,1,17,1,17,1,17,1,17,1,18,1,18,1,18,1,18,1,18,1,18,1,18,1,18,1,19,
		1,19,1,20,1,20,1,20,1,20,5,20,271,8,20,10,20,12,20,274,9,20,1,21,1,21,
		1,21,1,21,5,21,280,8,21,10,21,12,21,283,9,21,1,21,1,21,1,21,1,22,1,22,
		1,23,1,23,1,24,1,24,1,25,1,25,1,26,1,26,1,27,3,27,299,8,27,1,27,1,27,1,
		28,1,28,4,28,305,8,28,11,28,12,28,306,1,29,1,29,1,30,1,30,1,31,1,31,1,
		32,1,32,1,33,1,33,1,34,1,34,1,35,1,35,1,36,1,36,1,37,1,37,1,38,1,38,1,
		39,1,39,1,40,1,40,1,41,1,41,1,42,1,42,1,43,1,43,1,43,1,44,1,44,1,44,1,
		45,1,45,1,45,1,46,1,46,1,46,1,47,1,47,1,47,1,48,1,48,1,48,1,49,4,49,356,
		8,49,11,49,12,49,357,1,49,3,49,361,8,49,1,50,1,50,1,50,4,50,366,8,50,11,
		50,12,50,367,1,50,3,50,371,8,50,1,51,5,51,374,8,51,10,51,12,51,377,9,51,
		1,51,1,51,4,51,381,8,51,11,51,12,51,382,1,51,3,51,386,8,51,1,51,3,51,389,
		8,51,1,51,4,51,392,8,51,11,51,12,51,393,1,51,1,51,1,51,3,51,399,8,51,3,
		51,401,8,51,3,51,403,8,51,1,52,1,52,1,52,3,52,408,8,52,1,52,1,52,1,53,
		1,53,1,53,5,53,415,8,53,10,53,12,53,418,9,53,1,53,1,53,1,54,1,54,1,54,
		1,54,1,54,1,54,5,54,428,8,54,10,54,12,54,431,9,54,1,54,1,54,1,55,3,55,
		436,8,55,1,55,1,55,3,55,440,8,55,1,55,3,55,443,8,55,1,56,3,56,446,8,56,
		1,57,1,57,1,57,3,57,451,8,57,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,
		58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,3,
		58,475,8,58,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,
		1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,1,59,3,59,
		502,8,59,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,
		60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,3,60,524,8,60,1,61,1,61,3,61,528,
		8,61,1,61,4,61,531,8,61,11,61,12,61,532,1,62,1,62,3,62,537,8,62,1,63,1,
		63,1,64,1,64,1,64,3,64,544,8,64,1,65,1,65,5,65,548,8,65,10,65,12,65,551,
		9,65,1,66,1,66,3,66,555,8,66,1,67,1,67,1,67,1,67,1,67,3,67,562,8,67,1,
		68,1,68,1,68,1,68,1,68,1,68,1,68,3,68,571,8,68,1,69,1,69,3,69,575,8,69,
		1,70,1,70,3,70,579,8,70,1,71,1,71,1,71,3,71,584,8,71,1,72,1,72,3,72,588,
		8,72,1,73,1,73,1,74,1,74,1,75,1,75,1,76,1,76,1,77,1,77,1,78,1,78,1,79,
		1,79,1,80,1,80,1,81,1,81,1,82,1,82,1,83,1,83,1,281,0,84,1,1,3,2,5,3,7,
		4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,
		17,35,18,37,19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,
		29,59,30,61,31,63,32,65,33,67,34,69,35,71,36,73,37,75,38,77,39,79,40,81,
		41,83,42,85,43,87,44,89,45,91,46,93,47,95,48,97,49,99,50,101,51,103,52,
		105,53,107,54,109,55,111,0,113,0,115,0,117,0,119,0,121,0,123,0,125,0,127,
		0,129,0,131,0,133,0,135,0,137,0,139,0,141,0,143,0,145,0,147,0,149,0,151,
		0,153,0,155,0,157,0,159,0,161,0,163,0,165,0,167,0,1,0,25,2,0,10,10,13,
		13,1,0,48,57,2,0,88,88,120,120,6,0,68,68,70,70,77,77,100,100,102,102,109,
		109,6,0,10,10,13,13,39,39,92,92,133,133,8232,8233,6,0,10,10,13,13,34,34,
		92,92,133,133,8232,8233,1,0,34,34,2,0,76,76,108,108,2,0,85,85,117,117,
		3,0,48,57,65,70,97,102,2,0,69,69,101,101,2,0,43,43,45,45,2,0,9,9,11,12,
		9,0,32,32,160,160,5760,5760,6158,6158,8192,8198,8200,8202,8239,8239,8287,
		8287,12288,12288,4,0,10,10,13,13,133,133,8232,8233,82,0,65,90,192,214,
		216,222,256,310,313,327,330,381,385,386,388,395,398,401,403,404,406,408,
		412,413,415,416,418,425,428,435,437,444,452,461,463,475,478,494,497,500,
		502,504,506,562,570,571,573,574,577,582,584,590,880,882,886,895,902,906,
		908,929,931,939,975,980,984,1006,1012,1015,1017,1018,1021,1071,1120,1152,
		1162,1229,1232,1326,1329,1366,4256,4293,4295,4301,7680,7828,7838,7934,
		7944,7951,7960,7965,7976,7983,7992,7999,8008,8013,8025,8031,8040,8047,
		8120,8123,8136,8139,8152,8155,8168,8172,8184,8187,8450,8455,8459,8461,
		8464,8466,8469,8477,8484,8493,8496,8499,8510,8511,8517,8579,11264,11310,
		11360,11364,11367,11376,11378,11381,11390,11392,11394,11490,11499,11501,
		11506,42560,42562,42604,42624,42650,42786,42798,42802,42862,42873,42886,
		42891,42893,42896,42898,42902,42925,42928,42929,65313,65338,81,0,97,122,
		181,246,248,255,257,375,378,384,387,389,392,402,405,411,414,417,419,421,
		424,429,432,436,438,447,454,460,462,499,501,505,507,569,572,578,583,659,
		661,687,881,883,887,893,912,974,976,977,981,983,985,1011,1013,1119,1121,
		1153,1163,1215,1218,1327,1377,1415,7424,7467,7531,7543,7545,7578,7681,
		7837,7839,7943,7952,7957,7968,7975,7984,7991,8000,8005,8016,8023,8032,
		8039,8048,8061,8064,8071,8080,8087,8096,8103,8112,8116,8118,8119,8126,
		8132,8134,8135,8144,8147,8150,8151,8160,8167,8178,8180,8182,8183,8458,
		8467,8495,8505,8508,8509,8518,8521,8526,8580,11312,11358,11361,11372,11377,
		11387,11393,11500,11502,11507,11520,11557,11559,11565,42561,42605,42625,
		42651,42787,42801,42803,42872,42874,42876,42879,42887,42892,42894,42897,
		42901,42903,42921,43002,43866,43876,43877,64256,64262,64275,64279,65345,
		65370,6,0,453,459,498,8079,8088,8095,8104,8111,8124,8140,8188,8188,33,
		0,688,705,710,721,736,740,748,750,884,890,1369,1600,1765,1766,2036,2037,
		2042,2074,2084,2088,2417,3654,3782,4348,6103,6211,6823,7293,7468,7530,
		7544,7615,8305,8319,8336,8348,11388,11389,11631,11823,12293,12341,12347,
		12542,40981,42237,42508,42623,42652,42653,42775,42783,42864,42888,43000,
		43001,43471,43494,43632,43741,43763,43764,43868,43871,65392,65439,234,
		0,170,186,443,451,660,1514,1520,1522,1568,1599,1601,1610,1646,1647,1649,
		1747,1749,1788,1791,1808,1810,1839,1869,1957,1969,2026,2048,2069,2112,
		2136,2208,2226,2308,2361,2365,2384,2392,2401,2418,2432,2437,2444,2447,
		2448,2451,2472,2474,2480,2482,2489,2493,2510,2524,2525,2527,2529,2544,
		2545,2565,2570,2575,2576,2579,2600,2602,2608,2610,2611,2613,2614,2616,
		2617,2649,2652,2654,2676,2693,2701,2703,2705,2707,2728,2730,2736,2738,
		2739,2741,2745,2749,2768,2784,2785,2821,2828,2831,2832,2835,2856,2858,
		2864,2866,2867,2869,2873,2877,2913,2929,2947,2949,2954,2958,2960,2962,
		2965,2969,2970,2972,2986,2990,3001,3024,3084,3086,3088,3090,3112,3114,
		3129,3133,3212,3214,3216,3218,3240,3242,3251,3253,3257,3261,3294,3296,
		3297,3313,3314,3333,3340,3342,3344,3346,3386,3389,3406,3424,3425,3450,
		3455,3461,3478,3482,3505,3507,3515,3517,3526,3585,3632,3634,3635,3648,
		3653,3713,3714,3716,3722,3725,3735,3737,3743,3745,3747,3749,3751,3754,
		3755,3757,3760,3762,3763,3773,3780,3804,3807,3840,3911,3913,3948,3976,
		3980,4096,4138,4159,4181,4186,4189,4193,4208,4213,4225,4238,4346,4349,
		4680,4682,4685,4688,4694,4696,4701,4704,4744,4746,4749,4752,4784,4786,
		4789,4792,4798,4800,4805,4808,4822,4824,4880,4882,4885,4888,4954,4992,
		5007,5024,5108,5121,5740,5743,5759,5761,5786,5792,5866,5873,5880,5888,
		5900,5902,5905,5920,5937,5952,5969,5984,5996,5998,6000,6016,6067,6108,
		6210,6212,6263,6272,6312,6314,6389,6400,6430,6480,6509,6512,6516,6528,
		6571,6593,6599,6656,6678,6688,6740,6917,6963,6981,6987,7043,7072,7086,
		7087,7098,7141,7168,7203,7245,7247,7258,7287,7401,7404,7406,7409,7413,
		7414,8501,8504,11568,11623,11648,11670,11680,11686,11688,11694,11696,11702,
		11704,11710,11712,11718,11720,11726,11728,11734,11736,11742,12294,12348,
		12353,12438,12447,12538,12543,12589,12593,12686,12704,12730,12784,12799,
		13312,19893,19968,40908,40960,40980,40982,42124,42192,42231,42240,42507,
		42512,42527,42538,42539,42606,42725,42999,43009,43011,43013,43015,43018,
		43020,43042,43072,43123,43138,43187,43250,43255,43259,43301,43312,43334,
		43360,43388,43396,43442,43488,43492,43495,43503,43514,43518,43520,43560,
		43584,43586,43588,43595,43616,43631,43633,43638,43642,43695,43697,43709,
		43712,43714,43739,43740,43744,43754,43762,43782,43785,43790,43793,43798,
		43808,43814,43816,43822,43968,44002,44032,55203,55216,55238,55243,55291,
		63744,64109,64112,64217,64285,64296,64298,64310,64312,64316,64318,64433,
		64467,64829,64848,64911,64914,64967,65008,65019,65136,65140,65142,65276,
		65382,65391,65393,65437,65440,65470,65474,65479,65482,65487,65490,65495,
		65498,65500,2,0,5870,5872,8544,8559,3,0,2307,2307,2366,2368,2377,2380,
		3,0,173,173,1536,1539,1757,1757,6,0,95,95,8255,8256,8276,8276,65075,65076,
		65101,65103,65343,65343,37,0,48,57,1632,1641,1776,1785,1984,1993,2406,
		2415,2534,2543,2662,2671,2790,2799,2918,2927,3046,3055,3174,3183,3302,
		3311,3430,3439,3558,3567,3664,3673,3792,3801,3872,3881,4160,4169,4240,
		4249,6112,6121,6160,6169,6470,6479,6608,6617,6784,6793,6800,6809,6992,
		7001,7088,7097,7232,7241,7248,7257,42528,42537,43216,43225,43264,43273,
		43472,43481,43504,43513,43600,43609,44016,44025,65296,65305,643,0,1,1,
		0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,
		1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,
		0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,
		1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,
		0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,
		1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,
		0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,
		1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,
		0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,
		1,0,0,0,0,103,1,0,0,0,0,105,1,0,0,0,0,107,1,0,0,0,0,109,1,0,0,0,1,169,
		1,0,0,0,3,176,1,0,0,0,5,182,1,0,0,0,7,193,1,0,0,0,9,196,1,0,0,0,11,198,
		1,0,0,0,13,206,1,0,0,0,15,214,1,0,0,0,17,217,1,0,0,0,19,220,1,0,0,0,21,
		223,1,0,0,0,23,231,1,0,0,0,25,234,1,0,0,0,27,242,1,0,0,0,29,244,1,0,0,
		0,31,246,1,0,0,0,33,249,1,0,0,0,35,251,1,0,0,0,37,256,1,0,0,0,39,264,1,
		0,0,0,41,266,1,0,0,0,43,275,1,0,0,0,45,287,1,0,0,0,47,289,1,0,0,0,49,291,
		1,0,0,0,51,293,1,0,0,0,53,295,1,0,0,0,55,298,1,0,0,0,57,304,1,0,0,0,59,
		308,1,0,0,0,61,310,1,0,0,0,63,312,1,0,0,0,65,314,1,0,0,0,67,316,1,0,0,
		0,69,318,1,0,0,0,71,320,1,0,0,0,73,322,1,0,0,0,75,324,1,0,0,0,77,326,1,
		0,0,0,79,328,1,0,0,0,81,330,1,0,0,0,83,332,1,0,0,0,85,334,1,0,0,0,87,336,
		1,0,0,0,89,339,1,0,0,0,91,342,1,0,0,0,93,345,1,0,0,0,95,348,1,0,0,0,97,
		351,1,0,0,0,99,355,1,0,0,0,101,362,1,0,0,0,103,402,1,0,0,0,105,404,1,0,
		0,0,107,411,1,0,0,0,109,421,1,0,0,0,111,442,1,0,0,0,113,445,1,0,0,0,115,
		450,1,0,0,0,117,474,1,0,0,0,119,501,1,0,0,0,121,523,1,0,0,0,123,525,1,
		0,0,0,125,536,1,0,0,0,127,538,1,0,0,0,129,543,1,0,0,0,131,545,1,0,0,0,
		133,554,1,0,0,0,135,561,1,0,0,0,137,570,1,0,0,0,139,574,1,0,0,0,141,578,
		1,0,0,0,143,583,1,0,0,0,145,587,1,0,0,0,147,589,1,0,0,0,149,591,1,0,0,
		0,151,593,1,0,0,0,153,595,1,0,0,0,155,597,1,0,0,0,157,599,1,0,0,0,159,
		601,1,0,0,0,161,603,1,0,0,0,163,605,1,0,0,0,165,607,1,0,0,0,167,609,1,
		0,0,0,169,170,5,108,0,0,170,171,5,97,0,0,171,172,5,109,0,0,172,173,5,98,
		0,0,173,174,5,100,0,0,174,175,5,97,0,0,175,2,1,0,0,0,176,177,5,36,0,0,
		177,178,5,99,0,0,178,179,5,97,0,0,179,180,5,108,0,0,180,181,5,108,0,0,
		181,4,1,0,0,0,182,183,5,36,0,0,183,184,5,115,0,0,184,185,5,101,0,0,185,
		186,5,116,0,0,186,187,5,108,0,0,187,188,5,101,0,0,188,189,5,110,0,0,189,
		190,5,103,0,0,190,191,5,116,0,0,191,192,5,104,0,0,192,6,1,0,0,0,193,194,
		5,45,0,0,194,195,5,62,0,0,195,8,1,0,0,0,196,197,5,44,0,0,197,10,1,0,0,
		0,198,199,5,36,0,0,199,200,5,111,0,0,200,201,5,117,0,0,201,202,5,116,0,
		0,202,203,5,112,0,0,203,204,5,117,0,0,204,205,5,116,0,0,205,12,1,0,0,0,
		206,207,5,36,0,0,207,208,5,108,0,0,208,209,5,101,0,0,209,210,5,110,0,0,
		210,211,5,103,0,0,211,212,5,116,0,0,212,213,5,104,0,0,213,14,1,0,0,0,214,
		215,5,36,0,0,215,216,5,97,0,0,216,16,1,0,0,0,217,218,5,36,0,0,218,219,
		5,105,0,0,219,18,1,0,0,0,220,221,5,36,0,0,221,222,5,112,0,0,222,20,1,0,
		0,0,223,224,5,36,0,0,224,225,5,112,0,0,225,226,5,97,0,0,226,227,5,114,
		0,0,227,228,5,101,0,0,228,229,5,110,0,0,229,230,5,116,0,0,230,22,1,0,0,
		0,231,232,5,36,0,0,232,233,5,117,0,0,233,24,1,0,0,0,234,235,5,36,0,0,235,
		236,5,117,0,0,236,237,5,110,0,0,237,238,5,105,0,0,238,239,5,113,0,0,239,
		240,5,117,0,0,240,241,5,101,0,0,241,26,1,0,0,0,242,243,5,40,0,0,243,28,
		1,0,0,0,244,245,5,41,0,0,245,30,1,0,0,0,246,247,5,36,0,0,247,248,5,36,
		0,0,248,32,1,0,0,0,249,250,5,96,0,0,250,34,1,0,0,0,251,252,5,101,0,0,252,
		253,5,110,0,0,253,254,5,100,0,0,254,255,5,58,0,0,255,36,1,0,0,0,256,257,
		5,108,0,0,257,258,5,101,0,0,258,259,5,110,0,0,259,260,5,103,0,0,260,261,
		5,116,0,0,261,262,5,104,0,0,262,263,5,58,0,0,263,38,1,0,0,0,264,265,5,
		46,0,0,265,40,1,0,0,0,266,267,5,47,0,0,267,268,5,47,0,0,268,272,1,0,0,
		0,269,271,8,0,0,0,270,269,1,0,0,0,271,274,1,0,0,0,272,270,1,0,0,0,272,
		273,1,0,0,0,273,42,1,0,0,0,274,272,1,0,0,0,275,276,5,47,0,0,276,277,5,
		42,0,0,277,281,1,0,0,0,278,280,9,0,0,0,279,278,1,0,0,0,280,283,1,0,0,0,
		281,282,1,0,0,0,281,279,1,0,0,0,282,284,1,0,0,0,283,281,1,0,0,0,284,285,
		5,42,0,0,285,286,5,47,0,0,286,44,1,0,0,0,287,288,5,123,0,0,288,46,1,0,
		0,0,289,290,5,125,0,0,290,48,1,0,0,0,291,292,5,91,0,0,292,50,1,0,0,0,293,
		294,5,93,0,0,294,52,1,0,0,0,295,296,5,59,0,0,296,54,1,0,0,0,297,299,5,
		64,0,0,298,297,1,0,0,0,298,299,1,0,0,0,299,300,1,0,0,0,300,301,3,131,65,
		0,301,56,1,0,0,0,302,305,3,125,62,0,303,305,3,129,64,0,304,302,1,0,0,0,
		304,303,1,0,0,0,305,306,1,0,0,0,306,304,1,0,0,0,306,307,1,0,0,0,307,58,
		1,0,0,0,308,309,5,43,0,0,309,60,1,0,0,0,310,311,5,45,0,0,311,62,1,0,0,
		0,312,313,5,42,0,0,313,64,1,0,0,0,314,315,5,47,0,0,315,66,1,0,0,0,316,
		317,5,37,0,0,317,68,1,0,0,0,318,319,5,38,0,0,319,70,1,0,0,0,320,321,5,
		124,0,0,321,72,1,0,0,0,322,323,5,94,0,0,323,74,1,0,0,0,324,325,5,33,0,
		0,325,76,1,0,0,0,326,327,5,126,0,0,327,78,1,0,0,0,328,329,5,61,0,0,329,
		80,1,0,0,0,330,331,5,60,0,0,331,82,1,0,0,0,332,333,5,62,0,0,333,84,1,0,
		0,0,334,335,5,63,0,0,335,86,1,0,0,0,336,337,5,38,0,0,337,338,5,38,0,0,
		338,88,1,0,0,0,339,340,5,124,0,0,340,341,5,124,0,0,341,90,1,0,0,0,342,
		343,5,61,0,0,343,344,5,61,0,0,344,92,1,0,0,0,345,346,5,33,0,0,346,347,
		5,61,0,0,347,94,1,0,0,0,348,349,5,60,0,0,349,350,5,61,0,0,350,96,1,0,0,
		0,351,352,5,62,0,0,352,353,5,61,0,0,353,98,1,0,0,0,354,356,7,1,0,0,355,
		354,1,0,0,0,356,357,1,0,0,0,357,355,1,0,0,0,357,358,1,0,0,0,358,360,1,
		0,0,0,359,361,3,111,55,0,360,359,1,0,0,0,360,361,1,0,0,0,361,100,1,0,0,
		0,362,363,5,48,0,0,363,365,7,2,0,0,364,366,3,113,56,0,365,364,1,0,0,0,
		366,367,1,0,0,0,367,365,1,0,0,0,367,368,1,0,0,0,368,370,1,0,0,0,369,371,
		3,111,55,0,370,369,1,0,0,0,370,371,1,0,0,0,371,102,1,0,0,0,372,374,7,1,
		0,0,373,372,1,0,0,0,374,377,1,0,0,0,375,373,1,0,0,0,375,376,1,0,0,0,376,
		378,1,0,0,0,377,375,1,0,0,0,378,380,5,46,0,0,379,381,7,1,0,0,380,379,1,
		0,0,0,381,382,1,0,0,0,382,380,1,0,0,0,382,383,1,0,0,0,383,385,1,0,0,0,
		384,386,3,123,61,0,385,384,1,0,0,0,385,386,1,0,0,0,386,388,1,0,0,0,387,
		389,7,3,0,0,388,387,1,0,0,0,388,389,1,0,0,0,389,403,1,0,0,0,390,392,7,
		1,0,0,391,390,1,0,0,0,392,393,1,0,0,0,393,391,1,0,0,0,393,394,1,0,0,0,
		394,400,1,0,0,0,395,401,7,3,0,0,396,398,3,123,61,0,397,399,7,3,0,0,398,
		397,1,0,0,0,398,399,1,0,0,0,399,401,1,0,0,0,400,395,1,0,0,0,400,396,1,
		0,0,0,401,403,1,0,0,0,402,375,1,0,0,0,402,391,1,0,0,0,403,104,1,0,0,0,
		404,407,5,39,0,0,405,408,8,4,0,0,406,408,3,115,57,0,407,405,1,0,0,0,407,
		406,1,0,0,0,408,409,1,0,0,0,409,410,5,39,0,0,410,106,1,0,0,0,411,416,5,
		34,0,0,412,415,8,5,0,0,413,415,3,115,57,0,414,412,1,0,0,0,414,413,1,0,
		0,0,415,418,1,0,0,0,416,414,1,0,0,0,416,417,1,0,0,0,417,419,1,0,0,0,418,
		416,1,0,0,0,419,420,5,34,0,0,420,108,1,0,0,0,421,422,5,64,0,0,422,423,
		5,34,0,0,423,429,1,0,0,0,424,428,8,6,0,0,425,426,5,34,0,0,426,428,5,34,
		0,0,427,424,1,0,0,0,427,425,1,0,0,0,428,431,1,0,0,0,429,427,1,0,0,0,429,
		430,1,0,0,0,430,432,1,0,0,0,431,429,1,0,0,0,432,433,5,34,0,0,433,110,1,
		0,0,0,434,436,7,7,0,0,435,434,1,0,0,0,435,436,1,0,0,0,436,437,1,0,0,0,
		437,443,7,8,0,0,438,440,7,8,0,0,439,438,1,0,0,0,439,440,1,0,0,0,440,441,
		1,0,0,0,441,443,7,7,0,0,442,435,1,0,0,0,442,439,1,0,0,0,443,112,1,0,0,
		0,444,446,7,9,0,0,445,444,1,0,0,0,446,114,1,0,0,0,447,451,3,117,58,0,448,
		451,3,119,59,0,449,451,3,121,60,0,450,447,1,0,0,0,450,448,1,0,0,0,450,
		449,1,0,0,0,451,116,1,0,0,0,452,453,5,92,0,0,453,475,5,39,0,0,454,455,
		5,92,0,0,455,475,5,34,0,0,456,457,5,92,0,0,457,475,5,92,0,0,458,459,5,
		92,0,0,459,475,5,48,0,0,460,461,5,92,0,0,461,475,5,97,0,0,462,463,5,92,
		0,0,463,475,5,98,0,0,464,465,5,92,0,0,465,475,5,102,0,0,466,467,5,92,0,
		0,467,475,5,110,0,0,468,469,5,92,0,0,469,475,5,114,0,0,470,471,5,92,0,
		0,471,475,5,116,0,0,472,473,5,92,0,0,473,475,5,118,0,0,474,452,1,0,0,0,
		474,454,1,0,0,0,474,456,1,0,0,0,474,458,1,0,0,0,474,460,1,0,0,0,474,462,
		1,0,0,0,474,464,1,0,0,0,474,466,1,0,0,0,474,468,1,0,0,0,474,470,1,0,0,
		0,474,472,1,0,0,0,475,118,1,0,0,0,476,477,5,92,0,0,477,478,5,120,0,0,478,
		479,1,0,0,0,479,502,3,113,56,0,480,481,5,92,0,0,481,482,5,120,0,0,482,
		483,1,0,0,0,483,484,3,113,56,0,484,485,3,113,56,0,485,502,1,0,0,0,486,
		487,5,92,0,0,487,488,5,120,0,0,488,489,1,0,0,0,489,490,3,113,56,0,490,
		491,3,113,56,0,491,492,3,113,56,0,492,502,1,0,0,0,493,494,5,92,0,0,494,
		495,5,120,0,0,495,496,1,0,0,0,496,497,3,113,56,0,497,498,3,113,56,0,498,
		499,3,113,56,0,499,500,3,113,56,0,500,502,1,0,0,0,501,476,1,0,0,0,501,
		480,1,0,0,0,501,486,1,0,0,0,501,493,1,0,0,0,502,120,1,0,0,0,503,504,5,
		92,0,0,504,505,5,117,0,0,505,506,1,0,0,0,506,507,3,113,56,0,507,508,3,
		113,56,0,508,509,3,113,56,0,509,510,3,113,56,0,510,524,1,0,0,0,511,512,
		5,92,0,0,512,513,5,85,0,0,513,514,1,0,0,0,514,515,3,113,56,0,515,516,3,
		113,56,0,516,517,3,113,56,0,517,518,3,113,56,0,518,519,3,113,56,0,519,
		520,3,113,56,0,520,521,3,113,56,0,521,522,3,113,56,0,522,524,1,0,0,0,523,
		503,1,0,0,0,523,511,1,0,0,0,524,122,1,0,0,0,525,527,7,10,0,0,526,528,7,
		11,0,0,527,526,1,0,0,0,527,528,1,0,0,0,528,530,1,0,0,0,529,531,7,1,0,0,
		530,529,1,0,0,0,531,532,1,0,0,0,532,530,1,0,0,0,532,533,1,0,0,0,533,124,
		1,0,0,0,534,537,3,127,63,0,535,537,7,12,0,0,536,534,1,0,0,0,536,535,1,
		0,0,0,537,126,1,0,0,0,538,539,7,13,0,0,539,128,1,0,0,0,540,541,5,13,0,
		0,541,544,5,10,0,0,542,544,7,14,0,0,543,540,1,0,0,0,543,542,1,0,0,0,544,
		130,1,0,0,0,545,549,3,133,66,0,546,548,3,135,67,0,547,546,1,0,0,0,548,
		551,1,0,0,0,549,547,1,0,0,0,549,550,1,0,0,0,550,132,1,0,0,0,551,549,1,
		0,0,0,552,555,3,137,68,0,553,555,5,95,0,0,554,552,1,0,0,0,554,553,1,0,
		0,0,555,134,1,0,0,0,556,562,3,137,68,0,557,562,3,139,69,0,558,562,3,141,
		70,0,559,562,3,143,71,0,560,562,3,145,72,0,561,556,1,0,0,0,561,557,1,0,
		0,0,561,558,1,0,0,0,561,559,1,0,0,0,561,560,1,0,0,0,562,136,1,0,0,0,563,
		571,3,147,73,0,564,571,3,149,74,0,565,571,3,151,75,0,566,571,3,153,76,
		0,567,571,3,155,77,0,568,571,3,157,78,0,569,571,3,121,60,0,570,563,1,0,
		0,0,570,564,1,0,0,0,570,565,1,0,0,0,570,566,1,0,0,0,570,567,1,0,0,0,570,
		568,1,0,0,0,570,569,1,0,0,0,571,138,1,0,0,0,572,575,3,167,83,0,573,575,
		3,121,60,0,574,572,1,0,0,0,574,573,1,0,0,0,575,140,1,0,0,0,576,579,3,165,
		82,0,577,579,3,121,60,0,578,576,1,0,0,0,578,577,1,0,0,0,579,142,1,0,0,
		0,580,584,3,159,79,0,581,584,3,161,80,0,582,584,3,121,60,0,583,580,1,0,
		0,0,583,581,1,0,0,0,583,582,1,0,0,0,584,144,1,0,0,0,585,588,3,163,81,0,
		586,588,3,121,60,0,587,585,1,0,0,0,587,586,1,0,0,0,588,146,1,0,0,0,589,
		590,7,15,0,0,590,148,1,0,0,0,591,592,7,16,0,0,592,150,1,0,0,0,593,594,
		7,17,0,0,594,152,1,0,0,0,595,596,7,18,0,0,596,154,1,0,0,0,597,598,7,19,
		0,0,598,156,1,0,0,0,599,600,7,20,0,0,600,158,1,0,0,0,601,602,2,768,784,
		0,602,160,1,0,0,0,603,604,7,21,0,0,604,162,1,0,0,0,605,606,7,22,0,0,606,
		164,1,0,0,0,607,608,7,23,0,0,608,166,1,0,0,0,609,610,7,24,0,0,610,168,
		1,0,0,0,43,0,272,281,298,304,306,357,360,367,370,375,382,385,388,393,398,
		400,402,407,414,416,427,429,435,439,442,445,450,474,501,523,527,532,536,
		543,549,554,561,570,574,578,583,587,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
