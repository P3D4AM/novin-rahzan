using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabStripPaintBackgroundEventArgs : EventArgs
{
	private Graphics _Graphics;

	private SuperTabColorTable _ColorTable;

	public Graphics Graphics => _Graphics;

	public SuperTabColorTable ColorTable => _ColorTable;

	public SuperTabStripPaintBackgroundEventArgs(Graphics graphics, SuperTabColorTable colorTable)
	{
		_Graphics = graphics;
		_ColorTable = colorTable;
	}
}
