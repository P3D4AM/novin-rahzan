using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiMouseMoveEventConverter))]
public class StiMouseMoveEvent : StiEvent
{
	public override string ToString()
	{
		return "MouseMove";
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

	public StiMouseMoveEvent()
		: this("")
	{
	}

	public StiMouseMoveEvent(string script)
		: base(script)
	{
	}

	public StiMouseMoveEvent(StiComponent parent)
		: base(parent)
	{
	}
}
