using System;
using System.Collections;
using System.Collections.Generic;

namespace Antlr.Runtime.Tree;

public class TreeIterator : IEnumerator<object>, IDisposable, IEnumerator
{
	protected ITreeAdaptor adaptor;

	protected object root;

	protected object tree;

	protected bool firstTime = true;

	private bool reachedEof;

	public object up;

	public object down;

	public object eof;

	protected Queue<object> nodes;

	public object Current { get; private set; }

	public TreeIterator(CommonTree tree)
		: this(new CommonTreeAdaptor(), tree)
	{
	}

	public TreeIterator(ITreeAdaptor adaptor, object tree)
	{
		this.adaptor = adaptor;
		this.tree = tree;
		root = tree;
		nodes = new Queue<object>();
		down = adaptor.Create(2, "DOWN");
		up = adaptor.Create(3, "UP");
		eof = adaptor.Create(-1, "EOF");
	}

	public void Dispose()
	{
	}

	public bool MoveNext()
	{
		if (firstTime)
		{
			firstTime = false;
			if (adaptor.GetChildCount(tree) == 0)
			{
				nodes.Enqueue(eof);
			}
			Current = tree;
		}
		else if (nodes != null && nodes.Count > 0)
		{
			Current = nodes.Dequeue();
		}
		else if (tree == null)
		{
			Current = eof;
		}
		else if (adaptor.GetChildCount(tree) > 0)
		{
			tree = adaptor.GetChild(tree, 0);
			nodes.Enqueue(tree);
			Current = down;
		}
		else
		{
			object parent = adaptor.GetParent(tree);
			while (parent != null && adaptor.GetChildIndex(tree) + 1 >= adaptor.GetChildCount(parent))
			{
				nodes.Enqueue(up);
				tree = parent;
				parent = adaptor.GetParent(tree);
			}
			if (parent == null)
			{
				tree = null;
				nodes.Enqueue(eof);
				Current = nodes.Dequeue();
			}
			else
			{
				int i = adaptor.GetChildIndex(tree) + 1;
				tree = adaptor.GetChild(parent, i);
				nodes.Enqueue(tree);
				Current = nodes.Dequeue();
			}
		}
		bool result = Current != eof || !reachedEof;
		reachedEof = Current == eof;
		return result;
	}

	public void Reset()
	{
		firstTime = true;
		tree = root;
		nodes.Clear();
	}
}
