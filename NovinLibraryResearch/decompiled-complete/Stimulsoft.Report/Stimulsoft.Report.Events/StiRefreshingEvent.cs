using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiRefreshingEventConverter))]
public class StiRefreshingEvent : StiEvent
{
	public override string ToString()
	{
		return "Refreshing";
	}

	public StiRefreshingEvent()
		: this("")
	{
	}

	public StiRefreshingEvent(string script)
		: base(script)
	{
	}

	public StiRefreshingEvent(StiComponent parent)
		: base(parent)
	{
	}
}
