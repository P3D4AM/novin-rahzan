using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.Management.SqlParser.Common;

internal class PriorityQueue<T>
{
	private List<T> list;

	private IComparer<T> comparer;

	public IComparer<T> Comparer => comparer;

	public int Count => list.Count;

	public T Peek
	{
		get
		{
			if (Count == 0)
			{
				throw new InvalidOperationException("cannot call Peek on a PriorityQueue with Count = 0");
			}
			return list[0];
		}
	}

	public PriorityQueue()
		: this((IComparer<T>)Comparer<T>.Default)
	{
	}

	public PriorityQueue(IComparer<T> comparer)
	{
		list = new List<T>();
		this.comparer = comparer;
	}

	public void Enqueue(T value)
	{
		list.Add(value);
		if (list.Count > 1)
		{
			Enqueue(list.Count - 1, value);
		}
	}

	private void Enqueue(int hole, T value)
	{
		int num = (hole - 1) / 2;
		while (0 < hole && comparer.Compare(list[num], value) < 0)
		{
			list[hole] = list[num];
			hole = num;
			num = (hole - 1) / 2;
		}
		list[hole] = value;
	}

	public T Dequeue()
	{
		if (Count == 0)
		{
			throw new InvalidOperationException("cannot call Dequeue on a PriorityQueue with Count = 0");
		}
		if (Count > 1)
		{
			int num = list.Count - 1;
			T lastValue = list[num];
			list[num] = list[0];
			AdjustHeap(0, num, lastValue);
		}
		T result = list[list.Count - 1];
		list.RemoveAt(list.Count - 1);
		return result;
	}

	private void AdjustHeap(int hole, int bottom, T lastValue)
	{
		int num;
		for (num = 2 * hole + 2; num < bottom; num = 2 * num + 2)
		{
			if (comparer.Compare(list[num], list[num - 1]) < 0)
			{
				num--;
			}
			list[hole] = list[num];
			hole = num;
		}
		if (num == bottom)
		{
			list[hole] = list[bottom - 1];
			hole = bottom - 1;
		}
		Enqueue(hole, lastValue);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("( ");
		foreach (T item in list)
		{
			stringBuilder.Append(item);
			stringBuilder.Append(" ");
		}
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
