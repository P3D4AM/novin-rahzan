using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

internal class AtomicSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
{
	private readonly List<T> rep = new List<T>();

	public IEqualityComparer<T> Comparer { get; }

	public int Count => rep.Count;

	public bool IsReadOnly => false;

	public AtomicSet(IEqualityComparer<T> comparer)
	{
		Comparer = comparer;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		rep.CopyTo(array, arrayIndex);
	}

	public bool Remove(T item)
	{
		return rep.RemoveAll((T i) => Comparer.Equals(i, item)) > 0;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return rep.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)rep).GetEnumerator();
	}

	public void Add(T item)
	{
		GetOrAdd(item);
	}

	public void Clear()
	{
		rep.Clear();
	}

	public bool Contains(T item)
	{
		return rep.Exists((T i) => Comparer.Equals(i, item));
	}

	public T Find(T value)
	{
		return rep.Find((T i) => Comparer.Equals(value, i));
	}

	public T GetOrAdd(T value)
	{
		T val = Find(value);
		if (!EqualityComparer<T>.Default.Equals(val, default(T)))
		{
			return val;
		}
		rep.Add(value);
		return value;
	}
}
