using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRangeSeriesCoreXF : StiLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Range");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiLineRangeSeries stiLineRangeSeries = base.Series as IStiLineRangeSeries;
		if (stiLineRangeSeries.AllowApplyStyle)
		{
			stiLineRangeSeries.Brush = style.Core.GetAreaBrush(color);
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiRangeArea stiRangeArea = geom.Area as IStiRangeArea;
		IStiAxisArea stiAxisArea = geom.Area as IStiAxisArea;
		bool startFromZero = stiAxisArea.XAxis.Core.GetStartFromZero();
		int num = 0;
		for (int i = 0; i < series.Length; i++)
		{
			IStiLineRangeSeries stiLineRangeSeries = (IStiLineRangeSeries)series[i];
			int num2 = Math.Min(stiLineRangeSeries.Values.Length, stiLineRangeSeries.ValuesEnd.Length);
			PointF?[] array = new PointF?[num2];
			PointF?[] array2 = new PointF?[num2];
			string[] array3 = new string[num2];
			for (int j = 0; j < num2; j++)
			{
				double? num3 = (stiRangeArea.ReverseHor ? stiLineRangeSeries.Values[num2 - j - 1] : stiLineRangeSeries.Values[j]);
				double? num4 = (stiRangeArea.ReverseHor ? stiLineRangeSeries.ValuesEnd[num2 - j - 1] : stiLineRangeSeries.ValuesEnd[j]);
				if (num3 == num4)
				{
					array[j] = (array2[j] = GetYPoint(num3, stiLineRangeSeries, stiRangeArea, j));
				}
				else
				{
					array[j] = GetYPoint(num3, stiLineRangeSeries, stiRangeArea, j);
					array2[j] = GetYPoint(num4, stiLineRangeSeries, stiRangeArea, j);
				}
				object obj = (startFromZero ? stiAxisArea.XAxis.Info.StripLines[j + 1].ValueObject : stiAxisArea.XAxis.Info.StripLines[j].ValueObject);
				array3[j] = ((obj == null) ? "" : obj.ToString());
			}
			StiSeriesPointsInfo pointsInfo = new StiSeriesPointsInfo
			{
				Points = array,
				PointsEnd = array2,
				PointsIds = array3
			};
			StiSeriesPointsInfo pointsInfo2 = new StiSeriesPointsInfo
			{
				Points = array2,
				PointsIds = array3,
				AdditionalSeriesId = "_e"
			};
			RenderAreas(geom, pointsInfo, stiLineRangeSeries);
			RenderLines(context, geom, pointsInfo, stiLineRangeSeries.Values, stiLineRangeSeries);
			RenderLines(context, geom, pointsInfo2, stiLineRangeSeries.ValuesEnd, stiLineRangeSeries);
			float num5 = 0f;
			IStiAxisSeriesLabels seriesLabels = stiLineRangeSeries.Core.GetSeriesLabels();
			if (seriesLabels != null && seriesLabels.Visible)
			{
				for (int k = 0; k < num2; k++)
				{
					double? num6 = (stiAxisArea.ReverseHor ? stiLineRangeSeries.Values[stiLineRangeSeries.Values.Length - k - 1] : stiLineRangeSeries.Values[k]);
					double? num7 = (stiAxisArea.ReverseHor ? stiLineRangeSeries.ValuesEnd[stiLineRangeSeries.ValuesEnd.Length - k - 1] : stiLineRangeSeries.ValuesEnd[k]);
					if (!num6.HasValue && stiLineRangeSeries.ShowNulls)
					{
						num6 = 0.0;
					}
					if (!num7.HasValue && stiLineRangeSeries.ShowNulls)
					{
						num6 = 0.0;
					}
					double? num8 = num6;
					if (stiRangeArea.ReverseVert && num6.HasValue)
					{
						num8 = 0.0 - num8;
					}
					double? num9 = num7;
					if (stiRangeArea.ReverseVert && num7.HasValue)
					{
						num9 = 0.0 - num9;
					}
					num5 = ((stiLineRangeSeries.YAxis != 0) ? stiRangeArea.AxisCore.GetDividerRightY() : stiRangeArea.AxisCore.GetDividerY());
					PointF? pointF = array[k];
					PointF? pointF2 = array2[k];
					if (pointF.HasValue)
					{
						PointF point = new PointF(pointF.Value.X, num5);
						if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
						{
							StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiLineRangeSeries, context, CorrectPoint(pointF.Value, rect, (float)stiLineRangeSeries.LabelsOffset * context.Options.Zoom), CorrectPoint(point, rect, (float)stiLineRangeSeries.LabelsOffset * context.Options.Zoom), k, num8, num6, stiRangeArea.AxisCore.GetArgumentLabel(stiRangeArea.XAxis.Info.StripLines[k], stiLineRangeSeries), stiLineRangeSeries.Core.GetTag(k), 0, 1, rect);
							if (stiSeriesLabelsGeom != null)
							{
								geom.CreateChildGeoms();
								geom.ChildGeoms.Add(stiSeriesLabelsGeom);
								stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom.ClientRectangle);
							}
						}
					}
					if (!pointF2.HasValue)
					{
						continue;
					}
					PointF point2 = new PointF(pointF2.Value.X, num5);
					if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
					{
						StiSeriesLabelsGeom stiSeriesLabelsGeom2 = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiLineRangeSeries, context, CorrectPoint(pointF2.Value, rect, (float)stiLineRangeSeries.LabelsOffset * context.Options.Zoom), CorrectPoint(point2, rect, (float)stiLineRangeSeries.LabelsOffset * context.Options.Zoom), k, num9, num7, stiRangeArea.AxisCore.GetArgumentLabel(stiRangeArea.XAxis.Info.StripLines[k], stiLineRangeSeries), stiLineRangeSeries.Core.GetTag(k), 0, 1, rect);
						if (stiSeriesLabelsGeom2 != null)
						{
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiSeriesLabelsGeom2);
							stiSeriesLabelsGeom2.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom2.ClientRectangle);
						}
					}
				}
			}
			num++;
		}
	}

	private void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo, double?[] values, IStiLineRangeSeries series)
	{
		if (pointsInfo.Points != null && pointsInfo.Points.Length > 1)
		{
			StiLineSeriesGeom stiLineSeriesGeom = new StiLineSeriesGeom(geom, pointsInfo, series);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(stiLineSeriesGeom);
			if (base.Interaction != null)
			{
				stiLineSeriesGeom.Interactions = GetInteractions(context, geom, pointsInfo.Points);
			}
		}
		RenderMarkers(context, geom, pointsInfo.Points, values, series);
	}

	private void RenderMarkers(StiContext context, StiAreaGeom geom, PointF?[] points, double?[] values, IStiLineRangeSeries series)
	{
		IStiAxisArea stiAxisArea = geom.Area as IStiAxisArea;
		if (points.Length == 0)
		{
			return;
		}
		bool flag = !series.Marker.Visible && series.ToolTips.Length != 0;
		if (series.Marker == null || !(series.Marker.Visible || flag))
		{
			return;
		}
		int num = 0;
		int num2 = Math.Min(series.Values.Length, series.ValuesEnd.Length);
		for (int i = 0; i < points.Length; i++)
		{
			PointF? pointF = points[i];
			if (pointF.HasValue)
			{
				double? num3 = (stiAxisArea.ReverseHor ? values[num2 - num - 1] : values[num]);
				if (!num3.HasValue && series.ShowNulls)
				{
					num3 = 0.0;
				}
				RectangleF clientRectangle = ((StiAxisAreaGeom)geom).View.ClientRectangle;
				clientRectangle.X = 0f;
				clientRectangle.Y = 0f;
				clientRectangle.Inflate(10f, 10f);
				PointF value = pointF.Value;
				value.X += geom.ClientRectangle.X;
				value.Y += geom.ClientRectangle.Y;
				if (clientRectangle.Contains(value))
				{
					StiMarkerGeom stiMarkerGeom = new StiMarkerGeom(series, num, num3.GetValueOrDefault(), pointF.Value, series.Marker, series.ShowShadow, context.Options.Zoom, flag);
					if (stiMarkerGeom != null)
					{
						if (series.Core.Interaction != null)
						{
							stiMarkerGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, series, num);
						}
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(stiMarkerGeom);
					}
				}
			}
			num++;
		}
	}

	private PointF? GetYPoint(double? value, IStiLineRangeSeries currentSeries, IStiRangeArea axisArea, int index)
	{
		float x = (axisArea.XAxis.Core.GetStartFromZero() ? axisArea.XAxis.Info.StripPositions[index + 1] : axisArea.XAxis.Info.StripPositions[index]);
		PointF? result;
		if (!value.HasValue && !currentSeries.ShowNulls)
		{
			result = null;
		}
		else
		{
			if (!value.HasValue && currentSeries.ShowNulls)
			{
				value = 0.0;
			}
			if (axisArea.ReverseVert && value.HasValue)
			{
				value = 0.0 - value;
			}
			double num = 0.0;
			num = ((currentSeries.YAxis != 0) ? ((0.0 - value.GetValueOrDefault()) * (double)(float)axisArea.YRightAxis.Info.Dpi + (double)axisArea.AxisCore.GetDividerRightY()) : ((0.0 - value.GetValueOrDefault()) * (double)(float)axisArea.YAxis.Info.Dpi + (double)axisArea.AxisCore.GetDividerY()));
			result = new PointF(x, (float)num);
		}
		return result;
	}

	private void RenderAreas(StiAreaGeom geom, StiSeriesPointsInfo pointsInfo, IStiLineRangeSeries series)
	{
		if (pointsInfo.Points != null && pointsInfo.Points.Length > 1)
		{
			StiRangeSeriesGeom stiRangeSeriesGeom = new StiRangeSeriesGeom(geom, pointsInfo, series);
			if (stiRangeSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiRangeSeriesGeom);
			}
		}
	}

	public StiRangeSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
