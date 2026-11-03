using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiClosingFormEventConverter))]
public class StiClosingFormEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(CancelEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(CancelEventHandler);
	}

	public override string ToString()
	{
		return "ClosingForm";
	}

	public StiClosingFormEvent()
		: this("")
	{
	}

	public StiClosingFormEvent(string script)
		: base(script)
	{
	}

	public StiClosingFormEvent(StiComponent parent)
		: base(parent)
	{
	}
}
