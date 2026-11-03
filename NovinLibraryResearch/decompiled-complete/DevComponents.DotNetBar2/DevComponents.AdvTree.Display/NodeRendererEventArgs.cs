using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display;

public class NodeRendererEventArgs : EventArgs
{
	public Graphics Graphics;

	public Node Node;

	public Rectangle NodeBounds = Rectangle.Empty;

	public ElementStyle Style;

	public Color Color = Color.Empty;

	public NodeRendererEventArgs()
	{
	}

	public NodeRendererEventArgs(Graphics g, Node node, Rectangle bounds, ElementStyle style)
		: this(g, node, bounds, style, Color.Empty)
	{
	}

	public NodeRendererEventArgs(Graphics g, Node node, Rectangle bounds, ElementStyle style, Color color)
	{
		Graphics = g;
		Node = node;
		NodeBounds = bounds;
		Style = style;
		Color = color;
	}
}
