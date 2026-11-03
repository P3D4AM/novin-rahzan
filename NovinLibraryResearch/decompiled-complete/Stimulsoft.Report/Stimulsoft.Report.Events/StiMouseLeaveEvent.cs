using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiMouseLeaveEventConverter))]
public class StiMouseLeaveEvent : StiEvent
{
	public override string ToString()
	{
		return "MouseLeave";
	}

	public StiMouseLeaveEvent()
		: this("")
	{
	}

	public StiMouseLeaveEvent(string script)
		: base(script)
	{
	}

	public StiMouseLeaveEvent(StiComponent parent)
		: base(parent)
	{
	}
}
