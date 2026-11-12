using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public abstract class DictionaryCollectionBase<T, U> : IMutableMetadataCollection<T>, IMetadataCollection<T>, IEnumerable<T>, IEnumerable where T : class, IMetadataObject where U : class, IDictionary<string, T>
{
	private readonly U m_data;

	public int Count => m_data.Count;

	T IMetadataCollection<T>.this[string name] => this[name];

	public abstract IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

	public T this[string name]
	{
		get
		{
			if (m_data.TryGetValue(name, out var value))
			{
				return value;
			}
			return null;
		}
		set
		{
			m_data[name] = value;
		}
	}

	protected DictionaryCollectionBase(U container)
	{
		m_data = container;
	}

	public bool Contains(string name)
	{
		return m_data.ContainsKey(name);
	}

	public bool Contains(T item)
	{
		return this[item.Name] == item;
	}

	public IEnumerable<T> FindAll(Predicate<T> match)
	{
		foreach (T value in m_data.Values)
		{
			if (match(value))
			{
				yield return value;
			}
		}
	}

	public IEnumerable<T> FindAll(string name)
	{
		T val = this[name];
		if (val != null)
		{
			yield return val;
		}
	}

	public void Add(T item)
	{
		string name = item.Name;
		m_data.Add(name, item);
	}

	public void AddRange(IEnumerable<T> collection)
	{
		foreach (T item in collection)
		{
			string name = item.Name;
			m_data.Add(name, item);
		}
	}

	public bool Remove(string name)
	{
		return m_data.Remove(name);
	}

	public bool Remove(T item)
	{
		return Remove(item.Name);
	}

	public void Clear()
	{
		m_data.Clear();
	}

	public IMutableMetadataCollection<T> Clone()
	{
		return Clone(copyData: false);
	}

	public abstract IMutableMetadataCollection<T> Clone(bool copyData);

	public IEnumerator<T> GetEnumerator()
	{
		return m_data.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return m_data.Values.GetEnumerator();
	}
}
