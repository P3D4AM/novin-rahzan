using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Helpers;

public class StiMarginHelper
{
	public static RectangleD ApplyMargin(IStiElement element, RectangleD rect, bool isScale = false)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		double scale = (isScale ? (StiElementScale.Factor(element) * StiScale.Factor) : 1.0);
		return ApplyMargin(element, rect, scale);
	}

	public static RectangleD ApplyMargin(IStiElement element, RectangleD rect, double scale)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		StiMargin stiMargin = (element as IStiMargin)?.Margin;
		if (stiMargin == null)
		{
			return rect;
		}
		((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + stiMargin.Left * scale;
		((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + stiMargin.Top * scale;
		((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (stiMargin.Left * scale + stiMargin.Right * scale);
		((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - (stiMargin.Top * scale + stiMargin.Bottom * scale);
		return rect;
	}
}
