using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Helpers;

public class StiTitleMeasureHelper
{
	public static SizeD MeasureTitle(IStiTitle title)
	{
		return MeasureTitle(title, 1.0);
	}

	public static SizeD MeasureTitle(IStiTitle title, double zoom = 1.0)
	{
		return MeasureTitle(null, title, title?.Text, zoom);
	}

	public static SizeD MeasureTitle(RectangleD? rect, IStiTitle title, string titleText, double zoom = 1.0, double maxHeight = 0.0)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				return MeasureTitle(val2, rect, title, titleText, zoom, maxHeight);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static SizeD MeasureTitle(Graphics g, RectangleD? rect, IStiTitle title, string titleText, double zoom = 1.0, double maxHeight = 0.0)
	{
		if (title == null || !title.Visible)
		{
			return SizeD.Empty;
		}
		double num = (double)StiScale.I(30) * zoom;
		double width = rect.GetValueOrDefault(RectangleD.Empty).Width;
		if (string.IsNullOrWhiteSpace(titleText))
		{
			return new SizeD(width, num);
		}
		Font val = StiFontUtils.ChangeFontSize(title.Font, (float)((double)title.Font.Size * zoom));
		try
		{
			SizeD sizeD = ((title.SizeMode == StiTextSizeMode.WordWrap) ? StiAutoSizeTextHelper.Measure(g, titleText, val, width) : StiAutoSizeTextHelper.Measure(g, titleText, val));
			num = Math.Max(num, sizeD.Height);
			if (maxHeight > 0.0)
			{
				num = Math.Min(num, maxHeight * zoom);
			}
			if (rect.HasValue)
			{
				num = Math.Min(num, rect.Value.Height);
			}
			return new SizeD(sizeD.Width, num);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static SizeD MeasureTitle(Graphics g, RectangleD? rect, Font font, string titleText)
	{
		double num = StiScale.YY(30.0);
		if (string.IsNullOrWhiteSpace(titleText))
		{
			return new SizeD(rect.GetValueOrDefault(RectangleD.Empty).Width, num);
		}
		SizeD sizeD = StiAutoSizeTextHelper.Measure(g, titleText, font);
		num = Math.Max(num, sizeD.Height);
		if (rect.HasValue)
		{
			num = Math.Min(num, rect.Value.Height);
		}
		return new SizeD(sizeD.Width, num);
	}
}
