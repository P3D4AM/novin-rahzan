using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiPrintedEventConverter))]
public class StiPrintedEvent : StiEvent
{
	public override string ToString()
	{
		return "Printed";
	}

	public StiPrintedEvent()
		: this("")
	{
	}

	public StiPrintedEvent(string script)
		: base(script)
	{
	}

	public StiPrintedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
