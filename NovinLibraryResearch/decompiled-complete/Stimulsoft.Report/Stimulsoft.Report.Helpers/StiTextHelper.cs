using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Helpers;

public static class StiTextHelper
{
	public static string HtmlToPlainText(string html)
	{
		Regex regex = new Regex("<(br|BR)\\s{0,1}\\/{0,1}>", RegexOptions.Multiline);
		Regex regex2 = new Regex("<[^>]*(>|$)", RegexOptions.Multiline);
		Regex regex3 = new Regex("(>|$)(\\W|\\n|\\r)+<", RegexOptions.Multiline);
		string value = html;
		value = WebUtility.HtmlDecode(value);
		value = regex3.Replace(value, "><");
		value = regex.Replace(value, Environment.NewLine);
		return regex2.Replace(value, string.Empty);
	}

	public static string CheckHyperlinkFromHtmlText(StiComponent comp, RectangleD rect, Point point)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		if (!(comp is StiText stiText) || !stiText.CheckAllowHtmlTags() || string.IsNullOrWhiteSpace(stiText.Text))
		{
			return null;
		}
		try
		{
			RectangleD rect2 = stiText.Page.Unit.ConvertToHInches(stiText.ComponentToPage(stiText.ClientRectangle));
			RectangleD rect3 = stiText.ConvertTextMargins(rect2, convert: false);
			rect3 = stiText.ConvertTextBorders(rect3, convert: false);
			StiTextOptions textOptions = ((IStiTextOptions)stiText).TextOptions;
			Graphics val;
			if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
			{
				val = Graphics.FromHwnd(IntPtr.Zero);
			}
			else
			{
				Bitmap val2 = new Bitmap(1, 1);
				val = Graphics.FromImage((Image)(object)val2);
			}
			List<RunInfo> list = new List<RunInfo>();
			List<StiFontState> list2 = new List<StiFontState>();
			StiTextRenderer.DrawTextForOutput(val, (string)stiText.Text, stiText.Font, rect3, StiBrush.ToColor(stiText.TextBrush), StiBrush.ToColor(stiText.Brush), (double)(stiText.LineSpacing * StiOptions.Engine.TextLineSpacingScale), stiText.HorAlignment, stiText.VertAlignment, textOptions.WordWrap, textOptions.RightToLeft, 1.0, (double)textOptions.Angle, textOptions.Trimming, textOptions.LineLimit, stiText.CheckAllowHtmlTags(), list, list2, textOptions, (double?)1.0);
			double num = ((RectangleD)(ref rect2)).Width / ((RectangleD)(ref rect)).Width;
			PointD val3 = default(PointD);
			((PointD)(ref val3))._002Ector(((double)point.X - ((RectangleD)(ref rect)).X) * num, ((double)point.Y - ((RectangleD)(ref rect)).Y) * num);
			foreach (RunInfo item in list)
			{
				if (item.Href != null && ((PointD)(ref val3)).X >= item.XPos && ((PointD)(ref val3)).Y > item.YPos && ((PointD)(ref val3)).Y < item.YPos + list2[item.FontIndex].LineHeight)
				{
					int num2 = item.Widths.Sum();
					if (((PointD)(ref val3)).X < item.XPos + (double)num2)
					{
						return item.Href;
					}
				}
			}
		}
		catch
		{
		}
		return null;
	}
}
