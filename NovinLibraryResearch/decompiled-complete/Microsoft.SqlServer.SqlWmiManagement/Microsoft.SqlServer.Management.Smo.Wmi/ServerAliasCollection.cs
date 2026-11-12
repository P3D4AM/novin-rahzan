using System;
using System.Collections;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServerAliasCollection : WmiCollectionBase, ICollection, IEnumerable
{
	internal sealed class ServerAliasCollectionEnumerator : IEnumerator
	{
		internal IDictionaryEnumerator baseEnumerator;

		object IEnumerator.Current => baseEnumerator.Value as ServerAlias;

		public ServerAlias Current => baseEnumerator.Value as ServerAlias;

		internal ServerAliasCollectionEnumerator(WmiCollectionBase col)
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

	public ServerAlias this[int index] => GetObjectByIndex(index);

	public ServerAlias this[string name] => GetObjectByName(name);

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

	internal ServerAliasCollection(WmiSmoObject parentInstance)
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

	internal ServerAlias GetObjectByIndex(int index)
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return innerColl.GetByIndex(index) as ServerAlias;
	}

	internal ServerAlias GetObjectByName(string name)
	{
		object obj = innerColl[name];
		if (obj == null && !initialized)
		{
			obj = InitializeChildObject(name);
		}
		return obj as ServerAlias;
	}

	internal object InitializeChildObject(string name)
	{
		ServerAlias serverAlias = new ServerAlias(this, name);
		if (serverAlias.Initialize())
		{
			Add(serverAlias);
			return serverAlias;
		}
		return null;
	}

	internal void InitializeChildCollection()
	{
		SortedList sortedList = innerColl;
		innerColl = new SortedList();
		base.ParentInstance.EnumChildren(typeof(ServerAlias).ToString(), this);
		foreach (DictionaryEntry item in sortedList)
		{
			ServerAlias serverAlias = item.Value as ServerAlias;
			innerColl[serverAlias.Name] = serverAlias;
		}
		initialized = true;
	}

	public void CopyTo(ServerAlias[] array, int index)
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
			array.SetValue((ServerAlias)item.Value, num++);
		}
	}

	public IEnumerator GetEnumerator()
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return new ServerAliasCollectionEnumerator(this);
	}
}
