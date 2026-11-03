using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class TabMenuOpenEventArgs : CancelEventArgs
{
	private ButtonItem __2EF9;

	public ButtonItem TabMenu => __2EF9;

	public TabMenuOpenEventArgs(ButtonItem tabMenu)
	{
		__2EF9 = tabMenu;
	}
}
