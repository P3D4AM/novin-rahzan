using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class EventInfoCollection : SfcCollatedDictionaryCollection<EventInfo, EventInfo.Key, Package>, IXEObjectInfoCollection<EventInfo>
{
	public EventInfo this[string name] => base[new EventInfo.Key(name)];

	internal EventInfoCollection(Package parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new EventInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return EventInfo.GetObjectFactory();
	}
}
