using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiOutsideAxisLabelsCoreXF : StiAxisSeriesLabelsCoreXF
{
	public override int Position => 6;

	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsOutside");

	public override void ApplyStyle(IStiChartStyle style)
	{
		base.ApplyStyle(style);
		if (base.SeriesLabels.AllowApplyStyle)
		{
			((IStiOutsideAxisLabels)base.SeriesLabels).LineColor = style.Core.SeriesLabelsLineColor;
		}
	}

	public override StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, PointF endPoint, PointF startPoint, int pointIndex, double? value, double? labelValue, string argumentText, string tag, int colorIndex, int colorCount, RectangleF rect, StiAnimation animation = null)
	{
		IStiOutsideAxisLabels stiOutsideAxisLabels = base.SeriesLabels as IStiOutsideAxisLabels;
		if (!value.HasValue && !stiOutsideAxisLabels.ShowNulls)
		{
			return null;
		}
		if (value == 0.0 && !stiOutsideAxisLabels.ShowZeros)
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
		string labelText = GetLabelText(series, labelValue, argumentText, tag, series.CoreTitle);
		float num = 90f;
		if (base.SeriesLabels.Chart.Area is IStiStackedBarArea)
		{
			if (value <= 0.0)
			{
				num = 0f - num;
			}
			num += 90f;
		}
		else if (value < 0.0)
		{
			num = 0f - num;
		}
		num *= (float)Math.PI / 180f;
		Point point = new Point((int)Math.Round((double)endPoint.X + (double)stiOutsideAxisLabels.LineLength * Math.Cos(num) * (double)context.Options.Zoom), (int)Math.Round((double)endPoint.Y - (double)stiOutsideAxisLabels.LineLength * Math.Sin(num) * (double)context.Options.Zoom));
		StiFontGeom font = StiFontGeom.ChangeFontSize(stiOutsideAxisLabels.Font, stiOutsideAxisLabels.Font.Size * context.Options.Zoom);
		StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context);
		Size size = Size.Round(context.MeasureString(labelText, font, (int)((float)base.SeriesLabels.Width * context.Options.Zoom), stringFormatGeom));
		Rectangle rectangle = new Rectangle(point.X - size.Width / 2, point.Y - size.Height / 2, size.Width + 1, size.Height + 1);
		Color labelColor = GetLabelColor(series, colorIndex, colorCount);
		Color borderColor = GetBorderColor(series, colorIndex, colorCount);
		Color seriesBorderColor = (Color)series.Core.GetSeriesBorderColor(colorIndex, colorCount);
		StiBrush seriesBrush = series.Core.GetSeriesBrush(colorIndex, colorCount);
		seriesBrush = ProcessSeriesColors(pointIndex, seriesBrush, series);
		return new StiOutsideAxisLabelsGeom(base.SeriesLabels, series, pointIndex, value.Value, rectangle, labelText, labelColor, borderColor, stiOutsideAxisLabels.LineColor, seriesBrush, seriesBorderColor, font, point, endPoint, null);
	}

	public StiOutsideAxisLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
