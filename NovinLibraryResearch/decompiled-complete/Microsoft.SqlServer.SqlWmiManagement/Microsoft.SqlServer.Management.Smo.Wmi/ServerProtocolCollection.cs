using System;
using System.Collections;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerProtocolCollection : WmiCollectionBase, ICollection, IEnumerable
{
	internal sealed class ServerProtocolCollectionEnumerator : IEnumerator
	{
		internal IDictionaryEnumerator baseEnumerator;

		object IEnumerator.Current => baseEnumerator.Value as ServerProtocol;

		public ServerProtocol Current => baseEnumerator.Value as ServerProtocol;

		internal ServerProtocolCollectionEnumerator(WmiCollectionBase col)
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

	public ServerProtocol this[int index] => GetObjectByIndex(index);

	public ServerProtocol this[string name] => GetObjectByName(name);

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

	internal ServerProtocolCollection(WmiSmoObject parentInstance)
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

	internal ServerProtocol GetObjectByIndex(int index)
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return innerColl.GetByIndex(index) as ServerProtocol;
	}

	internal ServerProtocol GetObjectByName(string name)
	{
		object obj = innerColl[name];
		if (obj == null && !initialized)
		{
			obj = InitializeChildObject(name);
		}
		return obj as ServerProtocol;
	}

	protected sealed override void Add(WmiSmoObject wmiObj)
	{
		innerColl.Add(wmiObj.Name, wmiObj);
	}

	protected sealed override void Remove(string objname)
	{
		throw new NotImplementedException();
	}

	internal object InitializeChildObject(string name)
	{
		ServerProtocol serverProtocol = new ServerProtocol(this, name);
		if (serverProtocol.Initialize())
		{
			Add(serverProtocol);
			return serverProtocol;
		}
		return null;
	}

	internal void InitializeChildCollection()
	{
		SortedList sortedList = innerColl;
		innerColl = new SortedList();
		base.ParentInstance.EnumChildren(typeof(ServerProtocol).ToString(), this);
		foreach (DictionaryEntry item in sortedList)
		{
			ServerProtocol serverProtocol = item.Value as ServerProtocol;
			innerColl[serverProtocol.Name] = serverProtocol;
		}
		initialized = true;
	}

	public void CopyTo(ServerProtocol[] array, int index)
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
			array.SetValue((ServerProtocol)item.Value, num++);
		}
	}

	public IEnumerator GetEnumerator()
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return new ServerProtocolCollectionEnumerator(this);
	}
}
