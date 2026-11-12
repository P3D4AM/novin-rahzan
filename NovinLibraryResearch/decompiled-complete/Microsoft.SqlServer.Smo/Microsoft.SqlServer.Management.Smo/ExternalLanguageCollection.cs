using System;
using System.Collections;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class ExternalLanguageCollection : SimpleObjectCollectionBase
{
	public Database Parent => base.ParentInstance as Database;

	public ExternalLanguage this[int index] => GetObjectByIndex(index) as ExternalLanguage;

	public ExternalLanguage this[string name] => GetObjectByName(name) as ExternalLanguage;

	internal ExternalLanguageCollection(SqlSmoObject parentInstance)
		: base(parentInstance)
	{
	}

	public void CopyTo(ExternalLanguage[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	public ExternalLanguage ItemById(int id)
	{
		return (ExternalLanguage)GetItemById(id);
	}

	protected override Type GetCollectionElementType()
	{
		return typeof(ExternalLanguage);
	}

	internal override SqlSmoObject GetCollectionElementInstance(ObjectKeyBase key, SqlSmoState state)
	{
		return new ExternalLanguage(this, key, state);
	}

	public void Remove(ExternalLanguage externalLanguage)
	{
		if (externalLanguage == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.RemoveCollection, this, new ArgumentNullException("externalLanguage"));
		}
		RemoveObj(externalLanguage, new SimpleObjectKey(externalLanguage.Name));
	}

	public void Remove(string name)
	{
		Remove(new SimpleObjectKey(name));
	}

	public void Add(ExternalLanguage externalLanguage)
	{
		AddImpl(externalLanguage);
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
