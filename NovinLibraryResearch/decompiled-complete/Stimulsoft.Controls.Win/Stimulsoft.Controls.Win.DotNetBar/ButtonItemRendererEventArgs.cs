using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ButtonItemRendererEventArgs
{
	public Graphics Graphics;

	public ButtonItem ButtonItem;

	internal ItemPaintArgs ItemPaintArgs;

	public ButtonItemRendererEventArgs()
	{
	}

	public ButtonItemRendererEventArgs(Graphics g, ButtonItem button)
	{
		Graphics = g;
		ButtonItem = button;
	}

	internal ButtonItemRendererEventArgs(Graphics g, ButtonItem button, ItemPaintArgs pa)
	{
		Graphics = g;
		ButtonItem = button;
		ItemPaintArgs = pa;
	}
}
