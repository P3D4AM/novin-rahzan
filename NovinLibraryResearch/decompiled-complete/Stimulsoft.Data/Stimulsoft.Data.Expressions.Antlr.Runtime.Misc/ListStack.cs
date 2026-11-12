using System;
using System.Collections.Generic;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime.Misc;

public class ListStack<T> : List<T>
{
	public T Peek()
	{
		return Peek(0);
	}

	public T Peek(int depth)
	{
		if (!TryPeek(depth, out var item))
		{
			throw new InvalidOperationException();
		}
		return item;
	}

	public bool TryPeek(out T item)
	{
		return TryPeek(0, out item);
	}

	public bool TryPeek(int depth, out T item)
	{
		if (depth >= base.Count)
		{
			item = default(T);
			return false;
		}
		item = base[base.Count - depth - 1];
		return true;
	}

	public T Pop()
	{
		if (!TryPop(out var item))
		{
			throw new InvalidOperationException();
		}
		return item;
	}

	public bool TryPop(out T item)
	{
		if (base.Count == 0)
		{
			item = default(T);
			return false;
		}
		item = base[base.Count - 1];
		RemoveAt(base.Count - 1);
		return true;
	}

	public void Push(T item)
	{
		Add(item);
	}
}
