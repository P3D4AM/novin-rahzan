using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Helpers;

internal static class StiElementControlPainter
{
	internal static void PaintBackground(Graphics g, Rectangle rect, IStiElement element)
	{
		StiDrawing.FillRectangle(g, StiDashboardStyleHelper.GetBackColor(element), rect);
	}

	internal static void PaintBorder(Graphics g, Rectangle rect, StiSimpleBorder border, Color backColor)
	{
		if (border != null)
		{
			StiBorderGdiPainter.PaintBorder(g, rect, border, backColor, isTooLightBorderAdoptation: true);
		}
	}

	internal static RectangleD GetBorderContentRect(RectangleD rect, StiSimpleBorder border)
	{
		return StiBorderElementHelper.GetBorderContentRect(rect, border, StiScale.Factor, skipMinimalSize: false);
	}

	internal static Rectangle GetBorderContentRect(Rectangle rect, StiSimpleBorder border)
	{
		return GetBorderContentRect(RectangleD.CreateFromRectangle(rect), border).ToRectangle();
	}

	internal static RectangleD GetBorderContentRect(RectangleD rect, StiBorder border)
	{
		return StiBorderElementHelper.GetBorderContentRect(rect, border, StiScale.Factor, skipMinimalSize: false);
	}

	internal static Rectangle GetBorderContentRect(Rectangle rect, StiBorder border)
	{
		return GetBorderContentRect(RectangleD.CreateFromRectangle(rect), border).ToRectangle();
	}

	internal static Rectangle PaintTitle(Graphics g, StiElement element, Rectangle rect, StiTitle title)
	{
		if (!(element is IStiTitleElement element2))
		{
			return Rectangle.Empty;
		}
		string titleText = StiReportParser.Parse(title?.Text, element);
		return StiElementGdiPainter.PaintTitle(g, RectangleD.CreateFromRectangle(rect), element2, titleText, 1.0, isViewer: true).ToRectangle();
	}

	internal static RectangleD GetTitleLocation(StiElement element, RectangleD rect, StiTitle title)
	{
		string titleText = StiReportParser.Parse(title?.Text, element);
		double maxHeight = ((title != null && title.SizeMode == StiTextSizeMode.WordWrap) ? (rect.Height / 2.0) : 0.0);
		SizeD sizeD = StiTitleMeasureHelper.MeasureTitle(rect, title, titleText, 1.0, maxHeight);
		if (!sizeD.IsEmpty)
		{
			return new RectangleD(rect.X, rect.Y, rect.Width, sizeD.Height);
		}
		return RectangleD.Empty;
	}

	internal static Rectangle GetContentRect(IStiElement element, Rectangle rect)
	{
		StiSimpleBorder border = ((element is IStiSimpleBorder) ? (element as IStiSimpleBorder).Border : null);
		Rectangle borderContentRect = GetBorderContentRect(rect, border);
		StiTitle stiTitle = ((element is IStiTitleElement) ? ((element as IStiTitleElement).Title as StiTitle) : null);
		string titleText = StiReportParser.Parse(stiTitle?.Text, element as StiComponent);
		int num = ((stiTitle != null && stiTitle.SizeMode == StiTextSizeMode.WordWrap) ? (rect.Height / 2) : 0);
		SizeD sizeD = StiTitleMeasureHelper.MeasureTitle(RectangleD.CreateFromRectangle(rect), stiTitle, titleText, 1.0, num);
		if (!sizeD.IsEmpty)
		{
			borderContentRect.Y += (int)sizeD.Height;
			borderContentRect.Height -= (int)sizeD.Height;
		}
		RectangleD rect2 = RectangleD.CreateFromRectangle(borderContentRect);
		StiCornerRadius radius = (element as IStiCornerRadius)?.CornerRadius;
		double scale = StiElementScale.Factor(element);
		rect2 = StiCornerRadiusHelper.FitRectIntoCornerRadius(rect2, radius, scale, element is IStiFixedHeightElement);
		return StiPaddingHelper.ApplyPadding(element, rect2, StiScale.Factor).ToRectangle();
	}
}
