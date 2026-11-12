using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class TabMenuCloseEventArgs : EventArgs
{
	private ButtonItem _TabMenu;

	public ButtonItem TabMenu => _TabMenu;

	public TabMenuCloseEventArgs(ButtonItem tabMenu)
	{
		_TabMenu = tabMenu;
	}
}
