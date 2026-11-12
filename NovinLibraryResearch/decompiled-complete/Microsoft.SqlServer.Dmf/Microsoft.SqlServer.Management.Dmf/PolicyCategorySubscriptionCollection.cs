using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyCategorySubscriptionCollection : SfcDictionaryCollection<PolicyCategorySubscription, PolicyCategorySubscription.Key, PolicyStore>
{
	public PolicyCategorySubscription this[int id] => base[new PolicyCategorySubscription.Key(id)];

	public PolicyCategorySubscriptionCollection(PolicyStore parent)
		: base(parent)
	{
	}

	public bool Contains(int id)
	{
		return Contains(new PolicyCategorySubscription.Key(id));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return PolicyCategorySubscription.GetObjectFactory();
	}

	internal IEnumerable<string> EnumCategorySubscribers(string category, string targetType)
	{
		using IEnumerator<PolicyCategorySubscription> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			PolicyCategorySubscription current = enumerator.Current;
			if (current.PolicyCategory == category && current.TargetType == targetType)
			{
				yield return current.Target;
			}
		}
	}
}
