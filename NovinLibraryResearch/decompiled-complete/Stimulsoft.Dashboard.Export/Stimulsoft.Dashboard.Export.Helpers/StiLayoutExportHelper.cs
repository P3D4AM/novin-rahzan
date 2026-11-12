using System;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Export.Helpers;

public static class StiLayoutExportHelper
{
	public static RectangleD ExcludeMargin(IStiElement element, RectangleD rect)
	{
		if (element is IStiMargin)
		{
			StiMargin margin = ((IStiMargin)element).Margin;
			if (!margin.IsEmpty)
			{
				return new RectangleD(rect.X + margin.Left, rect.Y + margin.Top, Math.Max(1.0, rect.Width - margin.Left - margin.Right), Math.Max(1.0, rect.Height - margin.Top - margin.Bottom));
			}
		}
		return rect;
	}

	public static RectangleD ExcludePadding(IStiElement element, RectangleD rect)
	{
		if (element is IStiPadding)
		{
			StiPadding padding = ((IStiPadding)element).Padding;
			if (!padding.IsEmpty)
			{
				return new RectangleD(rect.X + padding.Left, rect.Y + padding.Top, Math.Max(1.0, rect.Width - padding.Left - padding.Right), Math.Max(1.0, rect.Height - padding.Top - padding.Bottom));
			}
		}
		return rect;
	}

	public static RectangleD ExcludeCornerRadius(IStiElement element, RectangleD rect)
	{
		if (element is IStiCornerRadius)
		{
			StiCornerRadius cornerRadius = ((IStiCornerRadius)element).CornerRadius;
			if (cornerRadius.IsDefault)
			{
				return rect;
			}
			double num = Math.Max(cornerRadius.TopLeft / 2f, cornerRadius.BottomLeft / 2f);
			double num2 = Math.Max(cornerRadius.TopLeft / 2f, cornerRadius.TopRight / 2f);
			double num3 = Math.Max(cornerRadius.TopRight / 2f, cornerRadius.BottomRight / 2f);
			double num4 = Math.Max(cornerRadius.BottomLeft / 2f, cornerRadius.BottomRight / 2f);
			if (element is IStiPadding)
			{
				StiPadding padding = ((IStiPadding)element).Padding;
				num = ((padding.Left > num) ? padding.Left : (num - padding.Left));
				num2 = ((padding.Top > num2) ? padding.Top : (num2 - padding.Top));
				num3 = ((padding.Right > num3) ? padding.Right : (num3 - padding.Right));
				num4 = ((padding.Bottom > num4) ? padding.Bottom : (num4 - padding.Bottom));
			}
			return new RectangleD(rect.X + num, rect.Y + num2, Math.Max(1.0, rect.Width - num - num3), Math.Max(1.0, rect.Height - num2 - num4));
		}
		return rect;
	}
}
