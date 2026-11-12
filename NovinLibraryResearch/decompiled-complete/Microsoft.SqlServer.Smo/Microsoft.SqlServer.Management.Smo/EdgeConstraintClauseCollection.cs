using System;
using System.Collections;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class EdgeConstraintClauseCollection : SimpleObjectCollectionBase
{
	public SqlSmoObject Parent => base.ParentInstance;

	public EdgeConstraintClause this[int index] => GetObjectByIndex(index) as EdgeConstraintClause;

	public EdgeConstraintClause this[string name] => GetObjectByName(name) as EdgeConstraintClause;

	internal EdgeConstraintClauseCollection(SqlSmoObject parentInstance)
		: base(parentInstance)
	{
	}

	public void CopyTo(EdgeConstraintClause[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	public EdgeConstraintClause ItemById(int id)
	{
		return (EdgeConstraintClause)GetItemById(id);
	}

	protected override Type GetCollectionElementType()
	{
		return typeof(EdgeConstraintClause);
	}

	internal override SqlSmoObject GetCollectionElementInstance(ObjectKeyBase key, SqlSmoState state)
	{
		return new EdgeConstraintClause(this, key, state);
	}

	public void Remove(EdgeConstraintClause edgeconstraintclause)
	{
		if (edgeconstraintclause == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.RemoveCollection, this, new ArgumentNullException("edgeconstraintclause"));
		}
		RemoveObj(edgeconstraintclause, new SimpleObjectKey(edgeconstraintclause.Name));
	}

	public void Remove(string name)
	{
		Remove(new SimpleObjectKey(name));
	}

	public void Add(EdgeConstraintClause edgeconstraintclause)
	{
		AddImpl(edgeconstraintclause);
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
