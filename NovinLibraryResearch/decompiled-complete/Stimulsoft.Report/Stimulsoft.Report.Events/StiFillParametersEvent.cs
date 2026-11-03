using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiFillParametersEventConverter))]
public class StiFillParametersEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiFillParametersEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiFillParametersEventHandler);
	}

	public override string ToString()
	{
		return "FillParameters";
	}

	public StiFillParametersEvent()
		: this("")
	{
	}

	public StiFillParametersEvent(string script)
		: base(script)
	{
	}

	public StiFillParametersEvent(StiComponent parent)
		: base(parent)
	{
	}
}
