using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiStateRestoreEventConverter))]
public class StiStateRestoreEvent : StiEvent
{
	public override string ToString()
	{
		return "StateRestore";
	}

	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(EventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(EventHandler);
	}

	public StiStateRestoreEvent()
		: this("")
	{
	}

	public StiStateRestoreEvent(string script)
		: base(script)
	{
	}

	public StiStateRestoreEvent(StiComponent parent)
		: base(parent)
	{
	}
}
