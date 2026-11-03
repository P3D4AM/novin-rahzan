using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiValueAxisLabelsCoreXF : StiCenterAxisLabelsCoreXF
{
	public override string LocalizedName => StiLocalization.Get("PropertyMain", "Value");

	public override int Position => 8;

	protected override RectangleF GetLabelRect(StiContext context, PointF endPoint, PointF startPoint, double? value, string labelText, bool checkHeight, StiFontGeom font, StiStringFormatGeom sf)
	{
		Rectangle rectangle = Rectangle.Round(context.MeasureRotatedString(labelText, font, new PointF(0f, 0f), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom)));
		return new RectangleF(endPoint.X - (float)(rectangle.Width / 2), endPoint.Y - (float)(rectangle.Height / 2), rectangle.Width, rectangle.Height);
	}

	public StiValueAxisLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
