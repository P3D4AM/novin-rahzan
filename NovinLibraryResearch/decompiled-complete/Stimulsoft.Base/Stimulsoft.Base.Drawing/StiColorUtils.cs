using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing;

public static class StiColorUtils
{
	public static Color GetSysColor(Win32.ColorType colorType)
	{
		return ColorTranslator.FromWin32(Win32.GetSysColor((int)colorType));
	}

	public static Color ChangeLightness(Color color, byte correctionFactor)
	{
		float correctionFactor2 = (float)(int)correctionFactor / 255f;
		return ChangeLightness(color, correctionFactor2);
	}

	public static Color ChangeLightness(Color color, float correctionFactor)
	{
		float num = (int)color.R;
		float num2 = (int)color.G;
		float num3 = (int)color.B;
		if (correctionFactor < 0f)
		{
			correctionFactor = 1f + correctionFactor;
			num *= correctionFactor;
			num2 *= correctionFactor;
			num3 *= correctionFactor;
		}
		else
		{
			num = (255f - num) * correctionFactor + num;
			num2 = (255f - num2) * correctionFactor + num2;
			num3 = (255f - num3) * correctionFactor + num3;
		}
		return Color.FromArgb(color.A, (byte)num, (byte)num2, (byte)num3);
	}

	public static Color ChangeDarkness(Color color, byte correctionFactor)
	{
		float percDarker = (float)(int)correctionFactor / 255f;
		return ChangeDarkness(color, percDarker);
	}

	public static Color ChangeDarkness(Color color, float percDarker)
	{
		return ChangeLightness(color, percDarker * -1f);
	}

	public static Color Light(string baseColor, byte value)
	{
		return Light(StiColor.Get(baseColor), value);
	}

	public static Color Light(Color baseColor, byte value)
	{
		byte r = baseColor.R;
		byte g = baseColor.G;
		byte b = baseColor.B;
		r = ((r + value <= 255) ? ((byte)(r + value)) : byte.MaxValue);
		g = ((g + value <= 255) ? ((byte)(g + value)) : byte.MaxValue);
		b = ((b + value <= 255) ? ((byte)(b + value)) : byte.MaxValue);
		return Color.FromArgb(r, g, b);
	}

	public static bool IsItTooLight(Color color)
	{
		if (color.R > 200 && color.G > 200)
		{
			return color.B > 200;
		}
		return false;
	}

	public static bool IsItTooDark(Color color)
	{
		if (color.R < 50 && color.G < 50)
		{
			return color.B < 50;
		}
		return false;
	}

	public static Color MixingColors(Color color1, Color color2, int alpha = 255)
	{
		int red = color2.R * alpha / 255 + color1.R * (255 - alpha) / 255;
		int green = color2.G * alpha / 255 + color1.G * (255 - alpha) / 255;
		int blue = color2.B * alpha / 255 + color1.B * (255 - alpha) / 255;
		return Color.FromArgb(255, red, green, blue);
	}

	public static Color Dark(string baseColor, byte value)
	{
		return Dark(StiColor.Get(baseColor), value);
	}

	public static Color Dark(Color baseColor, byte value)
	{
		byte r = baseColor.R;
		byte g = baseColor.G;
		byte b = baseColor.B;
		r = (byte)((r - value >= 0) ? ((byte)(r - value)) : 0);
		g = (byte)((g - value >= 0) ? ((byte)(g - value)) : 0);
		b = (byte)((b - value >= 0) ? ((byte)(b - value)) : 0);
		return Color.FromArgb(r, g, b);
	}

	public static Color GetDisabledColor(Control control)
	{
		Color backColor = control.BackColor;
		if (backColor.A == 0)
		{
			Control parent = control.Parent;
			while (backColor.A == 0 || backColor.IsEmpty)
			{
				if (parent == null)
				{
					return SystemColors.Control;
				}
				backColor = parent.BackColor;
				parent = control.Parent;
			}
		}
		return backColor;
	}

	public static int GetColorRop(Color color, int darkROP, int lightROP)
	{
		if (!(color.GetBrightness() < 0.5f))
		{
			return lightROP;
		}
		return darkROP;
	}
}
