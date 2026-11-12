using System;
using System.Collections;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class EdgeConstraintCollection : SimpleObjectCollectionBase
{
	public SqlSmoObject Parent => base.ParentInstance;

	public EdgeConstraint this[int index] => GetObjectByIndex(index) as EdgeConstraint;

	public EdgeConstraint this[string name] => GetObjectByName(name) as EdgeConstraint;

	internal EdgeConstraintCollection(SqlSmoObject parentInstance)
		: base(parentInstance)
	{
	}

	public void CopyTo(EdgeConstraint[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	public EdgeConstraint ItemById(int id)
	{
		return (EdgeConstraint)GetItemById(id);
	}

	protected override Type GetCollectionElementType()
	{
		return typeof(EdgeConstraint);
	}

	internal override SqlSmoObject GetCollectionElementInstance(ObjectKeyBase key, SqlSmoState state)
	{
		return new EdgeConstraint(this, key, state);
	}

	public void Remove(EdgeConstraint edgeconstraint)
	{
		if (edgeconstraint == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.RemoveCollection, this, new ArgumentNullException("edgeconstraint"));
		}
		RemoveObj(edgeconstraint, new SimpleObjectKey(edgeconstraint.Name));
	}

	public void Remove(string name)
	{
		Remove(new SimpleObjectKey(name));
	}

	public void Add(EdgeConstraint edgeconstraint)
	{
		AddImpl(edgeconstraint);
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
