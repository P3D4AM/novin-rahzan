using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public static class StiConvert
{
	public static double XToPage(IStiDesignerBase designer, int x)
	{
		StiPage currentPage = designer.Report.GetCurrentPage();
		return currentPage.Unit.ConvertFromHInches((double)x / (currentPage.Zoom * StiScale.Factor)) - currentPage.Margins.Left;
	}

	public static double YToPage(IStiDesignerBase designer, int y)
	{
		StiPage currentPage = designer.Report.GetCurrentPage();
		return currentPage.Unit.ConvertFromHInches((double)y / (currentPage.Zoom * StiScale.Factor)) - currentPage.Margins.Top;
	}

	public static int XToScreen(IStiDesignerBase designer, double x)
	{
		StiPage currentPage = designer.Report.GetCurrentPage();
		return (int)(currentPage.Unit.ConvertToHInches(x + currentPage.Margins.Left) * (currentPage.Zoom * StiScale.Factor));
	}

	public static int YToScreen(IStiDesignerBase designer, double y)
	{
		StiPage currentPage = designer.Report.GetCurrentPage();
		return (int)(currentPage.Unit.ConvertToHInches(y + currentPage.Margins.Top) * (currentPage.Zoom * StiScale.Factor));
	}

	public static RectangleD RectangleToPage(IStiDesignerBase designer, Rectangle rectangle)
	{
		StiPage currentPage = designer.Report.GetCurrentPage();
		return new RectangleD(XToPage(designer, rectangle.X), YToPage(designer, rectangle.Y), currentPage.Unit.ConvertFromHInches((double)rectangle.Width) / (currentPage.Zoom * StiScale.Factor), currentPage.Unit.ConvertFromHInches((double)rectangle.Height) / (currentPage.Zoom * StiScale.Factor));
	}

	public static Rectangle RectangleToScreen(IStiDesignerBase designer, RectangleD rectangle)
	{
		StiPage currentPage = designer.Report.GetCurrentPage();
		return new Rectangle(XToScreen(designer, rectangle.X), YToScreen(designer, rectangle.Y), (int)(currentPage.Unit.ConvertToHInches(rectangle.Width) * (currentPage.Zoom * StiScale.Factor)), (int)(currentPage.Unit.ConvertToHInches(rectangle.Height) * (currentPage.Zoom * StiScale.Factor)));
	}
}
