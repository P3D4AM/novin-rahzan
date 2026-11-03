using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiBubbleSeriesCoreXF : StiScatterSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Bubble");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		if (base.Series.AllowApplyStyle && base.Series is IStiBubbleSeries stiBubbleSeries)
		{
			stiBubbleSeries.Brush = style.Core.GetColumnBrush(color);
			stiBubbleSeries.BorderColor = style.Core.GetColumnBorder(color);
			stiBubbleSeries.BorderThickness = style.Core.SeriesBorderThickness;
		}
	}

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
	}

	protected void RenderBubbles(StiContext context, StiAreaGeom geom, IStiBubbleSeries series, PointF?[] points, double[] weights)
	{
		double num = 0.0;
		int num2 = 0;
		foreach (IStiSeries item in series.Chart.Series)
		{
			if (item is IStiBubbleSeries { Weights: var weights2 })
			{
				foreach (double num3 in weights2)
				{
					num = ((num2 != 0) ? Math.Max(num3, num) : num3);
					num2++;
				}
			}
		}
		IStiAxisArea stiAxisArea = geom.Area as IStiAxisArea;
		double num4 = (double)Math.Min(stiAxisArea.XAxis.Info.Step, stiAxisArea.YAxis.Info.Step) * 0.9;
		double num5 = num4 / num;
		TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
		num2 = 0;
		foreach (double num6 in weights)
		{
			PointF? pointF = points[num2];
			if (pointF.HasValue && num6 > 0.0)
			{
				float num7 = (float)(num6 * num5);
				RectangleF clientRectangle = new RectangleF(pointF.Value.X - num7 / 2f, pointF.Value.Y - num7 / 2f, num7, num7);
				StiBrush seriesBrush = GetSeriesBrush(num2, points.Length);
				Color seriesBorderColor = (Color)series.Core.GetSeriesBorderColor(num2, points.Length);
				StiBubbleSeriesElementGeom stiBubbleSeriesElementGeom = new StiBubbleSeriesElementGeom(geom, num6, num2, seriesBrush, seriesBorderColor, series, clientRectangle, new TimeSpan(globalBeginTimeElement.Ticks / ((StiAxisAreaCoreXF)stiAxisArea.Core).ValuesCount * num2));
				if (series.Core.Interaction != null)
				{
					stiBubbleSeriesElementGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, series, num2);
				}
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiBubbleSeriesElementGeom);
			}
			num2++;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		if (seriesArray == null || seriesArray.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiScatterArea stiScatterArea = geom.Area as IStiScatterArea;
		double num = 0.0;
		List<List<PointF?>> list = new List<List<PointF?>>();
		List<List<double>> list2 = new List<List<double>>();
		for (int i = 0; i < seriesArray.Length; i++)
		{
			IStiBubbleSeries stiBubbleSeries = seriesArray[i] as IStiBubbleSeries;
			int num2 = Math.Min(stiBubbleSeries.Values.Length, stiBubbleSeries.Arguments.Length);
			List<PointF?> list3 = new List<PointF?>();
			List<double> list4 = new List<double>();
			for (int j = 0; j < num2; j++)
			{
				double? num3 = stiBubbleSeries.Values[j];
				if (!num3.HasValue && stiBubbleSeries.ShowNulls)
				{
					num3 = 0.0;
				}
				if (stiScatterArea.ReverseVert && num3.HasValue)
				{
					num3 = 0.0 - num3;
				}
				double num4 = 0.0;
				num4 = ((stiBubbleSeries.YAxis != 0) ? ((0.0 - num3.GetValueOrDefault()) * (double)(float)stiScatterArea.YRightAxis.Info.Dpi + (double)stiScatterArea.AxisCore.GetDividerRightY()) : ((0.0 - num3.GetValueOrDefault()) * (double)(float)stiScatterArea.YAxis.Info.Dpi + (double)stiScatterArea.AxisCore.GetDividerY()));
				double? num5 = null;
				if (stiBubbleSeries.Arguments[j] is DateTime)
				{
					num5 = (float)Convert.ToDouble(((DateTime)stiBubbleSeries.Arguments[j]).ToOADate());
				}
				else
				{
					try
					{
						if (stiBubbleSeries.Arguments[j] != null && stiBubbleSeries.Arguments[j] != DBNull.Value)
						{
							object obj = stiBubbleSeries.Arguments[j];
							if (obj is string)
							{
								double result = 0.0;
								num5 = ((!double.TryParse((string)obj, out result)) ? null : new double?(result));
							}
							else
							{
								num5 = (double)StiConvert.ChangeType(obj, typeof(double));
							}
						}
					}
					catch
					{
						num5 = null;
					}
				}
				if (!num5.HasValue && stiBubbleSeries.ShowNulls)
				{
					num5 = 0.0;
				}
				if (num3.HasValue && num5.HasValue)
				{
					double num6 = (num5.Value - stiScatterArea.XAxis.Info.Minimum) * stiScatterArea.XAxis.Info.Dpi;
					if (stiScatterArea.ReverseHor)
					{
						num6 = (double)rect.Width - num6;
					}
					list3.Add(new PointF((float)num6, (float)num4));
				}
				else
				{
					list3.Add(null);
				}
				list4.Add((j < stiBubbleSeries.Weights.Length) ? stiBubbleSeries.Weights[j] : 0.0);
			}
			list.Add(list3);
			list2.Add(list4);
		}
		int num7 = 0;
		foreach (List<PointF?> item in list)
		{
			List<double> list5 = list2[num7];
			IStiBubbleSeries stiBubbleSeries2 = seriesArray[num7] as IStiBubbleSeries;
			((StiBubbleSeriesCoreXF)stiBubbleSeries2.Core).RenderBubbles(context, geom, stiBubbleSeries2, item.ToArray(), list5.ToArray());
			StiTrendLinesCollection trendLines = ((StiSeries)stiBubbleSeries2).TrendLines;
			PointF?[] array = new PointF?[item.Count];
			item.CopyTo(array);
			foreach (StiTrendLine item2 in trendLines)
			{
				item2.Core.RenderTrendLine(geom, array, stiScatterArea.AxisCore.GetDividerY());
			}
			IStiAxisSeriesLabels seriesLabels = stiBubbleSeries2.Core.GetSeriesLabels();
			if (seriesLabels != null && seriesLabels.Visible)
			{
				for (int k = 0; k < item.Count; k++)
				{
					double? num8 = stiBubbleSeries2.Values[k];
					double? num9 = num8;
					if (stiScatterArea.ReverseVert && num9.HasValue)
					{
						num9 = 0.0 - num9;
					}
					num = ((stiBubbleSeries2.YAxis != 0) ? ((double)stiScatterArea.AxisCore.GetDividerRightY()) : ((double)stiScatterArea.AxisCore.GetDividerY()));
					PointF? pointF = item[k];
					if (!pointF.HasValue)
					{
						continue;
					}
					PointF pointF2 = new PointF(pointF.Value.X, (float)num);
					if ((rect.Contains(pointF2) || pointF2.Y.Equals(rect.Bottom) || rect.Contains(pointF.Value)) && (seriesLabels.Step == 0 || k % seriesLabels.Step == 0))
					{
						double weight = ((stiBubbleSeries2.Weights.Length > k) ? stiBubbleSeries2.Weights[k] : 0.0);
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiBubbleSeries2, context, CorrectPoint(pointF.Value, rect, (float)stiBubbleSeries2.LabelsOffset * context.Options.Zoom), CorrectPoint(pointF2, rect, (float)stiBubbleSeries2.LabelsOffset * context.Options.Zoom), k, num9, num8, stiBubbleSeries2.Arguments[k].ToString(), stiBubbleSeries2.Core.GetTag(k), weight, k, item.Count, rect);
						if (stiSeriesLabelsGeom != null)
						{
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiSeriesLabelsGeom);
							stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom.ClientRectangle);
						}
					}
				}
			}
			num7++;
		}
	}

	public StiBubbleSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
