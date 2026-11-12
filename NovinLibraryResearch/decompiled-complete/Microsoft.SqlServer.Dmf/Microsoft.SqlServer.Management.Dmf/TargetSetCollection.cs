using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class TargetSetCollection : SfcCollatedDictionaryCollection<TargetSet, TargetSet.Key, ObjectSet>
{
	public TargetSet this[string filterTypeSkeleton] => base[new TargetSet.Key(filterTypeSkeleton)];

	public TargetSetCollection(ObjectSet parent)
		: base(parent)
	{
		base.IgnoreCase = true;
	}

	public TargetSetCollection(ObjectSet parent, IComparer<string> customComparer)
		: base(parent, customComparer)
	{
		base.IgnoreCase = true;
	}

	public bool Contains(string filterTypeSkeleton)
	{
		return Contains(new TargetSet.Key(filterTypeSkeleton));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return TargetSet.GetObjectFactory();
	}
}
