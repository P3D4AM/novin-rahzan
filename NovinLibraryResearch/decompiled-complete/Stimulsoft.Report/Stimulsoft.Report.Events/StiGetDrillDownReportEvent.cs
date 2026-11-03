using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetDrillDownReportEventConverter))]
public class StiGetDrillDownReportEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetDrillDownReportEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetDrillDownReportEventHandler);
	}

	public override string ToString()
	{
		return "GetDrillDownReport";
	}

	public StiGetDrillDownReportEvent()
		: this("")
	{
	}

	public StiGetDrillDownReportEvent(string script)
		: base(script)
	{
	}

	public StiGetDrillDownReportEvent(StiComponent parent)
		: base(parent)
	{
	}
}
