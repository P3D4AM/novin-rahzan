using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Antlr.Runtime.Tree;

public class TreeParser : BaseRecognizer
{
	public const int DOWN = 2;

	public const int UP = 3;

	private static string dotdot = ".*[^.]\\.\\.[^.].*";

	private static string doubleEtc = ".*\\.\\.\\.\\s+\\.\\.\\..*";

	private static Regex dotdotPattern = new Regex(dotdot, RegexOptions.Compiled);

	private static Regex doubleEtcPattern = new Regex(doubleEtc, RegexOptions.Compiled);

	protected ITreeNodeStream input;

	public override string SourceName => input.SourceName;

	public TreeParser(ITreeNodeStream input)
	{
		this.input = input;
	}

	public TreeParser(ITreeNodeStream input, RecognizerSharedState state)
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

	public virtual void SetTreeNodeStream(ITreeNodeStream input)
	{
		this.input = input;
	}

	public virtual ITreeNodeStream GetTreeNodeStream()
	{
		return input;
	}

	protected override object GetCurrentInputSymbol(IIntStream input)
	{
		return ((ITreeNodeStream)input).LT(1);
	}

	protected override object GetMissingSymbol(IIntStream input, RecognitionException e, int expectedTokenType, BitSet follow)
	{
		string text = "<missing " + TokenNames[expectedTokenType] + ">";
		ITreeAdaptor treeAdaptor = ((ITreeNodeStream)e.Input).TreeAdaptor;
		return treeAdaptor.Create(new CommonToken(expectedTokenType, text));
	}

	public override void MatchAny(IIntStream ignore)
	{
		state.errorRecovery = false;
		state.failed = false;
		input.Consume();
		int num = input.LA(1);
		if (num != 2)
		{
			return;
		}
		input.Consume();
		int num2 = 1;
		while (num2 > 0)
		{
			switch (input.LA(1))
			{
			case 2:
				num2++;
				break;
			case 3:
				num2--;
				break;
			case -1:
				return;
			}
			input.Consume();
		}
	}

	protected override object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow)
	{
		throw new MismatchedTreeNodeException(ttype, (ITreeNodeStream)input);
	}

	public override string GetErrorHeader(RecognitionException e)
	{
		return GrammarFileName + ": node from " + (e.ApproximateLineInfo ? "after " : "") + "line " + e.Line + ":" + e.CharPositionInLine;
	}

	public override string GetErrorMessage(RecognitionException e, string[] tokenNames)
	{
		if (this != null)
		{
			ITreeAdaptor treeAdaptor = ((ITreeNodeStream)e.Input).TreeAdaptor;
			e.Token = treeAdaptor.GetToken(e.Node);
			if (e.Token == null)
			{
				e.Token = new CommonToken(treeAdaptor.GetType(e.Node), treeAdaptor.GetText(e.Node));
			}
		}
		return base.GetErrorMessage(e, tokenNames);
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
