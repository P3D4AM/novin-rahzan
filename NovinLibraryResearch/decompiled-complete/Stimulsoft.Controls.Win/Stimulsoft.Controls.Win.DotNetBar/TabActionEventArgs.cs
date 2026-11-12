using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class TabActionEventArgs : EventArgs
{
	private BaseItem _Tab;

	public BaseItem Tab => _Tab;

	public TabActionEventArgs(BaseItem tab)
	{
		_Tab = tab;
	}
}
