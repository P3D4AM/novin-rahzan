using System.Collections.Generic;

namespace Babel.ParserGenerator;

internal class MinimalList<T> : IWriteableMinimalList<T>, IMinimalList<T>
{
	private readonly List<T> list;

	public int Count => list.Count;

	public T this[int index]
	{
		get
		{
			return list[index];
		}
		set
		{
			list[index] = value;
		}
	}

	internal MinimalList(int initialCapacity)
	{
		list = new List<T>(initialCapacity);
	}

	public int BinarySearch(T item, IComparer<T> comparer)
	{
		return list.BinarySearch(item, comparer);
	}

	public void Add(T item)
	{
		list.Add(item);
	}
}
