using System;

namespace DevComponents.DotNetBar;

public class SuperTabStripTabColorChangedEventArgs : EventArgs
{
	private SuperTabItem _Ὴ;

	public SuperTabItem Tab => _Ὴ;

	public SuperTabStripTabColorChangedEventArgs(SuperTabItem tab)
	{
		_Ὴ = tab;
	}
}
