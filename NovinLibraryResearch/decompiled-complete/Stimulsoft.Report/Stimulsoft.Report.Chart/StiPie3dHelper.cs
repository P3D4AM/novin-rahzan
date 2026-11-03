using System;
using System.Drawing;

namespace Stimulsoft.Report.Chart;

internal static class StiPie3dHelper
{
	public static readonly float BrightnessEnhancementFactor1 = 0.3f;

	public static Color CreateColorWithCorrectedLightness(Color color, float correctionFactor)
	{
		if (correctionFactor == 0f)
		{
			return color;
		}
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
		return Color.FromArgb(color.A, (int)num, (int)num2, (int)num3);
	}

	public static float GetActualAngle(RectangleF rect, float transformedAngle)
	{
		double x = (double)rect.Height * Math.Cos((double)transformedAngle * Math.PI / 180.0);
		double y = (double)rect.Width * Math.Sin((double)transformedAngle * Math.PI / 180.0);
		float num = (float)(Math.Atan2(y, x) * 180.0 / Math.PI);
		if (num < 0f)
		{
			return num + 360f;
		}
		return num;
	}

	public static float TransformAngle(RectangleF rect, float angle)
	{
		double x = (double)rect.Width * Math.Cos((double)angle * Math.PI / 180.0);
		double y = (double)rect.Height * Math.Sin((double)angle * Math.PI / 180.0);
		float num = (float)(Math.Atan2(y, x) * 180.0 / Math.PI);
		if (num < 0f)
		{
			return num + 360f;
		}
		return num;
	}
}
