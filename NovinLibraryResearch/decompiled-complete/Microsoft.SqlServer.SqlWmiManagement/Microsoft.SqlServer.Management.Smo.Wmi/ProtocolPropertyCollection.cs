using System;
using System.Collections;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public abstract class ProtocolPropertyCollection : ICollection, IEnumerable
{
	internal sealed class ProtocolPropertyEnumerator : IEnumerator
	{
		private IDictionaryEnumerator baseEnumerator;

		object IEnumerator.Current => ((DictionaryEntry)baseEnumerator.Current).Value;

		private ProtocolProperty Current => baseEnumerator.Value as ProtocolProperty;

		public ProtocolPropertyEnumerator(ProtocolPropertyCollection col)
		{
			baseEnumerator = col.sl.GetEnumerator();
		}

		public bool MoveNext()
		{
			return baseEnumerator.MoveNext();
		}

		public void Reset()
		{
			baseEnumerator.Reset();
		}
	}

	internal SortedList sl;

	public int Count => sl.Count;

	public bool IsSynchronized => sl.IsSynchronized;

	public object SyncRoot => sl.SyncRoot;

	public ProtocolProperty this[string name]
	{
		get
		{
			ProtocolProperty protocolProperty = LookupName(name);
			if (null == protocolProperty)
			{
				throw new UnknownPropertyException(name);
			}
			return protocolProperty;
		}
	}

	public ProtocolProperty this[int index]
	{
		get
		{
			if (0 > index || index >= sl.Count)
			{
				throw new IndexOutOfRangeException();
			}
			return sl.GetByIndex(index) as ProtocolProperty;
		}
	}

	internal ProtocolPropertyCollection()
	{
		sl = new SortedList();
	}

	public void CopyTo(ProtocolProperty[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	void ICollection.CopyTo(Array array, int index)
	{
		int num = index;
		foreach (DictionaryEntry item in sl)
		{
			array.SetValue(item.Value, num++);
		}
	}

	public IEnumerator GetEnumerator()
	{
		return new ProtocolPropertyEnumerator(this);
	}

	internal abstract void Add(ProtocolProperty property);

	internal void Remove(string name)
	{
		sl.Remove(name);
	}

	public bool Contains(string name)
	{
		return null != LookupName(name);
	}

	internal void SetAllDirty(bool bVal)
	{
		foreach (DictionaryEntry item in sl)
		{
			ProtocolProperty protocolProperty = item.Value as ProtocolProperty;
			if (null != protocolProperty)
			{
				protocolProperty.SetDirty(bVal);
			}
		}
	}

	internal void SetAllRetrieved(bool bVal)
	{
		foreach (DictionaryEntry item in sl)
		{
			ProtocolProperty protocolProperty = item.Value as ProtocolProperty;
			if (null != protocolProperty)
			{
				protocolProperty.SetRetrieved(bVal);
			}
		}
	}

	internal void Update(string propName, object value)
	{
		ProtocolProperty protocolProperty = this[propName];
		protocolProperty.SetValue(value);
		protocolProperty.SetRetrieved(retrieved: true);
	}

	internal void Clear()
	{
		sl.Clear();
	}

	internal bool ExistAndDirty(string propname)
	{
		ProtocolProperty protocolProperty = this[propname];
		if (protocolProperty.Value == null)
		{
			return false;
		}
		return protocolProperty.Dirty;
	}

	internal object GetIfDirty(string propname)
	{
		ProtocolProperty protocolProperty = this[propname];
		if (!protocolProperty.Dirty)
		{
			return null;
		}
		return protocolProperty.Value;
	}

	internal ProtocolProperty LookupName(string name)
	{
		foreach (DictionaryEntry item in sl)
		{
			ProtocolProperty protocolProperty = item.Value as ProtocolProperty;
			if (protocolProperty.Name == name)
			{
				return protocolProperty;
			}
		}
		return null;
	}
}
