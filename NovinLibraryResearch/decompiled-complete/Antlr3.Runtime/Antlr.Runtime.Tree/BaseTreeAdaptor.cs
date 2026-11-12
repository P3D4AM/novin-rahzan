using System;
using System.Collections.Generic;

namespace Antlr.Runtime.Tree;

public abstract class BaseTreeAdaptor : ITreeAdaptor
{
	protected IDictionary<object, int> treeToUniqueIDMap;

	protected int uniqueNodeID = 1;

	public virtual object Nil()
	{
		return Create(null);
	}

	public virtual object ErrorNode(ITokenStream input, IToken start, IToken stop, RecognitionException e)
	{
		return new CommonErrorNode(input, start, stop, e);
	}

	public virtual bool IsNil(object tree)
	{
		return ((ITree)tree).IsNil;
	}

	public virtual object DupNode(int type, object treeNode)
	{
		object obj = DupNode(treeNode);
		SetType(obj, type);
		return obj;
	}

	public virtual object DupNode(object treeNode, string text)
	{
		object obj = DupNode(treeNode);
		SetText(obj, text);
		return obj;
	}

	public virtual object DupNode(int type, object treeNode, string text)
	{
		object obj = DupNode(treeNode);
		SetType(obj, type);
		SetText(obj, text);
		return obj;
	}

	public virtual object DupTree(object tree)
	{
		return DupTree(tree, null);
	}

	public virtual object DupTree(object t, object parent)
	{
		if (t == null)
		{
			return null;
		}
		object obj = DupNode(t);
		SetChildIndex(obj, GetChildIndex(t));
		SetParent(obj, parent);
		int childCount = GetChildCount(t);
		for (int i = 0; i < childCount; i++)
		{
			object child = GetChild(t, i);
			object child2 = DupTree(child, t);
			AddChild(obj, child2);
		}
		return obj;
	}

	public virtual void AddChild(object t, object child)
	{
		if (t != null && child != null)
		{
			((ITree)t).AddChild((ITree)child);
		}
	}

	public virtual object BecomeRoot(object newRoot, object oldRoot)
	{
		ITree tree = (ITree)newRoot;
		ITree t = (ITree)oldRoot;
		if (oldRoot == null)
		{
			return newRoot;
		}
		if (tree.IsNil)
		{
			int childCount = tree.ChildCount;
			if (childCount == 1)
			{
				tree = tree.GetChild(0);
			}
			else if (childCount > 1)
			{
				throw new Exception("more than one node as root (TODO: make exception hierarchy)");
			}
		}
		tree.AddChild(t);
		return tree;
	}

	public virtual object RulePostProcessing(object root)
	{
		ITree tree = (ITree)root;
		if (tree != null && tree.IsNil)
		{
			if (tree.ChildCount == 0)
			{
				tree = null;
			}
			else if (tree.ChildCount == 1)
			{
				tree = tree.GetChild(0);
				tree.Parent = null;
				tree.ChildIndex = -1;
			}
		}
		return tree;
	}

	public virtual object BecomeRoot(IToken newRoot, object oldRoot)
	{
		return BecomeRoot(Create(newRoot), oldRoot);
	}

	public virtual object Create(int tokenType, IToken fromToken)
	{
		fromToken = CreateToken(fromToken);
		fromToken.Type = tokenType;
		return Create(fromToken);
	}

	public virtual object Create(int tokenType, IToken fromToken, string text)
	{
		if (fromToken == null)
		{
			return Create(tokenType, text);
		}
		fromToken = CreateToken(fromToken);
		fromToken.Type = tokenType;
		fromToken.Text = text;
		return Create(fromToken);
	}

	public virtual object Create(IToken fromToken, string text)
	{
		if (fromToken == null)
		{
			throw new ArgumentNullException("fromToken");
		}
		fromToken = CreateToken(fromToken);
		fromToken.Text = text;
		return Create(fromToken);
	}

	public virtual object Create(int tokenType, string text)
	{
		IToken payload = CreateToken(tokenType, text);
		return Create(payload);
	}

	public virtual int GetType(object t)
	{
		return GetTree(t)?.Type ?? 0;
	}

	public virtual void SetType(object t, int type)
	{
		throw new NotSupportedException("don't know enough about Tree node");
	}

	public virtual string GetText(object t)
	{
		return GetTree(t)?.Text;
	}

	public virtual void SetText(object t, string text)
	{
		throw new NotSupportedException("don't know enough about Tree node");
	}

	public virtual object GetChild(object t, int i)
	{
		return GetTree(t)?.GetChild(i);
	}

	public virtual void SetChild(object t, int i, object child)
	{
		ITree tree = GetTree(t);
		if (tree != null)
		{
			ITree tree2 = GetTree(child);
			tree.SetChild(i, tree2);
		}
	}

	public virtual object DeleteChild(object t, int i)
	{
		return ((ITree)t).DeleteChild(i);
	}

	public virtual int GetChildCount(object t)
	{
		return GetTree(t)?.ChildCount ?? 0;
	}

	public virtual int GetUniqueID(object node)
	{
		if (treeToUniqueIDMap == null)
		{
			treeToUniqueIDMap = new Dictionary<object, int>();
		}
		if (treeToUniqueIDMap.TryGetValue(node, out var value))
		{
			return value;
		}
		value = uniqueNodeID;
		treeToUniqueIDMap[node] = value;
		uniqueNodeID++;
		return value;
	}

	public abstract IToken CreateToken(int tokenType, string text);

	public abstract IToken CreateToken(IToken fromToken);

	public abstract object Create(IToken payload);

	public virtual object DupNode(object treeNode)
	{
		return GetTree(treeNode)?.DupNode();
	}

	public abstract IToken GetToken(object t);

	public virtual void SetTokenBoundaries(object t, IToken startToken, IToken stopToken)
	{
		ITree tree = GetTree(t);
		if (tree != null)
		{
			int tokenStartIndex = 0;
			int tokenStopIndex = 0;
			if (startToken != null)
			{
				tokenStartIndex = startToken.TokenIndex;
			}
			if (stopToken != null)
			{
				tokenStopIndex = stopToken.TokenIndex;
			}
			tree.TokenStartIndex = tokenStartIndex;
			tree.TokenStopIndex = tokenStopIndex;
		}
	}

	public virtual int GetTokenStartIndex(object t)
	{
		return GetTree(t)?.TokenStartIndex ?? (-1);
	}

	public virtual int GetTokenStopIndex(object t)
	{
		return GetTree(t)?.TokenStopIndex ?? (-1);
	}

	public virtual object GetParent(object t)
	{
		return GetTree(t)?.Parent;
	}

	public virtual void SetParent(object t, object parent)
	{
		ITree tree = GetTree(t);
		if (tree != null)
		{
			ITree tree2 = GetTree(parent);
			tree.Parent = tree2;
		}
	}

	public virtual int GetChildIndex(object t)
	{
		return GetTree(t)?.ChildIndex ?? 0;
	}

	public virtual void SetChildIndex(object t, int index)
	{
		ITree tree = GetTree(t);
		if (tree != null)
		{
			tree.ChildIndex = index;
		}
	}

	public virtual void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t)
	{
		GetTree(parent)?.ReplaceChildren(startChildIndex, stopChildIndex, t);
	}

	protected virtual ITree GetTree(object t)
	{
		if (t == null)
		{
			return null;
		}
		if (!(t is ITree result))
		{
			throw new NotSupportedException();
		}
		return result;
	}
}
