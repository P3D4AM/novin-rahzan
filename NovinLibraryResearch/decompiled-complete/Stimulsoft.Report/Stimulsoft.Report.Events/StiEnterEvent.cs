using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiEnterEventConverter))]
public class StiEnterEvent : StiEvent
{
	public override string ToString()
	{
		return "Enter";
	}

	public StiEnterEvent()
		: this("")
	{
	}

	public StiEnterEvent(string script)
		: base(script)
	{
	}

	public StiEnterEvent(StiComponent parent)
		: base(parent)
	{
	}
}
