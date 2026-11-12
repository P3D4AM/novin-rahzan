using System;

namespace Antlr.Runtime.Tree;

[Serializable]
public class CommonErrorNode : CommonTree
{
	public IIntStream input;

	public IToken start;

	public IToken stop;

	public RecognitionException trappedException;

	public override bool IsNil => false;

	public override string Text
	{
		get
		{
			string text = null;
			if (start != null)
			{
				int tokenIndex = start.TokenIndex;
				int num = stop.TokenIndex;
				if (stop.Type == -1)
				{
					num = ((ITokenStream)input).Count;
				}
				return ((ITokenStream)input).ToString(tokenIndex, num);
			}
			if (start is ITree)
			{
				return ((ITreeNodeStream)input).ToString(start, stop);
			}
			return "<unknown>";
		}
		set
		{
		}
	}

	public override int Type
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public CommonErrorNode(ITokenStream input, IToken start, IToken stop, RecognitionException e)
	{
		if (stop == null || (stop.TokenIndex < start.TokenIndex && stop.Type != -1))
		{
			stop = start;
		}
		this.input = input;
		this.start = start;
		this.stop = stop;
		trappedException = e;
	}

	public override string ToString()
	{
		if (trappedException is MissingTokenException)
		{
			return "<missing type: " + ((MissingTokenException)trappedException).MissingType + ">";
		}
		if (trappedException is UnwantedTokenException)
		{
			return string.Concat("<extraneous: ", ((UnwantedTokenException)trappedException).UnexpectedToken, ", resync=", Text, ">");
		}
		if (trappedException is MismatchedTokenException)
		{
			return string.Concat("<mismatched token: ", trappedException.Token, ", resync=", Text, ">");
		}
		if (trappedException is NoViableAltException)
		{
			return string.Concat("<unexpected: ", trappedException.Token, ", resync=", Text, ">");
		}
		return "<error: " + Text + ">";
	}
}
