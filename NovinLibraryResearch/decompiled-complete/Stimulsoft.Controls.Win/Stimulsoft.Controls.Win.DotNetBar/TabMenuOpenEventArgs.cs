using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class TabMenuOpenEventArgs : CancelEventArgs
{
	private ButtonItem _TabMenu;

	public ButtonItem TabMenu => _TabMenu;

	public TabMenuOpenEventArgs(ButtonItem tabMenu)
	{
		_TabMenu = tabMenu;
	}
}
