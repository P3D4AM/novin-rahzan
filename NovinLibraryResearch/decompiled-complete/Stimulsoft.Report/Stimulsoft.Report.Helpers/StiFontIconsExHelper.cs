using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Helpers;

public static class StiFontIconsExHelper
{
	public static bool NeedToUseStimulsoftFont(StiReport report)
	{
		if (report != null)
		{
			foreach (StiComponent component in report.GetComponents())
			{
				if (!component.Enabled)
				{
					continue;
				}
				StiImage obj = component as StiImage;
				if (obj == null || !obj.Icon.HasValue)
				{
					IStiImageElement obj2 = component as IStiImageElement;
					if ((obj2 == null || !obj2.Icon.HasValue) && !(component is IStiIndicatorElement))
					{
						if (component is StiChart stiChart && stiChart.Series.Count > 0 && (stiChart.Series[0] is StiPictorialSeries || stiChart.Series[0] is IStiFontIconsSeries))
						{
							return true;
						}
						if (component is IStiChartElement { IsPictorialChart: not false })
						{
							return true;
						}
						continue;
					}
				}
				return true;
			}
		}
		return false;
	}

	internal static float GetIconFontSize(StiContext context, SizeF size, string text)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		SizeF sizeF = context.MeasureString(text, new StiFontGeom(new Font(fontFamilyIcons, 1f)));
		if (sizeF.Width == 0f || sizeF.Height == 0f)
		{
			return 0f;
		}
		float num = size.Width / sizeF.Width;
		float num2 = size.Height / sizeF.Height;
		return (num > num2) ? num2 : num;
	}

	internal static void DrawFillIcons(StiContext context, object brush, RectangleF rect, SizeF singleSize, StiFontIcons icon, string toolTip)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		string content = StiFontIconsHelper.GetContent((StiFontIcons?)icon);
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		float iconFontSize = GetIconFontSize(context, singleSize, content);
		Font val = new Font(fontFamilyIcons, iconFontSize);
		StiFontGeom font = new StiFontGeom(val.FontFamily, val.FontFamily.Name, val.Size, val.Style, val.Unit, val.GdiCharSet, val.GdiVerticalFont);
		float num = rect.Y;
		float num2 = rect.X;
		for (; num < rect.Bottom; num += singleSize.Height)
		{
			for (; num2 < rect.Right; num2 += singleSize.Width)
			{
				RectangleF rect2 = new RectangleF(num2, num, singleSize.Width, singleSize.Height);
				context.DrawString(content, font, brush, rect2, GetStringFormatGeom(context), antialiasing: true, toolTip);
			}
			num2 = rect.X;
		}
	}

	internal static void DrawDirectionIcons(StiContext context, object brush, RectangleF rect, SizeF singleSize, StiFontIcons icon, string toolTip, bool verticalDirection, bool roundValues = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		string content = StiFontIconsHelper.GetContent((StiFontIcons?)icon);
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		float iconFontSize = GetIconFontSize(context, singleSize, content);
		if (iconFontSize <= 0f)
		{
			return;
		}
		Font val = new Font(fontFamilyIcons, iconFontSize);
		StiFontGeom font = new StiFontGeom(val.FontFamily, val.FontFamily.Name, val.Size, val.Style, val.Unit, val.GdiCharSet, val.GdiVerticalFont);
		if (!roundValues)
		{
			context.PushClip(rect);
		}
		if (verticalDirection)
		{
			float num = rect.Bottom;
			float x = rect.X;
			bool flag = true;
			while (num > rect.Top)
			{
				RectangleF rect2 = new RectangleF(x, num - singleSize.Height, singleSize.Width, singleSize.Height);
				if (flag && roundValues)
				{
					flag = false;
					context.DrawString(content, font, brush, rect2, GetStringFormatGeom(context), antialiasing: true, toolTip);
				}
				else if (!roundValues || (roundValues && num - rect.Top > singleSize.Height / 2f))
				{
					context.DrawString(content, font, brush, rect2, GetStringFormatGeom(context), antialiasing: true, toolTip);
				}
				num -= singleSize.Height;
			}
		}
		else
		{
			float y = rect.Y;
			for (float num2 = rect.X; num2 < rect.Right; num2 += singleSize.Width)
			{
				RectangleF rect3 = new RectangleF(num2, y, singleSize.Width, singleSize.Height);
				bool flag2 = true;
				if (flag2 && roundValues)
				{
					flag2 = false;
					context.DrawString(content, font, brush, rect3, GetStringFormatGeom(context), antialiasing: true, toolTip);
				}
				else if (!roundValues || (roundValues && rect.Right - num2 > singleSize.Width / 2f))
				{
					context.DrawString(content, font, brush, rect3, GetStringFormatGeom(context), antialiasing: true, toolTip);
				}
			}
		}
		if (!roundValues)
		{
			context.PopClip();
		}
	}

	private static StiStringFormatGeom GetStringFormatGeom(StiContext context)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		StiStringFormatGeom defaultStringFormat = context.GetDefaultStringFormat();
		defaultStringFormat.Trimming = (StringTrimming)0;
		defaultStringFormat.FormatFlags = (StringFormatFlags)(defaultStringFormat.FormatFlags | 0x1800);
		defaultStringFormat.Alignment = (StringAlignment)1;
		defaultStringFormat.LineAlignment = (StringAlignment)1;
		return defaultStringFormat;
	}
}
