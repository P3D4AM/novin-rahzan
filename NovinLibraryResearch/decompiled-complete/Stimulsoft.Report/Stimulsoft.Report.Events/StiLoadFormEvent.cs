using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiLoadFormEventConverter))]
public class StiLoadFormEvent : StiEvent
{
	public override string ToString()
	{
		return "LoadForm";
	}

	public StiLoadFormEvent()
		: this("")
	{
	}

	public StiLoadFormEvent(string script)
		: base(script)
	{
	}

	public StiLoadFormEvent(StiComponent parent)
		: base(parent)
	{
	}
}
