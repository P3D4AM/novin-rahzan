using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyCollection : SfcCollatedDictionaryCollection<Policy, Policy.Key, PolicyStore>
{
	public Policy this[string name] => base[new Policy.Key(name)];

	public PolicyCollection(PolicyStore parent)
		: base(parent)
	{
	}

	public PolicyCollection(PolicyStore parent, IComparer<string> customComparer)
		: base(parent, customComparer)
	{
	}

	public bool Contains(string name)
	{
		return Contains(new Policy.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return Policy.GetObjectFactory();
	}
}
