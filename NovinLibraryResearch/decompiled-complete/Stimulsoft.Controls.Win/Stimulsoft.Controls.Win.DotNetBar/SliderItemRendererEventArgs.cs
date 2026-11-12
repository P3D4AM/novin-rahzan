using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SliderItemRendererEventArgs : EventArgs
{
	public SliderItem SliderItem;

	public Graphics Graphics;

	internal ItemPaintArgs ItemPaintArgs;

	public SliderItemRendererEventArgs(SliderItem item, Graphics g)
	{
		SliderItem = item;
		Graphics = g;
	}
}
