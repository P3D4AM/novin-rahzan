using System;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime.Tree;

[Serializable]
public class CommonTree : BaseTree
{
	private IToken _token;

	protected int startIndex = -1;

	protected int stopIndex = -1;

	private CommonTree parent;

	private int childIndex = -1;

	public override int CharPositionInLine
	{
		get
		{
			if (Token == null || Token.CharPositionInLine == -1)
			{
				if (ChildCount > 0)
				{
					return Children[0].CharPositionInLine;
				}
				return 0;
			}
			return Token.CharPositionInLine;
		}
		set
		{
			base.CharPositionInLine = value;
		}
	}

	public override int ChildIndex
	{
		get
		{
			return childIndex;
		}
		set
		{
			childIndex = value;
		}
	}

	public override bool IsNil => Token == null;

	public override int Line
	{
		get
		{
			if (Token == null || Token.Line == 0)
			{
				if (ChildCount > 0)
				{
					return Children[0].Line;
				}
				return 0;
			}
			return Token.Line;
		}
		set
		{
			base.Line = value;
		}
	}

	public override ITree Parent
	{
		get
		{
			return parent;
		}
		set
		{
			parent = (CommonTree)value;
		}
	}

	public override string Text
	{
		get
		{
			if (Token == null)
			{
				return null;
			}
			return Token.Text;
		}
		set
		{
		}
	}

	public IToken Token
	{
		get
		{
			return _token;
		}
		set
		{
			_token = value;
		}
	}

	public override int TokenStartIndex
	{
		get
		{
			if (startIndex == -1 && Token != null)
			{
				return Token.TokenIndex;
			}
			return startIndex;
		}
		set
		{
			startIndex = value;
		}
	}

	public override int TokenStopIndex
	{
		get
		{
			if (stopIndex == -1 && Token != null)
			{
				return Token.TokenIndex;
			}
			return stopIndex;
		}
		set
		{
			stopIndex = value;
		}
	}

	public override int Type
	{
		get
		{
			if (Token == null)
			{
				return 0;
			}
			return Token.Type;
		}
		set
		{
		}
	}

	public CommonTree()
	{
	}

	public CommonTree(CommonTree node)
		: base(node)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		Token = node.Token;
		startIndex = node.startIndex;
		stopIndex = node.stopIndex;
	}

	public CommonTree(IToken t)
	{
		Token = t;
	}

	public override ITree DupNode()
	{
		return new CommonTree(this);
	}

	public virtual void SetUnknownTokenBoundaries()
	{
		if (Children == null)
		{
			if (startIndex < 0 || stopIndex < 0)
			{
				startIndex = (stopIndex = Token.TokenIndex);
			}
			return;
		}
		foreach (ITree child in Children)
		{
			if (child is CommonTree commonTree)
			{
				commonTree.SetUnknownTokenBoundaries();
			}
		}
		if ((startIndex < 0 || stopIndex < 0) && Children.Count > 0)
		{
			ITree tree = Children[0];
			ITree tree2 = Children[Children.Count - 1];
			startIndex = tree.TokenStartIndex;
			stopIndex = tree2.TokenStopIndex;
		}
	}

	public override string ToString()
	{
		if (IsNil)
		{
			return "nil";
		}
		if (Type == 0)
		{
			return "<errornode>";
		}
		if (Token == null)
		{
			return string.Empty;
		}
		return Token.Text;
	}
}
