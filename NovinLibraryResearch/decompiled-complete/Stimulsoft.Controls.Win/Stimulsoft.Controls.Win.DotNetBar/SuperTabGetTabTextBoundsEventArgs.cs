using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabGetTabTextBoundsEventArgs : EventArgs
{
	private SuperTabItem _Tab;

	private Rectangle _Bounds;

	public SuperTabItem Tab => _Tab;

	public Rectangle Bounds
	{
		get
		{
			return _Bounds;
		}
		set
		{
			_Bounds = value;
		}
	}

	public SuperTabGetTabTextBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
	{
		_Tab = tab;
		_Bounds = bounds;
	}
}
