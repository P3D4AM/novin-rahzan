using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class MapInfoCollection : SfcCollatedDictionaryCollection<MapInfo, MapInfo.Key, Package>, IXEObjectInfoCollection<MapInfo>
{
	public MapInfo this[string name] => base[new MapInfo.Key(name)];

	internal MapInfoCollection(Package parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new MapInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return MapInfo.GetObjectFactory();
	}
}
