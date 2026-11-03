using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge.Events.Design;

namespace Stimulsoft.Report.Gauge.Events;

[TypeConverter(typeof(StiGetSkipValuesEventConverter))]
public class StiGetSkipValuesEvent : StiEvent
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
		return "GetSkipValues";
	}

	public StiGetSkipValuesEvent()
		: this("")
	{
	}

	public StiGetSkipValuesEvent(string script)
		: base(script)
	{
	}

	public StiGetSkipValuesEvent(StiComponent parent)
		: base(parent)
	{
	}
}
