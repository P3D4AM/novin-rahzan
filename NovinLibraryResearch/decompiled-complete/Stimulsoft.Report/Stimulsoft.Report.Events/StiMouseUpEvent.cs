using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiMouseUpEventConverter))]
public class StiMouseUpEvent : StiEvent
{
	public override string ToString()
	{
		return "MouseUp";
	}

	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(MouseEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(MouseEventHandler);
	}

	public StiMouseUpEvent()
		: this("")
	{
	}

	public StiMouseUpEvent(string script)
		: base(script)
	{
	}

	public StiMouseUpEvent(StiComponent parent)
		: base(parent)
	{
	}
}
