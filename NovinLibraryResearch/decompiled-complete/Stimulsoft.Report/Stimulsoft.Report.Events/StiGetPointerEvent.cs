using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetPointerEventConverter))]
public class StiGetPointerEvent : StiEvent
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
		return "GetPointer";
	}

	public StiGetPointerEvent()
		: this("")
	{
	}

	public StiGetPointerEvent(string script)
		: base(script)
	{
	}

	public StiGetPointerEvent(StiComponent parent)
		: base(parent)
	{
	}
}
