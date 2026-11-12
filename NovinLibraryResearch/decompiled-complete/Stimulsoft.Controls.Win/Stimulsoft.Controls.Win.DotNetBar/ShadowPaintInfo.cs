using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ShadowPaintInfo
{
	public Graphics Graphics;

	public Rectangle Rectangle = Rectangle.Empty;

	public int Size = 3;

	public Rectangle ClipRectangle = Rectangle.Empty;

	public bool IsSquare;
}
