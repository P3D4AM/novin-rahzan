using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class CustomPaintEventArgs : EventArgs
{
	private Graphics m_Graphics;

	private Rectangle m_PaintRectangle = Rectangle.Empty;

	public Graphics Graphics => m_Graphics;

	public Rectangle PaintRectangle => m_PaintRectangle;

	public CustomPaintEventArgs(Graphics g, Rectangle r)
	{
		m_Graphics = g;
		m_PaintRectangle = r;
	}
}
