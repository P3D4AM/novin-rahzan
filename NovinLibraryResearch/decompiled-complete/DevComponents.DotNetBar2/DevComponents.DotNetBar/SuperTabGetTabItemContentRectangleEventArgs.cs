using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class SuperTabGetTabItemContentRectangleEventArgs : EventArgs
{
	private SuperTabItem _Ὴ;

	private Rectangle _ῒ;

	public SuperTabItem Tab => _Ὴ;

	public Rectangle ContentRectangle
	{
		get
		{
			return _ῒ;
		}
		set
		{
			_ῒ = value;
		}
	}

	public SuperTabGetTabItemContentRectangleEventArgs(SuperTabItem tab)
	{
		_Ὴ = tab;
		_ῒ = tab.Ὀ._1FDC();
	}
}
