using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiBeginRenderEventConverter))]
public class StiBeginRenderEvent : StiEvent
{
	public override string ToString()
	{
		return "BeginRender";
	}

	public StiBeginRenderEvent()
		: this("")
	{
	}

	public StiBeginRenderEvent(string script)
		: base(script)
	{
	}

	public StiBeginRenderEvent(StiComponent parent)
		: base(parent)
	{
	}
}
