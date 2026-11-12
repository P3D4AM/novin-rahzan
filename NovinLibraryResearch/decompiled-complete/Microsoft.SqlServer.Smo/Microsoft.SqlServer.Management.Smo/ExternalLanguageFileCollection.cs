using System;
using System.Collections;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class ExternalLanguageFileCollection : SimpleObjectCollectionBase
{
	public ExternalLanguage Parent => base.ParentInstance as ExternalLanguage;

	public ExternalLanguageFile this[int index] => GetObjectByIndex(index) as ExternalLanguageFile;

	public ExternalLanguageFile this[string name] => GetObjectByName(name) as ExternalLanguageFile;

	internal ExternalLanguageFileCollection(SqlSmoObject parentInstance)
		: base(parentInstance)
	{
	}

	public void CopyTo(ExternalLanguageFile[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	public ExternalLanguageFile ItemById(int id)
	{
		return (ExternalLanguageFile)GetItemById(id);
	}

	protected override Type GetCollectionElementType()
	{
		return typeof(ExternalLanguageFile);
	}

	internal override SqlSmoObject GetCollectionElementInstance(ObjectKeyBase key, SqlSmoState state)
	{
		return new ExternalLanguageFile(this, key, state);
	}

	public void Remove(ExternalLanguageFile externalLanguageFile)
	{
		if (externalLanguageFile == null)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.RemoveCollection, this, new ArgumentNullException("externalLanguageFile"));
		}
		RemoveObj(externalLanguageFile, new SimpleObjectKey(externalLanguageFile.Name));
	}

	public void Remove(string name)
	{
		Remove(new SimpleObjectKey(name));
	}

	public void Add(ExternalLanguageFile externalLanguageFile)
	{
		AddImpl(externalLanguageFile);
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
