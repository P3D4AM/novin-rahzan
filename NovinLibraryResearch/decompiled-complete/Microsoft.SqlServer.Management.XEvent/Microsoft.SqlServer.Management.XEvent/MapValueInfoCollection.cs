using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class MapValueInfoCollection : SfcCollatedDictionaryCollection<MapValueInfo, MapValueInfo.Key, MapInfo>
{
	public MapValueInfo this[string name] => base[new MapValueInfo.Key(name)];

	internal MapValueInfoCollection(MapInfo parent)
		: base(parent, parent.Parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new MapValueInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return MapValueInfo.GetObjectFactory();
	}
}
