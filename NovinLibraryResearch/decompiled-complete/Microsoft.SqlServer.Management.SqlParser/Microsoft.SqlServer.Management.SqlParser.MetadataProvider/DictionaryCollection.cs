using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class DictionaryCollection<T> : DictionaryCollectionBase<T, Dictionary<string, T>>, IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable where T : class, IMetadataObject
{
	private readonly CollationInfo m_collationInfo;

	public override IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

	int IMetadataCollection<IMetadataObject>.Count => base.Count;

	IMetadataObject IMetadataCollection<IMetadataObject>.this[string name] => base[name];

	IMetadataCollection<IMetadataObject> IMetadataCollection<IMetadataObject>.AsMetadataObjectCollection => this;

	public DictionaryCollection(int initialCapacity, CollationInfo collationInfo)
		: base(CreateContainer(initialCapacity, collationInfo))
	{
		m_collationInfo = collationInfo;
	}

	public override IMutableMetadataCollection<T> Clone(bool copyData)
	{
		DictionaryCollection<T> dictionaryCollection = new DictionaryCollection<T>((base.Count < 16) ? 16 : base.Count, m_collationInfo);
		if (copyData)
		{
			using IEnumerator<T> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				dictionaryCollection.Add(current);
			}
		}
		return dictionaryCollection;
	}

	private static Dictionary<string, T> CreateContainer(int initialCapacity, CollationInfo collationInfo)
	{
		return new Dictionary<string, T>(initialCapacity, collationInfo.EqualityComparer);
	}

	bool IMetadataCollection<IMetadataObject>.Contains(string name)
	{
		return Contains(name);
	}

	bool IMetadataCollection<IMetadataObject>.Contains(IMetadataObject item)
	{
		return base[item.Name] == item;
	}

	IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(Predicate<IMetadataObject> match)
	{
		using IEnumerator<T> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			T current = enumerator.Current;
			if (match(current))
			{
				yield return current;
			}
		}
	}

	IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(string name)
	{
		foreach (T item in FindAll(name))
		{
			yield return item;
		}
	}

	IEnumerator<IMetadataObject> IEnumerable<IMetadataObject>.GetEnumerator()
	{
		using IEnumerator<T> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			yield return enumerator.Current;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
