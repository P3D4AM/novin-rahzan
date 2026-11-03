using System;

namespace PacketParser.Events;

public class OrderEventAggregator
{
	public event EventHandler OrderSaved;

	public void RaiseOrderSaved(object sender, System.EventArgs e)
	{
		this.OrderSaved?.Invoke(sender, e);
	}
}
