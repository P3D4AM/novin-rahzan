using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetCheckedEventConverter))]
public class StiGetCheckedEvent : StiEvent
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
		return "GetChecked";
	}

	public StiGetCheckedEvent()
		: this("")
	{
	}

	public StiGetCheckedEvent(string script)
		: base(script)
	{
	}

	public StiGetCheckedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
