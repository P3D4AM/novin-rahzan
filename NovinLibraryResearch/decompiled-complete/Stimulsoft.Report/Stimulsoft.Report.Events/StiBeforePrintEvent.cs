using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiBeforePrintEventConverter))]
public class StiBeforePrintEvent : StiEvent
{
	public override string ToString()
	{
		return "BeforePrint";
	}

	public StiBeforePrintEvent()
		: this("")
	{
	}

	public StiBeforePrintEvent(string script)
		: base(script)
	{
	}

	public StiBeforePrintEvent(StiComponent parent)
		: base(parent)
	{
	}
}
