using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Export.Helpers;

public static class StiTitleExportHelper
{
	public static RectangleD Render(IStiElement element, StiPanel component, StiDashboardExportSettings settings)
	{
		IStiTitleElement stiTitleElement = element as IStiTitleElement;
		RectangleD borderContentRect = StiElementControlPainter.GetBorderContentRect(new RectangleD(0.0, 0.0, component.Width, component.Height), component.Border);
		IStiTitle stiTitle = stiTitleElement?.Title;
		string text = StiReportParser.Parse(stiTitle?.Text, element as StiComponent);
		if (stiTitle == null || !stiTitle.Visible || string.IsNullOrEmpty(text) || StiExportSettingsHelper.IsDataExport(settings))
		{
			return borderContentRect;
		}
		SizeD sizeD = StiTitleMeasureHelper.MeasureTitle(borderContentRect, stiTitle, text, 1.0, borderContentRect.Height / 2.0);
		sizeD.Width /= StiScale.Factor;
		sizeD.Height /= StiScale.Factor;
		double num = 1.0;
		if (stiTitle.SizeMode == StiTextSizeMode.Fit)
		{
			double value = ((sizeD.Width > borderContentRect.Width) ? (borderContentRect.Width / sizeD.Width) : 1.0);
			double value2 = ((sizeD.Height > borderContentRect.Height) ? (borderContentRect.Height / sizeD.Height) : 1.0);
			num = Math.Min(Math.Abs(value), Math.Abs(value2));
		}
		Color color = ((stiTitle.ForeColor == Color.Transparent) ? StiDashboardStyleHelper.GetTitleForeColor(element) : stiTitle.ForeColor);
		bool allowOpacity = settings is StiImageDashboardExportSettings || settings is StiHtmlDashboardExportSettings || settings is StiPdfDashboardExportSettings;
		Color color2 = ((stiTitle.BackColor == Color.Transparent) ? StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity) : stiTitle.BackColor);
		Color backColor = StiDashboardStyleHelper.GetBackColor(element, null, allowOpacity: true);
		if (backColor.A != byte.MaxValue && color2.Equals((object?)backColor))
		{
			color2 = Color.Transparent;
		}
		StiText stiText = new StiText
		{
			Name = element.Name + "_Title",
			ClientRectangle = new RectangleD(borderContentRect.X, borderContentRect.Y, borderContentRect.Width, sizeD.Height),
			Text = StiReportParser.Parse(text, element),
			Font = StiFontUtils.ChangeFontSize(stiTitle.Font, (float)((double)stiTitle.Font.Size * num)),
			HorAlignment = Convert(stiTitle.HorAlignment),
			VertAlignment = StiVertAlignment.Center,
			TextBrush = new StiSolidBrush(color),
			Brush = new StiSolidBrush(color2),
			Trimming = (StringTrimming)((stiTitle.SizeMode == StiTextSizeMode.Trimming) ? 3 : 0),
			WordWrap = (stiTitle.SizeMode == StiTextSizeMode.WordWrap)
		};
		borderContentRect.Height -= stiText.Height;
		borderContentRect.Y += stiText.Height;
		StiCornerRadius stiCornerRadius = (element as IStiCornerRadius)?.CornerRadius;
		if (stiCornerRadius != null)
		{
			if (stiCornerRadius.TopLeft > 0f)
			{
				stiText.Margins.Left = Math.Max(stiText.Margins.Left, stiCornerRadius.TopLeft / 2f);
			}
			if (stiCornerRadius.TopRight > 0f)
			{
				stiText.Margins.Right = Math.Max(stiText.Margins.Right, stiCornerRadius.TopRight / 2f);
			}
		}
		component.Components.Add(stiText);
		return borderContentRect;
	}

	public static StiTextHorAlignment Convert(StiHorAlignment alignment)
	{
		return alignment switch
		{
			StiHorAlignment.Left => StiTextHorAlignment.Left, 
			StiHorAlignment.Center => StiTextHorAlignment.Center, 
			StiHorAlignment.Right => StiTextHorAlignment.Right, 
			_ => throw new StiTypeNotRecognizedException(alignment), 
		};
	}
}
