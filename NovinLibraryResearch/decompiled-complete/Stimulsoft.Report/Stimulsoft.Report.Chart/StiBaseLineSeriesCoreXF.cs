using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

public abstract class StiBaseLineSeriesCoreXF : StiSeriesCoreXF, IStiApplyStyleSeries
{
	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		base.ApplyStyle(style, color);
		IStiBaseLineSeries stiBaseLineSeries = base.Series as IStiBaseLineSeries;
		if (!stiBaseLineSeries.AllowApplyStyle)
		{
			return;
		}
		stiBaseLineSeries.LineColor = color;
		stiBaseLineSeries.Lighting = style.Core.SeriesLighting;
		if (stiBaseLineSeries.Marker != null)
		{
			if (stiBaseLineSeries.Marker is StiMarker { ExtendedVisible: not StiExtendedStyleBool.FromStyle } stiMarker)
			{
				stiBaseLineSeries.Marker.Visible = stiMarker.ExtendedVisible == StiExtendedStyleBool.True;
			}
			else
			{
				stiBaseLineSeries.Marker.Visible = style.Core.MarkerVisible;
			}
			stiBaseLineSeries.Marker.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(color, (byte)100));
			stiBaseLineSeries.Marker.BorderColor = StiColorUtils.Dark(color, (byte)100);
		}
		if (stiBaseLineSeries.LineMarker != null)
		{
			stiBaseLineSeries.LineMarker.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(color, (byte)50));
			stiBaseLineSeries.LineMarker.BorderColor = StiColorUtils.Dark(color, (byte)150);
		}
	}

	protected PointF?[] ClipLinePoints(StiContext context, StiAreaGeom geom, PointF?[] points, out int startIndex, out int endIndex)
	{
		if (((IStiAxisArea)base.Series.Chart.Area).XAxis.Range.Auto)
		{
			startIndex = 0;
			endIndex = points.Length;
			return points;
		}
		startIndex = -1;
		endIndex = -1;
		RectangleF clientRectangle = ((StiAxisAreaGeom)geom).View.ClientRectangle;
		clientRectangle.X = 0f;
		clientRectangle.Y = 0f;
		int num = 0;
		for (int i = 0; i < points.Length; i++)
		{
			PointF? pointF = points[i];
			if (pointF.HasValue)
			{
				PointF value = pointF.Value;
				value.X += geom.ClientRectangle.X;
				value.Y += geom.ClientRectangle.Y;
				if (clientRectangle.X <= value.X && value.X < clientRectangle.Right && startIndex == -1)
				{
					startIndex = num;
				}
				if ((!(clientRectangle.X <= value.X) || !(value.X < clientRectangle.Right)) && startIndex != -1)
				{
					endIndex = num;
					break;
				}
			}
			num++;
		}
		if (endIndex == -1)
		{
			endIndex = points.Length - 1;
		}
		startIndex--;
		endIndex++;
		if (startIndex < 0)
		{
			startIndex = 0;
		}
		if (endIndex >= points.Length)
		{
			endIndex = points.Length - 1;
		}
		int num2 = endIndex - startIndex + 1;
		if (num2 == points.Length)
		{
			return points;
		}
		PointF?[] array = new PointF?[num2];
		Array.Copy(points, startIndex, array, 0, num2);
		return array;
	}

	protected virtual void RenderMarkers(StiContext context, StiAreaGeom geom, PointF?[] points)
	{
		IStiAxisArea stiAxisArea = geom.Area as IStiAxisArea;
		IStiBaseLineSeries stiBaseLineSeries = base.Series as IStiBaseLineSeries;
		if (points.Length == 0)
		{
			return;
		}
		bool isTooltipMode = !stiBaseLineSeries.Marker.Visible && stiBaseLineSeries.ToolTips.Length != 0 && !((StiComponent)base.Series.Chart).Report.IsDesigning;
		if (stiBaseLineSeries.Marker == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < points.Length; i++)
		{
			PointF? pointF = points[i];
			if (pointF.HasValue)
			{
				double? num2 = (stiAxisArea.ReverseHor ? stiBaseLineSeries.Values[stiBaseLineSeries.Values.Length - num - 1] : stiBaseLineSeries.Values[num]);
				if (!num2.HasValue && stiBaseLineSeries.ShowNulls)
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
					StiMarkerGeom stiMarkerGeom = new StiMarkerGeom(base.Series, num, num2.GetValueOrDefault(), pointF.Value, stiBaseLineSeries.Marker, stiBaseLineSeries.ShowShadow, context.Options.Zoom, isTooltipMode);
					if (stiMarkerGeom != null)
					{
						if (stiBaseLineSeries.Core.Interaction != null)
						{
							stiMarkerGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, stiBaseLineSeries, num);
						}
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(stiMarkerGeom);
					}
				}
			}
			num++;
		}
	}

	protected List<StiSeriesInteractionData> GetInteractions(StiContext context, StiAreaGeom geom, PointF?[] points)
	{
		List<StiSeriesInteractionData> list = new List<StiSeriesInteractionData>();
		for (int i = 0; i < points.Length; i++)
		{
			StiSeriesInteractionData stiSeriesInteractionData = new StiSeriesInteractionData(geom.Area as IStiAxisArea, base.Series, i);
			stiSeriesInteractionData.Point = points[i];
			list.Add(stiSeriesInteractionData);
		}
		return list;
	}

	protected virtual void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
	}

	protected virtual void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		IStiArea area = geom.Area;
		IStiAxisArea stiAxisArea = area as IStiAxisArea;
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
		bool startFromZero = stiAxisArea.XAxis.Core.GetStartFromZero();
		rect.Width += 0.001f;
		float num = 0f;
		List<PointF?[]> list = new List<PointF?[]>();
		List<PointF?[]> list2 = new List<PointF?[]>();
		List<List<PointF?>> list3 = new List<List<PointF?>>();
		List<List<PointF?>> list4 = new List<List<PointF?>>();
		List<string[]> list5 = new List<string[]>();
		for (int i = 0; i < series.Length; i++)
		{
			IStiBaseLineSeries stiBaseLineSeries = series[i] as IStiBaseLineSeries;
			int num2 = stiBaseLineSeries.Values.Length;
			PointF?[] array = new PointF?[num2];
			PointF?[] array2 = new PointF?[num2];
			string[] array3 = new string[num2];
			double?[] approximationValuesStart = ((StiSeries)series[i]).GetApproximationValuesStart();
			for (int j = 0; j < num2; j++)
			{
				int num3 = (startFromZero ? (j + 1) : j);
				if (num3 >= stiAxisArea.XAxis.Info.StripPositions.Length)
				{
					break;
				}
				float posX = stiAxisArea.XAxis.Info.StripPositions[num3];
				array[j] = GetPointValue(j, stiBaseLineSeries, stiBaseLineSeries.Values, stiAxisArea, posX);
				array2[j] = GetPointValue(j, stiBaseLineSeries, approximationValuesStart, stiAxisArea, posX);
				object valueObject = stiAxisArea.XAxis.Info.StripLines[num3].ValueObject;
				array3[j] = ((valueObject == null) ? "" : valueObject.ToString());
			}
			List<PointF?> pointsZeroConnect = GetPointsZeroConnect(stiBaseLineSeries, stiAxisArea);
			List<PointF?> pointsNullConnect = GetPointsNullConnect(stiBaseLineSeries, stiAxisArea);
			list3.Add(pointsZeroConnect);
			list4.Add(pointsNullConnect);
			list5.Add(array3);
			if (array.Length != 0)
			{
				array = StiPointHelper.OptimizePoints(array);
				int startIndex;
				int endIndex;
				PointF?[] points = ClipLinePoints(context, geom, array, out startIndex, out endIndex);
				PointF?[] pointsFrom = ClipLinePoints(context, geom, array2, out startIndex, out endIndex);
				StiSeriesPointsInfo pointsInfo = new StiSeriesPointsInfo
				{
					PointsFrom = pointsFrom,
					Points = points,
					PointsZeroConnect = pointsZeroConnect.ToArray(),
					PointsNullConnect = pointsNullConnect.ToArray(),
					PointsIds = array3
				};
				((StiBaseLineSeriesCoreXF)stiBaseLineSeries.Core).RenderAreas(context, geom, pointsInfo);
				if (!IsTopmostLine(stiBaseLineSeries))
				{
					((StiBaseLineSeriesCoreXF)stiBaseLineSeries.Core).RenderLines(context, geom, pointsInfo);
				}
			}
			list.Add(array);
			list2.Add(array2);
		}
		int num4 = 0;
		foreach (PointF?[] item in list)
		{
			int startIndex2;
			int endIndex2;
			PointF?[] points2 = ClipLinePoints(context, geom, item, out startIndex2, out endIndex2);
			PointF?[] pointsFrom2 = ClipLinePoints(context, geom, list2[num4], out startIndex2, out endIndex2);
			string[] pointsIds = list5[num4];
			int startIndex3 = startIndex2;
			int endIndex3 = endIndex2;
			PointF?[] source = ClipLinePoints(context, geom, list3[num4].ToArray(), out startIndex3, out endIndex3);
			PointF?[] source2 = ClipLinePoints(context, geom, list4[num4].ToArray(), out startIndex3, out endIndex3);
			IStiBaseLineSeries stiBaseLineSeries2 = series[num4] as IStiBaseLineSeries;
			if (IsTopmostLine(series[num4]))
			{
				StiSeriesPointsInfo pointsInfo2 = new StiSeriesPointsInfo
				{
					PointsFrom = pointsFrom2,
					Points = points2,
					PointsZeroConnect = source.ToArray(),
					PointsNullConnect = source2.ToArray(),
					PointsIds = pointsIds
				};
				((StiBaseLineSeriesCoreXF)stiBaseLineSeries2.Core).RenderLines(context, geom, pointsInfo2);
			}
			StiTrendLinesCollection trendLines = ((StiSeries)stiBaseLineSeries2).TrendLines;
			foreach (StiTrendLine item2 in trendLines)
			{
				item2.Core.RenderTrendLine(geom, points2, stiAxisArea.AxisCore.GetDividerY());
			}
			IStiAxisSeriesLabels seriesLabels = stiBaseLineSeries2.Core.GetSeriesLabels();
			if (seriesLabels != null && seriesLabels.Visible)
			{
				for (int k = startIndex2; k <= endIndex2; k++)
				{
					if (stiBaseLineSeries2.Values.Length <= k)
					{
						continue;
					}
					double? num5 = (stiAxisArea.ReverseHor ? stiBaseLineSeries2.Values[stiBaseLineSeries2.Values.Length - k - 1] : stiBaseLineSeries2.Values[k]);
					if (!num5.HasValue && stiBaseLineSeries2.ShowNulls)
					{
						num5 = 0.0;
					}
					double? num6 = num5;
					if (stiAxisArea.ReverseVert && num5.HasValue)
					{
						num6 = 0.0 - num6;
					}
					num = ((stiBaseLineSeries2.YAxis != 0) ? stiAxisArea.AxisCore.GetDividerRightY() : stiAxisArea.AxisCore.GetDividerY());
					PointF? pointF = ((item.Length > k) ? item[k] : null);
					if (!pointF.HasValue)
					{
						continue;
					}
					double? valueFrom = 0.0;
					if (stiBaseLineSeries2.ValuesStart != null && stiBaseLineSeries2.ValuesStart.Length > k)
					{
						valueFrom = (stiAxisArea.ReverseHor ? stiBaseLineSeries2.ValuesStart[stiBaseLineSeries2.Values.Length - k - 1] : stiBaseLineSeries2.ValuesStart[k]);
					}
					PointF point = new PointF(pointF.Value.X, num);
					StiAnimation animation = null;
					if (isAnimationChangingValues && seriesLabels.ValueType == StiSeriesLabelsValueType.Value)
					{
						if (seriesLabels.ValueType == StiSeriesLabelsValueType.Value && string.IsNullOrEmpty(seriesLabels.Format) && seriesLabels.FormatService is StiGeneralFormatService)
						{
							PointF? pointF2 = ((list2[num4].Length > k) ? list2[num4][k] : null);
							animation = new StiLabelAnimation(valueFrom, num5, pointF2.GetValueOrDefault(), pointF.GetValueOrDefault(), StiChartHelper.GlobalBeginTimeElement, TimeSpan.Zero);
						}
						else
						{
							animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, TimeSpan.Zero);
						}
					}
					if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
					{
						int index = (stiAxisArea.XAxis.StartFromZero ? (k + 1) : k);
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiBaseLineSeries2, context, CorrectPoint(pointF.Value, rect, (float)stiBaseLineSeries2.LabelsOffset * context.Options.Zoom), CorrectPoint(point, rect, (float)stiBaseLineSeries2.LabelsOffset * context.Options.Zoom), k, num6, num5, stiAxisArea.AxisCore.GetArgumentLabel(stiAxisArea.XAxis.Info.StripLines[index], stiBaseLineSeries2), stiBaseLineSeries2.Core.GetTag(k), 0, 1, rect, animation);
						if (stiSeriesLabelsGeom != null)
						{
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiSeriesLabelsGeom);
							stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom.ClientRectangle);
						}
					}
				}
			}
			num4++;
		}
		if (geom.Area.Chart.SeriesLabels.PreventIntersection)
		{
			CheckIntersectionLabels(geom);
		}
	}

	private List<PointF?> GetPointsZeroConnect(IStiBaseLineSeries series, IStiAxisArea axisArea)
	{
		List<PointF?> result = new List<PointF?>();
		if (series.ShowZerosAs == StiShowEmptyCellsAs.Gap)
		{
			return result;
		}
		List<List<int>> list = new List<List<int>>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < series.Values.Length; i++)
		{
			if (series.Values[i] == 0.0)
			{
				list2.Add(i);
				if (list2.Count == 1)
				{
					list.Add(list2);
				}
			}
			else
			{
				list2 = new List<int>();
			}
		}
		return GetPointConnect(series, axisArea, list);
	}

	private List<PointF?> GetPointsNullConnect(IStiBaseLineSeries series, IStiAxisArea axisArea)
	{
		List<PointF?> result = new List<PointF?>();
		if (series.ShowNullsAs == StiShowEmptyCellsAs.Gap)
		{
			return result;
		}
		List<List<int>> list = new List<List<int>>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < series.Values.Length; i++)
		{
			double? num = series.Values[i];
			if (!num.HasValue)
			{
				list2.Add(i);
				if (list2.Count == 1)
				{
					list.Add(list2);
				}
			}
			else
			{
				list2 = new List<int>();
			}
		}
		return GetPointConnect(series, axisArea, list);
	}

	private List<PointF?> GetPointConnect(IStiBaseLineSeries series, IStiAxisArea axisArea, List<List<int>> pointConnectIndexGroupList)
	{
		List<PointF?> list = new List<PointF?>();
		foreach (List<int> pointConnectIndexGroup in pointConnectIndexGroupList)
		{
			int num = pointConnectIndexGroup[0] - 1;
			int num2 = pointConnectIndexGroup[pointConnectIndexGroup.Count - 1] + 1;
			double num3 = 0.0;
			double num4 = 0.0;
			if (num >= 0 && num < series.Values.Length)
			{
				num3 = series.Values[num].GetValueOrDefault();
				PointF pointValue = GetPointValue(series, axisArea, num3, num);
				list.Add(pointValue);
			}
			if (num2 >= 0 && num2 < series.Values.Length)
			{
				num4 = series.Values[num2].GetValueOrDefault();
			}
			double num5 = (num4 - num3) / (double)(pointConnectIndexGroup.Count + 1);
			for (int i = 0; i < pointConnectIndexGroup.Count; i++)
			{
				double value = num3 + num5 * (double)(i + 1);
				PointF pointValue2 = GetPointValue(series, axisArea, value, pointConnectIndexGroup[i]);
				list.Add(pointValue2);
			}
			if (num2 >= 0 && num2 < series.Values.Length)
			{
				PointF pointValue3 = GetPointValue(series, axisArea, num4, num2);
				list.Add(pointValue3);
			}
			list.Add(null);
		}
		return list;
	}

	private static PointF GetPointValue(IStiBaseLineSeries series, IStiAxisArea axisArea, double value, int index)
	{
		index = (axisArea.XAxis.Core.GetStartFromZero() ? (index + 1) : index);
		if (index >= axisArea.XAxis.Info.StripPositions.Length)
		{
			return PointF.Empty;
		}
		float x = axisArea.XAxis.Info.StripPositions[index];
		double num = ((series.YAxis != 0) ? ((0.0 - value) * (double)(float)axisArea.YRightAxis.Info.Dpi + (double)axisArea.AxisCore.GetDividerRightY()) : ((0.0 - value) * (double)(float)axisArea.YAxis.Info.Dpi + (double)axisArea.AxisCore.GetDividerY()));
		return new PointF(x, (float)num);
	}

	private PointF? GetPointValue(int pointIndex, IStiBaseLineSeries currentSeries, double?[] values, IStiAxisArea axisArea, float posX)
	{
		double? value = 0.0;
		if (values.Length > pointIndex)
		{
			value = (axisArea.ReverseHor ? values[values.Length - pointIndex - 1] : values[pointIndex]);
		}
		return GetPointValue(value, currentSeries, axisArea, posX);
	}

	private PointF? GetPointValue(double? value, IStiBaseLineSeries currentSeries, IStiAxisArea axisArea, float posX)
	{
		bool flag = currentSeries is StiAreaSeries || currentSeries is StiSplineAreaSeries || currentSeries is StiStackedAreaSeries || currentSeries is StiStackedSplineAreaSeries || currentSeries is StiSteppedAreaSeries;
		if (value == 0.0 && !currentSeries.ShowZeros && !flag)
		{
			return null;
		}
		if (!value.HasValue && !currentSeries.ShowNulls && !flag)
		{
			return null;
		}
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
		return new PointF(posX, (float)num);
	}

	private bool IsTopmostLine(IStiSeries series)
	{
		if (series is IStiSplineAreaSeries)
		{
			return ((IStiSplineAreaSeries)series).TopmostLine;
		}
		if (series is IStiSteppedAreaSeries)
		{
			return ((IStiSteppedAreaSeries)series).TopmostLine;
		}
		if (series is IStiAreaSeries)
		{
			return ((IStiAreaSeries)series).TopmostLine;
		}
		return true;
	}

	protected PointF CorrectPoint(PointF point, RectangleF rect, float correctY)
	{
		if (point.Y + correctY < 0f)
		{
			return new PointF(point.X, 0f);
		}
		if (point.Y + correctY > rect.Height)
		{
			return new PointF(point.X, rect.Height);
		}
		return new PointF(point.X, point.Y + correctY);
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		IStiBaseLineSeries stiBaseLineSeries = base.Series as IStiBaseLineSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			if (!(base.Series is StiBubbleSeries stiBubbleSeries))
			{
				return (StiBrush)new StiSolidBrush(StiColorUtils.Dark(stiBaseLineSeries.LineColor, (byte)20));
			}
			return stiBubbleSeries.Brush;
		}
		return seriesBrush;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		IStiBaseLineSeries stiBaseLineSeries = base.Series as IStiBaseLineSeries;
		object seriesBorderColor = base.GetSeriesBorderColor(colorIndex, colorCount);
		if (seriesBorderColor == null)
		{
			if (base.Series is StiBubbleSeries stiBubbleSeries)
			{
				return stiBubbleSeries.BorderColor;
			}
			return stiBaseLineSeries.LineColor;
		}
		return seriesBorderColor;
	}

	public StiBaseLineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
