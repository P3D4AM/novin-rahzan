using System;

namespace PacketParser.Events;

public class ChequeShEventAggregator
{
	public event EventHandler OperationSaved;

	public void RaiseOperationSaved(object sender, System.EventArgs e)
	{
		this.OperationSaved?.Invoke(sender, e);
	}
}
