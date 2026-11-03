using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiRenderingEventConverter))]
public class StiRenderingEvent : StiEvent
{
	public override string ToString()
	{
		return "Rendering";
	}

	public StiRenderingEvent()
		: this("")
	{
	}

	public StiRenderingEvent(string script)
		: base(script)
	{
	}

	public StiRenderingEvent(StiComponent parent)
		: base(parent)
	{
	}
}
