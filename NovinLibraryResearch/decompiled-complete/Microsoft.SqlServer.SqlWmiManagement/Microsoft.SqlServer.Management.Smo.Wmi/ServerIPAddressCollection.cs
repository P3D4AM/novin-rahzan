using System;
using System.Collections;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerIPAddressCollection : WmiCollectionBase, ICollection, IEnumerable
{
	internal sealed class ServerIPAddressCollectionEnumerator : IEnumerator
	{
		internal IDictionaryEnumerator baseEnumerator;

		object IEnumerator.Current => baseEnumerator.Value as ServerIPAddress;

		public ServerIPAddress Current => baseEnumerator.Value as ServerIPAddress;

		internal ServerIPAddressCollectionEnumerator(WmiCollectionBase col)
		{
			baseEnumerator = col.innerColl.GetEnumerator();
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

	public ServerIPAddress this[int index] => GetObjectByIndex(index);

	public ServerIPAddress this[string name] => GetObjectByName(name);

	public int Count
	{
		get
		{
			if (!initialized)
			{
				InitializeChildCollection();
			}
			return innerColl.Count;
		}
	}

	internal ServerIPAddressCollection(WmiSmoObject parentInstance)
		: base(parentInstance)
	{
	}

	public bool Contains(string key)
	{
		if (innerColl.ContainsKey(key))
		{
			return true;
		}
		if (!initialized && InitializeChildObject(key) != null)
		{
			return true;
		}
		return false;
	}

	internal ServerIPAddress GetObjectByIndex(int index)
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return innerColl.GetByIndex(index) as ServerIPAddress;
	}

	internal ServerIPAddress GetObjectByName(string name)
	{
		object obj = innerColl[name];
		if (obj == null && !initialized)
		{
			obj = InitializeChildObject(name);
		}
		return obj as ServerIPAddress;
	}

	internal object InitializeChildObject(string name)
	{
		ServerIPAddress serverIPAddress = new ServerIPAddress(this, name);
		if (serverIPAddress.Initialize())
		{
			Add(serverIPAddress);
			return serverIPAddress;
		}
		return null;
	}

	internal void InitializeChildCollection()
	{
		SortedList sortedList = innerColl;
		innerColl = new SortedList();
		base.ParentInstance.EnumChildren(typeof(ServerIPAddress).ToString(), this);
		foreach (DictionaryEntry item in sortedList)
		{
			ServerIPAddress serverIPAddress = item.Value as ServerIPAddress;
			innerColl[serverIPAddress.Name] = serverIPAddress;
		}
		initialized = true;
	}

	public void CopyTo(ServerIPAddress[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	void ICollection.CopyTo(Array array, int index)
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		int num = index;
		foreach (DictionaryEntry item in innerColl)
		{
			array.SetValue((ServerIPAddress)item.Value, num++);
		}
	}

	public IEnumerator GetEnumerator()
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return new ServerIPAddressCollectionEnumerator(this);
	}
}
