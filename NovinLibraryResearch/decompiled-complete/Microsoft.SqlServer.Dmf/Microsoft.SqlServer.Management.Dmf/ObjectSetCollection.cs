using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ObjectSetCollection : SfcDictionaryCollection<ObjectSet, ObjectSet.Key, PolicyStore>
{
	public ObjectSet this[string name] => base[new ObjectSet.Key(name)];

	public ObjectSetCollection(PolicyStore parent)
		: base(parent)
	{
	}

	public bool Contains(string name)
	{
		return Contains(new ObjectSet.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return ObjectSet.GetObjectFactory();
	}
}
