using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiCheckedChangedEventConverter))]
public class StiCheckedChangedEvent : StiEvent
{
	public override string ToString()
	{
		return "CheckedChanged";
	}

	public StiCheckedChangedEvent()
		: this("")
	{
	}

	public StiCheckedChangedEvent(string script)
		: base(script)
	{
	}

	public StiCheckedChangedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
