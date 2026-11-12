using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Controls;

internal class StiPageViewHelper
{
	public static int PageViewHorDistance = (int)Math.Ceiling(10.0 * StiScale.X);

	public static int PageViewVerDistance = (int)Math.Ceiling(10.0 * StiScale.Y);

	internal static Rectangle GetPageViewLocation(StiPage page)
	{
		if (page == null)
		{
			return Rectangle.Empty;
		}
		double factor = StiScale.Factor;
		int num = (int)(page.Unit.ConvertToHInches(page.DisplayRectangle.Width) * page.Zoom * factor);
		int num2 = (int)(page.Unit.ConvertToHInches(page.DisplayRectangle.Height) * page.Zoom * factor);
		if (page.IsForm && page.IsSelected)
		{
			num = Math.Max(num + (int)(page.OffsetRectangle.Width * factor), StiScale.I(112));
			num2 = Math.Max(num2 + (int)(page.OffsetRectangle.Height * factor), StiScale.I(32));
		}
		return new Rectangle(PageViewHorDistance, PageViewVerDistance, num, num2);
	}

	internal static StiMargins GetPageViewMargins(StiPage page)
	{
		if (page == null)
		{
			return new StiMargins();
		}
		if (page.IsForm)
		{
			return new StiMargins(0.0, 0.0, 0.0, 0.0);
		}
		double factor = StiScale.Factor;
		int num = (int)(page.Unit.ConvertToHInches(page.Margins.Left) * page.Zoom * factor);
		int num2 = (int)(page.Unit.ConvertToHInches(page.Margins.Right) * page.Zoom * factor);
		int num3 = (int)(page.Unit.ConvertToHInches(page.Margins.Top) * page.Zoom * factor);
		int num4 = (int)(page.Unit.ConvertToHInches(page.Margins.Bottom) * page.Zoom * factor);
		return new StiMargins(num, num2, num3, num4);
	}
}
