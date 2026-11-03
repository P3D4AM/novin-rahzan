using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class SliderItemRendererEventArgs : EventArgs
{
	public SliderItem SliderItem;

	public Graphics Graphics;

	internal ItemPaintArgs က;

	public SliderItemRendererEventArgs(SliderItem item, Graphics g)
	{
		SliderItem = item;
		Graphics = g;
	}
}
