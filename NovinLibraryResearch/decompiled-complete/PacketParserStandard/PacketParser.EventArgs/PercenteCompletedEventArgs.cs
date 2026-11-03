using System;
using System.Threading;

namespace PacketParser.EventArgs;

public class PercenteCompletedEventArgs
{
	public int TotalItems { get; private set; }

	public int CurrentItem { get; private set; }

	public string CurrentItemName { get; private set; }

	public decimal Percentage
	{
		get
		{
			if (TotalItems <= 0 || CurrentItem >= TotalItems)
			{
				return 100m;
			}
			return Math.Round(100m * (decimal)CurrentItem / (decimal)TotalItems, 2);
		}
	}

	public CancellationToken Token { get; private set; }

	public PercenteCompletedEventArgs(int totalItems, int currentItem, string currentItemName, CancellationToken token)
	{
		TotalItems = totalItems;
		CurrentItem = currentItem;
		CurrentItemName = currentItemName;
		Token = token;
	}
}
