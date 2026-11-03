using System;
using System.Drawing;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Rendering;

public class TabFormStripPainterArgs : EventArgs
{
	public Graphics Graphics;

	public TabFormStripControl TabFormStrip;

	internal ItemPaintArgs က;

	public bool Cancel;

	public TabFormStripPainterArgs(TabFormStripControl tabFormStrip, Graphics graphics, ItemPaintArgs itemPaintArgs)
	{
		Graphics = graphics;
		TabFormStrip = tabFormStrip;
		က = itemPaintArgs;
	}
}
