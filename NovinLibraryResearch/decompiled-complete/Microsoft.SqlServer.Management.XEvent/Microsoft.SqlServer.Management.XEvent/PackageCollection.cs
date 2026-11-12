using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class PackageCollection : SfcCollatedDictionaryCollection<Package, Package.Key, BaseXEStore>
{
	private const string Pattern = "\\[(.+)\\]\\.(.+)";

	public Package this[Guid moduleID, string name] => base[new Package.Key(moduleID.ToString(), name)];

	public Package this[Guid packageID]
	{
		get
		{
			using (IEnumerator<Package> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Package current = enumerator.Current;
					if (current.ID.Equals(packageID))
					{
						return current;
					}
				}
			}
			return null;
		}
	}

	public Package this[string name]
	{
		get
		{
			Package package = null;
			using IEnumerator<Package> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				Package current = enumerator.Current;
				if (base.Parent.GetComparer().Compare(name, current.Name) == 0)
				{
					if (package != null)
					{
						throw new XEventException(ExceptionTemplates.PackageNameNotUnique(name));
					}
					package = current;
				}
			}
			return package;
		}
	}

	internal PackageCollection(BaseXEStore parent)
		: base(parent, parent.GetComparer())
	{
	}

	public bool Contains(Guid moduleID, string name)
	{
		return Contains(new Package.Key(moduleID.ToString(), name));
	}

	public bool Contains(string name)
	{
		return GetPackages(name).Count > 0;
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return Package.GetObjectFactory();
	}

	internal ICollection<Package> GetPackages(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return this;
		}
		List<Package> list = new List<Package>();
		using IEnumerator<Package> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			Package current = enumerator.Current;
			if (base.Parent.GetComparer().Compare(name, current.Name) == 0)
			{
				list.Add(current);
			}
		}
		return list;
	}
}
