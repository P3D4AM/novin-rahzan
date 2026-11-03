using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dictionary;

public class StiFunctionsDrawing
{
	public static Color ARGB(int alpha, int red, int green, int blue)
	{
		return Color.FromArgb(alpha, red, green, blue);
	}

	public static Color RGB(int red, int green, int blue)
	{
		return Color.FromArgb(255, red, green, blue);
	}

	public static Color ColorValue(string value)
	{
		return StiColor.Get(value);
	}

	public static Color ColorFade(object color, double fadeAmount)
	{
		if (fadeAmount == 0.0)
		{
			return GetColor(color);
		}
		if (fadeAmount > 0.0)
		{
			return StiColorUtils.Light(GetColor(color), (byte)(fadeAmount * 255.0));
		}
		return StiColorUtils.Dark(GetColor(color), (byte)(fadeAmount * 255.0));
	}

	public static StiSolidBrush SolidBrushValue(object color)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		return new StiSolidBrush(GetColor(color));
	}

	public static StiGradientBrush GradientBrushValue(object startColor, object endColor, double angle)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		return new StiGradientBrush(GetColor(startColor), GetColor(endColor), angle);
	}

	public static StiGlareBrush GlareBrushValue(object startColor, object endColor, double angle, double focus, double scale)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		return new StiGlareBrush(GetColor(startColor), GetColor(endColor), angle, (float)focus, (float)scale);
	}

	public static StiGlassBrush GlassBrushValue(object color, bool drawHatch, double blendFactor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		return new StiGlassBrush(GetColor(color), drawHatch, (float)blendFactor);
	}

	public static StiHatchBrush HatchBrushValue(HatchStyle style, object foreColor, object backColor)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		return new StiHatchBrush(style, GetColor(foreColor), GetColor(backColor));
	}

	private static Color GetColor(object color)
	{
		if (color == null)
		{
			return Color.Transparent;
		}
		if (color is Color)
		{
			return (Color)color;
		}
		StiBrush val = (StiBrush)((color is StiBrush) ? color : null);
		if (val != null)
		{
			return StiBrush.ToColor(val);
		}
		return StiColor.Get(color.ToString());
	}
}
