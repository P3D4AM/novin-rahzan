using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiSelectedIndexChangedEventConverter))]
public class StiSelectedIndexChangedEvent : StiEvent
{
	public override string ToString()
	{
		return "SelectedIndexChanged";
	}

	public StiSelectedIndexChangedEvent()
		: this("")
	{
	}

	public StiSelectedIndexChangedEvent(string script)
		: base(script)
	{
	}

	public StiSelectedIndexChangedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
