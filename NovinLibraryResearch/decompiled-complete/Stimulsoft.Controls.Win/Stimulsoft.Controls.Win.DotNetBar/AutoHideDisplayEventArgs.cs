using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class AutoHideDisplayEventArgs : EventArgs
{
	public Rectangle DisplayRectangle = Rectangle.Empty;

	public AutoHideDisplayEventArgs(Rectangle displayRectangle)
	{
		DisplayRectangle = displayRectangle;
	}
}
