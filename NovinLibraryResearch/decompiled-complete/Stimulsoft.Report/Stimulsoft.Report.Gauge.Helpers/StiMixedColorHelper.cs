using System.Collections.Generic;
using System.Drawing;

namespace Stimulsoft.Report.Gauge.Helpers;

public static class StiMixedColorHelper
{
	public static Color ColorMixed(List<Color> colors)
	{
		if (colors == null || colors.Count == 0)
		{
			return Color.Transparent;
		}
		if (colors.Count == 1)
		{
			return colors[0];
		}
		Color color = colors[0];
		int num = 0;
		while (++num < colors.Count)
		{
			color = ColorMixer(color, colors[num]);
		}
		return color;
	}

	private static Color ColorMixer(Color c1, Color c2)
	{
		byte red = (byte)((c1.R + c2.R) / 2);
		byte green = (byte)((c1.G + c2.G) / 2);
		byte blue = (byte)((c1.B + c2.B) / 2);
		return Color.FromArgb(red, green, blue);
	}
}
