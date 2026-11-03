using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiPositionChangedEventConverter))]
public class StiPositionChangedEvent : StiEvent
{
	public override string ToString()
	{
		return "PositionChanged";
	}

	public StiPositionChangedEvent()
		: this("")
	{
	}

	public StiPositionChangedEvent(string script)
		: base(script)
	{
	}

	public StiPositionChangedEvent(StiComponent parent)
		: base(parent)
	{
	}
}
