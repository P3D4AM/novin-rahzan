using System;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Events;

public class CheckedChangedEventArgs : EventArgs
{
	public readonly ButtonItem OldItem;

	public CheckedChangedEventArgs(ButtonItem oldItem)
	{
		OldItem = oldItem;
	}
}
