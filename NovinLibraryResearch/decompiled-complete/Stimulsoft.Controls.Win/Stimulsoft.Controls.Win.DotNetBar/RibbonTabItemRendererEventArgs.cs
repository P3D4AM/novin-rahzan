using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class RibbonTabItemRendererEventArgs
{
	public Graphics Graphics;

	public RibbonTabItem RibbonTabItem;

	internal ItemPaintArgs ItemPaintArgs;

	public RibbonTabItemRendererEventArgs()
	{
	}

	public RibbonTabItemRendererEventArgs(Graphics g, RibbonTabItem button)
	{
		Graphics = g;
		RibbonTabItem = button;
	}

	internal RibbonTabItemRendererEventArgs(Graphics g, RibbonTabItem button, ItemPaintArgs pa)
	{
		Graphics = g;
		RibbonTabItem = button;
		ItemPaintArgs = pa;
	}
}
