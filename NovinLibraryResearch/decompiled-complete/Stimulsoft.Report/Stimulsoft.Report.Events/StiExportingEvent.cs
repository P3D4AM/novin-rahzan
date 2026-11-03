using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiExportingEventConverter))]
public class StiExportingEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiExportEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiExportEventHandler);
	}

	public override string ToString()
	{
		return "Exporting";
	}

	public StiExportingEvent()
		: this("")
	{
	}

	public StiExportingEvent(string script)
		: base(script)
	{
	}

	public StiExportingEvent(StiComponent parent)
		: base(parent)
	{
	}
}
