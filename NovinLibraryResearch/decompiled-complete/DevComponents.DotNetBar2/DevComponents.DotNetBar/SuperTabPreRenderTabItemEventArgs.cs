using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class SuperTabPreRenderTabItemEventArgs : CancelEventArgs
{
	private SuperTabItem _Ὴ;

	private Graphics __187B;

	public SuperTabItem Tab => _Ὴ;

	public Graphics Graphics => __187B;

	public SuperTabPreRenderTabItemEventArgs(SuperTabItem tab, Graphics graphics)
	{
		_Ὴ = tab;
		__187B = graphics;
	}
}
