using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiValueChangedEventConverter))]
public class StiValueChangedEvent : StiEvent
{
	public override string ToString()
	{
		return "ValueChanged";
	}

	public StiValueChangedEvent()
		: this("")
	{
	}

	public StiValueChangedEvent(string script)
		: base(script)
	{
	}

	public StiValueChangedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
