using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiDisconnectingEventConverter))]
public class StiDisconnectingEvent : StiEvent
{
	public override string ToString()
	{
		return "Disconnecting";
	}

	public StiDisconnectingEvent()
		: this("")
	{
	}

	public StiDisconnectingEvent(string script)
		: base(script)
	{
	}

	public StiDisconnectingEvent(StiComponent parent)
		: base(parent)
	{
	}
}
