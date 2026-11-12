using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public abstract class OrderedCollectionBase<T> : IMutableMetadataOrderedCollection<T>, IMetadataOrderedCollection<T>, IMetadataCollection<T>, IEnumerable<T>, IEnumerable, IMutableMetadataCollection<T> where T : class, IMetadataObject
{
	private readonly List<T> m_data;

	private readonly CollationInfo m_collationInfo;

	public T this[int index] => m_data[index];

	public int Count => m_data.Count;

	T IMetadataCollection<T>.this[string name] => this[name];

	public abstract IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

	public T this[string name]
	{
		get
		{
			IEqualityComparer<string> equalityComparer = m_collationInfo.EqualityComparer;
			foreach (T datum in m_data)
			{
				if (equalityComparer.Equals(name, datum.Name))
				{
					return datum;
				}
			}
			return null;
		}
		set
		{
			IEqualityComparer<string> equalityComparer = m_collationInfo.EqualityComparer;
			for (int i = 0; i < m_data.Count; i++)
			{
				if (equalityComparer.Equals(name, m_data[i].Name))
				{
					m_data[i] = value;
				}
			}
		}
	}

	protected OrderedCollectionBase(int initialCapacity, CollationInfo collationInfo)
	{
		m_data = new List<T>(initialCapacity);
		m_collationInfo = collationInfo;
	}

	public void Add(T item)
	{
		m_data.Add(item);
	}

	public void AddRange(IEnumerable<T> items)
	{
		foreach (T item in items)
		{
			m_data.Add(item);
		}
	}

	public bool Contains(string name)
	{
		return this[name] != null;
	}

	public bool Contains(T item)
	{
		foreach (T datum in m_data)
		{
			if (datum == item)
			{
				return true;
			}
		}
		return false;
	}

	public IEnumerable<T> FindAll(Predicate<T> match)
	{
		foreach (T datum in m_data)
		{
			if (match(datum))
			{
				yield return datum;
			}
		}
	}

	public IEnumerable<T> FindAll(string name)
	{
		IEqualityComparer<string> comparer = m_collationInfo.EqualityComparer;
		foreach (T datum in m_data)
		{
			if (comparer.Equals(datum.Name, name))
			{
				yield return datum;
			}
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		return m_data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return m_data.GetEnumerator();
	}

	public bool Remove(string name)
	{
		IEqualityComparer<string> equalityComparer = m_collationInfo.EqualityComparer;
		bool result = false;
		int num = m_data.Count;
		for (int i = 0; i < num; i++)
		{
			if (equalityComparer.Equals(name, m_data[i].Name))
			{
				m_data.RemoveAt(i);
				num--;
				result = true;
			}
		}
		return result;
	}

	public bool Remove(T item)
	{
		return m_data.Remove(item);
	}

	public void Clear()
	{
		m_data.Clear();
	}

	public IMutableMetadataCollection<T> Clone()
	{
		return Clone(copyData: false);
	}

	public IMutableMetadataCollection<T> Clone(bool copyData)
	{
		IMutableMetadataCollection<T> mutableMetadataCollection = new OrderedCollection<T>(m_data.Capacity, m_collationInfo);
		if (copyData)
		{
			using IEnumerator<T> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				mutableMetadataCollection.Add(current);
			}
		}
		return mutableMetadataCollection;
	}
}
