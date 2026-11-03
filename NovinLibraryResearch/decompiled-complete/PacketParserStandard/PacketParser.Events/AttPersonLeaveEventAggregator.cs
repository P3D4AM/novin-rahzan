using System;

namespace PacketParser.Events;

public class AttPersonLeaveEventAggregator
{
	public event EventHandler LeaveSaved;

	public void RaiseLeaveSaved(object sender, System.EventArgs e)
	{
		this.LeaveSaved?.Invoke(sender, e);
	}
}
