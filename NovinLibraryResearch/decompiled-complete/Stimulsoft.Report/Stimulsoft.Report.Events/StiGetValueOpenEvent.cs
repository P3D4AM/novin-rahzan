using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetValueOpenEventConverter))]
public class StiGetValueOpenEvent : StiEvent
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
		return "GetValueOpen";
	}

	public StiGetValueOpenEvent()
		: this("")
	{
	}

	public StiGetValueOpenEvent(string script)
		: base(script)
	{
	}

	public StiGetValueOpenEvent(StiComponent parent)
		: base(parent)
	{
	}
}
