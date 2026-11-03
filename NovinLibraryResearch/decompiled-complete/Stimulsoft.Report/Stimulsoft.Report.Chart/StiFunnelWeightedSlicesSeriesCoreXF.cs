using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart.Geoms.Series.Funnel;

namespace Stimulsoft.Report.Chart;

public class StiFunnelWeightedSlicesSeriesCoreXF : StiSeriesCoreXF
{
	private IStiFunnelSeriesLabels labels;

	public override string LocalizedName => StiLocalization.Get("Chart", "FunnelWeightedSlices");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiFunnelWeightedSlicesSeries stiFunnelWeightedSlicesSeries = base.Series as IStiFunnelWeightedSlicesSeries;
		if (stiFunnelWeightedSlicesSeries.AllowApplyStyle)
		{
			stiFunnelWeightedSlicesSeries.Brush = style.Core.GetColumnBrush(color);
			stiFunnelWeightedSlicesSeries.BorderColor = style.Core.GetColumnBorder(color);
			stiFunnelWeightedSlicesSeries.BorderThickness = style.Core.SeriesBorderThickness;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		if (seriesArray == null || seriesArray.Length == 0 || base.Series.Chart == null)
		{
			RenderFunnelEmpty(geom, rect);
			return;
		}
		IStiFunnelSeries[] array = seriesArray.Cast<IStiFunnelSeries>().ToArray();
		int num = 0;
		IStiFunnelSeries[] array2 = array;
		foreach (IStiFunnelSeries stiFunnelSeries in array2)
		{
			if (stiFunnelSeries.Values != null)
			{
				num += stiFunnelSeries.Values.Length;
			}
		}
		if (num == 0)
		{
			RenderFunnelEmpty(geom, rect);
			return;
		}
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		double[] allTrueValues = GetAllTrueValues(array);
		double[] values = GetValues(array);
		float singleValueHeight = rect.Height * 0.9f / GetSumValues(allTrueValues);
		int num2 = 0;
		int num3 = 0;
		float centerAxis = ((!(labels is StiOutsideLeftFunnelLabels)) ? (rect.Width / 2f) : (rect.Width / 2f + rect.X));
		labels = base.Series.Chart.SeriesLabels as IStiFunnelSeriesLabels;
		foreach (IStiSeries stiSeries in seriesArray)
		{
			IStiFunnelWeightedSlicesSeries stiFunnelWeightedSlicesSeries = stiSeries as IStiFunnelWeightedSlicesSeries;
			int nonZeroValuesCount = 0;
			int firstNonZeroValueIndex = -1;
			IStiSeries firstNonZeroSeries = null;
			double? num4 = CheckNonZerovalue(seriesArray, out nonZeroValuesCount, out firstNonZeroValueIndex, out firstNonZeroSeries);
			switch (nonZeroValuesCount)
			{
			case 1:
			{
				StiBrush brush = stiFunnelWeightedSlicesSeries.Brush;
				if (stiFunnelWeightedSlicesSeries.AllowApplyBrush)
				{
					brush = stiFunnelWeightedSlicesSeries.Core.GetSeriesBrush(firstNonZeroValueIndex, stiFunnelWeightedSlicesSeries.Values.Length);
					brush = stiFunnelWeightedSlicesSeries.ProcessSeriesBrushes(firstNonZeroValueIndex, brush);
				}
				Color borderColor = stiFunnelWeightedSlicesSeries.BorderColor;
				if (stiFunnelWeightedSlicesSeries.AllowApplyBorderColor)
				{
					borderColor = (Color)stiFunnelWeightedSlicesSeries.Core.GetSeriesBorderColor(num3, num);
				}
				StiFunnelSeriesElementGeom stiFunnelSeriesElementGeom = RenderFunnelElement(borderColor, brush, stiFunnelWeightedSlicesSeries.Values[firstNonZeroValueIndex].GetValueOrDefault(), values, num3, firstNonZeroValueIndex, stiSeries, geom, rect, singleValueHeight, new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks));
				if (stiFunnelSeriesElementGeom != null)
				{
					if (stiFunnelWeightedSlicesSeries.Core.Interaction != null)
					{
						stiFunnelSeriesElementGeom.Interaction = new StiSeriesInteractionData(geom.Area, stiFunnelWeightedSlicesSeries, firstNonZeroValueIndex);
					}
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiFunnelSeriesElementGeom);
				}
				continue;
			}
			case 0:
				continue;
			}
			for (int k = 0; k < stiSeries.Values.Length; k++)
			{
				double valueOrDefault = stiSeries.Values[k].GetValueOrDefault();
				if (!stiFunnelWeightedSlicesSeries.ShowZeros && valueOrDefault == 0.0)
				{
					num3++;
					continue;
				}
				StiBrush brush2 = stiFunnelWeightedSlicesSeries.Brush;
				if (stiFunnelWeightedSlicesSeries.AllowApplyBrush)
				{
					brush2 = stiFunnelWeightedSlicesSeries.Core.GetSeriesBrush(num3, num);
					brush2 = stiFunnelWeightedSlicesSeries.ProcessSeriesBrushes(num3, brush2);
				}
				Color borderColor2 = stiFunnelWeightedSlicesSeries.BorderColor;
				if (stiFunnelWeightedSlicesSeries.AllowApplyBorderColor)
				{
					borderColor2 = (Color)stiFunnelWeightedSlicesSeries.Core.GetSeriesBorderColor(num3, num);
				}
				StiFunnelSeriesElementGeom stiFunnelSeriesElementGeom2 = RenderFunnelElement(borderColor2, brush2, valueOrDefault, values, num2, k, stiSeries, geom, rect, singleValueHeight, new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks / stiSeries.Values.Length * k));
				if (stiFunnelSeriesElementGeom2 != null)
				{
					if (stiSeries.Core.Interaction != null)
					{
						stiFunnelSeriesElementGeom2.Interaction = new StiSeriesInteractionData(geom.Area, stiSeries, k);
					}
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiFunnelSeriesElementGeom2);
				}
				if (labels != null && labels.Visible)
				{
					double valueNext = ((k == allTrueValues.Length - 1) ? valueOrDefault : allTrueValues[k + 1]);
					if (labels.Step == 0 || k % labels.Step == 0)
					{
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiFunnelSeriesLabelsCoreXF)labels.Core).RenderLabel(stiSeries, context, num3, valueOrDefault, valueNext, GetArgumentText(stiSeries, k), stiSeries.Core.GetTag(k), num3, num, rect, singleValueHeight, 0f, centerAxis, out rect);
						if (stiSeriesLabelsGeom != null)
						{
							list.Add(stiSeriesLabelsGeom);
						}
					}
				}
				num2++;
				num3++;
			}
		}
		foreach (StiSeriesLabelsGeom item in list)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
			item.ClientRectangle = CheckLabelsRect(labels, geom, item.ClientRectangle);
		}
	}

	private double? CheckNonZerovalue(IStiSeries[] seriesArray, out int nonZeroValuesCount, out int firstNonZeroValueIndex, out IStiSeries firstNonZeroSeries, bool isForValueFrom = false)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		double? result = 0.0;
		IStiSeries stiSeries = null;
		foreach (IStiSeries stiSeries2 in seriesArray)
		{
			double?[] array = (isForValueFrom ? stiSeries2.ValuesStart : stiSeries2.Values);
			double?[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				double? num4 = array2[j];
				if (num4 != 0.0 && num4.HasValue && !double.IsNaN(num4.Value))
				{
					num++;
					result = num4;
					stiSeries = stiSeries2;
					if (num == 1)
					{
						num3 = num2;
					}
				}
				num2++;
			}
		}
		firstNonZeroValueIndex = num3;
		nonZeroValuesCount = num;
		firstNonZeroSeries = stiSeries;
		return result;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		if (base.Series.Chart == null || base.Series.Chart.Area == null)
		{
			return null;
		}
		StiStyleCoreXF stiStyleCoreXF = ((base.Series.Chart.Style != null) ? base.Series.Chart.Style.Core : new StiStyleCoreXF29());
		Color colorByIndex = stiStyleCoreXF.GetColorByIndex(colorIndex, colorCount, base.SeriesColors);
		return stiStyleCoreXF.GetColumnBorder(colorByIndex);
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		if (base.Series.Chart == null || base.Series.Chart.Area == null)
		{
			return null;
		}
		StiStyleCoreXF stiStyleCoreXF = ((base.Series.Chart.Style != null) ? base.Series.Chart.Style.Core : new StiStyleCoreXF29());
		Color colorByIndex = stiStyleCoreXF.GetColorByIndex(colorIndex, colorCount, base.SeriesColors);
		return stiStyleCoreXF.GetColumnBrush(colorByIndex);
	}

	private double[] GetValues(IStiFunnelSeries[] funnelSeries)
	{
		List<double> list = new List<double>();
		foreach (IStiFunnelSeries stiFunnelSeries in funnelSeries)
		{
			double?[] values = stiFunnelSeries.Values;
			for (int j = 0; j < values.Length; j++)
			{
				double? num = values[j];
				if (stiFunnelSeries.ShowZeros || num.GetValueOrDefault() != 0.0)
				{
					list.Add(num.GetValueOrDefault());
				}
			}
		}
		return list.ToArray();
	}

	private double[] GetAllTrueValues(IStiFunnelSeries[] funnelSeries)
	{
		List<double> list = new List<double>();
		foreach (IStiFunnelSeries stiFunnelSeries in funnelSeries)
		{
			double?[] values = stiFunnelSeries.Values;
			foreach (double? num in values)
			{
				list.Add(num.GetValueOrDefault());
			}
		}
		return list.ToArray();
	}

	private string GetArgumentText(IStiSeries series, int index)
	{
		if (series.Arguments.Length > index && series.Arguments[index] != null)
		{
			return series.Arguments[index].ToString();
		}
		return string.Empty;
	}

	private void RenderFunnelEmpty(StiAreaGeom geom, RectangleF rect)
	{
		double[] array = new double[3] { 3.0, 2.0, 1.0 };
		float singleValueHeight = rect.Height * 0.9f / GetSumValues(array);
		for (int i = 0; i < array.Length; i++)
		{
			double num = array[i];
			List<StiSegmentGeom> path = MeasureFunnelElementCore(i, rect, singleValueHeight, array);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(new StiFunnelEmptySeriesElementGeom(rect, path));
		}
	}

	private List<StiSegmentGeom> GetPathFunnelEmpty(RectangleF rect)
	{
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		float num = rect.Height * 0.05f;
		float num2 = ((!(labels is StiOutsideLeftFunnelLabels)) ? (rect.Width / 2f) : (rect.Width / 2f + rect.X));
		PointF pointF = new PointF(num2 - rect.Width * 0.9f / 2f, num);
		PointF pointF2 = new PointF(num2 + rect.Width * 0.9f / 2f, num);
		PointF pointF3 = new PointF(num2 - rect.Width * 0.1f / 2f, rect.Height + num - rect.Height / 5f);
		PointF pointF4 = new PointF(num2 + rect.Width * 0.1f / 2f, rect.Height + num - rect.Height / 5f);
		PointF pointF5 = new PointF(num2 + rect.Width * 0.1f / 2f, rect.Height + num);
		PointF pointF6 = new PointF(num2 - rect.Width * 0.1f / 2f, rect.Height + num);
		PointF[] points = new PointF[7] { pointF, pointF2, pointF4, pointF5, pointF6, pointF3, pointF };
		list.Add(new StiLinesSegmentGeom(points));
		list.Add(new StiCloseFigureSegmentGeom());
		return list;
	}

	private StiFunnelSeriesElementGeom RenderFunnelElement(Color borderColor, StiBrush brush, double value, double[] values, int globalIndex, int index, IStiSeries currentSeries, StiAreaGeom geom, RectangleF rect, float singleValueHeight, TimeSpan beginTime)
	{
		List<StiSegmentGeom> path = MeasureFunnelElementCore(globalIndex, rect, singleValueHeight, values);
		return new StiFunnelSeriesElementGeom(geom, value, index, currentSeries, rect, brush, borderColor, path, beginTime);
	}

	private float GetSumValues(double[] values)
	{
		double num = 0.0;
		foreach (double value in values)
		{
			num += Math.Abs(value);
		}
		return (float)num;
	}

	private float GetSumLastValues(int indexCurrent, double[] values)
	{
		float num = 0f;
		for (int i = 0; i < indexCurrent && i < values.Length; i++)
		{
			num += (float)Math.Abs(values[i]);
		}
		return num;
	}

	private List<StiSegmentGeom> MeasureFunnelElementCore(int index, RectangleF rect, float singleValueHeight, double[] values)
	{
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		float num = rect.Height * 0.05f;
		float num2 = ((!(labels is StiOutsideLeftFunnelLabels)) ? (rect.Width / 2f) : (rect.Width / 2f + rect.X));
		PointF pointF = default(PointF);
		PointF pointF2 = default(PointF);
		bool flag = false;
		float sumLastValues = GetSumLastValues(index, values);
		bool flag2 = false;
		float num3 = rect.Height - singleValueHeight * sumLastValues;
		if (num3 < rect.Height / 5f)
		{
			num3 = rect.Height / 5f;
			flag2 = true;
		}
		float num4 = rect.Width * 0.9f * num3 / rect.Height;
		PointF pointF3 = new PointF(num2 - num4 / 2f, singleValueHeight * sumLastValues + num);
		PointF pointF4 = new PointF(num2 + num4 / 2f, singleValueHeight * sumLastValues + num);
		sumLastValues = GetSumLastValues(index + 1, values);
		num3 = rect.Height - singleValueHeight * sumLastValues;
		if (num3 < rect.Height / 5f)
		{
			num3 = rect.Height / 5f;
			if (!flag2)
			{
				num4 = rect.Width * 0.9f * num3 / rect.Height;
				pointF = new PointF(num2 - num4 / 2f, rect.Height + num - num3);
				pointF2 = new PointF(num2 + num4 / 2f, rect.Height + num - num3);
				flag = true;
			}
		}
		num4 = rect.Width * 0.9f * num3 / rect.Height;
		PointF pointF5 = new PointF(num2 + num4 / 2f, singleValueHeight * sumLastValues + num);
		PointF pointF6 = new PointF(num2 - num4 / 2f, singleValueHeight * sumLastValues + num);
		PointF[] points = ((!flag) ? new PointF[5] { pointF3, pointF4, pointF5, pointF6, pointF3 } : new PointF[7] { pointF3, pointF4, pointF2, pointF5, pointF6, pointF, pointF3 });
		list.Add(new StiLinesSegmentGeom(points));
		list.Add(new StiCloseFigureSegmentGeom());
		return list;
	}

	public StiFunnelWeightedSlicesSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
