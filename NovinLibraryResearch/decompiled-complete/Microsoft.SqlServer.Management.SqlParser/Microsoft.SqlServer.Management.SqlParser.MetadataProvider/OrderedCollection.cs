using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public class OrderedCollection<T> : OrderedCollectionBase<T>, IMetadataOrderedCollection<IMetadataObject>, IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable where T : class, IMetadataObject
{
	public override IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

	IMetadataObject IMetadataOrderedCollection<IMetadataObject>.this[int index] => base[index];

	int IMetadataCollection<IMetadataObject>.Count => base.Count;

	IMetadataObject IMetadataCollection<IMetadataObject>.this[string name] => base[name];

	IMetadataCollection<IMetadataObject> IMetadataCollection<IMetadataObject>.AsMetadataObjectCollection => this;

	public OrderedCollection(CollationInfo collationInfo)
		: this(16, collationInfo)
	{
	}

	public OrderedCollection(int initialCapacity, CollationInfo collationInfo)
		: base(initialCapacity, collationInfo)
	{
	}

	bool IMetadataCollection<IMetadataObject>.Contains(string name)
	{
		return Contains(name);
	}

	bool IMetadataCollection<IMetadataObject>.Contains(IMetadataObject item)
	{
		using (IEnumerator<T> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == item)
				{
					return true;
				}
			}
		}
		return false;
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
