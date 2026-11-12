using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Text;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Helpers;

public static class StiAutoSizeHtmlTextHelper
{
	internal static void Draw(Graphics g, RectangleD rect, string text, object textObj, double? scale = null, Color? defaultTextColor = null, bool wordWrap = false, StringTrimming trimming = 0)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		StiTextOptions defaultTextOptions = GetDefaultTextOptions(textObj);
		defaultTextOptions.WordWrap = wordWrap;
		defaultTextOptions.Trimming = trimming;
		StiTextHorAlignment horAlignment = GetHorAlignment(textObj);
		StiVertAlignment verAlignment = GetVerAlignment(textObj);
		double textScale = GetTextScale(scale, textObj);
		Font val = new Font("Arial", 12f);
		try
		{
			if (StiOptions.Engine.UseNewHtmlEngine)
			{
				StiPage page = new StiPage();
				StiText textBox = new StiText
				{
					ClientRectangle = rect.Multiply(1.0 / (textScale * StiScale.Factor)),
					Text = text,
					Font = val,
					TextBrush = new StiSolidBrush(defaultTextColor.GetValueOrDefault(Color.Black)),
					AllowHtmlTags = true,
					TextOptions = defaultTextOptions,
					HorAlignment = horAlignment,
					VertAlignment = verAlignment,
					Page = page
				};
				StiHtmlTextRender.DrawString(g, rect, text, textBox, textScale);
			}
			else
			{
				StiTextRenderer.DrawText(g, text, val, rect, defaultTextColor.GetValueOrDefault(Color.Black), Color.Transparent, 1.0, horAlignment, verAlignment, defaultTextOptions.WordWrap, defaultTextOptions.RightToLeft, textScale, defaultTextOptions.Angle, defaultTextOptions.Trimming, defaultTextOptions.LineLimit, allowHtmlTags: true, defaultTextOptions);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static string GetHyperlinkUnderMouse(Graphics g, RectangleD rect, string text, object textObj, Point point, double scale)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		StiTextOptions defaultTextOptions = GetDefaultTextOptions(textObj);
		StiTextHorAlignment horAlignment = GetHorAlignment(textObj);
		StiVertAlignment verAlignment = GetVerAlignment(textObj);
		double textScale = GetTextScale(scale, textObj);
		List<StiTextRenderer.RunInfo> list = new List<StiTextRenderer.RunInfo>();
		List<StiTextRenderer.StiFontState> list2 = new List<StiTextRenderer.StiFontState>();
		Font val = new Font("Arial", 12f);
		try
		{
			StiTextRenderer.DrawTextForOutput(g, text, val, rect, Color.Black, Color.Transparent, 1.0, horAlignment, verAlignment, defaultTextOptions.WordWrap, defaultTextOptions.RightToLeft, textScale, defaultTextOptions.Angle, defaultTextOptions.Trimming, defaultTextOptions.LineLimit, allowHtmlTags: true, list, list2, defaultTextOptions);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		PointD pointD = new PointD(((double)point.X - rect.X) * scale, ((double)point.Y - rect.Y) * scale);
		foreach (StiTextRenderer.RunInfo item in list)
		{
			if (item.Href != null && pointD.X >= item.XPos && pointD.Y > item.YPos && pointD.Y < item.YPos + list2[item.FontIndex].LineHeight)
			{
				int num = item.Widths.Sum();
				if (pointD.X < item.XPos + (double)num)
				{
					return item.Href;
				}
			}
		}
		return null;
	}

	internal static SizeD Measure(Graphics g, RectangleD rect, string text, object textObj, double? scale = null)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		StiTextOptions defaultTextOptions = GetDefaultTextOptions(textObj);
		double textScale = GetTextScale(scale, textObj);
		rect.Height = 10000.0;
		Font val = new Font("Arial", 12f);
		SizeD result;
		try
		{
			if (StiOptions.Engine.UseNewHtmlEngine)
			{
				StiPage page = new StiPage();
				StiText textBox = new StiText
				{
					ClientRectangle = rect.Multiply(1.0 / StiScale.Factor),
					Text = text,
					Font = val,
					AllowHtmlTags = true,
					TextOptions = defaultTextOptions,
					Page = page
				};
				result = StiHtmlTextRender.MeasureString(textBox);
				result = result.Multiply(StiScale.Factor * textScale);
			}
			else
			{
				result = StiTextRenderer.MeasureText(g, text, val, rect, 1.0, defaultTextOptions.WordWrap, defaultTextOptions.RightToLeft, textScale, defaultTextOptions.Angle, defaultTextOptions.Trimming, defaultTextOptions.LineLimit, allowHtmlTags: true, defaultTextOptions);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return result;
	}

	internal static StiTextOptions GetDefaultTextOptions(object textObj = null)
	{
		StiTextOptions stiTextOptions = new StiTextOptions
		{
			Angle = 0f,
			HotkeyPrefix = (HotkeyPrefix)0,
			LineLimit = false,
			RightToLeft = false,
			Trimming = (StringTrimming)0,
			WordWrap = false
		};
		if (textObj is StiTextElement stiTextElement)
		{
			stiTextOptions.RightToLeft = stiTextElement.RightToLeft;
		}
		return stiTextOptions;
	}

	internal static StiTextHorAlignment GetHorAlignment(object textObj)
	{
		if (textObj is IStiTextHorAlignment)
		{
			return ((IStiTextHorAlignment)textObj).HorAlignment;
		}
		if (textObj is IStiHorAlignment)
		{
			IStiHorAlignment stiHorAlignment = textObj as IStiHorAlignment;
			switch (stiHorAlignment.HorAlignment)
			{
			case StiHorAlignment.Left:
				return StiTextHorAlignment.Left;
			case StiHorAlignment.Right:
				return StiTextHorAlignment.Right;
			case StiHorAlignment.Center:
				return StiTextHorAlignment.Center;
			}
		}
		return StiTextHorAlignment.Left;
	}

	internal static StiVertAlignment GetVerAlignment(object textObj)
	{
		if (!(textObj is IStiVertAlignment))
		{
			return StiVertAlignment.Top;
		}
		return ((IStiVertAlignment)textObj).VertAlignment;
	}

	private static double GetTextScale(double? scale, object textObject)
	{
		double num = ((textObject is IStiElement { IsDesigning: not false }) ? ((IStiElement)textObject).Zoom : 1.0);
		return scale.GetValueOrDefault(1.0) * num;
	}
}
