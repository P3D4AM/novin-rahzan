using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyCategoryCollection : SfcCollatedDictionaryCollection<PolicyCategory, PolicyCategory.Key, PolicyStore>
{
	public PolicyCategory this[string name] => base[new PolicyCategory.Key(name)];

	public PolicyCategoryCollection(PolicyStore parent)
		: base(parent)
	{
	}

	public PolicyCategoryCollection(PolicyStore parent, IComparer<string> customComparer)
		: base(parent, customComparer)
	{
	}

	public bool Contains(string name)
	{
		return Contains(new PolicyCategory.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return PolicyCategory.GetObjectFactory();
	}
}
