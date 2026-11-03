using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class RangeSliderItemRendererEventArgs : EventArgs
{
	public RangeSliderItem SliderItem;

	public Graphics Graphics;

	internal ItemPaintArgs က;

	public RangeSliderItemRendererEventArgs(RangeSliderItem item, Graphics g)
	{
		SliderItem = item;
		Graphics = g;
	}
}
