using System;
using System.Collections;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class WorkloadManagementWorkloadClassifierCollection : SimpleObjectCollectionBase
{
	public Database Parent => base.ParentInstance as Database;

	public WorkloadManagementWorkloadClassifier this[int index] => GetObjectByIndex(index) as WorkloadManagementWorkloadClassifier;

	public WorkloadManagementWorkloadClassifier this[string name] => GetObjectByName(name) as WorkloadManagementWorkloadClassifier;

	internal WorkloadManagementWorkloadClassifierCollection(SqlSmoObject parentInstance)
		: base(parentInstance)
	{
	}

	public void CopyTo(WorkloadManagementWorkloadClassifier[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	public WorkloadManagementWorkloadClassifier ItemById(int id)
	{
		return (WorkloadManagementWorkloadClassifier)GetItemById(id);
	}

	protected override Type GetCollectionElementType()
	{
		return typeof(WorkloadManagementWorkloadClassifier);
	}

	internal override SqlSmoObject GetCollectionElementInstance(ObjectKeyBase key, SqlSmoState state)
	{
		return new WorkloadManagementWorkloadClassifier(this, key, state);
	}

	public void Add(WorkloadManagementWorkloadClassifier WorkloadManagementWorkloadClassifier)
	{
		AddImpl(WorkloadManagementWorkloadClassifier);
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
