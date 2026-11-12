using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Antlr.Runtime.Debug;

namespace Antlr.Runtime;

public abstract class BaseRecognizer
{
	public const int MemoRuleFailed = -2;

	public const int MemoRuleUnknown = -1;

	public const int InitialFollowStackSize = 100;

	public const int DefaultTokenChannel = 0;

	public const int Hidden = 99;

	public const string NextTokenRuleName = "nextToken";

	protected internal RecognizerSharedState state;

	public TextWriter TraceDestination { get; set; }

	public virtual int NumberOfSyntaxErrors => state.syntaxErrors;

	public virtual int BacktrackingLevel
	{
		get
		{
			return state.backtracking;
		}
		set
		{
			state.backtracking = value;
		}
	}

	public virtual bool Failed => state.failed;

	public virtual string[] TokenNames => null;

	public virtual string GrammarFileName => null;

	public abstract string SourceName { get; }

	public virtual IDebugEventListener DebugListener => null;

	public BaseRecognizer()
		: this(new RecognizerSharedState())
	{
	}

	public BaseRecognizer(RecognizerSharedState state)
	{
		if (state == null)
		{
			state = new RecognizerSharedState();
		}
		this.state = state;
		InitDFAs();
	}

	public virtual void SetState(RecognizerSharedState value)
	{
		state = value;
	}

	protected virtual void InitDFAs()
	{
	}

	public virtual void Reset()
	{
		if (state != null)
		{
			state._fsp = -1;
			state.errorRecovery = false;
			state.lastErrorIndex = -1;
			state.failed = false;
			state.syntaxErrors = 0;
			state.backtracking = 0;
			int num = 0;
			while (state.ruleMemo != null && num < state.ruleMemo.Length)
			{
				state.ruleMemo[num] = null;
				num++;
			}
		}
	}

	public virtual object Match(IIntStream input, int ttype, BitSet follow)
	{
		object currentInputSymbol = GetCurrentInputSymbol(input);
		if (input.LA(1) == ttype)
		{
			input.Consume();
			state.errorRecovery = false;
			state.failed = false;
			return currentInputSymbol;
		}
		if (state.backtracking > 0)
		{
			state.failed = true;
			return currentInputSymbol;
		}
		return RecoverFromMismatchedToken(input, ttype, follow);
	}

	public virtual void MatchAny(IIntStream input)
	{
		state.errorRecovery = false;
		state.failed = false;
		input.Consume();
	}

	public virtual bool MismatchIsUnwantedToken(IIntStream input, int ttype)
	{
		return input.LA(2) == ttype;
	}

	public virtual bool MismatchIsMissingToken(IIntStream input, BitSet follow)
	{
		if (follow == null)
		{
			return false;
		}
		if (follow.Member(1))
		{
			BitSet a = ComputeContextSensitiveRuleFOLLOW();
			follow = follow.Or(a);
			if (state._fsp >= 0)
			{
				follow.Remove(1);
			}
		}
		if (follow.Member(input.LA(1)) || follow.Member(1))
		{
			return true;
		}
		return false;
	}

	public virtual void ReportError(RecognitionException e)
	{
		if (!state.errorRecovery)
		{
			state.syntaxErrors++;
			state.errorRecovery = true;
			DisplayRecognitionError(TokenNames, e);
		}
	}

	public virtual void DisplayRecognitionError(string[] tokenNames, RecognitionException e)
	{
		string errorHeader = GetErrorHeader(e);
		string errorMessage = GetErrorMessage(e, tokenNames);
		EmitErrorMessage(errorHeader + " " + errorMessage);
	}

	public virtual string GetErrorMessage(RecognitionException e, string[] tokenNames)
	{
		string result = e.Message;
		if (e is UnwantedTokenException)
		{
			UnwantedTokenException ex = (UnwantedTokenException)e;
			string text = "<unknown>";
			result = string.Concat(str3: (ex.Expecting != -1) ? tokenNames[ex.Expecting] : "EndOfFile", str0: "extraneous input ", str1: GetTokenErrorDisplay(ex.UnexpectedToken), str2: " expecting ");
		}
		else if (e is MissingTokenException)
		{
			MissingTokenException ex2 = (MissingTokenException)e;
			string text2 = "<unknown>";
			text2 = ((ex2.Expecting != -1) ? tokenNames[ex2.Expecting] : "EndOfFile");
			result = "missing " + text2 + " at " + GetTokenErrorDisplay(e.Token);
		}
		else if (e is MismatchedTokenException)
		{
			MismatchedTokenException ex3 = (MismatchedTokenException)e;
			string text3 = "<unknown>";
			result = string.Concat(str3: (ex3.Expecting != -1) ? tokenNames[ex3.Expecting] : "EndOfFile", str0: "mismatched input ", str1: GetTokenErrorDisplay(e.Token), str2: " expecting ");
		}
		else if (e is MismatchedTreeNodeException)
		{
			MismatchedTreeNodeException ex4 = (MismatchedTreeNodeException)e;
			string text4 = "<unknown>";
			text4 = ((ex4.Expecting != -1) ? tokenNames[ex4.Expecting] : "EndOfFile");
			string text5 = ((ex4.Node != null) ? (ex4.Node.ToString() ?? string.Empty) : string.Empty);
			result = "mismatched tree node: " + text5 + " expecting " + text4;
		}
		else if (e is NoViableAltException)
		{
			result = "no viable alternative at input " + GetTokenErrorDisplay(e.Token);
		}
		else if (e is EarlyExitException)
		{
			result = "required (...)+ loop did not match anything at input " + GetTokenErrorDisplay(e.Token);
		}
		else if (e is MismatchedSetException)
		{
			MismatchedSetException ex5 = (MismatchedSetException)e;
			result = "mismatched input " + GetTokenErrorDisplay(e.Token) + " expecting set " + ex5.Expecting;
		}
		else if (e is MismatchedNotSetException)
		{
			MismatchedNotSetException ex6 = (MismatchedNotSetException)e;
			result = "mismatched input " + GetTokenErrorDisplay(e.Token) + " expecting set " + ex6.Expecting;
		}
		else if (e is FailedPredicateException)
		{
			FailedPredicateException ex7 = (FailedPredicateException)e;
			result = "rule " + ex7.RuleName + " failed predicate: {" + ex7.PredicateText + "}?";
		}
		return result;
	}

	public virtual string GetErrorHeader(RecognitionException e)
	{
		string text = SourceName ?? string.Empty;
		if (text.Length > 0)
		{
			text += ' ';
		}
		return $"{text}line {e.Line}:{e.CharPositionInLine + 1}";
	}

	public virtual string GetTokenErrorDisplay(IToken t)
	{
		string text = t.Text;
		if (text == null)
		{
			text = ((t.Type != -1) ? ("<" + t.Type + ">") : "<EOF>");
		}
		text = Regex.Replace(text, "\n", "\\\\n");
		text = Regex.Replace(text, "\r", "\\\\r");
		text = Regex.Replace(text, "\t", "\\\\t");
		return "'" + text + "'";
	}

	public virtual void EmitErrorMessage(string msg)
	{
		if (TraceDestination != null)
		{
			TraceDestination.WriteLine(msg);
		}
	}

	public virtual void Recover(IIntStream input, RecognitionException re)
	{
		if (state.lastErrorIndex == input.Index)
		{
			input.Consume();
		}
		state.lastErrorIndex = input.Index;
		BitSet set = ComputeErrorRecoverySet();
		BeginResync();
		ConsumeUntil(input, set);
		EndResync();
	}

	public virtual void BeginResync()
	{
	}

	public virtual void EndResync()
	{
	}

	protected virtual BitSet ComputeErrorRecoverySet()
	{
		return CombineFollows(exact: false);
	}

	protected virtual BitSet ComputeContextSensitiveRuleFOLLOW()
	{
		return CombineFollows(exact: true);
	}

	protected virtual BitSet CombineFollows(bool exact)
	{
		int fsp = state._fsp;
		BitSet bitSet = new BitSet();
		for (int num = fsp; num >= 0; num--)
		{
			BitSet bitSet2 = state.following[num];
			bitSet.OrInPlace(bitSet2);
			if (exact)
			{
				if (!bitSet2.Member(1))
				{
					break;
				}
				if (num > 0)
				{
					bitSet.Remove(1);
				}
			}
		}
		return bitSet;
	}

	protected virtual object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow)
	{
		RecognitionException e = null;
		if (MismatchIsUnwantedToken(input, ttype))
		{
			e = new UnwantedTokenException(ttype, input, TokenNames);
			BeginResync();
			input.Consume();
			EndResync();
			ReportError(e);
			object currentInputSymbol = GetCurrentInputSymbol(input);
			input.Consume();
			return currentInputSymbol;
		}
		if (MismatchIsMissingToken(input, follow))
		{
			object missingSymbol = GetMissingSymbol(input, e, ttype, follow);
			e = new MissingTokenException(ttype, input, missingSymbol);
			ReportError(e);
			return missingSymbol;
		}
		e = new MismatchedTokenException(ttype, input, TokenNames);
		throw e;
	}

	public virtual object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow)
	{
		if (MismatchIsMissingToken(input, follow))
		{
			ReportError(e);
			return GetMissingSymbol(input, e, 0, follow);
		}
		throw e;
	}

	protected virtual object GetCurrentInputSymbol(IIntStream input)
	{
		return null;
	}

	protected virtual object GetMissingSymbol(IIntStream input, RecognitionException e, int expectedTokenType, BitSet follow)
	{
		return null;
	}

	public virtual void ConsumeUntil(IIntStream input, int tokenType)
	{
		int num = input.LA(1);
		while (num != -1 && num != tokenType)
		{
			input.Consume();
			num = input.LA(1);
		}
	}

	public virtual void ConsumeUntil(IIntStream input, BitSet set)
	{
		int num = input.LA(1);
		while (num != -1 && !set.Member(num))
		{
			input.Consume();
			num = input.LA(1);
		}
	}

	protected void PushFollow(BitSet fset)
	{
		if (state._fsp + 1 >= state.following.Length)
		{
			Array.Resize(ref state.following, state.following.Length * 2);
		}
		state.following[++state._fsp] = fset;
	}

	protected void PopFollow()
	{
		state._fsp--;
	}

	public virtual IList<string> GetRuleInvocationStack()
	{
		return GetRuleInvocationStack(new StackTrace(fNeedFileInfo: true));
	}

	public static IList<string> GetRuleInvocationStack(StackTrace trace)
	{
		if (trace == null)
		{
			throw new ArgumentNullException("trace");
		}
		List<string> list = new List<string>();
		StackFrame[] array = trace.GetFrames() ?? new StackFrame[0];
		for (int num = array.Length - 1; num >= 0; num--)
		{
			StackFrame stackFrame = array[num];
			MethodBase method = stackFrame.GetMethod();
			GrammarRuleAttribute[] array2 = (GrammarRuleAttribute[])method.GetCustomAttributes(typeof(GrammarRuleAttribute), inherit: true);
			if (array2 != null && array2.Length > 0)
			{
				list.Add(array2[0].Name);
			}
		}
		return list;
	}

	public virtual List<string> ToStrings(ICollection<IToken> tokens)
	{
		if (tokens == null)
		{
			return null;
		}
		List<string> list = new List<string>(tokens.Count);
		foreach (IToken token in tokens)
		{
			list.Add(token.Text);
		}
		return list;
	}

	public virtual int GetRuleMemoization(int ruleIndex, int ruleStartIndex)
	{
		if (state.ruleMemo[ruleIndex] == null)
		{
			state.ruleMemo[ruleIndex] = new Dictionary<int, int>();
		}
		if (!state.ruleMemo[ruleIndex].TryGetValue(ruleStartIndex, out var value))
		{
			return -1;
		}
		return value;
	}

	public virtual bool AlreadyParsedRule(IIntStream input, int ruleIndex)
	{
		int ruleMemoization = GetRuleMemoization(ruleIndex, input.Index);
		switch (ruleMemoization)
		{
		case -1:
			return false;
		case -2:
			state.failed = true;
			break;
		default:
			input.Seek(ruleMemoization + 1);
			break;
		}
		return true;
	}

	public virtual void Memoize(IIntStream input, int ruleIndex, int ruleStartIndex)
	{
		int value = (state.failed ? (-2) : (input.Index - 1));
		if (state.ruleMemo == null && TraceDestination != null)
		{
			TraceDestination.WriteLine("!!!!!!!!! memo array is null for " + GrammarFileName);
		}
		if (ruleIndex >= state.ruleMemo.Length && TraceDestination != null)
		{
			TraceDestination.WriteLine("!!!!!!!!! memo size is " + state.ruleMemo.Length + ", but rule index is " + ruleIndex);
		}
		if (state.ruleMemo[ruleIndex] != null)
		{
			state.ruleMemo[ruleIndex][ruleStartIndex] = value;
		}
	}

	public virtual int GetRuleMemoizationCacheSize()
	{
		int num = 0;
		int num2 = 0;
		while (state.ruleMemo != null && num2 < state.ruleMemo.Length)
		{
			IDictionary<int, int> dictionary = state.ruleMemo[num2];
			if (dictionary != null)
			{
				num += dictionary.Count;
			}
			num2++;
		}
		return num;
	}

	public virtual void TraceIn(string ruleName, int ruleIndex, object inputSymbol)
	{
		if (TraceDestination != null)
		{
			TraceDestination.Write("enter " + ruleName + " " + inputSymbol);
			if (state.backtracking > 0)
			{
				TraceDestination.Write(" backtracking=" + state.backtracking);
			}
			TraceDestination.WriteLine();
		}
	}

	public virtual void TraceOut(string ruleName, int ruleIndex, object inputSymbol)
	{
		if (TraceDestination == null)
		{
			return;
		}
		TraceDestination.Write("exit " + ruleName + " " + inputSymbol);
		if (state.backtracking > 0)
		{
			TraceDestination.Write(" backtracking=" + state.backtracking);
			if (state.failed)
			{
				TraceDestination.Write(" failed");
			}
			else
			{
				TraceDestination.Write(" succeeded");
			}
		}
		TraceDestination.WriteLine();
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugEnterRule(string grammarFileName, string ruleName)
	{
		DebugListener?.EnterRule(grammarFileName, ruleName);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugExitRule(string grammarFileName, string ruleName)
	{
		DebugListener?.ExitRule(grammarFileName, ruleName);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugEnterSubRule(int decisionNumber)
	{
		DebugListener?.EnterSubRule(decisionNumber);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugExitSubRule(int decisionNumber)
	{
		DebugListener?.ExitSubRule(decisionNumber);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugEnterAlt(int alt)
	{
		DebugListener?.EnterAlt(alt);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugEnterDecision(int decisionNumber, bool couldBacktrack)
	{
		DebugListener?.EnterDecision(decisionNumber, couldBacktrack);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugExitDecision(int decisionNumber)
	{
		DebugListener?.ExitDecision(decisionNumber);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugLocation(int line, int charPositionInLine)
	{
		DebugListener?.Location(line, charPositionInLine);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugSemanticPredicate(bool result, string predicate)
	{
		DebugListener?.SemanticPredicate(result, predicate);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugBeginBacktrack(int level)
	{
		DebugListener?.BeginBacktrack(level);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugEndBacktrack(int level, bool successful)
	{
		DebugListener?.EndBacktrack(level, successful);
	}

	[Conditional("ANTLR_DEBUG")]
	protected virtual void DebugRecognitionException(RecognitionException ex)
	{
		DebugListener?.RecognitionException(ex);
	}
}
