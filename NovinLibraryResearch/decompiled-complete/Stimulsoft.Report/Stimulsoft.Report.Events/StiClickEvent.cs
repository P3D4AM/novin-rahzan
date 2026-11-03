using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiClickEventConverter))]
public class StiClickEvent : StiEvent
{
	public override string ToString()
	{
		return "Click";
	}

	public StiClickEvent()
		: this("")
	{
	}

	public StiClickEvent(string script)
		: base(script)
	{
	}

	public StiClickEvent(StiComponent parent)
		: base(parent)
	{
	}
}
