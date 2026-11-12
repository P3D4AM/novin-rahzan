using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.Text;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiTextElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		if (RenderEmptyDataMessage(element, destination, rect, settings))
		{
			return;
		}
		StiTextElement stiTextElement = element as StiTextElement;
		string text = string.Empty;
		if (StiOptions.Engine.DashboardTextElementExpressionParser == StiParserType.ReportParser || StiExpressionHelper.IsTimeExpression(stiTextElement.Text))
		{
			text = StiReportParser.Parse(stiTextElement.Text, stiTextElement, allowCache: false);
		}
		else if (stiTextElement.IsQuerable)
		{
			List<string> list = StiElementDataCache.TryToGetOrCreate(stiTextElement)?.Rows.FirstOrDefault()?.Select((object r) => StiValueHelper.TryToString(r)).ToList();
			if (list != null)
			{
				text = StiExpressionHelper.ReplaceExpressionBlocksByValues(stiTextElement.Text, list);
			}
		}
		else
		{
			text = stiTextElement.Text;
		}
		if (!string.IsNullOrEmpty(text) && text.ToLower().Replace(" ", "").Contains("javascript:"))
		{
			text = text.ToLower().Replace("javascript", "");
		}
		if (stiTextElement.SizeMode == StiTextSizeMode.Fit)
		{
			int num = 3;
			double num2;
			do
			{
				SizeD sizeD = StiAutoSizeHtmlTextHelper.Measure(destination.GetMeasureGraphics(), rect, text, stiTextElement, StiDpiHelper.DeviceCapsScale);
				double value = ((sizeD.Width > rect.Width) ? (rect.Width / sizeD.Width) : 1.0);
				double value2 = ((sizeD.Height > rect.Height) ? (rect.Height / sizeD.Height) : 1.0);
				num2 = Math.Min(Math.Abs(value), Math.Abs(value2));
				if (num2 < 1.0)
				{
					text = ChangeFontSize(text, num2 * 0.99);
				}
			}
			while (num2 < 1.0 && num-- > 0);
		}
		destination.Components.Add(new StiText(rect)
		{
			Name = element.Name + "_Content",
			AllowHtmlTags = true,
			VertAlignment = stiTextElement.VertAlignment,
			HorAlignment = stiTextElement.HorAlignment,
			Text = text,
			TextBrush = new StiSolidBrush(StiDashboardStyleHelper.GetForeColor(stiTextElement)),
			Trimming = (StringTrimming)((stiTextElement.SizeMode == StiTextSizeMode.Trimming) ? 3 : 0),
			WordWrap = (stiTextElement.SizeMode == StiTextSizeMode.WordWrap)
		});
	}

	private string ChangeFontSize(string text, double factor)
	{
		if (text == null || !text.StartsWith("<font ") || text.IndexOf(" size=\"") < 0)
		{
			return text;
		}
		int num = text.IndexOf(" size=\"") + 7;
		string text2 = text.Substring(num);
		text2 = text2.Substring(0, text2.IndexOf("\"")).Replace(",", ".").Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
		double result = 12.0;
		double.TryParse(text2, out result);
		result = Math.Ceiling(result * factor * 10.0) / 10.0;
		return text.Substring(0, num) + result.ToString().Replace(",", ".") + text.Substring(num + text2.Length);
	}
}
