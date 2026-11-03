using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public abstract class StiAxisSeriesLabelsCoreXF : StiSeriesLabelsCoreXF
{
	[Browsable(false)]
	internal int? currentIndex;

	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Axis;

	public abstract StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, PointF endPoint, PointF startPoint, int pointIndex, double? value, double? labelValue, string argumentText, string tag, int colorIndex, int colorCount, RectangleF rect, StiAnimation animation = null);

	public virtual StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, PointF endPoint, PointF startPoint, int pointIndex, double? value, double? labelValue, string argumentText, string tag, double weight, int colorIndex, int colorCount, RectangleF rect, StiAnimation animation = null)
	{
		return null;
	}

	internal override double RecalcValue(double value, int signs)
	{
		if (((IStiAxisSeriesLabels)base.SeriesLabels).ShowInPercent && base.SeriesLabels.Chart.Area is StiFullStackedColumnArea && currentIndex.HasValue)
		{
			List<IStiSeries> series = base.SeriesLabels.Chart.Area.Core.GetSeries();
			double num = 0.0;
			foreach (IStiSeries item in series)
			{
				if (currentIndex < item.Values.Length)
				{
					num += item.Values[currentIndex.Value].GetValueOrDefault();
				}
			}
			if (num != 0.0)
			{
				return Math.Round(100.0 * value / num, signs);
			}
		}
		return value;
	}

	public StiAxisSeriesLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
