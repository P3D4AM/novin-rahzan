using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabMeasureTabItemEventArgs : EventArgs
{
	private SuperTabItem _Tab;

	private Size _Size;

	private Graphics _Graphics;

	public SuperTabItem Tab => _Tab;

	public Size Size
	{
		get
		{
			return _Size;
		}
		set
		{
			_Size = value;
		}
	}

	public Graphics Graphics => _Graphics;

	public SuperTabMeasureTabItemEventArgs(SuperTabItem tab, Size size, Graphics graphics)
	{
		_Tab = tab;
		_Size = size;
		_Graphics = graphics;
	}
}
