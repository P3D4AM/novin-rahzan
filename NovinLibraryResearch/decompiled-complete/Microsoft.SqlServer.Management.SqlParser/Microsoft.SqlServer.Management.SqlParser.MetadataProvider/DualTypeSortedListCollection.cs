using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class DualTypeSortedListCollection<T, B> : SortedListCollection<T>, IMetadataCollection<B>, IEnumerable<B>, IEnumerable where T : class, B where B : class, IMetadataObject
{
	int IMetadataCollection<B>.Count => base.Count;

	B IMetadataCollection<B>.this[string name] => (B)base[name];

	IMetadataCollection<IMetadataObject> IMetadataCollection<B>.AsMetadataObjectCollection => base.AsMetadataObjectCollection;

	public DualTypeSortedListCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}

	bool IMetadataCollection<B>.Contains(string name)
	{
		return Contains(name);
	}

	bool IMetadataCollection<B>.Contains(B item)
	{
		return base[item.Name] == item;
	}

	IEnumerable<B> IMetadataCollection<B>.FindAll(Predicate<B> match)
	{
		using IEnumerator<T> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			T current = enumerator.Current;
			if (match((B)current))
			{
				yield return (B)current;
			}
		}
	}

	IEnumerable<B> IMetadataCollection<B>.FindAll(string name)
	{
		foreach (T item in FindAll(name))
		{
			yield return (B)item;
		}
	}

	IEnumerator<B> IEnumerable<B>.GetEnumerator()
	{
		using IEnumerator<T> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			T current = enumerator.Current;
			yield return (B)current;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
