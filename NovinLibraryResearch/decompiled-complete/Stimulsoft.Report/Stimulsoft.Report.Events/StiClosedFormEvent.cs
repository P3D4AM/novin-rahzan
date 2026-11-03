using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiClosedFormEventConverter))]
public class StiClosedFormEvent : StiEvent
{
	public override string ToString()
	{
		return "ClosedForm";
	}

	public StiClosedFormEvent()
		: this("")
	{
	}

	public StiClosedFormEvent(string script)
		: base(script)
	{
	}

	public StiClosedFormEvent(StiComponent parent)
		: base(parent)
	{
	}
}
