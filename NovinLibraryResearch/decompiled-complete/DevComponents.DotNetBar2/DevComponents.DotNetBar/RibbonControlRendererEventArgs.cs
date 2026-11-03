using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class RibbonControlRendererEventArgs : EventArgs
{
	public Graphics Graphics;

	public RibbonControl RibbonControl;

	public bool GlassEnabled;

	internal ItemPaintArgs က;

	public RibbonControlRendererEventArgs(Graphics g, RibbonControl rc, bool glassEnabled)
	{
		Graphics = g;
		RibbonControl = rc;
		GlassEnabled = glassEnabled;
	}
}
