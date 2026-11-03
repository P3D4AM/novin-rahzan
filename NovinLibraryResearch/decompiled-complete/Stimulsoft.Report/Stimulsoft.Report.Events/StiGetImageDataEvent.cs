using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetImageDataEventConverter))]
public class StiGetImageDataEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetImageDataEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetImageDataEventHandler);
	}

	public override string ToString()
	{
		return "GetImageData";
	}

	public StiGetImageDataEvent()
		: this("")
	{
	}

	public StiGetImageDataEvent(string script)
		: base(script)
	{
	}

	public StiGetImageDataEvent(StiComponent parent)
		: base(parent)
	{
	}
}
