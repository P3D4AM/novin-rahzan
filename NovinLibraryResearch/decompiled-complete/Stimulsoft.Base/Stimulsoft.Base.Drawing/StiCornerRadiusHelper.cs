using System;

namespace Stimulsoft.Base.Drawing;

public static class StiCornerRadiusHelper
{
	public static RectangleD FitRectIntoCornerRadius(RectangleD rect, StiCornerRadius radius, double scale = 1.0, bool isFixedHeight = false)
	{
		SizeD size = default(SizeD);
		return FitRectIntoCornerRadius(rect, ref size, radius, scale, isFixedHeight);
	}

	public static RectangleD FitRectIntoCornerRadius(RectangleD rect, ref SizeD size, StiCornerRadius radius, double scale = 1.0, bool isFixedHeight = false)
	{
		if (radius == null)
		{
			return rect;
		}
		int num = StiScale.I((double)Math.Max(radius.TopLeft / 2f, radius.BottomLeft / 2f) * scale);
		int num2 = StiScale.I((double)Math.Max(radius.TopRight / 2f, radius.BottomRight / 2f) * scale);
		int num3 = StiScale.I((double)Math.Max(radius.TopLeft / 2f, radius.TopRight / 2f) * scale);
		int num4 = StiScale.I((double)Math.Max(radius.BottomLeft / 2f, radius.BottomRight / 2f) * scale);
		rect.X += num;
		rect.Width -= num + num2;
		size.Width -= num + num2;
		if (!isFixedHeight)
		{
			rect.Y += num3;
			rect.Height -= num3 + num4;
			size.Height -= num3 + num4;
		}
		return rect;
	}
}
