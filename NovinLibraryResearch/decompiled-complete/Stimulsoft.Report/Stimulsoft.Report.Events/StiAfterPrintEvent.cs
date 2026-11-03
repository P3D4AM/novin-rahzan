using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiAfterPrintEventConverter))]
public class StiAfterPrintEvent : StiEvent
{
	public override string ToString()
	{
		return "AfterPrint";
	}

	public StiAfterPrintEvent()
		: this(string.Empty)
	{
	}

	public StiAfterPrintEvent(string script)
		: base(script)
	{
	}

	public StiAfterPrintEvent(StiComponent parent)
		: base(parent)
	{
	}
}
