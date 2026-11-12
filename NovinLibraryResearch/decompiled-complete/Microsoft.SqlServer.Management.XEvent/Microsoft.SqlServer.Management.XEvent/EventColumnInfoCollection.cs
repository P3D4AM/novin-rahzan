using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class EventColumnInfoCollection : SfcCollatedDictionaryCollection<EventColumnInfo, EventColumnInfo.Key, EventInfo>
{
	public EventColumnInfo this[string name] => base[new EventColumnInfo.Key(name)];

	internal EventColumnInfoCollection(EventInfo parent)
		: base(parent, parent.Parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new EventColumnInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return EventColumnInfo.GetObjectFactory();
	}
}
