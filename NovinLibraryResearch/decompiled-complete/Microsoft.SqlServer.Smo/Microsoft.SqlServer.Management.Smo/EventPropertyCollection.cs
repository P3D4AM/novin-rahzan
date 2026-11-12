using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class EventPropertyCollection : ICollection, IEnumerable, IEnumerable<EventProperty>
{
	private class Collection : KeyedCollection<string, EventProperty>
	{
		protected override string GetKeyForItem(EventProperty item)
		{
			return item.Name;
		}
	}

	private Collection collection;

	public EventProperty this[int index] => collection[index];

	public EventProperty this[string name] => collection[name];

	public int Count => collection.Count;

	bool ICollection.IsSynchronized => ((ICollection)collection).IsSynchronized;

	object ICollection.SyncRoot => ((ICollection)collection).SyncRoot;

	internal EventPropertyCollection(PropertyDataCollection properties)
	{
		collection = new Collection();
		PropertyDataEnumerator enumerator = properties.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PropertyData current = enumerator.Current;
				if (current.Name == "PostTime")
				{
					collection.Add(new EventProperty(current.Name, ConvertToDateTime((string)current.Value)));
				}
				else
				{
					collection.Add(new EventProperty(current.Name, current.Value));
				}
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}

	public void CopyTo(EventProperty[] array, int index)
	{
		collection.CopyTo(array, index);
	}

	[CLSCompliant(false)]
	public IEnumerator<EventProperty> GetEnumerator()
	{
		return collection.GetEnumerator();
	}

	void ICollection.CopyTo(Array array, int index)
	{
		((ICollection)collection).CopyTo(array, index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)collection).GetEnumerator();
	}

	private static DateTime ConvertToDateTime(string dateTime)
	{
		char[] value = dateTime.ToCharArray();
		IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
		return new DateTime(int.Parse(new string(value, 0, 4), NumberStyles.None, invariantCulture), int.Parse(new string(value, 4, 2), NumberStyles.None, invariantCulture), int.Parse(new string(value, 6, 2), NumberStyles.None, invariantCulture), int.Parse(new string(value, 8, 2), NumberStyles.None, invariantCulture), int.Parse(new string(value, 10, 2), NumberStyles.None, invariantCulture), int.Parse(new string(value, 12, 2), NumberStyles.None, invariantCulture));
	}

	internal void Add(string name, object value)
	{
		collection.Add(new EventProperty(name, value));
	}
}
