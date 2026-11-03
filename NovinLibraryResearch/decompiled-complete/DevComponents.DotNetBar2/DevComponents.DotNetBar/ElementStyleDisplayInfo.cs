using System.Drawing;

namespace DevComponents.DotNetBar;

public class ElementStyleDisplayInfo
{
	public ElementStyle Style;

	public Graphics Graphics;

	public Rectangle Bounds = Rectangle.Empty;

	public bool RightToLeft;

	public bool IsTwistedDoubleBorder;

	public ElementStyleDisplayInfo()
	{
	}

	public ElementStyleDisplayInfo(ElementStyle style, Graphics g, Rectangle bounds)
	{
		Style = style;
		Graphics = g;
		Bounds = bounds;
	}

	public ElementStyleDisplayInfo(ElementStyle style, Graphics g, Rectangle bounds, bool isTwistedDoubleBorder)
	{
		Style = style;
		Graphics = g;
		Bounds = bounds;
		IsTwistedDoubleBorder = isTwistedDoubleBorder;
	}
}
