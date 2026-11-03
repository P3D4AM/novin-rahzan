using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiConnectingEventConverter))]
public class StiConnectingEvent : StiEvent
{
	public override string ToString()
	{
		return "Connecting";
	}

	public StiConnectingEvent()
		: this("")
	{
	}

	public StiConnectingEvent(string script)
		: base(script)
	{
	}

	public StiConnectingEvent(StiComponent parent)
		: base(parent)
	{
	}
}
