using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemContainerRendererEventArgs : EventArgs
{
	public Graphics Graphics;

	public ItemContainer ItemContainer;

	internal ItemPaintArgs ItemPaintArgs;

	public ItemContainerRendererEventArgs(Graphics g, ItemContainer container)
	{
		Graphics = g;
		ItemContainer = container;
	}
}
