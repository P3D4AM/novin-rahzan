using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiMoveFooterToBottomEventConverter))]
public class StiMoveFooterToBottomEvent : StiEvent
{
	public override string ToString()
	{
		return "MoveFooterToBottom";
	}

	public StiMoveFooterToBottomEvent()
		: this("")
	{
	}

	public StiMoveFooterToBottomEvent(string script)
		: base(script)
	{
	}

	public StiMoveFooterToBottomEvent(StiComponent parent)
		: base(parent)
	{
	}
}
