using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class ListBoxItemRendererEventArgs : EventArgs
{
	public ListBoxItem Item;

	public Graphics Graphics;

	internal ItemPaintArgs က;

	public ListBoxItemRendererEventArgs(ListBoxItem item, Graphics g)
	{
		Item = item;
		Graphics = g;
	}
}
