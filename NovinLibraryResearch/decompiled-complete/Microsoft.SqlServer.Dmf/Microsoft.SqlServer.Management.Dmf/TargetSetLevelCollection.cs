using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class TargetSetLevelCollection : SfcCollatedDictionaryCollection<TargetSetLevel, TargetSetLevel.Key, TargetSet>
{
	public TargetSetLevel this[string filterTypeSkeleton] => base[new TargetSetLevel.Key(filterTypeSkeleton)];

	public TargetSetLevelCollection(TargetSet parent)
		: base(parent)
	{
		base.IgnoreCase = true;
	}

	public TargetSetLevelCollection(TargetSet parent, IComparer<string> customComparer)
		: base(parent, customComparer)
	{
		base.IgnoreCase = true;
	}

	public bool Contains(string filterTypeSkeleton)
	{
		return Contains(new TargetSetLevel.Key(filterTypeSkeleton));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return TargetSetLevel.GetObjectFactory();
	}
}
