using System;

namespace EntityCache.Business.Accounting;

public class DisposeRemainEventArgs : EventArgs
{
	private int remainSecondsNotif;

	public int RemainSecondsNotif
	{
		get
		{
			return remainSecondsNotif;
		}
		internal set
		{
			if (value > 60)
			{
				value = 60;
			}
			remainSecondsNotif = value;
		}
	}
}
