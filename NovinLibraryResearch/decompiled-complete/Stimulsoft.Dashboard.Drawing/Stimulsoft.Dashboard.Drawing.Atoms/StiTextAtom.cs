using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.Text;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiTextAtom : StiElementAtom
{
	internal string Text { get; set; }

	protected StiTextElement TextElement => base.Element as StiTextElement;

	public override void Paint(Graphics g)
	{
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		double num = 1.0;
		RectangleD rect = base.Location;
		double? scale = null;
		if (TextElement.SizeMode == StiTextSizeMode.Fit)
		{
			SizeD sizeD = StiAutoSizeHtmlTextHelper.Measure(g, base.Location, Text, TextElement);
			double val = ((sizeD.Width > rect.Width) ? (rect.Width / sizeD.Width) : 1.0);
			double val2 = ((sizeD.Height > rect.Height) ? (rect.Height / sizeD.Height) : 1.0);
			num = Math.Min(val, val2);
			if (num < 1.0)
			{
				num *= 0.99;
				rect = new RectangleD(rect.X + (rect.Width - sizeD.Width * num) / 2.0, rect.Y + (rect.Height - sizeD.Height * num) / 2.0, sizeD.Width * num, sizeD.Height * num);
			}
			scale = num;
		}
		Color foreColor = StiDashboardExpressionHelper.GetForeColor(TextElement, TextElement.ForeColor);
		Color value = ((foreColor != Color.Transparent) ? foreColor : StiDashboardStyleHelper.GetNativeForeColor(base.Element));
		bool wordWrap = TextElement.SizeMode == StiTextSizeMode.WordWrap;
		StringTrimming trimming = (StringTrimming)((TextElement.SizeMode == StiTextSizeMode.Trimming) ? 3 : 0);
		StiAutoSizeHtmlTextHelper.Draw(g, rect, Text, TextElement, scale, value, wordWrap, trimming);
	}

	public string GetHyperlinkUnderMouse(Point mousePosition)
	{
		Graphics tempGraphics = GetTempGraphics();
		SizeD sizeD = StiAutoSizeHtmlTextHelper.Measure(tempGraphics, base.Location, Text, TextElement);
		RectangleD rect = base.Location;
		double val = ((sizeD.Width > rect.Width) ? (rect.Width / sizeD.Width) : 1.0);
		double val2 = ((sizeD.Height > rect.Height) ? (rect.Height / sizeD.Height) : 1.0);
		double num = Math.Min(val, val2);
		if (num < 1.0)
		{
			rect = new RectangleD(rect.X + (rect.Width - sizeD.Width * num) / 2.0, rect.Y + (rect.Height - sizeD.Height * num) / 2.0, sizeD.Width * num, sizeD.Height * num);
		}
		return StiAutoSizeHtmlTextHelper.GetHyperlinkUnderMouse(tempGraphics, rect, Text, TextElement, mousePosition, num);
	}

	private static Graphics GetTempGraphics()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
		{
			return Graphics.FromHwnd(IntPtr.Zero);
		}
		Bitmap val = new Bitmap(1, 1);
		return Graphics.FromImage((Image)(object)val);
	}

	public override void Render()
	{
		if (StiOptions.Engine.DashboardTextElementExpressionParser == StiParserType.ReportParser || StiExpressionHelper.IsTimeExpression(TextElement.Text))
		{
			Text = StiReportParser.Parse(TextElement.Text, TextElement, allowCache: false);
		}
		else if (TextElement.IsQuerable)
		{
			List<string> list = DataTable?.Rows.FirstOrDefault()?.Select((object r) => StiValueHelper.TryToString(r)).ToList();
			if (list != null)
			{
				Text = StiExpressionHelper.ReplaceExpressionBlocksByValues(TextElement.Text, list);
			}
		}
		else
		{
			Text = TextElement.Text;
		}
	}

	public StiTextAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}
}
