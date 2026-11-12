using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SimpleElementLayoutInfo
{
	public ISimpleElement Element;

	public Graphics Graphics;

	public Font Font;

	public int Left;

	public int Top;

	public ElementStyle LayoutStyle;

	public bool LeftToRight = true;

	public bool VerticalPartAlignment;
}
