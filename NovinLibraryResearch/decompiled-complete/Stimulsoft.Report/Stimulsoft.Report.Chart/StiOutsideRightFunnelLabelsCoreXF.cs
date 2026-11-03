using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiOutsideRightFunnelLabelsCoreXF : StiFunnelSeriesLabelsCoreXF
{
	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Funnel;

	public override int Position => 15;

	public override string LocalizedName => StiLocalization.Get("PropertyMain", "Right");

	public override StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, int pointIndex, double value, double valueNext, string argumentText, string tag, int colorIndex, int colorCount, RectangleF rect, float singleValueHeight, float singleValueWidth, float centerAxis, out RectangleF measureRect)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		measureRect = rect;
		string labelText = GetLabelText(series, value, argumentText, tag, series.CoreTitle);
		StiBrush labelBrush = (StiBrush)new StiSolidBrush(GetLabelColor(series, colorIndex, colorCount));
		Color borderColor = GetBorderColor(series, colorIndex, colorCount);
		StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context);
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.SeriesLabels.Font, base.SeriesLabels.Font.Size * context.Options.Zoom);
		Color seriesBorderColor = (Color)series.Core.GetSeriesBorderColor(colorIndex, colorCount);
		StiBrush seriesBrush = series.Core.GetSeriesBrush(colorIndex, colorCount);
		Rectangle rectangle = Rectangle.Round(context.MeasureRotatedString(labelText, font, new PointF(0f, 0f), stringFormatGeom, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom)));
		measureRect.Width -= rectangle.Width + 5;
		PointF startPointLine = new PointF(centerAxis + (float)(value + valueNext) * singleValueWidth / 4f, singleValueHeight * (float)pointIndex + singleValueHeight / 2f + rect.Height * 0.05f);
		PointF pointF = new PointF(rect.Width * 0.95f - (float)rectangle.Width, singleValueHeight * (float)pointIndex + singleValueHeight / 2f + rect.Height * 0.05f);
		RectangleF rectangleF = context.MeasureRotatedString(labelText, font, pointF, stringFormatGeom, (StiRotationMode)1, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
		return new StiOutsideFunnelLabelsGeom(base.SeriesLabels, series, pointIndex, value, rectangleF, labelText, seriesBrush, labelBrush, borderColor, seriesBorderColor, rectangleF, startPointLine, pointF);
	}

	public StiOutsideRightFunnelLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
