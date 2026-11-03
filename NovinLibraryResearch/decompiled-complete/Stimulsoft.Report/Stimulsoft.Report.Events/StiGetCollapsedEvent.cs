using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetCollapsedEventConverter))]
public class StiGetCollapsedEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiValueEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiValueEventHandler);
	}

	public override string ToString()
	{
		return "GetCollapsed";
	}

	public StiGetCollapsedEvent()
		: this("")
	{
	}

	public StiGetCollapsedEvent(string script)
		: base(script)
	{
	}

	public StiGetCollapsedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
