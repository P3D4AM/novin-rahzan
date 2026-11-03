using System;

namespace EntityCache.EventArguments;

public class CountChangedEventArgs : EventArgs
{
	public int NewCount { get; set; }

	public CountChangedEventArgs(int newCount)
	{
		NewCount = newCount;
	}
}
