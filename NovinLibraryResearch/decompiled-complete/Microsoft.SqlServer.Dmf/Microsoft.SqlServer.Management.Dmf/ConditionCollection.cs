using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ConditionCollection : SfcDictionaryCollection<Condition, Condition.Key, PolicyStore>
{
	public Condition this[string name] => base[new Condition.Key(name)];

	public ConditionCollection(PolicyStore parent)
		: base(parent)
	{
	}

	public bool Contains(string name)
	{
		return Contains(new Condition.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return Condition.GetObjectFactory();
	}
}
