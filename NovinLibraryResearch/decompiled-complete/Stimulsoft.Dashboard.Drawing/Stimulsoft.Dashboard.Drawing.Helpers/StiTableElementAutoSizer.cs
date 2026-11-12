using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Helpers;

public class StiTableElementAutoSizer : IStiTableElementAutoSizer
{
	public void AutoMeasureToTable(IStiTableElement tableElement)
	{
		StiTableElement stiTableElement = tableElement as StiTableElement;
		double num = StiTableElementGdiPainter.MeasureHeaders(stiTableElement, tableElement.HeaderFont).Width;
		if (num > stiTableElement.Width * 0.699999988079071)
		{
			num = (float)stiTableElement.Width * 0.7f;
		}
		stiTableElement.Width = StiAlignValue.AlignToMaxGrid(num, Grid.SizeInPoints(stiTableElement), Grid.IsAlign(stiTableElement));
	}

	public void AutoMeasureToColumn(IStiTableElement tableElement)
	{
		StiTableElement stiTableElement = tableElement as StiTableElement;
		double width = StiTableElementGdiPainter.MeasureHeaders(stiTableElement, stiTableElement.HeaderFont).Width;
		if (width > stiTableElement.Width && width + stiTableElement.Left < stiTableElement.Page.Width)
		{
			RectangleD rectangleD = new RectangleD(stiTableElement.Left, stiTableElement.Top, StiAlignValue.AlignToMaxGrid(width, Grid.SizeInPoints(stiTableElement), Grid.IsAlign(stiTableElement)), stiTableElement.Height);
			if (!StiComponentIntersectionHelper.IsIntersect(rectangleD, stiTableElement.Page, stiTableElement))
			{
				stiTableElement.ClientRectangle = rectangleD;
			}
		}
	}
}
