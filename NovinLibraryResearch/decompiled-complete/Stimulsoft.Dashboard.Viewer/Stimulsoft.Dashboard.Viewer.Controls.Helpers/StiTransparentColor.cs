using System.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Controls.Helpers;

internal static class StiTransparentColor
{
	internal static Color Fix(Color color)
	{
		if (color.A >= byte.MaxValue)
		{
			return color;
		}
		return Color.FromArgb(255, color.R, color.G, color.B);
	}
}
