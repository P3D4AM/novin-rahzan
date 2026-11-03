using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

public class StiClusteredColumnSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "ClusteredColumn");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		if (base.Series.AllowApplyStyle)
		{
			((IStiClusteredColumnSeries)base.Series).Brush = style.Core.GetColumnBrush(color);
			((IStiClusteredColumnSeries)base.Series).BorderColor = style.Core.GetColumnBorder(color);
			((IStiClusteredColumnSeries)base.Series).BorderThickness = style.Core.SeriesBorderThickness;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		IStiArea area = geom.Area;
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
		IStiAxisArea stiAxisArea = area as IStiAxisArea;
		bool startFromZero = stiAxisArea.XAxis.Core.GetStartFromZero();
		float dividerY = stiAxisArea.AxisCore.GetDividerY();
		float dividerRightY = stiAxisArea.AxisCore.GetDividerRightY();
		int num = 0;
		int colorCount = stiAxisArea.AxisCore.ValuesCount * series.Length;
		List<List<PointF?>> list2 = new List<List<PointF?>>();
		for (int i = 0; i < series.Length; i++)
		{
			list2.Add(new List<PointF?>());
		}
		float num2 = 0f;
		foreach (IStiSeries stiSeries in series)
		{
			num2 += stiAxisArea.XAxis.Info.Step / (float)series.Length * ((IStiClusteredColumnSeries)stiSeries).Width;
		}
		for (int k = 0; k < stiAxisArea.AxisCore.ValuesCount; k++)
		{
			float num3 = MeasureLeftPosition(stiAxisArea, k, num2, startFromZero);
			int num4 = 0;
			if (stiAxisArea.ReverseHor)
			{
				num4 = series.Length - 1;
			}
			while ((num4 < series.Length && !stiAxisArea.ReverseHor) || (num4 >= 0 && stiAxisArea.ReverseHor))
			{
				IStiClusteredColumnSeries stiClusteredColumnSeries = series[num4] as IStiClusteredColumnSeries;
				float num5 = stiAxisArea.XAxis.Info.Step / (float)series.Length * stiClusteredColumnSeries.Width;
				double?[] approximationValuesStart = ((StiSeries)series[num4]).GetApproximationValuesStart();
				if (k < stiClusteredColumnSeries.Values.Length)
				{
					double? seriesCurrentValue = GetSeriesCurrentValue(stiAxisArea, stiClusteredColumnSeries, k);
					RectangleF columnRect = GetColumnRect(context, stiClusteredColumnSeries, seriesCurrentValue, num3, num5);
					double? seriesValueStart = GetSeriesValueStart(stiAxisArea, stiClusteredColumnSeries.ValuesStart, k);
					RectangleF columnRect2 = GetColumnRect(context, stiClusteredColumnSeries, GetSeriesValueStart(stiAxisArea, approximationValuesStart, k).GetValueOrDefault(), num3, num5);
					if (!((StiSeries)stiClusteredColumnSeries).TrendLines.ToList().All((StiTrendLine l) => l is IStiTrendLineNone))
					{
						list2[num4].Add(new PointF(num3 + num5 / 2f, columnRect.Y));
					}
					RectangleF clientRectangle = ((StiAxisAreaGeom)geom).View.ClientRectangle;
					clientRectangle.X = 0f;
					clientRectangle.Y = 0f;
					RectangleF rectangleF = columnRect;
					rectangleF.X += geom.ClientRectangle.X;
					rectangleF.Y += geom.ClientRectangle.Y;
					if ((rectangleF.Right > clientRectangle.X && rectangleF.X < clientRectangle.Right) || ((IStiAxisArea)base.Series.Chart.Area).XAxis.Range.Auto)
					{
						StiBrush seriesBrush = GetSeriesBrush(stiClusteredColumnSeries, seriesCurrentValue, num, colorCount, k);
						Color seriesBorderColor = (Color)stiClusteredColumnSeries.Core.GetSeriesBorderColor(num, colorCount);
						if (base.Series.Chart != null && base.Series.Chart.Style != null && columnRect.Height > 0f)
						{
							StiClusteredColumnSeriesElementGeom stiClusteredColumnSeriesElementGeom = new StiClusteredColumnSeriesElementGeom(geom, seriesCurrentValue.GetValueOrDefault(), k, seriesBrush, seriesBorderColor, stiClusteredColumnSeries, columnRect, columnRect2);
							if (stiClusteredColumnSeries.Core.Interaction != null)
							{
								stiClusteredColumnSeriesElementGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, stiClusteredColumnSeries, k);
							}
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiClusteredColumnSeriesElementGeom);
						}
						IStiAxisSeriesLabels seriesLabels = stiClusteredColumnSeries.Core.GetSeriesLabels();
						if (seriesLabels != null && seriesLabels.Visible)
						{
							PointF pointEnd = GetPointEnd(stiClusteredColumnSeries, seriesCurrentValue, num3, num5);
							double? labelValue = 0.0;
							if (seriesCurrentValue.HasValue)
							{
								labelValue = ((!stiAxisArea.ReverseVert) ? seriesCurrentValue : (0.0 - seriesCurrentValue));
							}
							StiAnimation animation = null;
							if (isAnimationChangingValues)
							{
								if (seriesLabels.ValueType == StiSeriesLabelsValueType.Value && string.IsNullOrEmpty(seriesLabels.Format) && seriesLabels.FormatService is StiGeneralFormatService)
								{
									PointF pointEnd2 = GetPointEnd(stiClusteredColumnSeries, seriesValueStart, num3, num5);
									animation = new StiLabelAnimation(seriesValueStart, seriesCurrentValue, new PointF(pointEnd2.X, pointEnd2.Y), new PointF(pointEnd.X, pointEnd.Y), StiChartHelper.GlobalBeginTimeElement, TimeSpan.Zero);
								}
								else
								{
									animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, TimeSpan.Zero);
								}
							}
							if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
							{
								int index = (stiAxisArea.XAxis.StartFromZero ? (k + 1) : k);
								StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiClusteredColumnSeries, context, CorrectPoint(pointEnd, rect), CorrectPoint(new PointF(pointEnd.X, dividerY), rect), k, seriesCurrentValue, labelValue, stiAxisArea.AxisCore.GetArgumentLabel(stiAxisArea.XAxis.Info.StripLines[index], stiClusteredColumnSeries), stiClusteredColumnSeries.Core.GetTag(k), num, colorCount, rect, animation);
								if (stiSeriesLabelsGeom != null)
								{
									list.Add(stiSeriesLabelsGeom);
									stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom.ClientRectangle);
								}
							}
						}
					}
				}
				num3 += num5;
				num++;
				num4 = ((!stiAxisArea.ReverseHor) ? (num4 + 1) : (num4 - 1));
			}
		}
		for (int m = 0; m < series.Length; m++)
		{
			StiTrendLinesCollection trendLines = ((StiSeries)series[m]).TrendLines;
			List<PointF?> list3 = list2[m];
			foreach (StiTrendLine item in trendLines)
			{
				item.Core.RenderTrendLine(geom, list3.ToArray(), stiAxisArea.AxisCore.GetDividerY());
			}
		}
		foreach (StiSeriesLabelsGeom item2 in list)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item2);
		}
		if (geom.Area.Chart.SeriesLabels.PreventIntersection)
		{
			CheckIntersectionLabels(geom);
		}
	}

	protected PointF GetPointEnd(IStiClusteredColumnSeries currentSeries, double? value, float seriesLeftPos, float seriesWidth)
	{
		IStiAxisArea stiAxisArea = currentSeries.Chart.Area as IStiAxisArea;
		float dividerY = stiAxisArea.AxisCore.GetDividerY();
		float dividerRightY = stiAxisArea.AxisCore.GetDividerRightY();
		return (currentSeries.YAxis != 0) ? new PointF(seriesLeftPos + seriesWidth / 2f, 0f - (float)(value.GetValueOrDefault() * stiAxisArea.YRightAxis.Info.Dpi) + dividerRightY) : new PointF(seriesLeftPos + seriesWidth / 2f, 0f - (float)(value.GetValueOrDefault() * stiAxisArea.YAxis.Info.Dpi) + dividerY);
	}

	protected RectangleF GetColumnRect(StiContext context, IStiClusteredColumnSeries currentSeries, double? value, float seriesLeftPos, float seriesWidth)
	{
		IStiAxisArea stiAxisArea = currentSeries.Chart.Area as IStiAxisArea;
		float num = 0f;
		float num2 = 0f;
		float dividerY = stiAxisArea.AxisCore.GetDividerY();
		float dividerRightY = stiAxisArea.AxisCore.GetDividerRightY();
		if (currentSeries.YAxis == StiSeriesYAxis.LeftYAxis)
		{
			if (!stiAxisArea.ReverseVert)
			{
				num = (float)(value.GetValueOrDefault() * stiAxisArea.YAxis.Info.Dpi);
				num2 = 0f - num + dividerY;
			}
			else
			{
				num = (float)(value.GetValueOrDefault() * stiAxisArea.YAxis.Info.Dpi);
				num2 = 0f - num + dividerY;
			}
		}
		else
		{
			num = (float)(value.GetValueOrDefault() * stiAxisArea.YRightAxis.Info.Dpi);
			num2 = 0f - num + dividerRightY;
		}
		if (currentSeries.ShowZeros && (value == 0.0 || !value.HasValue))
		{
			num = Math.Max(context.Options.Zoom, 2f);
			if (!stiAxisArea.ReverseVert)
			{
				num2 -= num;
			}
		}
		if (value < 0.0)
		{
			num2 = ((currentSeries.YAxis != 0) ? dividerRightY : dividerY);
			num = 0f - num;
		}
		return new RectangleF(seriesLeftPos, num2, seriesWidth, num);
	}

	protected PointF CorrectPoint(PointF point, RectangleF rect)
	{
		if (point.Y < 0f)
		{
			return new PointF(point.X, 0f);
		}
		if (point.Y > rect.Height)
		{
			return new PointF(point.X, rect.Height);
		}
		return point;
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return ((IStiClusteredColumnSeries)base.Series).Brush;
		}
		return seriesBrush;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		object seriesBorderColor = base.GetSeriesBorderColor(colorIndex, colorCount);
		if (seriesBorderColor == null)
		{
			return ((IStiClusteredColumnSeries)base.Series).BorderColor;
		}
		return seriesBorderColor;
	}

	private float MeasureLeftPosition(IStiAxisArea axisArea, int pointIndex, float sumSeriesWidth, bool startFromZero)
	{
		float num = (startFromZero ? axisArea.XAxis.Info.StripPositions[pointIndex + 1] : axisArea.XAxis.Info.StripPositions[pointIndex]);
		num -= axisArea.XAxis.Info.Step / 2f;
		return num + (axisArea.XAxis.Info.Step - sumSeriesWidth) / 2f;
	}

	private double? GetSeriesCurrentValue(IStiAxisArea axisArea, IStiClusteredColumnSeries currentSeries, int pointIndex)
	{
		double? num = (axisArea.ReverseHor ? currentSeries.Values[currentSeries.Values.Length - pointIndex - 1] : currentSeries.Values[pointIndex]);
		if (axisArea.ReverseVert && num.HasValue)
		{
			num = 0.0 - num;
		}
		return num;
	}

	private double? GetSeriesValueStart(IStiAxisArea axisArea, double?[] valuesStart, int pointIndex)
	{
		double? num = null;
		if (((StiChart)base.Series.Chart).IsAnimationChangingValues)
		{
			if (valuesStart != null && valuesStart.Length > pointIndex)
			{
				num = (axisArea.ReverseHor ? valuesStart[valuesStart.Length - pointIndex - 1] : valuesStart[pointIndex]);
			}
			if (axisArea.ReverseVert && num.HasValue)
			{
				num = 0.0 - num;
			}
		}
		return num;
	}

	private StiBrush GetSeriesBrush(IStiClusteredColumnSeries currentSeries, double? value, int colorIndex, int colorCount, int pointIndex)
	{
		StiBrush seriesBrush = currentSeries.Core.GetSeriesBrush(colorIndex, colorCount);
		if (currentSeries.AllowApplyBrushNegative && value < 0.0)
		{
			seriesBrush = currentSeries.BrushNegative;
		}
		return currentSeries.ProcessSeriesBrushes(pointIndex, seriesBrush);
	}

	public StiClusteredColumnSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
