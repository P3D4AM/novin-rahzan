using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiStateSaveEventConverter))]
public class StiStateSaveEvent : StiEvent
{
	public override string ToString()
	{
		return "StateSave";
	}

	public StiStateSaveEvent()
		: this("")
	{
	}

	public StiStateSaveEvent(string script)
		: base(script)
	{
	}

	public StiStateSaveEvent(StiComponent parent)
		: base(parent)
	{
	}
}
