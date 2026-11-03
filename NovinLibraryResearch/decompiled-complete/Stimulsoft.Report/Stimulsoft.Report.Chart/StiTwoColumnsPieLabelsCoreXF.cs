using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiTwoColumnsPieLabelsCoreXF : StiOutsidePieLabelsCoreXF
{
	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Pie | StiSeriesLabelsType.Doughnut;

	public override int Position => 13;

	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsTwoColumns");

	public override StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, PointF centerPie, float radius, float radius2, float pieAngle, int pointIndex, double? value, double? labelValue, string argumentText, string tag, bool measure, int colorIndex, int colorCount, float percentPerValue, out RectangleF measureRect, bool drawValue, float deltaY)
	{
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		base.percentPerValue = percentPerValue;
		measureRect = RectangleF.Empty;
		if (!labelValue.HasValue && !base.SeriesLabels.ShowNulls)
		{
			return null;
		}
		if (labelValue == 0.0 && !base.SeriesLabels.ShowZeros)
		{
			return null;
		}
		if (!value.HasValue)
		{
			value = 0.0;
		}
		if (!labelValue.HasValue)
		{
			labelValue = 0.0;
		}
		if (float.IsNaN(radius) || float.IsNaN(radius2))
		{
			return null;
		}
		if (drawValue)
		{
			return base.RenderLabel(series, context, centerPie, radius, radius2, pieAngle, pointIndex, value, labelValue, GetFormattedValue(series, labelValue.Value), tag, measure, colorIndex, colorCount, percentPerValue, out measureRect, drawValue, deltaY);
		}
		string labelText = GetLabelText(series, labelValue, argumentText, tag, series.CoreTitle);
		float angleRad = (float)(Math.PI * (double)pieAngle / 180.0);
		PointF labelPoint = GetLabelPoint(centerPie, radius, angleRad);
		PointF labelPoint2 = GetLabelPoint(centerPie, radius + ((IStiTwoColumnsPieLabels)base.SeriesLabels).LineLength * context.Options.Zoom, angleRad);
		int num = 0;
		num = ((!(labelPoint.X > labelPoint2.X)) ? 1 : (-1));
		PointF pointF = new PointF(centerPie.X + radius * (float)num + ((IStiTwoColumnsPieLabels)base.SeriesLabels).LineLength * (float)num + (float)(10 * num), labelPoint2.Y);
		pointF.Y += deltaY * Math.Min(context.Options.Zoom, 1f);
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.SeriesLabels.Font, base.SeriesLabels.Font.Size * context.Options.Zoom);
		StiStringFormatGeom stringFormatGeom = base.SeriesLabels.Core.GetStringFormatGeom(context);
		stringFormatGeom.Alignment = (StringAlignment)1;
		stringFormatGeom.LineAlignment = (StringAlignment)1;
		Rectangle rectangle = Rectangle.Round(GetLabelRect(context, pointF, labelText, font, stringFormatGeom));
		Rectangle empty = Rectangle.Empty;
		empty = ((num >= 0) ? Rectangle.Round(new RectangleF(pointF.X, pointF.Y - (float)(rectangle.Height / 2), rectangle.Width, rectangle.Height)) : Rectangle.Round(new RectangleF(pointF.X - (float)rectangle.Width, pointF.Y - (float)(rectangle.Height / 2), rectangle.Width, rectangle.Height)));
		if (measure)
		{
			measureRect = empty;
			return null;
		}
		measureRect = RectangleF.Empty;
		Color borderColor = GetBorderColor(series, colorIndex, colorCount);
		Color seriesBorderColor = (Color)series.Core.GetSeriesBorderColor(colorIndex, colorCount);
		StiBrush seriesBrush = series.Core.GetSeriesBrush(colorIndex, colorCount);
		seriesBrush = ProcessSeriesColors(pointIndex, seriesBrush, series);
		StiSolidBrush labelBrush = new StiSolidBrush(GetLabelColor(series, colorIndex, colorCount));
		Color lineColor = GetLineColor(series, colorIndex, colorCount);
		StiBrush seriesLabelsBrush = ProcessSeriesColors(pointIndex, base.SeriesLabels.Brush, series);
		return new StiTwoColumnsPieLabelsGeom(base.SeriesLabels, series, pointIndex, value.Value, empty, labelText, seriesBrush, (StiBrush)(object)labelBrush, seriesLabelsBrush, borderColor, seriesBorderColor, empty, lineColor, labelPoint2, pointF, labelPoint, centerPie, null);
	}

	public StiTwoColumnsPieLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
