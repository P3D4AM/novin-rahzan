using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class ShapePaintInfo
{
	public Graphics Graphics;

	public System.Drawing.Rectangle Bounds = System.Drawing.Rectangle.Empty;

	public Region ChildContentClip;

	public ShapePaintInfo(Graphics g, System.Drawing.Rectangle bounds)
	{
		Graphics = g;
		Bounds = bounds;
	}
}
