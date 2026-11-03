using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class SuperTabMeasureTabItemEventArgs : EventArgs
{
	private SuperTabItem _Ὴ;

	private Size __206C;

	private Graphics __187B;

	public SuperTabItem Tab => _Ὴ;

	public Size Size
	{
		get
		{
			return __206C;
		}
		set
		{
			__206C = value;
		}
	}

	public Graphics Graphics => __187B;

	public SuperTabMeasureTabItemEventArgs(SuperTabItem tab, Size size, Graphics graphics)
	{
		_Ὴ = tab;
		__206C = size;
		__187B = graphics;
	}
}
