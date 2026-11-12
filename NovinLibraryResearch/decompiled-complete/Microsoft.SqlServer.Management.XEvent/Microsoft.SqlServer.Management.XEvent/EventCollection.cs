using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class EventCollection : SfcCollatedDictionaryCollection<Event, Event.Key, Session>
{
	public Event this[string name] => base[new Event.Key(name)];

	internal EventCollection(Session parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new Event.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return Event.GetObjectFactory();
	}

	public void AppendAlterScripts(StringBuilder addScript, StringBuilder dropScript)
	{
		XEUtils.AppendAlterScripts(addScript, dropScript, this, base.Parent);
	}
}
