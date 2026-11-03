using System;

namespace DevComponents.DotNetBar;

public class TabMenuCloseEventArgs : EventArgs
{
	private ButtonItem __2EF9;

	public ButtonItem TabMenu => __2EF9;

	public TabMenuCloseEventArgs(ButtonItem tabMenu)
	{
		__2EF9 = tabMenu;
	}
}
