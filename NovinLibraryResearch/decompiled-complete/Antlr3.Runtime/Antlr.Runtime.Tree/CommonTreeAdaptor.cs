namespace Antlr.Runtime.Tree;

public class CommonTreeAdaptor : BaseTreeAdaptor
{
	public override object Create(IToken payload)
	{
		return new CommonTree(payload);
	}

	public override IToken CreateToken(int tokenType, string text)
	{
		return new CommonToken(tokenType, text);
	}

	public override IToken CreateToken(IToken fromToken)
	{
		return new CommonToken(fromToken);
	}

	public override IToken GetToken(object t)
	{
		if (t is CommonTree)
		{
			return ((CommonTree)t).Token;
		}
		return null;
	}
}
