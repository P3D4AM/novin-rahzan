using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiPrintingEventConverter))]
public class StiPrintingEvent : StiEvent
{
	public override string ToString()
	{
		return "Printing";
	}

	public StiPrintingEvent()
		: this("")
	{
	}

	public StiPrintingEvent(string script)
		: base(script)
	{
	}

	public StiPrintingEvent(StiComponent parent)
		: base(parent)
	{
	}
}
