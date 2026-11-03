using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public abstract class StiPieSeriesLabelsCoreXF : StiSeriesLabelsCoreXF
{
	[Browsable(false)]
	public float percentPerValue;

	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Pie;

	public abstract StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, PointF centerPie, float radius, float radius2, float pieAngle, int pointIndex, double? value, double? labelValue, string argumentText, string tag, bool measure, int colorIndex, int colorCount, float percentPerValue, out RectangleF measureRect, bool drawValue, float deltaY);

	internal override double RecalcValue(double value, int signs)
	{
		if (((IStiPieSeriesLabels)base.SeriesLabels).ShowInPercent)
		{
			List<IStiSeries> series = base.SeriesLabels.Chart.Area.Core.GetSeries();
			if (series.Count > 0)
			{
				IStiSeries[] array = new IStiSeries[series.Count];
				series.CopyTo(array);
				if (array.Length != 0 && array[0] is StiPieSeries)
				{
					percentPerValue = ((StiPieSeriesCoreXF)array[0].Core).GetPercentPerValue(array);
				}
			}
			return Math.Round(value * (double)percentPerValue, signs);
		}
		return value;
	}

	public StiPieSeriesLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
