using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiCenterFunnelLabelsCoreXF : StiFunnelSeriesLabelsCoreXF
{
	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Funnel;

	public override int Position => 14;

	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsCenter");

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
		PointF point = ((!(series is StiFunnelWeightedSlicesSeries)) ? new PointF(rect.Width / 2f, singleValueHeight * (float)pointIndex + singleValueHeight / 2f + rect.Height * 0.05f) : new PointF(rect.Width / 2f, rect.Height * 0.05f + singleValueHeight * GetSumLastValues(series, pointIndex) + singleValueHeight * Math.Abs((float)value / 2f)));
		RectangleF rectangleF = context.MeasureRotatedString(labelText, font, point, stringFormatGeom, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
		return new StiCenterFunnelLabelsGeom(base.SeriesLabels, series, pointIndex, value, rectangleF, labelText, seriesBrush, labelBrush, borderColor, seriesBorderColor, rectangleF, null);
	}

	private float GetSumLastValues(IStiSeries series, int indexCurrent)
	{
		List<double> list = new List<double>();
		foreach (IStiFunnelSeries item in series.Chart.Series)
		{
			double?[] values = item.Values;
			for (int i = 0; i < values.Length; i++)
			{
				double? num = values[i];
				if (item.ShowZeros || num.GetValueOrDefault() != 0.0)
				{
					list.Add(num.GetValueOrDefault());
				}
			}
		}
		float num2 = 0f;
		for (int j = 0; j < indexCurrent && j < list.Count; j++)
		{
			num2 += (float)Math.Abs(list[j]);
		}
		return num2;
	}

	public StiCenterFunnelLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
