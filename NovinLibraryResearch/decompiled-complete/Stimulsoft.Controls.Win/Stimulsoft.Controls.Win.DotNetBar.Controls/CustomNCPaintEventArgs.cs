using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

internal class CustomNCPaintEventArgs : EventArgs
{
	public Graphics Graphics;

	public Rectangle Bounds = Rectangle.Empty;

	public CustomNCPaintEventArgs(Graphics g, Rectangle r)
	{
		Graphics = g;
		Bounds = r;
	}
}
