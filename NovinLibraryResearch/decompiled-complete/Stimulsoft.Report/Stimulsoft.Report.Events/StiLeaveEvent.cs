using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiLeaveEventConverter))]
public class StiLeaveEvent : StiEvent
{
	public override string ToString()
	{
		return "Leave";
	}

	public StiLeaveEvent()
		: this("")
	{
	}

	public StiLeaveEvent(string script)
		: base(script)
	{
	}

	public StiLeaveEvent(StiComponent parent)
		: base(parent)
	{
	}
}
