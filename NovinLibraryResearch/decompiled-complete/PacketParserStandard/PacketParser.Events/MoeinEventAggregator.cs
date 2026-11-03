using System;

namespace PacketParser.Events;

public class MoeinEventAggregator
{
	public bool HasEventHandler => this.MoeinSaved != null;

	public event EventHandler MoeinSaved;

	public void RaiseAccountSaved(object sender, System.EventArgs e)
	{
		this.MoeinSaved?.Invoke(sender, e);
	}
}
