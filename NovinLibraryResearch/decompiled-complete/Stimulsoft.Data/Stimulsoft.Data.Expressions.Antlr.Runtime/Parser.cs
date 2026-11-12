using System.Diagnostics;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime;

public class Parser : BaseRecognizer
{
	public ITokenStream input;

	public virtual ITokenStream TokenStream
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

	public Parser(ITokenStream input)
	{
		TokenStream = input;
	}

	public Parser(ITokenStream input, RecognizerSharedState state)
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
	}

	protected override object GetCurrentInputSymbol(IIntStream input)
	{
		return ((ITokenStream)input).LT(1);
	}

	protected override object GetMissingSymbol(IIntStream input, RecognitionException e, int expectedTokenType, BitSet follow)
	{
		string text = null;
		text = ((expectedTokenType != -1) ? ("<missing " + TokenNames[expectedTokenType] + ">") : "<missing EOF>");
		CommonToken commonToken = new CommonToken(expectedTokenType, text);
		IToken token = ((ITokenStream)input).LT(1);
		if (token.Type == -1)
		{
			token = ((ITokenStream)input).LT(-1);
		}
		commonToken.Line = token.Line;
		commonToken.CharPositionInLine = token.CharPositionInLine;
		commonToken.Channel = 0;
		commonToken.InputStream = token.InputStream;
		return commonToken;
	}

	[Conditional("ANTLR_TRACE")]
	public virtual void TraceIn(string ruleName, int ruleIndex)
	{
		base.TraceIn(ruleName, ruleIndex, input.LT(1));
	}

	[Conditional("ANTLR_TRACE")]
	public virtual void TraceOut(string ruleName, int ruleIndex)
	{
		base.TraceOut(ruleName, ruleIndex, input.LT(1));
	}
}
