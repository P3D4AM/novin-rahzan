using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class ItemContainerRendererEventArgs : EventArgs
{
	public Graphics Graphics;

	public ItemContainer ItemContainer;

	internal ItemPaintArgs က;

	public ItemContainerRendererEventArgs(Graphics g, ItemContainer container)
	{
		Graphics = g;
		ItemContainer = container;
	}
}
