using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiCenterAxisLabelsCoreXF : StiAxisSeriesLabelsCoreXF
{
	public override int Position => 3;

	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsCenter");

	public override StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, PointF endPoint, PointF startPoint, int pointIndex, double? value, double? labelValue, string argumentText, string tag, int colorIndex, int colorCount, RectangleF rect, StiAnimation animation = null)
	{
		currentIndex = pointIndex;
		return RenderLabel(series, context, endPoint, startPoint, pointIndex, value, labelValue, argumentText, tag, 0.0, colorIndex, colorCount, rect, animation);
	}

	public override StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, PointF endPoint, PointF startPoint, int pointIndex, double? value, double? labelValue, string argumentText, string tag, double weight, int colorIndex, int colorCount, RectangleF rect, StiAnimation animation = null)
	{
		if (!value.HasValue && !base.SeriesLabels.ShowNulls)
		{
			return null;
		}
		if (value == 0.0 && !base.SeriesLabels.ShowZeros)
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
		string labelText = GetLabelText(series, labelValue, argumentText, tag, series.CoreTitle, weight, useLegendValueType: false);
		Color labelColor = GetLabelColor(series, colorIndex, colorCount);
		Color borderColor = GetBorderColor(series, colorIndex, colorCount);
		StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context);
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.SeriesLabels.Font, base.SeriesLabels.Font.Size * context.Options.Zoom);
		Color seriesBorderColor = (Color)series.Core.GetSeriesBorderColor(colorIndex, colorCount);
		StiBrush seriesBrush = series.Core.GetSeriesBrush(colorIndex, colorCount);
		StiBrush seriesLabelsBrush = ProcessSeriesColors(pointIndex, base.SeriesLabels.Brush, series);
		RectangleF labelRect = GetLabelRect(context, endPoint, startPoint, value, labelText, checkHeight: true, font, stringFormatGeom);
		if (animation is StiLabelAnimation stiLabelAnimation)
		{
			stiLabelAnimation.LabelRect = GetLabelRect(context, stiLabelAnimation.PointFrom, startPoint, stiLabelAnimation.ValueFrom, stiLabelAnimation.ValueFrom.ToString(), checkHeight: true, font, stringFormatGeom);
		}
		return new StiCenterAxisLabelsGeom(base.SeriesLabels, series, pointIndex, value.Value, labelRect, labelText, labelColor, borderColor, seriesBrush, seriesLabelsBrush, seriesBorderColor, font, animation);
	}

	protected virtual RectangleF GetLabelRect(StiContext context, PointF endPoint, PointF startPoint, double? value, string labelText, bool checkHeight, StiFontGeom font, StiStringFormatGeom sf)
	{
		if (base.SeriesLabels.Chart.Area.Core.SeriesOrientation == StiChartSeriesOrientation.Vertical)
		{
			return context.MeasureRotatedString(labelText, font, new PointF(endPoint.X, (endPoint.Y + startPoint.Y) / 2f), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
		}
		return context.MeasureRotatedString(labelText, font, new PointF((endPoint.X + startPoint.X) / 2f, endPoint.Y), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
	}

	public StiCenterAxisLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
