using System;
using System.Drawing;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class SuperTabStripPaintBackgroundEventArgs : EventArgs
{
	private Graphics __187B;

	private SuperTabColorTable __137C;

	public Graphics Graphics => __187B;

	public SuperTabColorTable ColorTable => __137C;

	public SuperTabStripPaintBackgroundEventArgs(Graphics graphics, SuperTabColorTable colorTable)
	{
		__187B = graphics;
		__137C = colorTable;
	}
}
