using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

internal static class StiMouseOverHelper
{
	public static float MouseOverLineDistance = 8f;

	public static float MouseOverSplineDistance = 15f;

	public static Color GetMouseOverColor()
	{
		return Color.FromArgb(100, Color.White);
	}

	public static Color GetLineMouseOverColor(Color color)
	{
		return Color.FromArgb(110, StiColorUtils.Light(color, (byte)10));
	}

	public static Color GetLineMouseOverColor(StiBrush brush)
	{
		Color color = StiBrush.ToColor(brush);
		return Color.FromArgb(110, StiColorUtils.Light(color, (byte)10));
	}
}
