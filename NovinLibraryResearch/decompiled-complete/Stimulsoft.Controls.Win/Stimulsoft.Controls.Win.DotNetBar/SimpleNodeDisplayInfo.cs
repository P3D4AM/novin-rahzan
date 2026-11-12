using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SimpleNodeDisplayInfo
{
	public ElementStyle Style;

	public Graphics Graphics;

	public ISimpleElement Element;

	public Font Font;

	public Rectangle TextBounds = Rectangle.Empty;

	public bool RightToLeft;

	public SimpleNodeDisplayInfo(ElementStyle style, Graphics g, ISimpleElement elem, Font font, bool rightToLeft)
	{
		Style = style;
		Graphics = g;
		Element = elem;
		Font = font;
		RightToLeft = rightToLeft;
	}
}
