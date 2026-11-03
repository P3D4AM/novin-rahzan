using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRightAxisLabelsCoreXF : StiCenterAxisLabelsCoreXF
{
	public override string LocalizedName => StiLocalization.Get("PropertyMain", "Right");

	public override int Position => 9;

	protected override RectangleF GetLabelRect(StiContext context, PointF endPoint, PointF startPoint, double? value, string labelText, bool checkHeight, StiFontGeom font, StiStringFormatGeom sf)
	{
		SizeF sizeF = context.MeasureString(labelText, font);
		if (base.SeriesLabels.Width > 0)
		{
			sizeF.Width = (float)base.SeriesLabels.Width * context.Options.Zoom;
		}
		else
		{
			sizeF.Width += context.Options.Zoom * 2f;
		}
		if (!((IStiAxisArea)base.SeriesLabels.Chart.Area).ReverseHor)
		{
			return context.MeasureRotatedString(labelText, font, new PointF(endPoint.X + sizeF.Width, endPoint.Y), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
		}
		return context.MeasureRotatedString(labelText, font, new PointF(endPoint.X - sizeF.Width, endPoint.Y), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
	}

	public StiRightAxisLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
