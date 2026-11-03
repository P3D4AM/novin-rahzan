using System;

namespace PacketParser.Events;

public class AghsatMEventAggregator
{
	public event EventHandler GhetstSaved;

	public void RaiseGhestSaved(object sender, System.EventArgs e)
	{
		this.GhetstSaved?.Invoke(sender, e);
	}
}
