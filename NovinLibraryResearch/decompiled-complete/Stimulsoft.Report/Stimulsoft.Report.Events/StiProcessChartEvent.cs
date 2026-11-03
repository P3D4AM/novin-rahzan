using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiProcessChartEventConverter))]
public class StiProcessChartEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(EventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(EventHandler);
	}

	public override string ToString()
	{
		return "ProcessChart";
	}

	public StiProcessChartEvent()
		: this("")
	{
	}

	public StiProcessChartEvent(string script)
		: base(script)
	{
	}

	public StiProcessChartEvent(StiComponent parent)
		: base(parent)
	{
	}
}
