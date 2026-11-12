using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public static class ColorExt
{
	public static Color RemoveAlpha(this Color color)
	{
		return Color.FromArgb(255, color.R, color.G, color.B);
	}

	public static Color ChangeAlpha(this Color color, int alpha)
	{
		return Color.FromArgb(alpha, color.R, color.G, color.B);
	}

	public static string ToHex(this Color color)
	{
		string empty = string.Empty;
		try
		{
			return (color.A != byte.MaxValue) ? $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}" : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
		}
		catch
		{
			return empty;
		}
	}
}
