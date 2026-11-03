using System.Drawing;

namespace DevComponents.DotNetBar;

public class ButtonItemRendererEventArgs
{
	public Graphics Graphics;

	public ButtonItem ButtonItem;

	internal ItemPaintArgs က;

	public bool Cancel;

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
		က = pa;
	}
}
