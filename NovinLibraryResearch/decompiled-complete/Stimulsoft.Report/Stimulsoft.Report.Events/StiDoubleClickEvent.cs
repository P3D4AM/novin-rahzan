using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiDoubleClickEventConverter))]
public class StiDoubleClickEvent : StiEvent
{
	public override string ToString()
	{
		return "DoubleClick";
	}

	public StiDoubleClickEvent()
		: this("")
	{
	}

	public StiDoubleClickEvent(string script)
		: base(script)
	{
	}

	public StiDoubleClickEvent(StiComponent parent)
		: base(parent)
	{
	}
}
