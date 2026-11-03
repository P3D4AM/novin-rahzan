using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiScatterSeriesCoreXF : StiBaseLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Scatter");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiBaseLineSeries stiBaseLineSeries = base.Series as IStiBaseLineSeries;
		if (stiBaseLineSeries.AllowApplyStyle && !style.Core.MarkerVisible)
		{
			stiBaseLineSeries.Marker.Visible = true;
		}
	}

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		RenderMarkers(context, geom, pointsInfo.Points);
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		if (seriesArray == null || seriesArray.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiScatterArea stiScatterArea = geom.Area as IStiScatterArea;
		bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
		double num = 0.0;
		List<PointF?[]> list = new List<PointF?[]>();
		List<string[]> list2 = new List<string[]>();
		for (int i = 0; i < seriesArray.Length; i++)
		{
			IStiScatterSeries stiScatterSeries = seriesArray[i] as IStiScatterSeries;
			int num2 = Math.Min(stiScatterSeries.Values.Length, stiScatterSeries.Arguments.Length);
			PointF?[] array = new PointF?[num2];
			string[] array2 = new string[num2];
			for (int j = 0; j < num2; j++)
			{
				double? num3 = stiScatterSeries.Values[j];
				if (!num3.HasValue && ((IStiScatterSeries)base.Series).ShowNulls)
				{
					num3 = 0.0;
				}
				if (num3 == 0.0 && !((IStiScatterSeries)base.Series).ShowZeros)
				{
					array[j] = null;
					array2[j] = "_zero_";
				}
				else if (num3.HasValue)
				{
					if (stiScatterArea.ReverseVert && num3.HasValue && !stiScatterArea.YAxis.LogarithmicScale)
					{
						num3 = 0.0 - num3;
					}
					double num4 = 0.0;
					if (!stiScatterArea.YAxis.LogarithmicScale)
					{
						num4 = ((stiScatterSeries.YAxis != 0) ? ((0.0 - num3.GetValueOrDefault()) * stiScatterArea.YRightAxis.Info.Dpi + (double)stiScatterArea.AxisCore.GetDividerRightY()) : ((0.0 - num3.GetValueOrDefault()) * stiScatterArea.YAxis.Info.Dpi + (double)stiScatterArea.AxisCore.GetDividerY()));
					}
					else
					{
						int count = stiScatterArea.YAxis.Info.StripLines.Count;
						int index = 0;
						int index2 = count - 1;
						double value = stiScatterArea.YAxis.Info.StripLines[index].Value;
						double value2 = stiScatterArea.YAxis.Info.StripLines[index2].Value;
						double num5 = Math.Abs((double)rect.Height / (Math.Log10(value2) - Math.Log10(value)));
						num4 = Math.Abs(Math.Log10(stiScatterArea.YAxis.Info.StripLines[index].Value) * num5 - Math.Log10(num3.GetValueOrDefault()) * num5);
					}
					double? num6 = null;
					if (stiScatterSeries.Arguments[j] is DateTime)
					{
						num6 = Convert.ToDouble(((DateTime)stiScatterSeries.Arguments[j]).ToOADate());
					}
					else
					{
						try
						{
							if (stiScatterSeries.Arguments[j] == null)
							{
								num6 = null;
							}
							else
							{
								object obj = stiScatterSeries.Arguments[j];
								if (obj is string)
								{
									double result = 0.0;
									num6 = ((!StiSeries.TryParseValue((string)obj, ((StiChart)stiScatterSeries.Chart).Report.Culture, out result)) ? null : new double?(result));
								}
								else
								{
									num6 = (double)StiConvert.ChangeType(obj, typeof(double));
								}
							}
						}
						catch
						{
							num6 = null;
						}
					}
					if (!num6.HasValue)
					{
						array[j] = null;
						array2[j] = "_zero_";
						continue;
					}
					double num7 = 0.0;
					if (stiScatterArea.XAxis.LogarithmicScale && stiScatterArea.XAxis.Info.StripLines.Count > 0)
					{
						int count2 = stiScatterArea.XAxis.Info.StripLines.Count;
						int index3 = (stiScatterArea.ReverseHor ? (count2 - 1) : 0);
						int index4 = ((!stiScatterArea.ReverseHor) ? (count2 - 1) : 0);
						double value3 = stiScatterArea.XAxis.Info.StripLines[index3].Value;
						double value4 = stiScatterArea.XAxis.Info.StripLines[index4].Value;
						double num8 = Math.Abs((double)rect.Width / (Math.Log10(value4) - Math.Log10(value3)));
						num7 = Math.Log10(num6.Value) * num8 - Math.Log10(stiScatterArea.XAxis.Info.StripLines[index3].Value) * num8;
					}
					else
					{
						num7 = (num6.Value - stiScatterArea.XAxis.Info.Minimum) * stiScatterArea.XAxis.Info.Dpi;
					}
					if (stiScatterArea.ReverseHor)
					{
						num7 = (double)rect.Width - num7;
					}
					array[j] = new PointF((float)num7, (float)num4);
					array2[j] = num6.Value.ToString();
				}
				else
				{
					array[j] = null;
					array2[j] = "_zero_";
				}
			}
			list.Add(array);
			list2.Add(array2);
		}
		int num9 = 0;
		foreach (PointF?[] item in list)
		{
			IStiScatterSeries stiScatterSeries2 = seriesArray[num9] as IStiScatterSeries;
			StiSeriesPointsInfo pointsInfo = new StiSeriesPointsInfo
			{
				Points = item,
				PointsIds = list2[num9]
			};
			((StiScatterSeriesCoreXF)stiScatterSeries2.Core).RenderLines(context, geom, pointsInfo);
			StiTrendLinesCollection trendLines = ((StiSeries)stiScatterSeries2).TrendLines;
			foreach (StiTrendLine item2 in trendLines)
			{
				item2.Core.RenderTrendLine(geom, item, stiScatterArea.AxisCore.GetDividerY());
			}
			IStiAxisSeriesLabels seriesLabels = stiScatterSeries2.Core.GetSeriesLabels();
			if (seriesLabels != null && seriesLabels.Visible)
			{
				for (int k = 0; k < item.Length; k++)
				{
					double? num10 = stiScatterSeries2.Values[k];
					double? num11 = num10;
					if (stiScatterArea.ReverseVert && num10.HasValue)
					{
						num11 = 0.0 - num11;
					}
					num = ((stiScatterSeries2.YAxis != 0) ? ((double)stiScatterArea.AxisCore.GetDividerRightY()) : ((double)stiScatterArea.AxisCore.GetDividerY()));
					PointF? pointF = item[k];
					if (!pointF.HasValue)
					{
						continue;
					}
					PointF pointF2 = new PointF(pointF.Value.X, (float)num);
					RectangleF clientRectangle = ((StiAxisAreaGeom)geom).View.ClientRectangle;
					clientRectangle.X = 0f;
					clientRectangle.Y = 0f;
					clientRectangle.Inflate(10f, 10f);
					if ((rect.Contains(pointF2) || pointF2.Y.Equals(rect.Bottom) || rect.Contains(pointF.Value) || clientRectangle.Contains(pointF.Value)) && (seriesLabels.Step == 0 || k % seriesLabels.Step == 0))
					{
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiScatterSeries2, context, CorrectPoint(pointF.Value, rect, (float)stiScatterSeries2.LabelsOffset * context.Options.Zoom), CorrectPoint(pointF2, rect, (float)stiScatterSeries2.LabelsOffset * context.Options.Zoom), k, num11, num10, stiScatterSeries2.Arguments[k].ToString(), stiScatterSeries2.Core.GetTag(k), 0, 1, rect);
						if (stiSeriesLabelsGeom != null)
						{
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiSeriesLabelsGeom);
							stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom.ClientRectangle);
						}
					}
				}
			}
			num9++;
		}
		if (geom.Area.Chart.SeriesLabels.PreventIntersection)
		{
			CheckIntersectionLabels(geom);
		}
	}

	public StiScatterSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
