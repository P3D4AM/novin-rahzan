using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiBoxAndWhiskerSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "BoxAndWhisker");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		if (base.Series.AllowApplyStyle && base.Series is IStiBoxAndWhiskerSeries stiBoxAndWhiskerSeries)
		{
			stiBoxAndWhiskerSeries.Brush = style.Core.GetColumnBrush(color);
			stiBoxAndWhiskerSeries.BorderThickness = style.Core.SeriesBorderThickness;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		IStiBoxAndWhiskerArea stiBoxAndWhiskerArea = geom.Area as IStiBoxAndWhiskerArea;
		float num = (float)stiBoxAndWhiskerArea.XAxis.Info.Dpi;
		float num2 = (float)stiBoxAndWhiskerArea.YAxis.Info.Dpi;
		float dividerY = stiBoxAndWhiskerArea.AxisCore.GetDividerY();
		int num3 = 1;
		for (int i = 0; i < series.Length; i++)
		{
			IStiBoxAndWhiskerSeries stiBoxAndWhiskerSeries = (IStiBoxAndWhiskerSeries)series[i];
			List<Tuple<double, PointF>> list = new List<Tuple<double, PointF>>();
			StiBrush val = stiBoxAndWhiskerSeries.Brush;
			if (stiBoxAndWhiskerSeries.AllowApplyBrush)
			{
				val = stiBoxAndWhiskerSeries.Core.GetSeriesBrush(num3 - 1, series.Length);
				val = stiBoxAndWhiskerSeries.ProcessSeriesBrushes(num3 - 1, val);
			}
			if (stiBoxAndWhiskerSeries.Values.Length != 0)
			{
				double valueOrDefault = stiBoxAndWhiskerSeries.Values.Min().GetValueOrDefault();
				double valueOrDefault2 = stiBoxAndWhiskerSeries.Values.Max().GetValueOrDefault();
				double median = GetMedian(stiBoxAndWhiskerSeries.Values);
				double firstQuartile = GetFirstQuartile(stiBoxAndWhiskerSeries.Values);
				double thirdQuartile = GetThirdQuartile(stiBoxAndWhiskerSeries.Values);
				float num4 = (float)num3 * num;
				float num5 = Math.Abs(dividerY - (float)valueOrDefault2 * num2);
				float num6 = Math.Abs(dividerY - (float)valueOrDefault * num2);
				float num7 = Math.Abs(dividerY - (float)median * num2);
				float num8 = Math.Abs(dividerY - (float)firstQuartile * num2);
				float num9 = Math.Abs(dividerY - (float)thirdQuartile * num2);
				RectangleF clientRectangle = new RectangleF(num4 - num / 4f, num6, num / 2f, num5 - num6);
				Color borderColor = StiBrush.ToColor(StiBrush.Dark(val, (byte)30));
				list.Add(Tuple.Create(valueOrDefault, new PointF(num4, num6)));
				list.Add(Tuple.Create(valueOrDefault2, new PointF(num4, num5)));
				list.Add(Tuple.Create(median, new PointF(num4, num7)));
				list.Add(Tuple.Create(firstQuartile, new PointF(num4, num8)));
				list.Add(Tuple.Create(thirdQuartile, new PointF(num4, num9)));
				List<double> list2 = new List<double>();
				if (stiBoxAndWhiskerSeries.ShowInnerPoints)
				{
					double?[] values = stiBoxAndWhiskerSeries.Values;
					foreach (double? num10 in values)
					{
						double valueOrDefault3 = num10.GetValueOrDefault();
						if (valueOrDefault3 != valueOrDefault && valueOrDefault3 != valueOrDefault2 && !list2.Contains(valueOrDefault3))
						{
							double num11 = Math.Abs((double)dividerY - valueOrDefault3 * (double)num2);
							list2.Add(num11);
							list.Add(Tuple.Create(valueOrDefault3, new PointF(num4, (float)num11)));
						}
					}
				}
				double? mean = null;
				if (stiBoxAndWhiskerSeries.ShowMeanMarkers)
				{
					double? num12 = stiBoxAndWhiskerSeries.Values.Sum() / (double)stiBoxAndWhiskerSeries.Values.Length;
					mean = Math.Abs((double)dividerY - num12.GetValueOrDefault() * (double)num2);
					list.Add(Tuple.Create(num12.GetValueOrDefault(), new PointF(num4, (float)mean.GetValueOrDefault())));
				}
				StiBoxAndWhiskerSeriesElementGeom item = new StiBoxAndWhiskerSeriesElementGeom(geom, stiBoxAndWhiskerSeries, num4, num6, num5, num8, num9, num7, list2.ToArray(), mean, clientRectangle, val, borderColor, new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks / series.Length * num3));
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(item);
				IStiAxisSeriesLabels seriesLabels = stiBoxAndWhiskerSeries.Core.GetSeriesLabels();
				if (seriesLabels != null && seriesLabels.Visible)
				{
					foreach (Tuple<double, PointF> item2 in list)
					{
						StiOpacityAnimation animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, TimeSpan.Zero);
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiBoxAndWhiskerSeries, context, item2.Item2, item2.Item2, num3, item2.Item1, item2.Item1, item2.Item1.ToString(), null, 0.0, 0, 1, rect, animation);
						if (stiSeriesLabelsGeom != null)
						{
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiSeriesLabelsGeom);
						}
					}
				}
			}
			num3++;
		}
	}

	private double?[] GetFirstValues(double?[] values)
	{
		int[] medianIndices = GetMedianIndices(values);
		double?[] array = ((medianIndices.Length == 1) ? new double?[medianIndices[0] + 1] : new double?[medianIndices[1]]);
		Array.Copy(values, 0, array, 0, array.Length);
		return array;
	}

	private double?[] GetSecondValues(double?[] values)
	{
		int[] medianIndices = GetMedianIndices(values);
		double?[] array = ((medianIndices.Length == 1) ? new double?[medianIndices[0] + 1] : new double?[medianIndices[1]]);
		Array.Copy(values, array.Length - 1, array, 0, array.Length);
		return array;
	}

	private double GetThirdQuartile(double?[] values)
	{
		Array.Sort(values);
		double?[] secondValues = GetSecondValues(values);
		return GetMedian(secondValues);
	}

	private double GetFirstQuartile(double?[] values)
	{
		Array.Sort(values);
		double?[] firstValues = GetFirstValues(values);
		return GetMedian(firstValues);
	}

	private double GetMedian(double?[] values)
	{
		Array.Sort(values);
		int[] medianIndices = GetMedianIndices(values);
		double num = 0.0;
		int[] array = medianIndices;
		foreach (int num2 in array)
		{
			num += values[num2].GetValueOrDefault();
		}
		return num / (double)medianIndices.Length;
	}

	private int[] GetMedianIndices(double?[] values)
	{
		int[] array = ((values.Length % 2 == 0) ? new int[2] : new int[1]);
		if (array.Length == 1)
		{
			int num = (int)Math.Ceiling((double)values.Length / 2.0) - 1;
			array[0] = num;
		}
		else
		{
			array[1] = (array[0] = values.Length / 2 - 1) + 1;
		}
		return array;
	}

	public StiBoxAndWhiskerSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
