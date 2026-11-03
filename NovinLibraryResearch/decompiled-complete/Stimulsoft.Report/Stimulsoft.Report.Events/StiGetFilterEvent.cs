using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetFilterEventConverter))]
public class StiGetFilterEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiFilterEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiFilterEventHandler);
	}

	public override string ToString()
	{
		return "GetFilter";
	}

	public StiGetFilterEvent()
		: this("")
	{
	}

	public StiGetFilterEvent(string script)
		: base(script)
	{
	}

	public StiGetFilterEvent(StiComponent parent)
		: base(parent)
	{
	}
}
