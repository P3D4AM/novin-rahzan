using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class CustomPaintEventArgs : EventArgs
{
	private Graphics ܟ;

	private Rectangle _2449 = Rectangle.Empty;

	public Graphics Graphics => ܟ;

	public Rectangle PaintRectangle => _2449;

	public CustomPaintEventArgs(Graphics g, Rectangle r)
	{
		ܟ = g;
		_2449 = r;
	}
}
