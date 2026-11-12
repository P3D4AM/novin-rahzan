using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr.Runtime.Tree;

[Serializable]
public class ParseTree : BaseTree
{
	public object payload;

	public List<IToken> hiddenTokens;

	public override string Text
	{
		get
		{
			return ToString();
		}
		set
		{
		}
	}

	public override int TokenStartIndex
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public override int TokenStopIndex
	{
		get
		{
			return 0;
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

	public ParseTree(object label)
	{
		payload = label;
	}

	public override ITree DupNode()
	{
		return null;
	}

	public override string ToString()
	{
		if (payload is IToken)
		{
			IToken token = (IToken)payload;
			if (token.Type == -1)
			{
				return "<EOF>";
			}
			return token.Text;
		}
		return payload.ToString();
	}

	public virtual string ToStringWithHiddenTokens()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (hiddenTokens != null)
		{
			for (int i = 0; i < hiddenTokens.Count; i++)
			{
				IToken token = hiddenTokens[i];
				stringBuilder.Append(token.Text);
			}
		}
		string text = ToString();
		if (!text.Equals("<EOF>"))
		{
			stringBuilder.Append(text);
		}
		return stringBuilder.ToString();
	}

	public virtual string ToInputString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		ToStringLeaves(stringBuilder);
		return stringBuilder.ToString();
	}

	protected virtual void ToStringLeaves(StringBuilder buf)
	{
		if (payload is IToken)
		{
			buf.Append(ToStringWithHiddenTokens());
			return;
		}
		int num = 0;
		while (Children != null && num < Children.Count)
		{
			ParseTree parseTree = (ParseTree)Children[num];
			parseTree.ToStringLeaves(buf);
			num++;
		}
	}
}
