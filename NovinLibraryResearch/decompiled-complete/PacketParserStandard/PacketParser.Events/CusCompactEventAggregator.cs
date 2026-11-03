using System;
using PacketParser.EventArgs;

namespace PacketParser.Events;

public class CusCompactEventAggregator
{
	public event EventHandler<AccountChangedEventArgs> SumAghsatChanged;

	public event EventHandler AccountSaved;

	public void RaiseSumAghsatChanged(object sender, decimal newAccount, Guid tafsilGuid, string tranName)
	{
		this.SumAghsatChanged?.Invoke(sender, new AccountChangedEventArgs
		{
			TafsilGuid = tafsilGuid,
			TranName = tranName
		});
	}

	public void RaiseAccountSaved(object sender, System.EventArgs e)
	{
		this.AccountSaved?.Invoke(sender, e);
	}
}
