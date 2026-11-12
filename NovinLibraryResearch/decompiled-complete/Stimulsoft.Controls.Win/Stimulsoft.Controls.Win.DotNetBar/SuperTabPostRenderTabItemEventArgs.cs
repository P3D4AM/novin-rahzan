using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabPostRenderTabItemEventArgs : EventArgs
{
	private SuperTabItem _Tab;

	private Graphics _Graphics;

	public SuperTabItem Tab => _Tab;

	public Graphics Graphics => _Graphics;

	public SuperTabPostRenderTabItemEventArgs(SuperTabItem tab, Graphics graphics)
	{
		_Tab = tab;
		_Graphics = graphics;
	}
}
