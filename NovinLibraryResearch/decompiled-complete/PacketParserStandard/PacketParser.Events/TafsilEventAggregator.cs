using System;
using PacketParser.EventArgs;

namespace PacketParser.Events;

public class TafsilEventAggregator
{
	public bool HasEventHandler
	{
		get
		{
			if (this.AccountChanged == null)
			{
				return this.AccountSaved != null;
			}
			return true;
		}
	}

	public event EventHandler<AccountChangedEventArgs> AccountChanged;

	public event EventHandler AccountSaved;

	public void RaiseAccountChanged(object sender, Guid tafsilGuid, string tranName)
	{
		this.AccountChanged?.Invoke(sender, new AccountChangedEventArgs
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
