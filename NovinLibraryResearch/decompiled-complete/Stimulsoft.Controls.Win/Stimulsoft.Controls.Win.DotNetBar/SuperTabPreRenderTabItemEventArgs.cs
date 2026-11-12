using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabPreRenderTabItemEventArgs : CancelEventArgs
{
	private SuperTabItem _Tab;

	private Graphics _Graphics;

	public SuperTabItem Tab => _Tab;

	public Graphics Graphics => _Graphics;

	public SuperTabPreRenderTabItemEventArgs(SuperTabItem tab, Graphics graphics)
	{
		_Tab = tab;
		_Graphics = graphics;
	}
}
