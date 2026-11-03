using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetValueEndEventConverter))]
public class StiGetValueEndEvent : StiEvent
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
		return "GetValueEnd";
	}

	public StiGetValueEndEvent()
		: this("")
	{
	}

	public StiGetValueEndEvent(string script)
		: base(script)
	{
	}

	public StiGetValueEndEvent(StiComponent parent)
		: base(parent)
	{
	}
}
