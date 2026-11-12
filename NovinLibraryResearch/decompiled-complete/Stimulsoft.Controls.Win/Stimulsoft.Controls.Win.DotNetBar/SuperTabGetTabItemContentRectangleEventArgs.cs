using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabGetTabItemContentRectangleEventArgs : EventArgs
{
	private SuperTabItem _Tab;

	private Rectangle _ContentRectangle;

	public SuperTabItem Tab => _Tab;

	public Rectangle ContentRectangle
	{
		get
		{
			return _ContentRectangle;
		}
		set
		{
			_ContentRectangle = value;
		}
	}

	public SuperTabGetTabItemContentRectangleEventArgs(SuperTabItem tab)
	{
		_Tab = tab;
		_ContentRectangle = tab.TabItemDisplay.ContentRectangle();
	}
}
