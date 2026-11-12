using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class RibbonBarRendererEventArgs : EventArgs
{
	public Graphics Graphics;

	public Rectangle Bounds = Rectangle.Empty;

	public RibbonBar RibbonBar;

	public bool MouseOver;

	public bool Pressed;

	public Region ContentClip;

	public RibbonBarRendererEventArgs(Graphics g, Rectangle bounds, RibbonBar ribbon)
	{
		Graphics = g;
		Bounds = bounds;
		RibbonBar = ribbon;
	}
}
