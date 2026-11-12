using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ColorItemRendererEventArgs : EventArgs
{
	public Graphics Graphics;

	public ColorItem ColorItem;

	public ColorItemRendererEventArgs(Graphics g, ColorItem item)
	{
		Graphics = g;
		ColorItem = item;
	}
}
