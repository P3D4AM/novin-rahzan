using System;

namespace DevComponents.DotNetBar;

public class TabActionEventArgs : EventArgs
{
	private BaseItem _Ὴ;

	public BaseItem Tab => _Ὴ;

	public TabActionEventArgs(BaseItem tab)
	{
		_Ὴ = tab;
	}
}
