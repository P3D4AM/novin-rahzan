using System.Diagnostics;

namespace Antlr.Runtime;

public abstract class Lexer : BaseRecognizer, ITokenSource
{
	protected ICharStream input;

	public string Text
	{
		get
		{
			if (state.text != null)
			{
				return state.text;
			}
			return input.Substring(state.tokenStartCharIndex, CharIndex - state.tokenStartCharIndex);
		}
		set
		{
			state.text = value;
		}
	}

	public int Line
	{
		get
		{
			return input.Line;
		}
		set
		{
			input.Line = value;
		}
	}

	public int CharPositionInLine
	{
		get
		{
			return input.CharPositionInLine;
		}
		set
		{
			input.CharPositionInLine = value;
		}
	}

	public virtual ICharStream CharStream
	{
		get
		{
			return input;
		}
		set
		{
			input = null;
			Reset();
			input = value;
		}
	}

	public override string SourceName => input.SourceName;

	public virtual int CharIndex => input.Index;

	public Lexer()
	{
	}

	public Lexer(ICharStream input)
	{
		this.input = input;
	}

	public Lexer(ICharStream input, RecognizerSharedState state)
		: base(state)
	{
		this.input = input;
	}

	public override void Reset()
	{
		base.Reset();
		if (input != null)
		{
			input.Seek(0);
		}
		if (state != null)
		{
			state.token = null;
			state.type = 0;
			state.channel = 0;
			state.tokenStartCharIndex = -1;
			state.tokenStartCharPositionInLine = -1;
			state.tokenStartLine = -1;
			state.text = null;
		}
	}

	public virtual IToken NextToken()
	{
		while (true)
		{
			state.token = null;
			state.channel = 0;
			state.tokenStartCharIndex = input.Index;
			state.tokenStartCharPositionInLine = input.CharPositionInLine;
			state.tokenStartLine = input.Line;
			state.text = null;
			if (input.LA(1) == -1)
			{
				break;
			}
			try
			{
				ParseNextToken();
				if (state.token == null)
				{
					Emit();
					goto IL_00af;
				}
				if (state.token != Tokens.Skip)
				{
					goto IL_00af;
				}
				goto end_IL_007c;
				IL_00af:
				return state.token;
				end_IL_007c:;
			}
			catch (MismatchedRangeException e)
			{
				ReportError(e);
			}
			catch (MismatchedTokenException e2)
			{
				ReportError(e2);
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(ex);
			}
		}
		return GetEndOfFileToken();
	}

	public virtual IToken GetEndOfFileToken()
	{
		IToken token = new CommonToken(input, -1, 0, input.Index, input.Index);
		token.Line = Line;
		token.CharPositionInLine = CharPositionInLine;
		return token;
	}

	public virtual void Skip()
	{
		state.token = Tokens.Skip;
	}

	public abstract void mTokens();

	public virtual void Emit(IToken token)
	{
		state.token = token;
	}

	public virtual IToken Emit()
	{
		IToken token = new CommonToken(input, state.type, state.channel, state.tokenStartCharIndex, CharIndex - 1);
		token.Line = state.tokenStartLine;
		token.Text = state.text;
		token.CharPositionInLine = state.tokenStartCharPositionInLine;
		Emit(token);
		return token;
	}

	public virtual void Match(string s)
	{
		int num = 0;
		while (num < s.Length)
		{
			if (input.LA(1) != s[num])
			{
				if (state.backtracking > 0)
				{
					state.failed = true;
					break;
				}
				MismatchedTokenException ex = new MismatchedTokenException(s[num], input, TokenNames);
				Recover(ex);
				throw ex;
			}
			num++;
			input.Consume();
			state.failed = false;
		}
	}

	public virtual void MatchAny()
	{
		input.Consume();
	}

	public virtual void Match(int c)
	{
		if (input.LA(1) != c)
		{
			if (state.backtracking <= 0)
			{
				MismatchedTokenException ex = new MismatchedTokenException(c, input, TokenNames);
				Recover(ex);
				throw ex;
			}
			state.failed = true;
		}
		else
		{
			input.Consume();
			state.failed = false;
		}
	}

	public virtual void MatchRange(int a, int b)
	{
		if (input.LA(1) < a || input.LA(1) > b)
		{
			if (state.backtracking <= 0)
			{
				MismatchedRangeException ex = new MismatchedRangeException(a, b, input);
				Recover(ex);
				throw ex;
			}
			state.failed = true;
		}
		else
		{
			input.Consume();
			state.failed = false;
		}
	}

	public override void ReportError(RecognitionException e)
	{
		DisplayRecognitionError(TokenNames, e);
	}

	public override string GetErrorMessage(RecognitionException e, string[] tokenNames)
	{
		string text = null;
		if (e is MismatchedTokenException)
		{
			MismatchedTokenException ex = (MismatchedTokenException)e;
			return "mismatched character " + GetCharErrorDisplay(e.Character) + " expecting " + GetCharErrorDisplay(ex.Expecting);
		}
		if (e is NoViableAltException)
		{
			_ = (NoViableAltException)e;
			return "no viable alternative at character " + GetCharErrorDisplay(e.Character);
		}
		if (e is EarlyExitException)
		{
			_ = (EarlyExitException)e;
			return "required (...)+ loop did not match anything at character " + GetCharErrorDisplay(e.Character);
		}
		if (e is MismatchedNotSetException)
		{
			MismatchedNotSetException ex2 = (MismatchedNotSetException)e;
			return "mismatched character " + GetCharErrorDisplay(e.Character) + " expecting set " + ex2.Expecting;
		}
		if (e is MismatchedSetException)
		{
			MismatchedSetException ex3 = (MismatchedSetException)e;
			return "mismatched character " + GetCharErrorDisplay(e.Character) + " expecting set " + ex3.Expecting;
		}
		if (e is MismatchedRangeException)
		{
			MismatchedRangeException ex4 = (MismatchedRangeException)e;
			return "mismatched character " + GetCharErrorDisplay(e.Character) + " expecting set " + GetCharErrorDisplay(ex4.A) + ".." + GetCharErrorDisplay(ex4.B);
		}
		return base.GetErrorMessage(e, tokenNames);
	}

	public virtual string GetCharErrorDisplay(int c)
	{
		string text = ((char)c).ToString();
		switch (c)
		{
		case -1:
			text = "<EOF>";
			break;
		case 10:
			text = "\\n";
			break;
		case 9:
			text = "\\t";
			break;
		case 13:
			text = "\\r";
			break;
		}
		return "'" + text + "'";
	}

	public virtual void Recover(RecognitionException re)
	{
		input.Consume();
	}

	[Conditional("ANTLR_TRACE")]
	public virtual void TraceIn(string ruleName, int ruleIndex)
	{
		string inputSymbol = (char)input.LT(1) + " line=" + Line + ":" + CharPositionInLine;
		base.TraceIn(ruleName, ruleIndex, inputSymbol);
	}

	[Conditional("ANTLR_TRACE")]
	public virtual void TraceOut(string ruleName, int ruleIndex)
	{
		string inputSymbol = (char)input.LT(1) + " line=" + Line + ":" + CharPositionInLine;
		base.TraceOut(ruleName, ruleIndex, inputSymbol);
	}

	protected virtual void ParseNextToken()
	{
		mTokens();
	}
}
