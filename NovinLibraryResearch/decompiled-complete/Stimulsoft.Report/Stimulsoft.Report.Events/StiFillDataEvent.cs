using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiLoadFormEventConverter))]
public class StiFillDataEvent : StiEvent
{
	public override string ToString()
	{
		return "FillData";
	}

	public StiFillDataEvent()
		: this("")
	{
	}

	public StiFillDataEvent(string script)
		: base(script)
	{
	}

	public StiFillDataEvent(StiComponent parent)
		: base(parent)
	{
	}
}
