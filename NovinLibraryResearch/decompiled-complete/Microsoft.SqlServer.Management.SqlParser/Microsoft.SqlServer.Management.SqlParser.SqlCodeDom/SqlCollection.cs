using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCollection<T> : ISqlCodeCollection<T>, IEnumerable<T>, IEnumerable where T : SqlCodeObject
{
	private readonly List<T> internalList;

	public int Count => internalList.Count;

	public bool IsReadOnly => true;

	public T this[int index] => internalList[index];

	internal SqlCollection()
	{
		internalList = new List<T>(0);
	}

	internal SqlCollection(T item)
	{
		internalList = new List<T>(1);
		internalList.Add(item);
	}

	internal SqlCollection(IEnumerable<T> items)
		: this(items, 4)
	{
	}

	internal SqlCollection(List<T> items)
		: this((IEnumerable<T>)items, items.Count)
	{
	}

	internal SqlCollection(params T[] items)
		: this((IEnumerable<T>)items, items.Length)
	{
	}

	private SqlCollection(IEnumerable<T> items, int count)
	{
		internalList = new List<T>(count);
		internalList.AddRange(items);
	}

	public bool Contains(T item)
	{
		return internalList.Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		internalList.CopyTo(array, arrayIndex);
	}

	public int IndexOf(T item)
	{
		return internalList.IndexOf(item);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return internalList.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return internalList.GetEnumerator();
	}
}
