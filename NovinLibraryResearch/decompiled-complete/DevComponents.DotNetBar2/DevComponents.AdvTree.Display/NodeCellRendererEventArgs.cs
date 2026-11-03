using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display;

public class NodeCellRendererEventArgs : NodeRendererEventArgs
{
	public Cell Cell;

	public Rectangle CellBounds = Rectangle.Empty;

	internal Point _2750 = Point.Empty;

	internal ColorScheme ԫ;

	internal Image _2751;

	internal Image _2752;

	internal Image _2753;

	internal ItemPaintArgs က;

	public NodeCellRendererEventArgs()
		: base(null, null, Rectangle.Empty, null)
	{
	}

	public NodeCellRendererEventArgs(Graphics g, Node node, Rectangle bounds, ElementStyle style, Cell cell, Rectangle cellBounds)
		: base(g, node, bounds, style)
	{
		Cell = cell;
		CellBounds = cellBounds;
	}
}
