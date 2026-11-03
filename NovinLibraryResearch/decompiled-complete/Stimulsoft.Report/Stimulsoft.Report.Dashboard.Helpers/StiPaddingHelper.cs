using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.App;

namespace Stimulsoft.Report.Dashboard.Helpers;

public class StiPaddingHelper
{
	public static RectangleD ApplyPadding(IStiElement element, RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return ApplyPadding(element, rect, StiElementScale.Factor(element));
	}

	public static RectangleD ApplyPadding(IStiComponentUI comp, RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return ApplyPadding(comp, rect, StiElementScale.Factor(comp));
	}

	public static RectangleD ApplyPadding(object comp, RectangleD rect, double scale)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		StiPadding stiPadding = (comp as IStiPadding)?.Padding;
		if (stiPadding == null)
		{
			return rect;
		}
		((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + stiPadding.Left * scale;
		((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + stiPadding.Top * scale;
		((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (stiPadding.Left * scale + stiPadding.Right * scale);
		((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - (stiPadding.Top * scale + stiPadding.Bottom * scale);
		return rect;
	}
}
