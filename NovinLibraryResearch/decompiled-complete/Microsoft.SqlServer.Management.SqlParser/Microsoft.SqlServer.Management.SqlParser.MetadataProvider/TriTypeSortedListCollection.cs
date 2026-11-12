using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class TriTypeSortedListCollection<T, B1, B2> : DualTypeSortedListCollection<T, B1>, IMetadataCollection<B2>, IEnumerable<B2>, IEnumerable where T : class, B1, B2 where B1 : class, IMetadataObject where B2 : class, IMetadataObject
{
	int IMetadataCollection<B2>.Count => base.Count;

	B2 IMetadataCollection<B2>.this[string name] => (B2)base[name];

	IMetadataCollection<IMetadataObject> IMetadataCollection<B2>.AsMetadataObjectCollection => base.AsMetadataObjectCollection;

	public TriTypeSortedListCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}

	bool IMetadataCollection<B2>.Contains(string name)
	{
		return Contains(name);
	}

	bool IMetadataCollection<B2>.Contains(B2 item)
	{
		return base[item.Name] == item;
	}

	IEnumerable<B2> IMetadataCollection<B2>.FindAll(Predicate<B2> match)
	{
		using IEnumerator<T> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			T current = enumerator.Current;
			if (match((B2)current))
			{
				yield return (B2)current;
			}
		}
	}

	IEnumerable<B2> IMetadataCollection<B2>.FindAll(string name)
	{
		foreach (T item in FindAll(name))
		{
			yield return (B2)item;
		}
	}

	IEnumerator<B2> IEnumerable<B2>.GetEnumerator()
	{
		using IEnumerator<T> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			T current = enumerator.Current;
			yield return (B2)current;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
