using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiConnectedEventConverter))]
public class StiConnectedEvent : StiEvent
{
	public override string ToString()
	{
		return "Connected";
	}

	public StiConnectedEvent()
		: this("")
	{
	}

	public StiConnectedEvent(string script)
		: base(script)
	{
	}

	public StiConnectedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
