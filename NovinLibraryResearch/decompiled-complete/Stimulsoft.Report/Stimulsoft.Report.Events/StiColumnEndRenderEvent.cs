using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiColumnEndRenderEventConverter))]
public class StiColumnEndRenderEvent : StiEvent
{
	public override string ToString()
	{
		return "ColumnEndRender";
	}

	public StiColumnEndRenderEvent()
		: this("")
	{
	}

	public StiColumnEndRenderEvent(string script)
		: base(script)
	{
	}

	public StiColumnEndRenderEvent(StiComponent parent)
		: base(parent)
	{
	}
}
