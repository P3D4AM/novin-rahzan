using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSteppedRangeSeriesCoreXF : StiSteppedLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SteppedRange");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiSteppedRangeSeries stiSteppedRangeSeries = base.Series as IStiSteppedRangeSeries;
		if (stiSteppedRangeSeries.AllowApplyStyle)
		{
			stiSteppedRangeSeries.Brush = style.Core.GetAreaBrush(color);
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiSteppedRangeArea stiSteppedRangeArea = geom.Area as IStiSteppedRangeArea;
		IStiAxisArea stiAxisArea = geom.Area as IStiAxisArea;
		bool startFromZero = stiAxisArea.XAxis.Core.GetStartFromZero();
		for (int i = 0; i < series.Length; i++)
		{
			IStiSteppedRangeSeries stiSteppedRangeSeries = (IStiSteppedRangeSeries)series[i];
			int num = Math.Min(stiSteppedRangeSeries.Values.Length, stiSteppedRangeSeries.ValuesEnd.Length);
			PointF?[] array = new PointF?[num];
			PointF?[] array2 = new PointF?[num];
			string[] array3 = new string[num];
			for (int j = 0; j < num; j++)
			{
				double? value = (stiSteppedRangeArea.ReverseHor ? stiSteppedRangeSeries.Values[num - j - 1] : stiSteppedRangeSeries.Values[j]);
				double? value2 = (stiSteppedRangeArea.ReverseHor ? stiSteppedRangeSeries.ValuesEnd[num - j - 1] : stiSteppedRangeSeries.ValuesEnd[j]);
				array[j] = GetYPoint(value, stiSteppedRangeSeries, stiSteppedRangeArea, j);
				array2[j] = GetYPoint(value2, stiSteppedRangeSeries, stiSteppedRangeArea, j);
				object obj = (startFromZero ? stiAxisArea.XAxis.Info.StripLines[j + 1].ValueObject : stiAxisArea.XAxis.Info.StripLines[j].ValueObject);
				array3[j] = ((obj == null) ? "" : obj.ToString());
			}
			StiSeriesPointsInfo pointsInfo = new StiSeriesPointsInfo
			{
				Points = array,
				PointsIds = array3,
				PointsEnd = array2
			};
			StiSeriesPointsInfo pointsInfo2 = new StiSeriesPointsInfo
			{
				Points = array2,
				PointsIds = array3,
				AdditionalSeriesId = "_e"
			};
			RenderAreas(context, geom, pointsInfo, stiSteppedRangeSeries);
			RenderLines(context, geom, pointsInfo, stiSteppedRangeSeries.Values, stiSteppedRangeSeries);
			RenderLines(context, geom, pointsInfo2, stiSteppedRangeSeries.ValuesEnd, stiSteppedRangeSeries);
			float num2 = 0f;
			IStiAxisSeriesLabels seriesLabels = stiSteppedRangeSeries.Core.GetSeriesLabels();
			if (seriesLabels == null || !seriesLabels.Visible)
			{
				continue;
			}
			for (int k = 0; k < num; k++)
			{
				double? num3 = (stiAxisArea.ReverseHor ? stiSteppedRangeSeries.Values[stiSteppedRangeSeries.Values.Length - k - 1] : stiSteppedRangeSeries.Values[k]);
				double? num4 = (stiAxisArea.ReverseHor ? stiSteppedRangeSeries.ValuesEnd[stiSteppedRangeSeries.ValuesEnd.Length - k - 1] : stiSteppedRangeSeries.ValuesEnd[k]);
				if (!num3.HasValue && stiSteppedRangeSeries.ShowNulls)
				{
					num3 = 0.0;
				}
				if (!num4.HasValue && stiSteppedRangeSeries.ShowNulls)
				{
					num3 = 0.0;
				}
				double? num5 = num3;
				if (stiSteppedRangeArea.ReverseVert && num3.HasValue)
				{
					num5 = 0.0 - num5;
				}
				double? num6 = num4;
				if (stiSteppedRangeArea.ReverseVert && num4.HasValue)
				{
					num6 = 0.0 - num6;
				}
				num2 = ((stiSteppedRangeSeries.YAxis != 0) ? stiSteppedRangeArea.AxisCore.GetDividerRightY() : stiSteppedRangeArea.AxisCore.GetDividerY());
				PointF? pointF = array[k];
				PointF? pointF2 = array2[k];
				if (pointF.HasValue)
				{
					PointF point = new PointF(pointF.Value.X, num2);
					if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
					{
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiSteppedRangeSeries, context, CorrectPoint(pointF.Value, rect, (float)stiSteppedRangeSeries.LabelsOffset * context.Options.Zoom), CorrectPoint(point, rect, (float)stiSteppedRangeSeries.LabelsOffset * context.Options.Zoom), k, num5, num3, stiSteppedRangeArea.AxisCore.GetArgumentLabel(stiSteppedRangeArea.XAxis.Info.StripLines[k], stiSteppedRangeSeries), GetTag(k), 0, 1, rect);
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
				PointF point2 = new PointF(pointF2.Value.X, num2);
				if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
				{
					StiSeriesLabelsGeom stiSeriesLabelsGeom2 = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiSteppedRangeSeries, context, CorrectPoint(pointF2.Value, rect, (float)stiSteppedRangeSeries.LabelsOffset * context.Options.Zoom), CorrectPoint(point2, rect, (float)stiSteppedRangeSeries.LabelsOffset * context.Options.Zoom), k, num6, num4, stiSteppedRangeArea.AxisCore.GetArgumentLabel(stiSteppedRangeArea.XAxis.Info.StripLines[k], stiSteppedRangeSeries), GetTag(k), 0, 1, rect);
					if (stiSeriesLabelsGeom2 != null)
					{
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(stiSeriesLabelsGeom2);
						stiSeriesLabelsGeom2.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom2.ClientRectangle);
					}
				}
			}
		}
	}

	private void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo, IStiSteppedRangeSeries series)
	{
		if (pointsInfo.Points != null && pointsInfo.Points.Length > 1 && pointsInfo.PointsEnd != null && pointsInfo.PointsEnd.Length > 1)
		{
			StiSteppedRangeSeriesGeom stiSteppedRangeSeriesGeom = new StiSteppedRangeSeriesGeom(geom, pointsInfo, series);
			if (stiSteppedRangeSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiSteppedRangeSeriesGeom);
			}
		}
	}

	private void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo, double?[] values, IStiSteppedRangeSeries series)
	{
		if (pointsInfo.Points.Length > 1 || (series.PointAtCenter && pointsInfo.Points.Length != 0))
		{
			StiSteppedLineSeriesGeom stiSteppedLineSeriesGeom = new StiSteppedLineSeriesGeom(geom, pointsInfo, series);
			if (stiSteppedLineSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiSteppedLineSeriesGeom);
			}
			if (base.Interaction != null)
			{
				stiSteppedLineSeriesGeom.Interactions = GetInteractions(context, geom, pointsInfo.Points);
			}
		}
		RenderMarkers(context, geom, pointsInfo.Points, values, series);
	}

	private void RenderMarkers(StiContext context, StiAreaGeom geom, PointF?[] points, double?[] values, IStiSteppedRangeSeries series)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
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
		StiSolidBrush val = new StiSolidBrush(Color.FromArgb(55, Color.Black));
		StiPenGeom stiPenGeom = new StiPenGeom(series.Marker.BorderColor);
		int num = 0;
		for (int i = 0; i < points.Length; i++)
		{
			PointF? pointF = points[i];
			if (pointF.HasValue)
			{
				double? num2 = (stiAxisArea.ReverseHor ? values[series.Values.Length - num - 1] : values[num]);
				if (!num2.HasValue && series.ShowNulls)
				{
					num2 = 0.0;
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
					StiMarkerGeom stiMarkerGeom = new StiMarkerGeom(series, num, num2.GetValueOrDefault(), pointF.Value, series.Marker, series.ShowShadow, context.Options.Zoom, flag);
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

	private PointF? GetYPoint(double? value, IStiSteppedRangeSeries currentSeries, IStiSteppedRangeArea axisArea, int index)
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

	public StiSteppedRangeSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
