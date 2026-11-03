using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetDataUrlEventConverter))]
public class StiGetDataUrlEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetDataUrlEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetDataUrlEventHandler);
	}

	public override string ToString()
	{
		return "GetDataUrl";
	}

	public StiGetDataUrlEvent()
		: this("")
	{
	}

	public StiGetDataUrlEvent(string script)
		: base(script)
	{
	}

	public StiGetDataUrlEvent(StiComponent parent)
		: base(parent)
	{
	}
}
