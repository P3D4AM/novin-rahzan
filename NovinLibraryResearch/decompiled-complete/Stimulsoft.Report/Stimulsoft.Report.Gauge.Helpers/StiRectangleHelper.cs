using System.Drawing;

namespace Stimulsoft.Report.Gauge.Helpers;

public static class StiRectangleHelper
{
	public static float CenterX(RectangleF rect)
	{
		return rect.Left + rect.Width / 2f;
	}

	public static float CenterY(RectangleF rect)
	{
		return rect.Top + rect.Height / 2f;
	}
}
