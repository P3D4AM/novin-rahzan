using System.Collections;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public abstract class WmiCollectionBase
{
	private WmiSmoObject parentInstance;

	protected internal SortedList innerColl;

	protected internal bool initialized;

	public bool IsSynchronized => innerColl.IsSynchronized;

	public object SyncRoot => innerColl.SyncRoot;

	internal WmiSmoObject ParentInstance => parentInstance;

	internal WmiCollectionBase(WmiSmoObject parentInstance)
	{
		this.parentInstance = parentInstance;
		initialized = false;
		innerColl = new SortedList(new CaseInsensitiveComparer(SmoApplication.DefaultCulture));
	}

	protected virtual void Add(WmiSmoObject wmiObj)
	{
		innerColl.Add(wmiObj.Name, wmiObj);
	}

	internal void AddInternal(WmiSmoObject wmiObj)
	{
		Add(wmiObj);
	}

	protected virtual void Remove(string objname)
	{
		innerColl.Remove(objname);
	}

	internal void RemoveInternal(string key)
	{
		Remove(key);
	}
}
