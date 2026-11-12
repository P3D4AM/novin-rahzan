using System;
using System.Collections;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ClientProtocolCollection : WmiCollectionBase, ICollection, IEnumerable
{
	internal sealed class ClientProtocolCollectionEnumerator : IEnumerator
	{
		internal IDictionaryEnumerator baseEnumerator;

		object IEnumerator.Current => baseEnumerator.Value as ClientProtocol;

		public ClientProtocol Current => baseEnumerator.Value as ClientProtocol;

		internal ClientProtocolCollectionEnumerator(WmiCollectionBase col)
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

	public ClientProtocol this[int index] => GetObjectByIndex(index);

	public ClientProtocol this[string name] => GetObjectByName(name);

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

	internal ClientProtocolCollection(WmiSmoObject parentInstance)
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

	internal ClientProtocol GetObjectByIndex(int index)
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return innerColl.GetByIndex(index) as ClientProtocol;
	}

	internal ClientProtocol GetObjectByName(string name)
	{
		object obj = innerColl[name];
		if (obj == null && !initialized)
		{
			obj = InitializeChildObject(name);
		}
		return obj as ClientProtocol;
	}

	internal object InitializeChildObject(string name)
	{
		ClientProtocol clientProtocol = new ClientProtocol(this, name);
		if (clientProtocol.Initialize())
		{
			Add(clientProtocol);
			return clientProtocol;
		}
		return null;
	}

	internal void InitializeChildCollection()
	{
		SortedList sortedList = innerColl;
		innerColl = new SortedList();
		base.ParentInstance.EnumChildren(typeof(ClientProtocol).ToString(), this);
		foreach (DictionaryEntry item in sortedList)
		{
			ClientProtocol clientProtocol = item.Value as ClientProtocol;
			innerColl[clientProtocol.Name] = clientProtocol;
		}
		initialized = true;
	}

	public void CopyTo(ClientProtocol[] array, int index)
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
			array.SetValue((ClientProtocol)item.Value, num++);
		}
	}

	public IEnumerator GetEnumerator()
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return new ClientProtocolCollectionEnumerator(this);
	}
}
