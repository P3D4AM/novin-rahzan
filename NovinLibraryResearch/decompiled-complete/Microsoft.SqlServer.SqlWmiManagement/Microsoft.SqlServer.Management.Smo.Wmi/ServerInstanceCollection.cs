using System;
using System.Collections;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerInstanceCollection : WmiCollectionBase, ICollection, IEnumerable
{
	internal sealed class ServerInstanceCollectionEnumerator : IEnumerator
	{
		internal IDictionaryEnumerator baseEnumerator;

		object IEnumerator.Current => baseEnumerator.Value as ServerInstance;

		public ServerInstance Current => baseEnumerator.Value as ServerInstance;

		internal ServerInstanceCollectionEnumerator(WmiCollectionBase col)
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

	public ServerInstance this[int index] => GetObjectByIndex(index);

	public ServerInstance this[string name] => GetObjectByName(name);

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

	internal ServerInstanceCollection(WmiSmoObject parentInstance)
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

	internal ServerInstance GetObjectByIndex(int index)
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return innerColl.GetByIndex(index) as ServerInstance;
	}

	internal ServerInstance GetObjectByName(string name)
	{
		object obj = innerColl[name];
		if (obj == null && !initialized)
		{
			obj = InitializeChildObject(name);
		}
		return obj as ServerInstance;
	}

	internal object InitializeChildObject(string name)
	{
		ServerInstance serverInstance = new ServerInstance(this, name);
		if (serverInstance.Initialize())
		{
			Add(serverInstance);
			return serverInstance;
		}
		return null;
	}

	internal void InitializeChildCollection()
	{
		SortedList sortedList = innerColl;
		innerColl = new SortedList();
		base.ParentInstance.EnumChildren(typeof(ServerInstance).ToString(), this);
		foreach (DictionaryEntry item in sortedList)
		{
			ServerInstance serverInstance = item.Value as ServerInstance;
			innerColl[serverInstance.Name] = serverInstance;
		}
		initialized = true;
	}

	public void CopyTo(ServerInstance[] array, int index)
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
			array.SetValue((ServerInstance)item.Value, num++);
		}
	}

	public IEnumerator GetEnumerator()
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return new ServerInstanceCollectionEnumerator(this);
	}
}
