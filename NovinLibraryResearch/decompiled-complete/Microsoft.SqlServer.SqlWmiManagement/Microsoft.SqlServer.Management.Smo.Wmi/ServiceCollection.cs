using System;
using System.Collections;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class ServiceCollection : WmiCollectionBase, ICollection, IEnumerable
{
	internal sealed class ServiceCollectionEnumerator : IEnumerator
	{
		internal IDictionaryEnumerator baseEnumerator;

		object IEnumerator.Current => baseEnumerator.Value as Service;

		public Service Current => baseEnumerator.Value as Service;

		internal ServiceCollectionEnumerator(WmiCollectionBase col)
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

	public Service this[int index] => GetObjectByIndex(index);

	public Service this[string name] => GetObjectByName(name);

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

	internal ServiceCollection(WmiSmoObject parentInstance)
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

	internal Service GetObjectByIndex(int index)
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return innerColl.GetByIndex(index) as Service;
	}

	internal Service GetObjectByName(string name)
	{
		object obj = innerColl[name];
		if (obj == null && !initialized)
		{
			obj = InitializeChildObject(name);
		}
		return obj as Service;
	}

	internal object InitializeChildObject(string name)
	{
		Service service = new Service(this, name);
		if (service.Initialize())
		{
			Add(service);
			return service;
		}
		return null;
	}

	internal void InitializeChildCollection()
	{
		SortedList sortedList = innerColl;
		innerColl = new SortedList();
		base.ParentInstance.EnumChildren(typeof(Service).ToString(), this);
		foreach (DictionaryEntry item in sortedList)
		{
			Service service = item.Value as Service;
			innerColl[service.Name] = service;
		}
		initialized = true;
	}

	public void CopyTo(Service[] array, int index)
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
			array.SetValue((Service)item.Value, num++);
		}
	}

	public IEnumerator GetEnumerator()
	{
		if (!initialized)
		{
			InitializeChildCollection();
		}
		return new ServiceCollectionEnumerator(this);
	}
}
