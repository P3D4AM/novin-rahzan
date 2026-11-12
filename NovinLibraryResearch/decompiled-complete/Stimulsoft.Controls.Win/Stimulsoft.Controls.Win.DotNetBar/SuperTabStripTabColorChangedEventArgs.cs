using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabStripTabColorChangedEventArgs : EventArgs
{
	private SuperTabItem _Tab;

	public SuperTabItem Tab => _Tab;

	public SuperTabStripTabColorChangedEventArgs(SuperTabItem tab)
	{
		_Tab = tab;
	}
}
