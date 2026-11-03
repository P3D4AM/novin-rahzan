using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiEndRenderEventConverter))]
public class StiEndRenderEvent : StiEvent
{
	public override string ToString()
	{
		return "EndRender";
	}

	public StiEndRenderEvent()
		: this("")
	{
	}

	public StiEndRenderEvent(string script)
		: base(script)
	{
	}

	public StiEndRenderEvent(StiComponent parent)
		: base(parent)
	{
	}
}
