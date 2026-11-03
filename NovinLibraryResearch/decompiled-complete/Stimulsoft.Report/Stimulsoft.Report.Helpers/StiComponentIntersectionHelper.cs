using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Helpers;

public static class StiComponentIntersectionHelper
{
	public static bool IsIntersect(RectangleD rect, StiPage page, StiComponent skipComp)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		foreach (StiComponent component in page.GetComponents())
		{
			if (component != skipComp)
			{
				RectangleD paintRectangle = component.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
				if (((RectangleD)(ref paintRectangle)).IntersectsWith(rect))
				{
					return true;
				}
			}
		}
		return false;
	}
}
