using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Antlr.Runtime.Tree;

[Serializable]
[DebuggerTypeProxy(typeof(AntlrRuntime_BaseTreeDebugView))]
public abstract class BaseTree : ITree
{
	private IList<ITree> _children;

	public virtual IList<ITree> Children
	{
		get
		{
			return _children;
		}
		private set
		{
			_children = value;
		}
	}

	public virtual int ChildCount
	{
		get
		{
			if (Children == null)
			{
				return 0;
			}
			return Children.Count;
		}
	}

	public virtual ITree Parent
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual int ChildIndex
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public virtual bool IsNil => false;

	public abstract int TokenStartIndex { get; set; }

	public abstract int TokenStopIndex { get; set; }

	public abstract int Type { get; set; }

	public abstract string Text { get; set; }

	public virtual int Line { get; set; }

	public virtual int CharPositionInLine { get; set; }

	public BaseTree()
	{
	}

	public BaseTree(ITree node)
	{
	}

	public virtual ITree GetChild(int i)
	{
		if (i < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (Children == null || i >= Children.Count)
		{
			return null;
		}
		return Children[i];
	}

	public virtual ITree GetFirstChildWithType(int type)
	{
		foreach (ITree child in Children)
		{
			if (child.Type == type)
			{
				return child;
			}
		}
		return null;
	}

	public virtual void AddChild(ITree t)
	{
		if (t == null)
		{
			return;
		}
		if (t.IsNil)
		{
			BaseTree baseTree = t as BaseTree;
			if (baseTree != null && Children != null && Children == baseTree.Children)
			{
				throw new Exception("attempt to add child list to itself");
			}
			if (t.ChildCount <= 0)
			{
				return;
			}
			if (Children != null || baseTree == null)
			{
				if (Children == null)
				{
					Children = CreateChildrenList();
				}
				int childCount = t.ChildCount;
				for (int i = 0; i < childCount; i++)
				{
					ITree child = t.GetChild(i);
					Children.Add(child);
					child.Parent = this;
					child.ChildIndex = Children.Count - 1;
				}
			}
			else
			{
				Children = baseTree.Children;
				FreshenParentAndChildIndexes();
			}
		}
		else
		{
			if (Children == null)
			{
				Children = CreateChildrenList();
			}
			Children.Add(t);
			t.Parent = this;
			t.ChildIndex = Children.Count - 1;
		}
	}

	public virtual void AddChildren(IEnumerable<ITree> kids)
	{
		if (kids == null)
		{
			throw new ArgumentNullException("kids");
		}
		foreach (ITree kid in kids)
		{
			AddChild(kid);
		}
	}

	public virtual void SetChild(int i, ITree t)
	{
		if (i < 0)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		if (t != null)
		{
			if (t.IsNil)
			{
				throw new ArgumentException("Can't set single child to a list");
			}
			if (Children == null)
			{
				Children = CreateChildrenList();
			}
			Children[i] = t;
			t.Parent = this;
			t.ChildIndex = i;
		}
	}

	public virtual void InsertChild(int i, ITree t)
	{
		if (i < 0)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		if (i > ChildCount)
		{
			throw new ArgumentException();
		}
		if (i == ChildCount)
		{
			AddChild(t);
			return;
		}
		Children.Insert(i, t);
		FreshenParentAndChildIndexes(i);
	}

	public virtual object DeleteChild(int i)
	{
		if (i < 0)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		if (i >= ChildCount)
		{
			throw new ArgumentException();
		}
		if (Children == null)
		{
			return null;
		}
		ITree result = Children[i];
		Children.RemoveAt(i);
		FreshenParentAndChildIndexes(i);
		return result;
	}

	public virtual void ReplaceChildren(int startChildIndex, int stopChildIndex, object t)
	{
		if (startChildIndex < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (stopChildIndex < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (t == null)
		{
			throw new ArgumentNullException("t");
		}
		if (stopChildIndex < startChildIndex)
		{
			throw new ArgumentException();
		}
		if (Children == null)
		{
			throw new ArgumentException("indexes invalid; no children in list");
		}
		int num = stopChildIndex - startChildIndex + 1;
		ITree tree = (ITree)t;
		IList<ITree> list = null;
		if (tree.IsNil)
		{
			if (tree is BaseTree { Children: not null } baseTree)
			{
				list = baseTree.Children;
			}
			else
			{
				list = CreateChildrenList();
				int childCount = tree.ChildCount;
				for (int i = 0; i < childCount; i++)
				{
					list.Add(tree.GetChild(i));
				}
			}
		}
		else
		{
			list = new List<ITree>(1);
			list.Add(tree);
		}
		int count = list.Count;
		int count2 = list.Count;
		int num2 = num - count;
		if (num2 == 0)
		{
			int num3 = 0;
			for (int j = startChildIndex; j <= stopChildIndex; j++)
			{
				ITree tree2 = list[num3];
				Children[j] = tree2;
				tree2.Parent = this;
				tree2.ChildIndex = j;
				num3++;
			}
		}
		else if (num2 > 0)
		{
			for (int k = 0; k < count2; k++)
			{
				Children[startChildIndex + k] = list[k];
			}
			int num4 = startChildIndex + count2;
			for (int l = num4; l <= stopChildIndex; l++)
			{
				Children.RemoveAt(num4);
			}
			FreshenParentAndChildIndexes(startChildIndex);
		}
		else
		{
			for (int m = 0; m < num; m++)
			{
				Children[startChildIndex + m] = list[m];
			}
			for (int n = num; n < count; n++)
			{
				Children.Insert(startChildIndex + n, list[n]);
			}
			FreshenParentAndChildIndexes(startChildIndex);
		}
	}

	protected virtual IList<ITree> CreateChildrenList()
	{
		return new List<ITree>();
	}

	public virtual void FreshenParentAndChildIndexes()
	{
		FreshenParentAndChildIndexes(0);
	}

	public virtual void FreshenParentAndChildIndexes(int offset)
	{
		int childCount = ChildCount;
		for (int i = offset; i < childCount; i++)
		{
			ITree child = GetChild(i);
			child.ChildIndex = i;
			child.Parent = this;
		}
	}

	public virtual void FreshenParentAndChildIndexesDeeply()
	{
		FreshenParentAndChildIndexesDeeply(0);
	}

	public virtual void FreshenParentAndChildIndexesDeeply(int offset)
	{
		int childCount = ChildCount;
		for (int i = offset; i < childCount; i++)
		{
			ITree child = GetChild(i);
			child.ChildIndex = i;
			child.Parent = this;
			if (child is BaseTree baseTree)
			{
				baseTree.FreshenParentAndChildIndexesDeeply();
			}
		}
	}

	public virtual void SanityCheckParentAndChildIndexes()
	{
		SanityCheckParentAndChildIndexes(null, -1);
	}

	public virtual void SanityCheckParentAndChildIndexes(ITree parent, int i)
	{
		if (parent != Parent)
		{
			throw new InvalidOperationException(string.Concat("parents don't match; expected ", parent, " found ", Parent));
		}
		if (i != ChildIndex)
		{
			throw new InvalidOperationException("child indexes don't match; expected " + i + " found " + ChildIndex);
		}
		int childCount = ChildCount;
		for (int j = 0; j < childCount; j++)
		{
			BaseTree baseTree = (BaseTree)GetChild(j);
			baseTree.SanityCheckParentAndChildIndexes(this, j);
		}
	}

	public virtual bool HasAncestor(int ttype)
	{
		return GetAncestor(ttype) != null;
	}

	public virtual ITree GetAncestor(int ttype)
	{
		ITree tree = this;
		for (tree = tree.Parent; tree != null; tree = tree.Parent)
		{
			if (tree.Type == ttype)
			{
				return tree;
			}
		}
		return null;
	}

	public virtual IList<ITree> GetAncestors()
	{
		if (Parent == null)
		{
			return null;
		}
		List<ITree> list = new List<ITree>();
		ITree tree = this;
		for (tree = tree.Parent; tree != null; tree = tree.Parent)
		{
			list.Insert(0, tree);
		}
		return list;
	}

	public virtual string ToStringTree()
	{
		if (Children == null || Children.Count == 0)
		{
			return ToString();
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (!IsNil)
		{
			stringBuilder.Append("(");
			stringBuilder.Append(ToString());
			stringBuilder.Append(' ');
		}
		int num = 0;
		while (Children != null && num < Children.Count)
		{
			ITree tree = Children[num];
			if (num > 0)
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(tree.ToStringTree());
			num++;
		}
		if (!IsNil)
		{
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	public abstract override string ToString();

	public abstract ITree DupNode();
}
