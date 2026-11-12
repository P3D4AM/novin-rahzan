using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr.Runtime.Misc;

public class FastQueue<T>
{
	internal List<T> _data = new List<T>();

	internal int _p;

	public virtual int Count => _data.Count - _p;

	public virtual int Range { get; protected set; }

	public virtual T this[int i]
	{
		get
		{
			int num = _p + i;
			if (num >= _data.Count)
			{
				throw new ArgumentException($"queue index {num} > last index {_data.Count - 1}");
			}
			if (num < 0)
			{
				throw new ArgumentException($"queue index {num} < 0");
			}
			if (num > Range)
			{
				Range = num;
			}
			return _data[num];
		}
	}

	public virtual T Dequeue()
	{
		if (Count == 0)
		{
			throw new InvalidOperationException();
		}
		T result = this[0];
		_p++;
		if (_p == _data.Count)
		{
			Clear();
		}
		return result;
	}

	public virtual void Enqueue(T o)
	{
		_data.Add(o);
	}

	public virtual T Peek()
	{
		return this[0];
	}

	public virtual void Clear()
	{
		_p = 0;
		_data.Clear();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int count = Count;
		for (int i = 0; i < count; i++)
		{
			stringBuilder.Append(this[i]);
			if (i + 1 < count)
			{
				stringBuilder.Append(" ");
			}
		}
		return stringBuilder.ToString();
	}
}
