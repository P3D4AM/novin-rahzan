using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiCenterTreemapLabelsCoreXF : StiSeriesLabelsCoreXF
{
	public override int Position => 17;

	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsCenter");

	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Treemap;

	public StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, int pointIndex, double value, string argumentText, string tag, int colorIndex, int colorCount, RectangleF rect, StiAnimation animation = null)
	{
		string labelText = GetLabelText(series, value, argumentText, tag, series.CoreTitle);
		Color labelColor = GetLabelColor(series, colorIndex, colorCount);
		Color borderColor = GetBorderColor(series, colorIndex, colorCount);
		StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context);
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.SeriesLabels.Font, base.SeriesLabels.Font.Size * context.Options.Zoom);
		Color seriesBorderColor = (Color)series.Core.GetSeriesBorderColor(colorIndex, colorCount);
		StiBrush seriesBrush = series.Core.GetSeriesBrush(colorIndex, colorCount);
		StiBrush seriesLabelsBrush = ProcessSeriesColors(pointIndex, base.SeriesLabels.Brush, series);
		RectangleF labelRect = GetLabelRect(context, rect, value, labelText, checkHeight: true, font, stringFormatGeom);
		return new StiCenterTreemapLabelsGeom(base.SeriesLabels, series, pointIndex, value, labelRect, labelText, labelColor, borderColor, seriesBrush, seriesLabelsBrush, seriesBorderColor, font, animation);
	}

	protected virtual RectangleF GetLabelRect(StiContext context, RectangleF rect, double? value, string labelText, bool checkHeight, StiFontGeom font, StiStringFormatGeom sf)
	{
		return context.MeasureRotatedString(labelText, font, new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
	}

	public StiCenterTreemapLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
