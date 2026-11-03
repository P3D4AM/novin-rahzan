using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiColumnBeginRenderEventConverter))]
public class StiColumnBeginRenderEvent : StiEvent
{
	public override string ToString()
	{
		return "ColumnBeginRender";
	}

	public StiColumnBeginRenderEvent()
		: this("")
	{
	}

	public StiColumnBeginRenderEvent(string script)
		: base(script)
	{
	}

	public StiColumnBeginRenderEvent(StiComponent parent)
		: base(parent)
	{
	}
}
