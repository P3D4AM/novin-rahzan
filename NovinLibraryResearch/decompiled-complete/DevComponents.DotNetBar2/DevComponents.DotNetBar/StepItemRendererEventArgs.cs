using System;
using System.Drawing;

namespace DevComponents.DotNetBar;

public class StepItemRendererEventArgs : EventArgs
{
	public StepItem Item;

	public Graphics Graphics;

	internal ItemPaintArgs က;

	public StepItemRendererEventArgs(StepItem item, Graphics g)
	{
		Item = item;
		Graphics = g;
	}
}
