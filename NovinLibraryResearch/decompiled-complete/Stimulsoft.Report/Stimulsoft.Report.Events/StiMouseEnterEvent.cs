using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiMouseEnterEventConverter))]
public class StiMouseEnterEvent : StiEvent
{
	public override string ToString()
	{
		return "MouseEnter";
	}

	public StiMouseEnterEvent()
		: this("")
	{
	}

	public StiMouseEnterEvent(string script)
		: base(script)
	{
	}

	public StiMouseEnterEvent(StiComponent parent)
		: base(parent)
	{
	}
}
