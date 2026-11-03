using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class SuperTabGetTabTextBoundsEventArgs : EventArgs
{
	private SuperTabItem _Ὴ;

	private Rectangle __1734;

	public SuperTabItem Tab => _Ὴ;

	public Rectangle Bounds
	{
		get
		{
			return __1734;
		}
		set
		{
			__1734 = value;
		}
	}

	public SuperTabGetTabTextBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
	{
		_Ὴ = tab;
		__1734 = bounds;
	}
}
