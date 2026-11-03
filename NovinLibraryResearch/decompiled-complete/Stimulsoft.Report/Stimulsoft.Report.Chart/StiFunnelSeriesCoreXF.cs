using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart.Geoms.Series.Funnel;

namespace Stimulsoft.Report.Chart;

public class StiFunnelSeriesCoreXF : StiSeriesCoreXF
{
	private IStiFunnelSeriesLabels labels;

	public override string LocalizedName => StiLocalization.Get("Chart", "Funnel");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiFunnelSeries stiFunnelSeries = base.Series as IStiFunnelSeries;
		if (stiFunnelSeries.AllowApplyStyle)
		{
			stiFunnelSeries.Brush = style.Core.GetColumnBrush(color);
			stiFunnelSeries.BorderColor = style.Core.GetColumnBorder(color);
			stiFunnelSeries.BorderThickness = style.Core.SeriesBorderThickness;
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
		double[] allValues = GetAllValues(array);
		double[] allTrueValues = GetAllTrueValues(array);
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
		float singleValueHeight = GetSingleValueHeight(num, rect);
		float singleValueWidth = GetSingleValueWidth(allValues, rect);
		labels = base.Series.Chart.SeriesLabels as IStiFunnelSeriesLabels;
		RectangleF measureRect = rect;
		RectangleF rectangleF = rect;
		IStiFunnelSeries[] array3 = array;
		foreach (IStiFunnelSeries stiFunnelSeries2 in array3)
		{
			if (labels == null || !labels.Visible)
			{
				continue;
			}
			for (int k = 0; k < stiFunnelSeries2.Values.Length; k++)
			{
				if (stiFunnelSeries2.Values.Length > k)
				{
					double valueOrDefault = stiFunnelSeries2.Values[k].GetValueOrDefault();
					double valueNext = ((k == stiFunnelSeries2.Values.Length - 1) ? valueOrDefault : stiFunnelSeries2.Values[k + 1].GetValueOrDefault());
					if (labels.Step == 0 || k % labels.Step == 0)
					{
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiFunnelSeriesLabelsCoreXF)labels.Core).RenderLabel(stiFunnelSeries2, context, k, valueOrDefault, valueNext, GetArgumentText(stiFunnelSeries2, k), stiFunnelSeries2.Core.GetTag(k), 0, num, rect, singleValueHeight, singleValueWidth, 1f, out measureRect);
						rectangleF = ((measureRect.Width < rectangleF.Width) ? measureRect : rectangleF);
					}
				}
			}
			measureRect = rectangleF;
		}
		singleValueHeight = GetSingleValueHeight(num, measureRect);
		singleValueWidth = GetSingleValueWidth(allValues, measureRect);
		int num2 = 0;
		int num3 = 0;
		float centerAxis = ((!(labels is StiOutsideLeftFunnelLabels)) ? (measureRect.Width / 2f) : (measureRect.Width / 2f + measureRect.X));
		RectangleF rect2 = measureRect;
		IStiFunnelSeries[] array4 = array;
		foreach (IStiFunnelSeries stiFunnelSeries3 in array4)
		{
			int nonZeroValuesCount = 0;
			int firstNonZeroValueIndex = -1;
			IStiSeries firstNonZeroSeries = null;
			double? num4 = CheckNonZerovalue(seriesArray, out nonZeroValuesCount, out firstNonZeroValueIndex, out firstNonZeroSeries);
			switch (nonZeroValuesCount)
			{
			case 1:
			{
				StiBrush brush = stiFunnelSeries3.Brush;
				if (stiFunnelSeries3.AllowApplyBrush)
				{
					brush = stiFunnelSeries3.Core.GetSeriesBrush(firstNonZeroValueIndex, stiFunnelSeries3.Values.Length);
					brush = stiFunnelSeries3.ProcessSeriesBrushes(firstNonZeroValueIndex, brush);
				}
				Color borderColor = stiFunnelSeries3.BorderColor;
				if (stiFunnelSeries3.AllowApplyBorderColor)
				{
					borderColor = (Color)stiFunnelSeries3.Core.GetSeriesBorderColor(num3, num);
				}
				StiFunnelSeriesElementGeom stiFunnelSeriesElementGeom = RenderFunnelElement(borderColor, brush, stiFunnelSeries3.Values[firstNonZeroValueIndex].GetValueOrDefault(), 0.0, num3, firstNonZeroValueIndex, stiFunnelSeries3, geom, rect2, singleValueHeight, singleValueWidth, new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks));
				if (stiFunnelSeriesElementGeom != null)
				{
					if (stiFunnelSeries3.Core.Interaction != null)
					{
						stiFunnelSeriesElementGeom.Interaction = new StiSeriesInteractionData(geom.Area, stiFunnelSeries3, firstNonZeroValueIndex);
					}
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiFunnelSeriesElementGeom);
				}
				continue;
			}
			case 0:
				continue;
			}
			for (int m = 0; m < stiFunnelSeries3.Values.Length; m++)
			{
				double? num5 = GetCurrentValue(stiFunnelSeries3, num2, allTrueValues);
				if (num5 == 0.0 && !stiFunnelSeries3.ShowZeros)
				{
					num2++;
					continue;
				}
				double? nextCurrentValue = GetNextCurrentValue(stiFunnelSeries3, num2, allTrueValues);
				StiBrush brush2 = stiFunnelSeries3.Brush;
				if (stiFunnelSeries3.AllowApplyBrush)
				{
					brush2 = stiFunnelSeries3.Core.GetSeriesBrush(num2, num);
					brush2 = stiFunnelSeries3.ProcessSeriesBrushes(num2, brush2);
				}
				Color borderColor2 = stiFunnelSeries3.BorderColor;
				if (stiFunnelSeries3.AllowApplyBorderColor)
				{
					borderColor2 = (Color)stiFunnelSeries3.Core.GetSeriesBorderColor(num3, num);
				}
				StiFunnelSeriesElementGeom stiFunnelSeriesElementGeom2 = RenderFunnelElement(borderColor2, brush2, num5.GetValueOrDefault(), nextCurrentValue.GetValueOrDefault(), num3, m, stiFunnelSeries3, geom, rect2, singleValueHeight, singleValueWidth, new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks / allValues.Length * m));
				if (stiFunnelSeriesElementGeom2 != null)
				{
					if (stiFunnelSeries3.Core.Interaction != null)
					{
						stiFunnelSeriesElementGeom2.Interaction = new StiSeriesInteractionData(geom.Area, stiFunnelSeries3, m);
					}
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiFunnelSeriesElementGeom2);
				}
				if (labels != null && labels.Visible && (labels.Step == 0 || m % labels.Step == 0))
				{
					StiSeriesLabelsGeom stiSeriesLabelsGeom2 = ((StiFunnelSeriesLabelsCoreXF)labels.Core).RenderLabel(stiFunnelSeries3, context, num3, num5.GetValueOrDefault(), nextCurrentValue.GetValueOrDefault(), GetArgumentText(stiFunnelSeries3, m), stiFunnelSeries3.Core.GetTag(m), num3, num, rect, singleValueHeight, singleValueWidth, centerAxis, out measureRect);
					if (stiSeriesLabelsGeom2 != null)
					{
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(stiSeriesLabelsGeom2);
						stiSeriesLabelsGeom2.ClientRectangle = CheckLabelsRect(labels, geom, stiSeriesLabelsGeom2.ClientRectangle);
					}
				}
				num3++;
				num2++;
			}
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

	private double GetCurrentValue(IStiFunnelSeries funnelSeries, int index, double[] values)
	{
		return values[index];
	}

	private double? GetNextCurrentValue(IStiFunnelSeries funnelSeries, int index, double[] values)
	{
		if (index != values.Length - 1)
		{
			double num = values[index + 1];
			if (num == 0.0 && !funnelSeries.ShowZeros)
			{
				while (num == 0.0 && index < values.Length - 2)
				{
					index++;
					num = values[index + 1];
				}
			}
			return num;
		}
		return values[index];
	}

	private double[] GetAllValues(IStiFunnelSeries[] funnelSeries)
	{
		List<double> list = new List<double>();
		foreach (IStiFunnelSeries stiFunnelSeries in funnelSeries)
		{
			double?[] values = stiFunnelSeries.Values;
			for (int j = 0; j < values.Length; j++)
			{
				double? num = values[j];
				if (num.GetValueOrDefault() != 0.0 || stiFunnelSeries.ShowZeros)
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
		float singleValueHeight = rect.Height * 0.9f / (float)array.Length;
		float singleValueWidth = rect.Width * 0.9f / (float)array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			double value = array[i];
			double? num = ((i != array.Length - 1) ? array[i + 1] : array[i]);
			List<StiSegmentGeom> path = MeasureFunnelElementCore(value, num.GetValueOrDefault(), i, rect, singleValueHeight, singleValueWidth);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(new StiFunnelEmptySeriesElementGeom(rect, path));
		}
	}

	private StiFunnelSeriesElementGeom RenderFunnelElement(Color borderColor, StiBrush brush, double value, double valueNext, int globalIndex, int index, IStiSeries currentSeries, StiAreaGeom geom, RectangleF rect, float singleValueHeight, float singleValueWidth, TimeSpan beginTime)
	{
		List<StiSegmentGeom> path = MeasureFunnelElementCore(value, valueNext, globalIndex, rect, singleValueHeight, singleValueWidth);
		return new StiFunnelSeriesElementGeom(geom, value, index, currentSeries, rect, brush, borderColor, path, beginTime);
	}

	private float GetSingleValueHeight(int count, RectangleF rect)
	{
		return rect.Height * 0.9f / (float)count;
	}

	private float GetSingleValueWidth(double[] values, RectangleF rect)
	{
		float num = float.MinValue;
		for (int i = 0; i < values.Length; i++)
		{
			double? num2 = values[i];
			num = (((double)num > num2.GetValueOrDefault()) ? num : ((float)num2.GetValueOrDefault()));
		}
		return rect.Width * 0.9f / num;
	}

	private List<StiSegmentGeom> MeasureFunnelElementCore(double value, double valueNext, int index, RectangleF rect, float singleValueHeight, float singleValueWidth)
	{
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		float num = rect.Height * 0.05f;
		float num2 = ((!(labels is StiOutsideLeftFunnelLabels)) ? (rect.Width / 2f) : (rect.Width / 2f + rect.X));
		PointF pointF = new PointF(num2 - (float)value / 2f * singleValueWidth, singleValueHeight * (float)index + num);
		PointF pointF2 = new PointF(num2 + (float)value / 2f * singleValueWidth, singleValueHeight * (float)index + num);
		PointF pointF3 = new PointF(num2 + (float)valueNext / 2f * singleValueWidth, singleValueHeight * (float)(index + 1) + num);
		PointF pointF4 = new PointF(num2 - (float)valueNext / 2f * singleValueWidth, singleValueHeight * (float)(index + 1) + num);
		PointF[] points = new PointF[5] { pointF, pointF2, pointF3, pointF4, pointF };
		list.Add(new StiLinesSegmentGeom(points));
		list.Add(new StiCloseFigureSegmentGeom());
		return list;
	}

	public StiFunnelSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
