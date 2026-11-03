using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetValueLowEventConverter))]
public class StiGetValueLowEvent : StiEvent
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
		return "GetValueLow";
	}

	public StiGetValueLowEvent()
		: this("")
	{
	}

	public StiGetValueLowEvent(string script)
		: base(script)
	{
	}

	public StiGetValueLowEvent(StiComponent parent)
		: base(parent)
	{
	}
}
