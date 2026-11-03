using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class SuperTabPostRenderTabItemEventArgs : EventArgs
{
	private SuperTabItem _Ὴ;

	private Graphics __187B;

	public SuperTabItem Tab => _Ὴ;

	public Graphics Graphics => __187B;

	public SuperTabPostRenderTabItemEventArgs(SuperTabItem tab, Graphics graphics)
	{
		_Ὴ = tab;
		__187B = graphics;
	}
}
