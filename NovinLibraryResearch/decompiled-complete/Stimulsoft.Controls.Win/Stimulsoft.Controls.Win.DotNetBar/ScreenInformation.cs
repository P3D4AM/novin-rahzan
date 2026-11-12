using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ScreenInformation
{
	public Rectangle Bounds = Rectangle.Empty;

	public Rectangle WorkingArea = Rectangle.Empty;

	public ScreenInformation(Rectangle bounds, Rectangle workingarea)
	{
		Bounds = bounds;
		WorkingArea = workingarea;
	}
}
