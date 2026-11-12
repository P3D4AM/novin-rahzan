using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class EventFieldCollection : SfcCollatedDictionaryCollection<EventField, EventField.Key, Event>
{
	public EventField this[string name] => base[new EventField.Key(name)];

	internal EventFieldCollection(Event parent)
		: base(parent, parent.Parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new EventField.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return EventField.GetObjectFactory();
	}
}
