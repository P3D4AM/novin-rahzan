using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge.Events.Design;

namespace Stimulsoft.Report.Gauge.Events;

[TypeConverter(typeof(StiGetSkipIndicesEventConverter))]
public class StiGetSkipIndicesEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetValueEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetValueEventHandler);
	}

	public override string ToString()
	{
		return "GetSkipIndices";
	}

	public StiGetSkipIndicesEvent()
		: this("")
	{
	}

	public StiGetSkipIndicesEvent(string script)
		: base(script)
	{
	}

	public StiGetSkipIndicesEvent(StiComponent parent)
		: base(parent)
	{
	}
}
