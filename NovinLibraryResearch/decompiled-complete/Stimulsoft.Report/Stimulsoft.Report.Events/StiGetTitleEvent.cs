using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetTitleEventConverter))]
public class StiGetTitleEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetTitleEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetTitleEventHandler);
	}

	public override string ToString()
	{
		return "GetTitle";
	}

	public StiGetTitleEvent()
		: this("")
	{
	}

	public StiGetTitleEvent(string script)
		: base(script)
	{
	}

	public StiGetTitleEvent(StiComponent parent)
		: base(parent)
	{
	}
}
