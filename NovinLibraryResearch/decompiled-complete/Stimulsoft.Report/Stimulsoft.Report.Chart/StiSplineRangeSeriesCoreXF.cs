using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSplineRangeSeriesCoreXF : StiSplineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SplineRange");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiSplineRangeSeries stiSplineRangeSeries = base.Series as IStiSplineRangeSeries;
		if (stiSplineRangeSeries.AllowApplyStyle)
		{
			stiSplineRangeSeries.Brush = style.Core.GetAreaBrush(color);
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiSplineRangeArea stiSplineRangeArea = geom.Area as IStiSplineRangeArea;
		IStiAxisArea stiAxisArea = geom.Area as IStiAxisArea;
		bool startFromZero = stiAxisArea.XAxis.Core.GetStartFromZero();
		int num = 0;
		for (int i = 0; i < series.Length; i++)
		{
			IStiSplineRangeSeries stiSplineRangeSeries = (IStiSplineRangeSeries)series[i];
			int num2 = stiSplineRangeSeries.Values.Length;
			if (stiSplineRangeSeries.ValuesEnd.Length < num2)
			{
				num2 = stiSplineRangeSeries.ValuesEnd.Length;
			}
			PointF?[] array = new PointF?[num2];
			PointF?[] array2 = new PointF?[num2];
			string[] array3 = new string[num2];
			for (int j = 0; j < num2; j++)
			{
				double? value = (stiSplineRangeArea.ReverseHor ? stiSplineRangeSeries.Values[stiSplineRangeSeries.Values.Length - j - 1] : stiSplineRangeSeries.Values[j]);
				double? value2 = (stiSplineRangeArea.ReverseHor ? stiSplineRangeSeries.ValuesEnd[stiSplineRangeSeries.Values.Length - j - 1] : stiSplineRangeSeries.ValuesEnd[j]);
				array[j] = GetYPoint(value, stiSplineRangeSeries, stiSplineRangeArea, j);
				array2[j] = GetYPoint(value2, stiSplineRangeSeries, stiSplineRangeArea, j);
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
			RenderAreas(context, geom, pointsInfo, stiSplineRangeSeries);
			RenderLines(context, geom, pointsInfo);
			RenderLines(context, geom, pointsInfo2);
			float num3 = 0f;
			IStiAxisSeriesLabels seriesLabels = stiSplineRangeSeries.Core.GetSeriesLabels();
			if (seriesLabels != null && seriesLabels.Visible)
			{
				for (int k = 0; k < num2; k++)
				{
					double? num4 = (stiAxisArea.ReverseHor ? stiSplineRangeSeries.Values[stiSplineRangeSeries.Values.Length - k - 1] : stiSplineRangeSeries.Values[k]);
					double? num5 = (stiAxisArea.ReverseHor ? stiSplineRangeSeries.ValuesEnd[stiSplineRangeSeries.ValuesEnd.Length - k - 1] : stiSplineRangeSeries.ValuesEnd[k]);
					if (!num4.HasValue && stiSplineRangeSeries.ShowNulls)
					{
						num4 = 0.0;
					}
					if (!num5.HasValue && stiSplineRangeSeries.ShowNulls)
					{
						num4 = 0.0;
					}
					double? num6 = num4;
					if (stiSplineRangeArea.ReverseVert && num4.HasValue)
					{
						num6 = 0.0 - num6;
					}
					double? num7 = num5;
					if (stiSplineRangeArea.ReverseVert && num5.HasValue)
					{
						num7 = 0.0 - num7;
					}
					num3 = ((stiSplineRangeSeries.YAxis != 0) ? stiSplineRangeArea.AxisCore.GetDividerRightY() : stiSplineRangeArea.AxisCore.GetDividerY());
					PointF? pointF = array[k];
					PointF? pointF2 = array2[k];
					if (pointF.HasValue)
					{
						PointF point = new PointF(pointF.Value.X, num3);
						if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
						{
							StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiSplineRangeSeries, context, CorrectPoint(pointF.Value, rect, (float)stiSplineRangeSeries.LabelsOffset * context.Options.Zoom), CorrectPoint(point, rect, (float)stiSplineRangeSeries.LabelsOffset * context.Options.Zoom), k, num6, num4, stiSplineRangeArea.AxisCore.GetArgumentLabel(stiSplineRangeArea.XAxis.Info.StripLines[k], stiSplineRangeSeries), stiSplineRangeSeries.Core.GetTag(k), 0, 1, rect);
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
					PointF point2 = new PointF(pointF2.Value.X, num3);
					if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
					{
						StiSeriesLabelsGeom stiSeriesLabelsGeom2 = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiSplineRangeSeries, context, CorrectPoint(pointF2.Value, rect, (float)stiSplineRangeSeries.LabelsOffset * context.Options.Zoom), CorrectPoint(point2, rect, (float)stiSplineRangeSeries.LabelsOffset * context.Options.Zoom), k, num7, num5, stiSplineRangeArea.AxisCore.GetArgumentLabel(stiSplineRangeArea.XAxis.Info.StripLines[k], stiSplineRangeSeries), stiSplineRangeSeries.Core.GetTag(k), 0, 1, rect);
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

	private void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsinfo, double?[] values, IStiSplineRangeSeries series)
	{
		if (pointsinfo.Points != null || pointsinfo.Points.Length > 1)
		{
			StiLineSeriesGeom stiLineSeriesGeom = new StiLineSeriesGeom(geom, pointsinfo, series);
			if (stiLineSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiLineSeriesGeom);
			}
			if (base.Interaction != null)
			{
				stiLineSeriesGeom.Interactions = GetInteractions(context, geom, pointsinfo.Points);
			}
		}
		RenderMarkers(context, geom, pointsinfo.Points, values, series);
	}

	private void RenderMarkers(StiContext context, StiAreaGeom geom, PointF?[] points, double?[] values, IStiSplineRangeSeries series)
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

	private PointF? GetYPoint(double? value, IStiSplineRangeSeries currentSeries, IStiSplineRangeArea axisArea, int index)
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

	private void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo, IStiSplineRangeSeries series)
	{
		if (pointsInfo.Points != null && pointsInfo.Points.Length > 1)
		{
			StiSplineRangeSeriesGeom stiSplineRangeSeriesGeom = new StiSplineRangeSeriesGeom(geom, pointsInfo, series);
			if (stiSplineRangeSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiSplineRangeSeriesGeom);
			}
		}
	}

	public StiSplineRangeSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
