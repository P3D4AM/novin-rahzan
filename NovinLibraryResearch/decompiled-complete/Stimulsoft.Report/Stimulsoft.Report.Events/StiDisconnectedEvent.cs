using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiDisconnectedEventConverter))]
public class StiDisconnectedEvent : StiEvent
{
	public override string ToString()
	{
		return "Disconnected";
	}

	public StiDisconnectedEvent()
		: this("")
	{
	}

	public StiDisconnectedEvent(string script)
		: base(script)
	{
	}

	public StiDisconnectedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
