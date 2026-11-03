using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiExportedEventConverter))]
public class StiExportedEvent : StiEvent
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
		return "Exported";
	}

	public StiExportedEvent()
		: this("")
	{
	}

	public StiExportedEvent(string script)
		: base(script)
	{
	}

	public StiExportedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
