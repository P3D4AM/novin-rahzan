using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge.Events.Design;

namespace Stimulsoft.Report.Gauge.Events;

[TypeConverter(typeof(StiGetTextEventConverter))]
public class StiGetTextEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetTextEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetTextEventHandler);
	}

	public override string ToString()
	{
		return "GetText";
	}

	public StiGetTextEvent()
		: this("")
	{
	}

	public StiGetTextEvent(string script)
		: base(script)
	{
	}
}
