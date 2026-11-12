using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Stimulsoft.Data.Expressions.Antlr.Runtime;
using Stimulsoft.Data.Expressions.Antlr.Runtime.Tree;
using Stimulsoft.Data.Expressions.NCalc.Domain;

[GeneratedCode("ANTLR", "3.5.0.2")]
[CLSCompliant(false)]
public class NCalcParser : Parser
{
	public sealed class ncalcExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public ncalcExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class logicalExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public logicalExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class conditionalExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public conditionalExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class booleanAndExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public booleanAndExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class bitwiseOrExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public bitwiseOrExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class bitwiseXOrExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public bitwiseXOrExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class bitwiseAndExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public bitwiseAndExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class equalityExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public equalityExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class relationalExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public relationalExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class shiftExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public shiftExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class additiveExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public additiveExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class multiplicativeExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public multiplicativeExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class unaryExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public unaryExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class primaryExpression_return : AstParserRuleReturnScope<object, IToken>
	{
		public LogicalExpression value;

		public primaryExpression_return(NCalcParser grammar)
		{
		}
	}

	private sealed class value_return : AstParserRuleReturnScope<object, IToken>
	{
		public ValueExpression value;

		public value_return(NCalcParser grammar)
		{
		}
	}

	private sealed class identifier_return : AstParserRuleReturnScope<object, IToken>
	{
		public Identifier value;

		public identifier_return(NCalcParser grammar)
		{
		}
	}

	private sealed class expressionList_return : AstParserRuleReturnScope<object, IToken>
	{
		public List<LogicalExpression> value;

		public expressionList_return(NCalcParser grammar)
		{
		}
	}

	private sealed class arguments_return : AstParserRuleReturnScope<object, IToken>
	{
		public List<LogicalExpression> value;

		public arguments_return(NCalcParser grammar)
		{
		}
	}

	private static class Follow
	{
		public static readonly BitSet _logicalExpression_in_ncalcExpression52 = new BitSet(new ulong[1]);

		public static readonly BitSet _EOF_in_ncalcExpression54 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _conditionalExpression_in_logicalExpression74 = new BitSet(new ulong[1] { 2199023255554uL });

		public static readonly BitSet _41_in_logicalExpression80 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _conditionalExpression_in_logicalExpression84 = new BitSet(new ulong[1] { 2147483648uL });

		public static readonly BitSet _31_in_logicalExpression86 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _conditionalExpression_in_logicalExpression90 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _booleanAndExpression_in_conditionalExpression117 = new BitSet(new ulong[1] { 175921860444162uL });

		public static readonly BitSet _set_in_conditionalExpression126 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _conditionalExpression_in_conditionalExpression142 = new BitSet(new ulong[1] { 175921860444162uL });

		public static readonly BitSet _bitwiseOrExpression_in_booleanAndExpression176 = new BitSet(new ulong[1] { 8796097216514uL });

		public static readonly BitSet _set_in_booleanAndExpression185 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _bitwiseOrExpression_in_booleanAndExpression201 = new BitSet(new ulong[1] { 8796097216514uL });

		public static readonly BitSet _bitwiseXOrExpression_in_bitwiseOrExpression233 = new BitSet(new ulong[1] { 70368744177666uL });

		public static readonly BitSet _46_in_bitwiseOrExpression242 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _bitwiseOrExpression_in_bitwiseOrExpression252 = new BitSet(new ulong[1] { 70368744177666uL });

		public static readonly BitSet _bitwiseAndExpression_in_bitwiseXOrExpression286 = new BitSet(new ulong[1] { 4398046511106uL });

		public static readonly BitSet _42_in_bitwiseXOrExpression295 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _bitwiseAndExpression_in_bitwiseXOrExpression305 = new BitSet(new ulong[1] { 4398046511106uL });

		public static readonly BitSet _equalityExpression_in_bitwiseAndExpression337 = new BitSet(new ulong[1] { 8388610uL });

		public static readonly BitSet _23_in_bitwiseAndExpression346 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _equalityExpression_in_bitwiseAndExpression356 = new BitSet(new ulong[1] { 8388610uL });

		public static readonly BitSet _relationalExpression_in_equalityExpression390 = new BitSet(new ulong[1] { 240519217154uL });

		public static readonly BitSet _set_in_equalityExpression401 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _set_in_equalityExpression418 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _relationalExpression_in_equalityExpression437 = new BitSet(new ulong[1] { 240519217154uL });

		public static readonly BitSet _shiftExpression_in_relationalExpression470 = new BitSet(new ulong[1] { 846108557314uL });

		public static readonly BitSet _32_in_relationalExpression481 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _34_in_relationalExpression491 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _38_in_relationalExpression502 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _39_in_relationalExpression512 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _shiftExpression_in_relationalExpression524 = new BitSet(new ulong[1] { 846108557314uL });

		public static readonly BitSet _additiveExpression_in_shiftExpression556 = new BitSet(new ulong[1] { 1108101562370uL });

		public static readonly BitSet _33_in_shiftExpression567 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _40_in_shiftExpression577 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _additiveExpression_in_shiftExpression589 = new BitSet(new ulong[1] { 1108101562370uL });

		public static readonly BitSet _multiplicativeExpression_in_additiveExpression621 = new BitSet(new ulong[1] { 671088642uL });

		public static readonly BitSet _27_in_additiveExpression632 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _29_in_additiveExpression642 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _multiplicativeExpression_in_additiveExpression654 = new BitSet(new ulong[1] { 671088642uL });

		public static readonly BitSet _unaryExpression_in_multiplicativeExpression686 = new BitSet(new ulong[1] { 1142947842uL });

		public static readonly BitSet _26_in_multiplicativeExpression697 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _30_in_multiplicativeExpression707 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _21_in_multiplicativeExpression717 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _unaryExpression_in_multiplicativeExpression729 = new BitSet(new ulong[1] { 1142947842uL });

		public static readonly BitSet _primaryExpression_in_unaryExpression756 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _set_in_unaryExpression767 = new BitSet(new ulong[1] { 16898832uL });

		public static readonly BitSet _primaryExpression_in_unaryExpression775 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _48_in_unaryExpression787 = new BitSet(new ulong[1] { 16898832uL });

		public static readonly BitSet _primaryExpression_in_unaryExpression790 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _29_in_unaryExpression801 = new BitSet(new ulong[1] { 16898832uL });

		public static readonly BitSet _primaryExpression_in_unaryExpression803 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _24_in_primaryExpression825 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _logicalExpression_in_primaryExpression827 = new BitSet(new ulong[1] { 33554432uL });

		public static readonly BitSet _25_in_primaryExpression829 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _value_in_primaryExpression839 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _identifier_in_primaryExpression847 = new BitSet(new ulong[1] { 16777218uL });

		public static readonly BitSet _arguments_in_primaryExpression852 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _INTEGER_in_value872 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _FLOAT_in_value880 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _STRING_in_value888 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _DATETIME_in_value897 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _TRUE_in_value904 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _FALSE_in_value912 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _ID_in_identifier930 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _NAME_in_identifier938 = new BitSet(new ulong[1] { 2uL });

		public static readonly BitSet _logicalExpression_in_expressionList962 = new BitSet(new ulong[1] { 268435458uL });

		public static readonly BitSet _28_in_expressionList969 = new BitSet(new ulong[1] { 299067717049104uL });

		public static readonly BitSet _logicalExpression_in_expressionList973 = new BitSet(new ulong[1] { 268435458uL });

		public static readonly BitSet _24_in_arguments1002 = new BitSet(new ulong[1] { 299067750603536uL });

		public static readonly BitSet _expressionList_in_arguments1006 = new BitSet(new ulong[1] { 33554432uL });

		public static readonly BitSet _25_in_arguments1013 = new BitSet(new ulong[1] { 2uL });
	}

	internal static readonly string[] tokenNames = new string[49]
	{
		"<invalid>", "<EOR>", "<DOWN>", "<UP>", "DATETIME", "DIGIT", "E", "EscapeSequence", "FALSE", "FLOAT",
		"HexDigit", "ID", "INTEGER", "LETTER", "NAME", "STRING", "TRUE", "UnicodeEscape", "WS", "'!'",
		"'!='", "'%'", "'&&'", "'&'", "'('", "')'", "'*'", "'+'", "','", "'-'",
		"'/'", "':'", "'<'", "'<<'", "'<='", "'<>'", "'='", "'=='", "'>'", "'>='",
		"'>>'", "'?'", "'^'", "'and'", "'not'", "'or'", "'|'", "'||'", "'~'"
	};

	public const int EOF = -1;

	public const int DATETIME = 4;

	public const int DIGIT = 5;

	public const int E = 6;

	public const int EscapeSequence = 7;

	public const int FALSE = 8;

	public const int FLOAT = 9;

	public const int HexDigit = 10;

	public const int ID = 11;

	public const int INTEGER = 12;

	public const int LETTER = 13;

	public const int NAME = 14;

	public const int STRING = 15;

	public const int TRUE = 16;

	public const int UnicodeEscape = 17;

	public const int WS = 18;

	public const int T__19 = 19;

	public const int T__20 = 20;

	public const int T__21 = 21;

	public const int T__22 = 22;

	public const int T__23 = 23;

	public const int T__24 = 24;

	public const int T__25 = 25;

	public const int T__26 = 26;

	public const int T__27 = 27;

	public const int T__28 = 28;

	public const int T__29 = 29;

	public const int T__30 = 30;

	public const int T__31 = 31;

	public const int T__32 = 32;

	public const int T__33 = 33;

	public const int T__34 = 34;

	public const int T__35 = 35;

	public const int T__36 = 36;

	public const int T__37 = 37;

	public const int T__38 = 38;

	public const int T__39 = 39;

	public const int T__40 = 40;

	public const int T__41 = 41;

	public const int T__42 = 42;

	public const int T__43 = 43;

	public const int T__44 = 44;

	public const int T__45 = 45;

	public const int T__46 = 46;

	public const int T__47 = 47;

	public const int T__48 = 48;

	private ITreeAdaptor adaptor;

	private const char BS = '\\';

	private static NumberFormatInfo numberFormatInfo = new NumberFormatInfo();

	public ITreeAdaptor TreeAdaptor
	{
		get
		{
			return adaptor;
		}
		set
		{
			adaptor = value;
		}
	}

	public override string[] TokenNames => tokenNames;

	public override string GrammarFileName => "C:\\Work\\Projects\\OSS\\NCalc\\Grammar\\NCalc.g";

	public List<string> Errors { get; private set; }

	public NCalcParser(ITokenStream input)
		: this(input, new RecognizerSharedState())
	{
	}

	public NCalcParser(ITokenStream input, RecognizerSharedState state)
		: base(input, state)
	{
		ITreeAdaptor treeAdaptor = null;
		TreeAdaptor = treeAdaptor ?? new CommonTreeAdaptor();
	}

	private string extractString(string text)
	{
		StringBuilder stringBuilder = new StringBuilder(text);
		int startIndex = 1;
		int num = -1;
		while ((num = stringBuilder.ToString().IndexOf('\\', startIndex)) != -1)
		{
			char c = stringBuilder[num + 1];
			switch (c)
			{
			case 'u':
			{
				string text2 = string.Concat(stringBuilder[num + 4], stringBuilder[num + 5]);
				string text3 = string.Concat(stringBuilder[num + 2], stringBuilder[num + 3]);
				char c2 = Encoding.Unicode.GetChars(new byte[2]
				{
					Convert.ToByte(text2, 16),
					Convert.ToByte(text3, 16)
				})[0];
				stringBuilder.Remove(num, 6).Insert(num, c2);
				break;
			}
			case 'n':
				stringBuilder.Remove(num, 2).Insert(num, '\n');
				break;
			case 'r':
				stringBuilder.Remove(num, 2).Insert(num, '\r');
				break;
			case 't':
				stringBuilder.Remove(num, 2).Insert(num, '\t');
				break;
			case '\'':
				stringBuilder.Remove(num, 2).Insert(num, '\'');
				break;
			case '\\':
				stringBuilder.Remove(num, 2).Insert(num, '\\');
				break;
			default:
				throw new RecognitionException("Unvalid escape sequence: \\" + c);
			}
			startIndex = num + 1;
		}
		stringBuilder.Remove(0, 1);
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		return stringBuilder.ToString();
	}

	public override void DisplayRecognitionError(string[] tokenNames, RecognitionException e)
	{
		base.DisplayRecognitionError(tokenNames, e);
		if (Errors == null)
		{
			Errors = new List<string>();
		}
		string errorHeader = GetErrorHeader(e);
		string errorMessage = GetErrorMessage(e, tokenNames);
		Errors.Add(errorMessage + " at " + errorHeader);
	}

	[GrammarRule("ncalcExpression")]
	public ncalcExpression_return ncalcExpression()
	{
		ncalcExpression_return ncalcExpression_return = new ncalcExpression_return(this);
		ncalcExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		object obj2 = null;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._logicalExpression_in_ncalcExpression52);
				astParserRuleReturnScope = logicalExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				token = (IToken)Match(input, -1, Follow._EOF_in_ncalcExpression54);
				ncalcExpression_return.value = ((astParserRuleReturnScope != null) ? ((logicalExpression_return)astParserRuleReturnScope).value : null);
				ncalcExpression_return.Stop = input.LT(-1);
				ncalcExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(ncalcExpression_return.Tree, ncalcExpression_return.Start, ncalcExpression_return.Stop);
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				ncalcExpression_return.Tree = adaptor.ErrorNode(input, ncalcExpression_return.Start, input.LT(-1), ex);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return ncalcExpression_return;
	}

	[GrammarRule("logicalExpression")]
	private logicalExpression_return logicalExpression()
	{
		logicalExpression_return logicalExpression_return = new logicalExpression_return(this);
		logicalExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope3 = null;
		object obj2 = null;
		object obj3 = null;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._conditionalExpression_in_logicalExpression74);
				astParserRuleReturnScope = conditionalExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				logicalExpression_return.value = ((astParserRuleReturnScope != null) ? ((conditionalExpression_return)astParserRuleReturnScope).value : null);
				int num = 2;
				try
				{
					try
					{
						int num2 = input.LA(1);
						if (num2 == 41)
						{
							num = 1;
						}
					}
					finally
					{
					}
					if (num == 1)
					{
						token = (IToken)Match(input, 41, Follow._41_in_logicalExpression80);
						obj2 = adaptor.Create(token);
						adaptor.AddChild(obj, obj2);
						PushFollow(Follow._conditionalExpression_in_logicalExpression84);
						astParserRuleReturnScope2 = conditionalExpression();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
						token2 = (IToken)Match(input, 31, Follow._31_in_logicalExpression86);
						obj3 = adaptor.Create(token2);
						adaptor.AddChild(obj, obj3);
						PushFollow(Follow._conditionalExpression_in_logicalExpression90);
						astParserRuleReturnScope3 = conditionalExpression();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope3.Tree);
						logicalExpression_return.value = new TernaryExpression((astParserRuleReturnScope != null) ? ((conditionalExpression_return)astParserRuleReturnScope).value : null, (astParserRuleReturnScope2 != null) ? ((conditionalExpression_return)astParserRuleReturnScope2).value : null, (astParserRuleReturnScope3 != null) ? ((conditionalExpression_return)astParserRuleReturnScope3).value : null);
					}
				}
				finally
				{
				}
				logicalExpression_return.Stop = input.LT(-1);
				logicalExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(logicalExpression_return.Tree, logicalExpression_return.Start, logicalExpression_return.Stop);
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				logicalExpression_return.Tree = adaptor.ErrorNode(input, logicalExpression_return.Start, input.LT(-1), ex);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return logicalExpression_return;
	}

	[GrammarRule("conditionalExpression")]
	private conditionalExpression_return conditionalExpression()
	{
		conditionalExpression_return conditionalExpression_return = new conditionalExpression_return(this);
		conditionalExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._booleanAndExpression_in_conditionalExpression117);
				astParserRuleReturnScope = booleanAndExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				conditionalExpression_return.value = ((astParserRuleReturnScope != null) ? ((booleanAndExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 45 || num2 == 47)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num == 1)
						{
							token = input.LT(1);
							if (input.LA(1) == 45 || input.LA(1) == 47)
							{
								input.Consume();
								adaptor.AddChild(obj, adaptor.Create(token));
								state.errorRecovery = false;
								binaryExpressionType = BinaryExpressionType.Or;
								PushFollow(Follow._conditionalExpression_in_conditionalExpression142);
								astParserRuleReturnScope2 = conditionalExpression();
								PopFollow();
								adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
								conditionalExpression_return.value = new BinaryExpression(binaryExpressionType, conditionalExpression_return.value, (astParserRuleReturnScope2 != null) ? ((conditionalExpression_return)astParserRuleReturnScope2).value : null);
								continue;
							}
							MismatchedSetException ex = new MismatchedSetException(null, input);
							throw ex;
						}
						break;
					}
				}
				finally
				{
				}
				conditionalExpression_return.Stop = input.LT(-1);
				conditionalExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(conditionalExpression_return.Tree, conditionalExpression_return.Start, conditionalExpression_return.Stop);
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				conditionalExpression_return.Tree = adaptor.ErrorNode(input, conditionalExpression_return.Start, input.LT(-1), ex2);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return conditionalExpression_return;
	}

	[GrammarRule("booleanAndExpression")]
	private booleanAndExpression_return booleanAndExpression()
	{
		booleanAndExpression_return booleanAndExpression_return = new booleanAndExpression_return(this);
		booleanAndExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._bitwiseOrExpression_in_booleanAndExpression176);
				astParserRuleReturnScope = bitwiseOrExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				booleanAndExpression_return.value = ((astParserRuleReturnScope != null) ? ((bitwiseOrExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 22 || num2 == 43)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num == 1)
						{
							token = input.LT(1);
							if (input.LA(1) == 22 || input.LA(1) == 43)
							{
								input.Consume();
								adaptor.AddChild(obj, adaptor.Create(token));
								state.errorRecovery = false;
								binaryExpressionType = BinaryExpressionType.And;
								PushFollow(Follow._bitwiseOrExpression_in_booleanAndExpression201);
								astParserRuleReturnScope2 = bitwiseOrExpression();
								PopFollow();
								adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
								booleanAndExpression_return.value = new BinaryExpression(binaryExpressionType, booleanAndExpression_return.value, (astParserRuleReturnScope2 != null) ? ((bitwiseOrExpression_return)astParserRuleReturnScope2).value : null);
								continue;
							}
							MismatchedSetException ex = new MismatchedSetException(null, input);
							throw ex;
						}
						break;
					}
				}
				finally
				{
				}
				booleanAndExpression_return.Stop = input.LT(-1);
				booleanAndExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(booleanAndExpression_return.Tree, booleanAndExpression_return.Start, booleanAndExpression_return.Stop);
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				booleanAndExpression_return.Tree = adaptor.ErrorNode(input, booleanAndExpression_return.Start, input.LT(-1), ex2);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return booleanAndExpression_return;
	}

	[GrammarRule("bitwiseOrExpression")]
	private bitwiseOrExpression_return bitwiseOrExpression()
	{
		bitwiseOrExpression_return bitwiseOrExpression_return = new bitwiseOrExpression_return(this);
		bitwiseOrExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._bitwiseXOrExpression_in_bitwiseOrExpression233);
				astParserRuleReturnScope = bitwiseXOrExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				bitwiseOrExpression_return.value = ((astParserRuleReturnScope != null) ? ((bitwiseXOrExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 46)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num == 1)
						{
							token = (IToken)Match(input, 46, Follow._46_in_bitwiseOrExpression242);
							obj2 = adaptor.Create(token);
							adaptor.AddChild(obj, obj2);
							binaryExpressionType = BinaryExpressionType.BitwiseOr;
							PushFollow(Follow._bitwiseOrExpression_in_bitwiseOrExpression252);
							astParserRuleReturnScope2 = bitwiseOrExpression();
							PopFollow();
							adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
							bitwiseOrExpression_return.value = new BinaryExpression(binaryExpressionType, bitwiseOrExpression_return.value, (astParserRuleReturnScope2 != null) ? ((bitwiseOrExpression_return)astParserRuleReturnScope2).value : null);
							continue;
						}
						break;
					}
				}
				finally
				{
				}
				bitwiseOrExpression_return.Stop = input.LT(-1);
				bitwiseOrExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(bitwiseOrExpression_return.Tree, bitwiseOrExpression_return.Start, bitwiseOrExpression_return.Stop);
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				bitwiseOrExpression_return.Tree = adaptor.ErrorNode(input, bitwiseOrExpression_return.Start, input.LT(-1), ex);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return bitwiseOrExpression_return;
	}

	[GrammarRule("bitwiseXOrExpression")]
	private bitwiseXOrExpression_return bitwiseXOrExpression()
	{
		bitwiseXOrExpression_return bitwiseXOrExpression_return = new bitwiseXOrExpression_return(this);
		bitwiseXOrExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._bitwiseAndExpression_in_bitwiseXOrExpression286);
				astParserRuleReturnScope = bitwiseAndExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				bitwiseXOrExpression_return.value = ((astParserRuleReturnScope != null) ? ((bitwiseAndExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 42)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num == 1)
						{
							token = (IToken)Match(input, 42, Follow._42_in_bitwiseXOrExpression295);
							obj2 = adaptor.Create(token);
							adaptor.AddChild(obj, obj2);
							binaryExpressionType = BinaryExpressionType.BitwiseXOr;
							PushFollow(Follow._bitwiseAndExpression_in_bitwiseXOrExpression305);
							astParserRuleReturnScope2 = bitwiseAndExpression();
							PopFollow();
							adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
							bitwiseXOrExpression_return.value = new BinaryExpression(binaryExpressionType, bitwiseXOrExpression_return.value, (astParserRuleReturnScope2 != null) ? ((bitwiseAndExpression_return)astParserRuleReturnScope2).value : null);
							continue;
						}
						break;
					}
				}
				finally
				{
				}
				bitwiseXOrExpression_return.Stop = input.LT(-1);
				bitwiseXOrExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(bitwiseXOrExpression_return.Tree, bitwiseXOrExpression_return.Start, bitwiseXOrExpression_return.Stop);
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				bitwiseXOrExpression_return.Tree = adaptor.ErrorNode(input, bitwiseXOrExpression_return.Start, input.LT(-1), ex);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return bitwiseXOrExpression_return;
	}

	[GrammarRule("bitwiseAndExpression")]
	private bitwiseAndExpression_return bitwiseAndExpression()
	{
		bitwiseAndExpression_return bitwiseAndExpression_return = new bitwiseAndExpression_return(this);
		bitwiseAndExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._equalityExpression_in_bitwiseAndExpression337);
				astParserRuleReturnScope = equalityExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				bitwiseAndExpression_return.value = ((astParserRuleReturnScope != null) ? ((equalityExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 23)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num == 1)
						{
							token = (IToken)Match(input, 23, Follow._23_in_bitwiseAndExpression346);
							obj2 = adaptor.Create(token);
							adaptor.AddChild(obj, obj2);
							binaryExpressionType = BinaryExpressionType.BitwiseAnd;
							PushFollow(Follow._equalityExpression_in_bitwiseAndExpression356);
							astParserRuleReturnScope2 = equalityExpression();
							PopFollow();
							adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
							bitwiseAndExpression_return.value = new BinaryExpression(binaryExpressionType, bitwiseAndExpression_return.value, (astParserRuleReturnScope2 != null) ? ((equalityExpression_return)astParserRuleReturnScope2).value : null);
							continue;
						}
						break;
					}
				}
				finally
				{
				}
				bitwiseAndExpression_return.Stop = input.LT(-1);
				bitwiseAndExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(bitwiseAndExpression_return.Tree, bitwiseAndExpression_return.Start, bitwiseAndExpression_return.Stop);
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				bitwiseAndExpression_return.Tree = adaptor.ErrorNode(input, bitwiseAndExpression_return.Start, input.LT(-1), ex);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return bitwiseAndExpression_return;
	}

	[GrammarRule("equalityExpression")]
	private equalityExpression_return equalityExpression()
	{
		equalityExpression_return equalityExpression_return = new equalityExpression_return(this);
		equalityExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		object obj3 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._relationalExpression_in_equalityExpression390);
				astParserRuleReturnScope = relationalExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				equalityExpression_return.value = ((astParserRuleReturnScope != null) ? ((relationalExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							switch (input.LA(1))
							{
							case 20:
							case 35:
							case 36:
							case 37:
								num = 1;
								break;
							}
						}
						finally
						{
						}
						if (num != 1)
						{
							break;
						}
						int num2 = 2;
						try
						{
							try
							{
								int num3 = input.LA(1);
								if (num3 >= 36 && num3 <= 37)
								{
									num2 = 1;
								}
								else
								{
									if (num3 != 20 && num3 != 35)
									{
										NoViableAltException ex = new NoViableAltException("", 7, 0, input, 1);
										throw ex;
									}
									num2 = 2;
								}
							}
							finally
							{
							}
							switch (num2)
							{
							case 1:
							{
								token = input.LT(1);
								if (input.LA(1) >= 36 && input.LA(1) <= 37)
								{
									input.Consume();
									adaptor.AddChild(obj, adaptor.Create(token));
									state.errorRecovery = false;
									type = BinaryExpressionType.Equal;
									break;
								}
								MismatchedSetException ex3 = new MismatchedSetException(null, input);
								throw ex3;
							}
							case 2:
							{
								token2 = input.LT(1);
								if (input.LA(1) == 20 || input.LA(1) == 35)
								{
									input.Consume();
									adaptor.AddChild(obj, adaptor.Create(token2));
									state.errorRecovery = false;
									type = BinaryExpressionType.NotEqual;
									break;
								}
								MismatchedSetException ex2 = new MismatchedSetException(null, input);
								throw ex2;
							}
							}
						}
						finally
						{
						}
						PushFollow(Follow._relationalExpression_in_equalityExpression437);
						astParserRuleReturnScope2 = relationalExpression();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
						equalityExpression_return.value = new BinaryExpression(type, equalityExpression_return.value, (astParserRuleReturnScope2 != null) ? ((relationalExpression_return)astParserRuleReturnScope2).value : null);
					}
				}
				finally
				{
				}
				equalityExpression_return.Stop = input.LT(-1);
				equalityExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(equalityExpression_return.Tree, equalityExpression_return.Start, equalityExpression_return.Stop);
			}
			catch (RecognitionException ex4)
			{
				ReportError(ex4);
				Recover(input, ex4);
				equalityExpression_return.Tree = adaptor.ErrorNode(input, equalityExpression_return.Start, input.LT(-1), ex4);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return equalityExpression_return;
	}

	[GrammarRule("relationalExpression")]
	private relationalExpression_return relationalExpression()
	{
		relationalExpression_return relationalExpression_return = new relationalExpression_return(this);
		relationalExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		IToken token3 = null;
		IToken token4 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		object obj3 = null;
		object obj4 = null;
		object obj5 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._shiftExpression_in_relationalExpression470);
				astParserRuleReturnScope = shiftExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				relationalExpression_return.value = ((astParserRuleReturnScope != null) ? ((shiftExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							switch (input.LA(1))
							{
							case 32:
							case 34:
							case 38:
							case 39:
								num = 1;
								break;
							}
						}
						finally
						{
						}
						if (num != 1)
						{
							break;
						}
						int num2 = 4;
						try
						{
							try
							{
								switch (input.LA(1))
								{
								case 32:
									num2 = 1;
									break;
								case 34:
									num2 = 2;
									break;
								case 38:
									num2 = 3;
									break;
								case 39:
									num2 = 4;
									break;
								default:
								{
									NoViableAltException ex = new NoViableAltException("", 9, 0, input, 1);
									throw ex;
								}
								}
							}
							finally
							{
							}
							switch (num2)
							{
							case 1:
								token = (IToken)Match(input, 32, Follow._32_in_relationalExpression481);
								obj2 = adaptor.Create(token);
								adaptor.AddChild(obj, obj2);
								type = BinaryExpressionType.Lesser;
								break;
							case 2:
								token2 = (IToken)Match(input, 34, Follow._34_in_relationalExpression491);
								obj3 = adaptor.Create(token2);
								adaptor.AddChild(obj, obj3);
								type = BinaryExpressionType.LesserOrEqual;
								break;
							case 3:
								token3 = (IToken)Match(input, 38, Follow._38_in_relationalExpression502);
								obj4 = adaptor.Create(token3);
								adaptor.AddChild(obj, obj4);
								type = BinaryExpressionType.Greater;
								break;
							case 4:
								token4 = (IToken)Match(input, 39, Follow._39_in_relationalExpression512);
								obj5 = adaptor.Create(token4);
								adaptor.AddChild(obj, obj5);
								type = BinaryExpressionType.GreaterOrEqual;
								break;
							}
						}
						finally
						{
						}
						PushFollow(Follow._shiftExpression_in_relationalExpression524);
						astParserRuleReturnScope2 = shiftExpression();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
						relationalExpression_return.value = new BinaryExpression(type, relationalExpression_return.value, (astParserRuleReturnScope2 != null) ? ((shiftExpression_return)astParserRuleReturnScope2).value : null);
					}
				}
				finally
				{
				}
				relationalExpression_return.Stop = input.LT(-1);
				relationalExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(relationalExpression_return.Tree, relationalExpression_return.Start, relationalExpression_return.Stop);
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				relationalExpression_return.Tree = adaptor.ErrorNode(input, relationalExpression_return.Start, input.LT(-1), ex2);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return relationalExpression_return;
	}

	[GrammarRule("shiftExpression")]
	private shiftExpression_return shiftExpression()
	{
		shiftExpression_return shiftExpression_return = new shiftExpression_return(this);
		shiftExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		object obj3 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._additiveExpression_in_shiftExpression556);
				astParserRuleReturnScope = additiveExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				shiftExpression_return.value = ((astParserRuleReturnScope != null) ? ((additiveExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 33 || num2 == 40)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num != 1)
						{
							break;
						}
						int num3 = 2;
						try
						{
							try
							{
								switch (input.LA(1))
								{
								case 33:
									num3 = 1;
									break;
								case 40:
									num3 = 2;
									break;
								default:
								{
									NoViableAltException ex = new NoViableAltException("", 11, 0, input, 1);
									throw ex;
								}
								}
							}
							finally
							{
							}
							switch (num3)
							{
							case 1:
								token = (IToken)Match(input, 33, Follow._33_in_shiftExpression567);
								obj2 = adaptor.Create(token);
								adaptor.AddChild(obj, obj2);
								type = BinaryExpressionType.LeftShift;
								break;
							case 2:
								token2 = (IToken)Match(input, 40, Follow._40_in_shiftExpression577);
								obj3 = adaptor.Create(token2);
								adaptor.AddChild(obj, obj3);
								type = BinaryExpressionType.RightShift;
								break;
							}
						}
						finally
						{
						}
						PushFollow(Follow._additiveExpression_in_shiftExpression589);
						astParserRuleReturnScope2 = additiveExpression();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
						shiftExpression_return.value = new BinaryExpression(type, shiftExpression_return.value, (astParserRuleReturnScope2 != null) ? ((additiveExpression_return)astParserRuleReturnScope2).value : null);
					}
				}
				finally
				{
				}
				shiftExpression_return.Stop = input.LT(-1);
				shiftExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(shiftExpression_return.Tree, shiftExpression_return.Start, shiftExpression_return.Stop);
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				shiftExpression_return.Tree = adaptor.ErrorNode(input, shiftExpression_return.Start, input.LT(-1), ex2);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return shiftExpression_return;
	}

	[GrammarRule("additiveExpression")]
	private additiveExpression_return additiveExpression()
	{
		additiveExpression_return additiveExpression_return = new additiveExpression_return(this);
		additiveExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		object obj3 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._multiplicativeExpression_in_additiveExpression621);
				astParserRuleReturnScope = multiplicativeExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				additiveExpression_return.value = ((astParserRuleReturnScope != null) ? ((multiplicativeExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 27 || num2 == 29)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num != 1)
						{
							break;
						}
						int num3 = 2;
						try
						{
							try
							{
								switch (input.LA(1))
								{
								case 27:
									num3 = 1;
									break;
								case 29:
									num3 = 2;
									break;
								default:
								{
									NoViableAltException ex = new NoViableAltException("", 13, 0, input, 1);
									throw ex;
								}
								}
							}
							finally
							{
							}
							switch (num3)
							{
							case 1:
								token = (IToken)Match(input, 27, Follow._27_in_additiveExpression632);
								obj2 = adaptor.Create(token);
								adaptor.AddChild(obj, obj2);
								type = BinaryExpressionType.Plus;
								break;
							case 2:
								token2 = (IToken)Match(input, 29, Follow._29_in_additiveExpression642);
								obj3 = adaptor.Create(token2);
								adaptor.AddChild(obj, obj3);
								type = BinaryExpressionType.Minus;
								break;
							}
						}
						finally
						{
						}
						PushFollow(Follow._multiplicativeExpression_in_additiveExpression654);
						astParserRuleReturnScope2 = multiplicativeExpression();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
						additiveExpression_return.value = new BinaryExpression(type, additiveExpression_return.value, (astParserRuleReturnScope2 != null) ? ((multiplicativeExpression_return)astParserRuleReturnScope2).value : null);
					}
				}
				finally
				{
				}
				additiveExpression_return.Stop = input.LT(-1);
				additiveExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(additiveExpression_return.Tree, additiveExpression_return.Start, additiveExpression_return.Stop);
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				additiveExpression_return.Tree = adaptor.ErrorNode(input, additiveExpression_return.Start, input.LT(-1), ex2);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return additiveExpression_return;
	}

	[GrammarRule("multiplicativeExpression")]
	private multiplicativeExpression_return multiplicativeExpression()
	{
		multiplicativeExpression_return multiplicativeExpression_return = new multiplicativeExpression_return(this);
		multiplicativeExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		IToken token3 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		object obj3 = null;
		object obj4 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._unaryExpression_in_multiplicativeExpression686);
				astParserRuleReturnScope = unaryExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				multiplicativeExpression_return.value = ((astParserRuleReturnScope != null) ? ((unaryExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 21 || num2 == 26 || num2 == 30)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num != 1)
						{
							break;
						}
						int num3 = 3;
						try
						{
							try
							{
								switch (input.LA(1))
								{
								case 26:
									num3 = 1;
									break;
								case 30:
									num3 = 2;
									break;
								case 21:
									num3 = 3;
									break;
								default:
								{
									NoViableAltException ex = new NoViableAltException("", 15, 0, input, 1);
									throw ex;
								}
								}
							}
							finally
							{
							}
							switch (num3)
							{
							case 1:
								token = (IToken)Match(input, 26, Follow._26_in_multiplicativeExpression697);
								obj2 = adaptor.Create(token);
								adaptor.AddChild(obj, obj2);
								type = BinaryExpressionType.Times;
								break;
							case 2:
								token2 = (IToken)Match(input, 30, Follow._30_in_multiplicativeExpression707);
								obj3 = adaptor.Create(token2);
								adaptor.AddChild(obj, obj3);
								type = BinaryExpressionType.Div;
								break;
							case 3:
								token3 = (IToken)Match(input, 21, Follow._21_in_multiplicativeExpression717);
								obj4 = adaptor.Create(token3);
								adaptor.AddChild(obj, obj4);
								type = BinaryExpressionType.Modulo;
								break;
							}
						}
						finally
						{
						}
						PushFollow(Follow._unaryExpression_in_multiplicativeExpression729);
						astParserRuleReturnScope2 = unaryExpression();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
						multiplicativeExpression_return.value = new BinaryExpression(type, multiplicativeExpression_return.value, (astParserRuleReturnScope2 != null) ? ((unaryExpression_return)astParserRuleReturnScope2).value : null);
					}
				}
				finally
				{
				}
				multiplicativeExpression_return.Stop = input.LT(-1);
				multiplicativeExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(multiplicativeExpression_return.Tree, multiplicativeExpression_return.Start, multiplicativeExpression_return.Stop);
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				multiplicativeExpression_return.Tree = adaptor.ErrorNode(input, multiplicativeExpression_return.Start, input.LT(-1), ex2);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return multiplicativeExpression_return;
	}

	[GrammarRule("unaryExpression")]
	private unaryExpression_return unaryExpression()
	{
		unaryExpression_return unaryExpression_return = new unaryExpression_return(this);
		unaryExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		IToken token3 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope3 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope4 = null;
		object obj2 = null;
		object obj3 = null;
		object obj4 = null;
		try
		{
			try
			{
				int num = 4;
				try
				{
					switch (input.LA(1))
					{
					case 4:
					case 8:
					case 9:
					case 11:
					case 12:
					case 14:
					case 15:
					case 16:
					case 24:
						num = 1;
						break;
					case 19:
					case 44:
						num = 2;
						break;
					case 48:
						num = 3;
						break;
					case 29:
						num = 4;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException("", 17, 0, input, 1);
						throw ex;
					}
					}
				}
				finally
				{
				}
				switch (num)
				{
				case 1:
					obj = adaptor.Nil();
					PushFollow(Follow._primaryExpression_in_unaryExpression756);
					astParserRuleReturnScope = primaryExpression();
					PopFollow();
					adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
					unaryExpression_return.value = ((astParserRuleReturnScope != null) ? ((primaryExpression_return)astParserRuleReturnScope).value : null);
					break;
				case 2:
				{
					obj = adaptor.Nil();
					token = input.LT(1);
					if (input.LA(1) == 19 || input.LA(1) == 44)
					{
						input.Consume();
						adaptor.AddChild(obj, adaptor.Create(token));
						state.errorRecovery = false;
						PushFollow(Follow._primaryExpression_in_unaryExpression775);
						astParserRuleReturnScope2 = primaryExpression();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
						unaryExpression_return.value = new UnaryExpression(UnaryExpressionType.Not, (astParserRuleReturnScope2 != null) ? ((primaryExpression_return)astParserRuleReturnScope2).value : null);
						break;
					}
					MismatchedSetException ex2 = new MismatchedSetException(null, input);
					throw ex2;
				}
				case 3:
					obj = adaptor.Nil();
					token2 = (IToken)Match(input, 48, Follow._48_in_unaryExpression787);
					obj3 = adaptor.Create(token2);
					adaptor.AddChild(obj, obj3);
					PushFollow(Follow._primaryExpression_in_unaryExpression790);
					astParserRuleReturnScope3 = primaryExpression();
					PopFollow();
					adaptor.AddChild(obj, astParserRuleReturnScope3.Tree);
					unaryExpression_return.value = new UnaryExpression(UnaryExpressionType.BitwiseNot, (astParserRuleReturnScope3 != null) ? ((primaryExpression_return)astParserRuleReturnScope3).value : null);
					break;
				case 4:
					obj = adaptor.Nil();
					token3 = (IToken)Match(input, 29, Follow._29_in_unaryExpression801);
					obj4 = adaptor.Create(token3);
					adaptor.AddChild(obj, obj4);
					PushFollow(Follow._primaryExpression_in_unaryExpression803);
					astParserRuleReturnScope4 = primaryExpression();
					PopFollow();
					adaptor.AddChild(obj, astParserRuleReturnScope4.Tree);
					unaryExpression_return.value = new UnaryExpression(UnaryExpressionType.Negate, (astParserRuleReturnScope4 != null) ? ((primaryExpression_return)astParserRuleReturnScope4).value : null);
					break;
				}
				unaryExpression_return.Stop = input.LT(-1);
				unaryExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(unaryExpression_return.Tree, unaryExpression_return.Start, unaryExpression_return.Stop);
			}
			catch (RecognitionException ex3)
			{
				ReportError(ex3);
				Recover(input, ex3);
				unaryExpression_return.Tree = adaptor.ErrorNode(input, unaryExpression_return.Start, input.LT(-1), ex3);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return unaryExpression_return;
	}

	[GrammarRule("primaryExpression")]
	private primaryExpression_return primaryExpression()
	{
		primaryExpression_return primaryExpression_return = new primaryExpression_return(this);
		primaryExpression_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope3 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope4 = null;
		object obj2 = null;
		object obj3 = null;
		try
		{
			try
			{
				int num = 3;
				try
				{
					switch (input.LA(1))
					{
					case 24:
						num = 1;
						break;
					case 4:
					case 8:
					case 9:
					case 12:
					case 15:
					case 16:
						num = 2;
						break;
					case 11:
					case 14:
						num = 3;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException("", 19, 0, input, 1);
						throw ex;
					}
					}
				}
				finally
				{
				}
				switch (num)
				{
				case 1:
					obj = adaptor.Nil();
					token = (IToken)Match(input, 24, Follow._24_in_primaryExpression825);
					obj2 = adaptor.Create(token);
					adaptor.AddChild(obj, obj2);
					PushFollow(Follow._logicalExpression_in_primaryExpression827);
					astParserRuleReturnScope2 = logicalExpression();
					PopFollow();
					adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
					token2 = (IToken)Match(input, 25, Follow._25_in_primaryExpression829);
					obj3 = adaptor.Create(token2);
					adaptor.AddChild(obj, obj3);
					primaryExpression_return.value = ((astParserRuleReturnScope2 != null) ? ((logicalExpression_return)astParserRuleReturnScope2).value : null);
					break;
				case 2:
					obj = adaptor.Nil();
					PushFollow(Follow._value_in_primaryExpression839);
					astParserRuleReturnScope = value();
					PopFollow();
					adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
					primaryExpression_return.value = ((astParserRuleReturnScope != null) ? ((value_return)astParserRuleReturnScope).value : null);
					break;
				case 3:
				{
					obj = adaptor.Nil();
					PushFollow(Follow._identifier_in_primaryExpression847);
					astParserRuleReturnScope3 = identifier();
					PopFollow();
					adaptor.AddChild(obj, astParserRuleReturnScope3.Tree);
					primaryExpression_return.value = ((astParserRuleReturnScope3 != null) ? ((identifier_return)astParserRuleReturnScope3).value : null);
					int num2 = 2;
					try
					{
						try
						{
							int num3 = input.LA(1);
							if (num3 == 24)
							{
								num2 = 1;
							}
						}
						finally
						{
						}
						if (num2 == 1)
						{
							PushFollow(Follow._arguments_in_primaryExpression852);
							astParserRuleReturnScope4 = arguments();
							PopFollow();
							adaptor.AddChild(obj, astParserRuleReturnScope4.Tree);
							primaryExpression_return.value = new Function((astParserRuleReturnScope3 != null) ? ((identifier_return)astParserRuleReturnScope3).value : null, ((astParserRuleReturnScope4 != null) ? ((arguments_return)astParserRuleReturnScope4).value : null).ToArray());
						}
					}
					finally
					{
					}
					break;
				}
				}
				primaryExpression_return.Stop = input.LT(-1);
				primaryExpression_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(primaryExpression_return.Tree, primaryExpression_return.Start, primaryExpression_return.Stop);
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				primaryExpression_return.Tree = adaptor.ErrorNode(input, primaryExpression_return.Start, input.LT(-1), ex2);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return primaryExpression_return;
	}

	[GrammarRule("value")]
	private value_return value()
	{
		value_return value_return = new value_return(this);
		value_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		IToken token3 = null;
		IToken token4 = null;
		IToken token5 = null;
		IToken token6 = null;
		object obj2 = null;
		object obj3 = null;
		object obj4 = null;
		object obj5 = null;
		object obj6 = null;
		object obj7 = null;
		try
		{
			try
			{
				int num = 6;
				try
				{
					switch (input.LA(1))
					{
					case 12:
						num = 1;
						break;
					case 9:
						num = 2;
						break;
					case 15:
						num = 3;
						break;
					case 4:
						num = 4;
						break;
					case 16:
						num = 5;
						break;
					case 8:
						num = 6;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException("", 20, 0, input, 1);
						throw ex;
					}
					}
				}
				finally
				{
				}
				switch (num)
				{
				case 1:
					obj = adaptor.Nil();
					token = (IToken)Match(input, 12, Follow._INTEGER_in_value872);
					obj2 = adaptor.Create(token);
					adaptor.AddChild(obj, obj2);
					try
					{
						value_return.value = new ValueExpression(int.Parse(token?.Text));
					}
					catch (OverflowException)
					{
						value_return.value = new ValueExpression(long.Parse(token?.Text));
					}
					break;
				case 2:
					obj = adaptor.Nil();
					token2 = (IToken)Match(input, 9, Follow._FLOAT_in_value880);
					obj3 = adaptor.Create(token2);
					adaptor.AddChild(obj, obj3);
					value_return.value = new ValueExpression(double.Parse(token2?.Text, NumberStyles.Float, numberFormatInfo));
					break;
				case 3:
					obj = adaptor.Nil();
					token3 = (IToken)Match(input, 15, Follow._STRING_in_value888);
					obj4 = adaptor.Create(token3);
					adaptor.AddChild(obj, obj4);
					value_return.value = new ValueExpression(extractString(token3?.Text));
					break;
				case 4:
					obj = adaptor.Nil();
					token4 = (IToken)Match(input, 4, Follow._DATETIME_in_value897);
					obj5 = adaptor.Create(token4);
					adaptor.AddChild(obj, obj5);
					value_return.value = new ValueExpression(DateTime.Parse((token4?.Text).Substring(1, (token4?.Text).Length - 2)));
					break;
				case 5:
					obj = adaptor.Nil();
					token5 = (IToken)Match(input, 16, Follow._TRUE_in_value904);
					obj6 = adaptor.Create(token5);
					adaptor.AddChild(obj, obj6);
					value_return.value = new ValueExpression(value: true);
					break;
				case 6:
					obj = adaptor.Nil();
					token6 = (IToken)Match(input, 8, Follow._FALSE_in_value912);
					obj7 = adaptor.Create(token6);
					adaptor.AddChild(obj, obj7);
					value_return.value = new ValueExpression(value: false);
					break;
				}
				value_return.Stop = input.LT(-1);
				value_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(value_return.Tree, value_return.Start, value_return.Stop);
			}
			catch (RecognitionException ex3)
			{
				ReportError(ex3);
				Recover(input, ex3);
				value_return.Tree = adaptor.ErrorNode(input, value_return.Start, input.LT(-1), ex3);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return value_return;
	}

	[GrammarRule("identifier")]
	private identifier_return identifier()
	{
		identifier_return identifier_return = new identifier_return(this);
		identifier_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		object obj2 = null;
		object obj3 = null;
		try
		{
			try
			{
				int num = 2;
				try
				{
					switch (input.LA(1))
					{
					case 11:
						num = 1;
						break;
					case 14:
						num = 2;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException("", 21, 0, input, 1);
						throw ex;
					}
					}
				}
				finally
				{
				}
				switch (num)
				{
				case 1:
					obj = adaptor.Nil();
					token = (IToken)Match(input, 11, Follow._ID_in_identifier930);
					obj2 = adaptor.Create(token);
					adaptor.AddChild(obj, obj2);
					identifier_return.value = new Identifier(token?.Text);
					break;
				case 2:
					obj = adaptor.Nil();
					token2 = (IToken)Match(input, 14, Follow._NAME_in_identifier938);
					obj3 = adaptor.Create(token2);
					adaptor.AddChild(obj, obj3);
					identifier_return.value = new Identifier((token2?.Text).Substring(1, (token2?.Text).Length - 2));
					break;
				}
				identifier_return.Stop = input.LT(-1);
				identifier_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(identifier_return.Tree, identifier_return.Start, identifier_return.Stop);
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				identifier_return.Tree = adaptor.ErrorNode(input, identifier_return.Start, input.LT(-1), ex2);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return identifier_return;
	}

	[GrammarRule("expressionList")]
	private expressionList_return expressionList()
	{
		expressionList_return expressionList_return = new expressionList_return(this);
		expressionList_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope2 = null;
		object obj2 = null;
		List<LogicalExpression> list = new List<LogicalExpression>();
		try
		{
			try
			{
				obj = adaptor.Nil();
				PushFollow(Follow._logicalExpression_in_expressionList962);
				astParserRuleReturnScope = logicalExpression();
				PopFollow();
				adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
				list.Add((astParserRuleReturnScope != null) ? ((logicalExpression_return)astParserRuleReturnScope).value : null);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 28)
							{
								num = 1;
							}
						}
						finally
						{
						}
						if (num == 1)
						{
							token = (IToken)Match(input, 28, Follow._28_in_expressionList969);
							obj2 = adaptor.Create(token);
							adaptor.AddChild(obj, obj2);
							PushFollow(Follow._logicalExpression_in_expressionList973);
							astParserRuleReturnScope2 = logicalExpression();
							PopFollow();
							adaptor.AddChild(obj, astParserRuleReturnScope2.Tree);
							list.Add((astParserRuleReturnScope2 != null) ? ((logicalExpression_return)astParserRuleReturnScope2).value : null);
							continue;
						}
						break;
					}
				}
				finally
				{
				}
				expressionList_return.value = list;
				expressionList_return.Stop = input.LT(-1);
				expressionList_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(expressionList_return.Tree, expressionList_return.Start, expressionList_return.Stop);
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				expressionList_return.Tree = adaptor.ErrorNode(input, expressionList_return.Start, input.LT(-1), ex);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return expressionList_return;
	}

	[GrammarRule("arguments")]
	private arguments_return arguments()
	{
		arguments_return arguments_return = new arguments_return(this);
		arguments_return.Start = input.LT(1);
		object obj = null;
		IToken token = null;
		IToken token2 = null;
		AstParserRuleReturnScope<object, IToken> astParserRuleReturnScope = null;
		object obj2 = null;
		object obj3 = null;
		arguments_return.value = new List<LogicalExpression>();
		try
		{
			try
			{
				obj = adaptor.Nil();
				token = (IToken)Match(input, 24, Follow._24_in_arguments1002);
				obj2 = adaptor.Create(token);
				adaptor.AddChild(obj, obj2);
				int num = 2;
				try
				{
					try
					{
						int num2 = input.LA(1);
						switch (num2)
						{
						default:
							if ((num2 >= 11 && num2 <= 12) || (num2 >= 14 && num2 <= 16) || num2 == 19 || num2 == 24 || num2 == 29 || num2 == 44 || num2 == 48)
							{
								break;
							}
							goto end_IL_0077;
						case 4:
						case 8:
						case 9:
							break;
						}
						num = 1;
						end_IL_0077:;
					}
					finally
					{
					}
					if (num == 1)
					{
						PushFollow(Follow._expressionList_in_arguments1006);
						astParserRuleReturnScope = expressionList();
						PopFollow();
						adaptor.AddChild(obj, astParserRuleReturnScope.Tree);
						arguments_return.value = ((astParserRuleReturnScope != null) ? ((expressionList_return)astParserRuleReturnScope).value : null);
					}
				}
				finally
				{
				}
				token2 = (IToken)Match(input, 25, Follow._25_in_arguments1013);
				obj3 = adaptor.Create(token2);
				adaptor.AddChild(obj, obj3);
				arguments_return.Stop = input.LT(-1);
				arguments_return.Tree = adaptor.RulePostProcessing(obj);
				adaptor.SetTokenBoundaries(arguments_return.Tree, arguments_return.Start, arguments_return.Stop);
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				arguments_return.Tree = adaptor.ErrorNode(input, arguments_return.Start, input.LT(-1), ex);
			}
			finally
			{
			}
		}
		finally
		{
		}
		return arguments_return;
	}
}
