using System;
using System.Collections;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class WorkloadManagementWorkloadGroupCollection : SimpleObjectCollectionBase
{
	public Database Parent => base.ParentInstance as Database;

	public WorkloadManagementWorkloadGroup this[int index] => GetObjectByIndex(index) as WorkloadManagementWorkloadGroup;

	public WorkloadManagementWorkloadGroup this[string name] => GetObjectByName(name) as WorkloadManagementWorkloadGroup;

	internal WorkloadManagementWorkloadGroupCollection(SqlSmoObject parentInstance)
		: base(parentInstance)
	{
	}

	public void CopyTo(WorkloadManagementWorkloadGroup[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	public WorkloadManagementWorkloadGroup ItemById(int id)
	{
		return (WorkloadManagementWorkloadGroup)GetItemById(id);
	}

	protected override Type GetCollectionElementType()
	{
		return typeof(WorkloadManagementWorkloadGroup);
	}

	internal override SqlSmoObject GetCollectionElementInstance(ObjectKeyBase key, SqlSmoState state)
	{
		return new WorkloadManagementWorkloadGroup(this, key, state);
	}

	public void Add(WorkloadManagementWorkloadGroup WorkloadManagementWorkloadGroup)
	{
		AddImpl(WorkloadManagementWorkloadGroup);
	}

	internal SqlSmoObject GetObjectByName(string name)
	{
		return GetObjectByKey(new SimpleObjectKey(name));
	}

	internal override ObjectKeyBase CreateKeyFromUrn(Urn urn)
	{
		string attribute = urn.GetAttribute("Name");
		if (attribute == null || attribute.Length == 0)
		{
			throw new SmoException(ExceptionTemplatesImpl.PropertyMustBeSpecifiedInUrn("Name", urn.Type));
		}
		return new SimpleObjectKey(attribute);
	}
}
