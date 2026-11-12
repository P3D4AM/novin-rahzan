using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Helpers;

internal static class ColorHelpers
{
	public static HSVColor ColorToHSV(Color color)
	{
		double num = (double)(int)color.R / 255.0;
		double num2 = (double)(int)color.G / 255.0;
		double num3 = (double)(int)color.B / 255.0;
		double num4 = Min(num, num2, num3);
		double num5 = Max(num, num2, num3);
		double num6 = num5 - num4;
		HSVColor result = default(HSVColor);
		result.Value = num5;
		if (num6 == 0.0)
		{
			result.Hue = 0.0;
			result.Saturation = 0.0;
		}
		else
		{
			result.Saturation = num6 / num5;
			double num7 = ((num5 - num) / 6.0 + num6 / 2.0) / num6;
			double num8 = ((num5 - num2) / 6.0 + num6 / 2.0) / num6;
			double num9 = ((num5 - num3) / 6.0 + num6 / 2.0) / num6;
			if (num == num5)
			{
				result.Hue = num9 - num8;
			}
			else if (num2 == num5)
			{
				result.Hue = 1.0 / 3.0 + num7 - num9;
			}
			else if (num3 == num5)
			{
				result.Hue = 2.0 / 3.0 + num8 - num7;
			}
			if (result.Hue < 0.0)
			{
				result.Hue += 1.0;
			}
			if (result.Hue > 1.0)
			{
				result.Hue -= 1.0;
			}
		}
		return result;
	}

	public static Color HSVToColor(double h, double s, double v)
	{
		return HSVToColor(new HSVColor(h, s, v));
	}

	public static Color HSVToColor(HSVColor hsv)
	{
		if (hsv.Value < 0.0)
		{
			hsv.Value = 0.0;
		}
		else if (hsv.Value > 1.0)
		{
			hsv.Value = 1.0;
		}
		if (hsv.Saturation > 1.0)
		{
			hsv.Saturation = 1.0;
		}
		else if (hsv.Saturation < 0.0)
		{
			hsv.Saturation = 0.0;
		}
		if (hsv.Saturation == 0.0)
		{
			return Color.FromArgb((int)(hsv.Value * 255.0), (int)(hsv.Value * 255.0), (int)(hsv.Value * 255.0));
		}
		double num = hsv.Hue * 6.0;
		if (num == 6.0)
		{
			num = 0.0;
		}
		int num2 = (int)num;
		double num3 = hsv.Value * (1.0 - hsv.Saturation);
		double num4 = hsv.Value * (1.0 - hsv.Saturation * (num - (double)num2));
		double num5 = hsv.Value * (1.0 - hsv.Saturation * (1.0 - (num - (double)num2)));
		double num6;
		double num7;
		double num8;
		switch (num2)
		{
		case 0:
			num6 = hsv.Value;
			num7 = num5;
			num8 = num3;
			break;
		case 1:
			num6 = num4;
			num7 = hsv.Value;
			num8 = num3;
			break;
		case 2:
			num6 = num3;
			num7 = hsv.Value;
			num8 = num5;
			break;
		case 3:
			num6 = num3;
			num7 = num4;
			num8 = hsv.Value;
			break;
		case 4:
			num6 = num5;
			num7 = num3;
			num8 = hsv.Value;
			break;
		default:
			num6 = hsv.Value;
			num7 = num3;
			num8 = num4;
			break;
		}
		return Color.FromArgb((int)(num6 * 255.0), (int)(num7 * 255.0), (int)(num8 * 255.0));
	}

	private static double Max(double rR, double rG, double rB)
	{
		double num = 0.0;
		if (rR > rG)
		{
			if (rR > rB)
			{
				return rR;
			}
			return rB;
		}
		if (rB > rG)
		{
			return rB;
		}
		return rG;
	}

	private static double Min(double rR, double rG, double rB)
	{
		double num = 0.0;
		if (rR < rG)
		{
			if (rR < rB)
			{
				return rR;
			}
			return rB;
		}
		if (rB < rG)
		{
			return rB;
		}
		return rG;
	}
}
