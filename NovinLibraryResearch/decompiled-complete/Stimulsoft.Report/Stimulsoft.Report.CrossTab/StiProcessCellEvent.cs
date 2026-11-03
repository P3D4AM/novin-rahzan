using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.CrossTab.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CrossTab;

[TypeConverter(typeof(StiProcessCellEventConverter))]
public class StiProcessCellEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiProcessCellEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiProcessCellEventHandler);
	}

	public override string ToString()
	{
		return "ProcessCell";
	}

	public StiProcessCellEvent()
		: this("")
	{
	}

	public StiProcessCellEvent(string script)
		: base(script)
	{
	}
}
