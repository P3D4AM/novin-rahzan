using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiMouseDownEventConverter))]
public class StiMouseDownEvent : StiEvent
{
	public override string ToString()
	{
		return "MouseDown";
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

	public StiMouseDownEvent()
		: this("")
	{
	}

	public StiMouseDownEvent(string script)
		: base(script)
	{
	}

	public StiMouseDownEvent(StiComponent parent)
		: base(parent)
	{
	}
}
