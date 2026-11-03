using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public abstract class StiStackedBaseLineSeriesCoreXF : StiSeriesCoreXF
{
	public bool IsFullStacked
	{
		get
		{
			if (!(this is StiFullStackedAreaSeriesCoreXF) && !(this is StiFullStackedLineSeriesCoreXF) && !(this is StiFullStackedSplineSeriesCoreXF))
			{
				return this is StiFullStackedSplineAreaSeriesCoreXF;
			}
			return true;
		}
	}

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		base.ApplyStyle(style, color);
		IStiStackedBaseLineSeries stiStackedBaseLineSeries = base.Series as IStiStackedBaseLineSeries;
		if (stiStackedBaseLineSeries.AllowApplyStyle)
		{
			stiStackedBaseLineSeries.LineColor = color;
			stiStackedBaseLineSeries.Lighting = style.Core.SeriesLighting;
			stiStackedBaseLineSeries.Marker.Visible = style.Core.MarkerVisible;
			if (stiStackedBaseLineSeries.Marker != null)
			{
				stiStackedBaseLineSeries.Marker.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(color, (byte)100));
				stiStackedBaseLineSeries.Marker.BorderColor = StiColorUtils.Dark(color, (byte)100);
			}
			if (stiStackedBaseLineSeries.LineMarker != null)
			{
				stiStackedBaseLineSeries.LineMarker.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(color, (byte)50));
				stiStackedBaseLineSeries.LineMarker.BorderColor = StiColorUtils.Dark(color, (byte)150);
			}
		}
	}

	protected void ClipLinePoints(StiContext context, StiAreaGeom geom, PointF?[] startPoints, PointF?[] endPoints, out PointF?[] newStartPoints, out PointF?[] newEndPoints, out int startIndex, out int endIndex)
	{
		if (((IStiAxisArea)base.Series.Chart.Area).XAxis.Range.Auto)
		{
			startIndex = 0;
			endIndex = startPoints.Length;
			newStartPoints = startPoints;
			newEndPoints = endPoints;
			return;
		}
		startIndex = -1;
		endIndex = -1;
		RectangleF clientRectangle = ((StiAxisAreaGeom)geom).View.ClientRectangle;
		clientRectangle.X = 0f;
		clientRectangle.Y = 0f;
		int num = 0;
		for (int i = 0; i < startPoints.Length; i++)
		{
			PointF value = startPoints[i].Value;
			PointF pointF = value;
			pointF.X += geom.ClientRectangle.X;
			pointF.Y += geom.ClientRectangle.Y;
			if (clientRectangle.X <= pointF.X && pointF.X < clientRectangle.Right && startIndex == -1)
			{
				startIndex = num;
			}
			if ((!(clientRectangle.X <= pointF.X) || !(pointF.X < clientRectangle.Right)) && startIndex != -1)
			{
				endIndex = num;
				break;
			}
			num++;
		}
		if (endIndex == -1)
		{
			endIndex = startPoints.Length - 1;
		}
		startIndex--;
		endIndex++;
		if (startIndex < 0)
		{
			startIndex = 0;
		}
		if (endIndex >= startPoints.Length)
		{
			endIndex = startPoints.Length - 1;
		}
		int num2 = endIndex - startIndex + 1;
		if (num2 == startPoints.Length)
		{
			newStartPoints = startPoints;
			newEndPoints = endPoints;
			return;
		}
		newStartPoints = new PointF?[num2];
		Array.Copy(startPoints, startIndex, newStartPoints, 0, num2);
		if (endPoints != null)
		{
			newEndPoints = new PointF?[num2];
			Array.Copy(endPoints, startIndex, newEndPoints, 0, num2);
		}
		else
		{
			newEndPoints = null;
		}
	}

	protected virtual void RenderMarkers(StiContext context, StiAreaGeom geom, PointF?[] points)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		IStiAxisArea stiAxisArea = geom.Area as IStiAxisArea;
		IStiStackedBaseLineSeries stiStackedBaseLineSeries = base.Series as IStiStackedBaseLineSeries;
		if (points.Length == 0)
		{
			return;
		}
		bool flag = !stiStackedBaseLineSeries.Marker.Visible && stiStackedBaseLineSeries.ToolTips.Length != 0;
		if (stiStackedBaseLineSeries.Marker == null || !(stiStackedBaseLineSeries.Marker.Visible || flag))
		{
			return;
		}
		StiSolidBrush val = new StiSolidBrush(Color.FromArgb(55, Color.Black));
		StiPenGeom stiPenGeom = new StiPenGeom(stiStackedBaseLineSeries.Marker.BorderColor);
		int num = 0;
		for (int i = 0; i < points.Length; i++)
		{
			PointF? pointF = points[i];
			if (pointF.HasValue)
			{
				double? num2 = (stiAxisArea.ReverseHor ? base.Series.Values[base.Series.Values.Length - num - 1] : base.Series.Values[num]);
				if (!num2.HasValue && stiStackedBaseLineSeries.ShowNulls)
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
					StiMarkerGeom stiMarkerGeom = new StiMarkerGeom(base.Series, num, num2.GetValueOrDefault(), pointF.Value, stiStackedBaseLineSeries.Marker, base.Series.ShowShadow, context.Options.Zoom, flag);
					if (stiMarkerGeom != null)
					{
						if (stiStackedBaseLineSeries.Core.Interaction != null)
						{
							stiMarkerGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, stiStackedBaseLineSeries, num);
						}
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(stiMarkerGeom);
					}
				}
			}
			num++;
		}
	}

	protected virtual void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
	}

	protected virtual void RenderAreas(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		IStiStackedBaseLineSeries stiStackedBaseLineSeries = base.Series as IStiStackedBaseLineSeries;
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		IStiArea area = geom.Area;
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiAxisArea stiAxisArea = area as IStiAxisArea;
		float num = ((base.Series.YAxis == StiSeriesYAxis.LeftYAxis) ? stiAxisArea.AxisCore.GetDividerY() : stiAxisArea.AxisCore.GetDividerRightY());
		bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
		bool startFromZero = stiAxisArea.XAxis.Core.GetStartFromZero();
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		List<PointF?[]> list2 = new List<PointF?[]>();
		List<PointF?[]> list3 = new List<PointF?[]>();
		List<string[]> list4 = new List<string[]>();
		float num2 = (float)stiAxisArea.YAxis.Info.Dpi;
		if (base.Series.YAxis == StiSeriesYAxis.RightYAxis)
		{
			num2 = (float)stiAxisArea.YRightAxis.Info.Dpi;
		}
		for (int i = 0; i < series.Length; i++)
		{
			IStiStackedBaseLineSeries stiStackedBaseLineSeries2 = series[i] as IStiStackedBaseLineSeries;
			int num3 = stiStackedBaseLineSeries2.Values.Length;
			PointF?[] array = new PointF?[num3];
			PointF?[] array2 = new PointF?[num3];
			string[] array3 = new string[num3];
			for (int j = 0; j < num3; j++)
			{
				float x = (startFromZero ? stiAxisArea.XAxis.Info.StripPositions[j + 1] : stiAxisArea.XAxis.Info.StripPositions[j]);
				double? num4 = (stiAxisArea.ReverseHor ? stiStackedBaseLineSeries2.Values[stiStackedBaseLineSeries2.Values.Length - j - 1] : stiStackedBaseLineSeries2.Values[j]);
				if (!num4.HasValue && !stiStackedBaseLineSeries2.ShowNulls)
				{
					array2[j] = null;
					array[j] = null;
					array3[j] = null;
					continue;
				}
				if (!num4.HasValue && stiStackedBaseLineSeries2.ShowNulls)
				{
					num4 = 0.0;
				}
				double totalPositiveHeight = 0.0;
				double totalNegativeHeight = 0.0;
				CalculateTotalHeight(series, j, out totalPositiveHeight, out totalNegativeHeight);
				if (((StiStackedBaseLineSeriesCoreXF)stiStackedBaseLineSeries.Core).IsFullStacked && (totalPositiveHeight != 0.0 || totalNegativeHeight != 0.0) && num4.HasValue)
				{
					num2 = ((!(num4 >= 0.0)) ? ((rect.Height - num) / (float)totalNegativeHeight) : (num / (float)totalPositiveHeight));
				}
				if (stiAxisArea.ReverseVert && num4.HasValue)
				{
					num4 = 0.0 - num4;
				}
				float num5 = (0f - (float)num4.GetValueOrDefault()) * num2;
				float num6 = 0f;
				if (num4 >= 0.0)
				{
					if (hashtable[j] != null)
					{
						num6 = (float)hashtable[j];
					}
				}
				else if (hashtable2[j] != null)
				{
					num6 = (float)hashtable2[j];
				}
				num5 += num6;
				if (num4 >= 0.0)
				{
					hashtable[j] = num5;
				}
				else
				{
					hashtable2[j] = num5;
				}
				array2[j] = new PointF(x, num6 + num);
				array[j] = new PointF(x, num5 + num);
				object obj = (startFromZero ? stiAxisArea.XAxis.Info.StripLines[j + 1].ValueObject : stiAxisArea.XAxis.Info.StripLines[j].ValueObject);
				array3[j] = ((obj == null) ? "" : obj.ToString());
			}
			if (array.Length != 0 && array2.Length != 0)
			{
				PointF?[] newStartPoints = null;
				PointF?[] newEndPoints = null;
				ClipLinePoints(context, geom, array2, array, out newStartPoints, out newEndPoints, out var _, out var _);
				StiSeriesPointsInfo pointsInfo = new StiSeriesPointsInfo
				{
					PointsStart = newStartPoints,
					PointsEnd = newEndPoints,
					PointsIds = array3
				};
				((StiStackedBaseLineSeriesCoreXF)stiStackedBaseLineSeries2.Core).RenderAreas(context, geom, pointsInfo);
			}
			list2.Add(array);
			list3.Add(array2);
			list4.Add(array3);
		}
		int num7 = 0;
		float[] array4 = new float[stiAxisArea.AxisCore.ValuesCount];
		float[] array5 = new float[stiAxisArea.AxisCore.ValuesCount];
		for (int k = 0; k < stiAxisArea.AxisCore.ValuesCount; k++)
		{
			array4[k] = num;
			array5[k] = num;
		}
		foreach (PointF?[] item in list2)
		{
			PointF?[] newStartPoints2 = null;
			PointF?[] newEndPoints2 = null;
			ClipLinePoints(context, geom, item, null, out newStartPoints2, out newEndPoints2, out var startIndex2, out var endIndex2);
			IStiStackedBaseLineSeries stiStackedBaseLineSeries3 = series[num7] as IStiStackedBaseLineSeries;
			StiSeriesPointsInfo pointsInfo2 = new StiSeriesPointsInfo
			{
				Points = newStartPoints2,
				PointsFrom = list3[num7],
				PointsIds = list4[num7]
			};
			((StiStackedBaseLineSeriesCoreXF)stiStackedBaseLineSeries3.Core).RenderLines(context, geom, pointsInfo2);
			IStiAxisSeriesLabels seriesLabels = stiStackedBaseLineSeries3.Core.GetSeriesLabels();
			if (seriesLabels != null && seriesLabels.Visible)
			{
				for (int l = startIndex2; l <= endIndex2; l++)
				{
					if (l >= stiStackedBaseLineSeries3.Values.Length)
					{
						continue;
					}
					double? num8 = (stiAxisArea.ReverseHor ? stiStackedBaseLineSeries3.Values[stiStackedBaseLineSeries3.Values.Length - l - 1] : stiStackedBaseLineSeries3.Values[l]);
					if (!num8.HasValue && stiStackedBaseLineSeries.ShowNulls)
					{
						num8 = 0.0;
					}
					float num9 = num;
					if (num8 > 0.0)
					{
						num9 = array4[l];
						if (item[l].HasValue)
						{
							array4[l] = item[l].Value.Y;
						}
					}
					else
					{
						num9 = array5[l];
						if (item[l].HasValue)
						{
							array5[l] = item[l].Value.Y;
						}
					}
					double? num10 = num8;
					if (stiAxisArea.ReverseVert && num8.HasValue)
					{
						num10 = 0.0 - num10;
					}
					PointF? pointF = item[l];
					if (!pointF.HasValue)
					{
						continue;
					}
					PointF startPoint = new PointF(pointF.Value.X, num9);
					if (seriesLabels.Step == 0 || l % seriesLabels.Step == 0)
					{
						int index = (stiAxisArea.XAxis.StartFromZero ? (l + 1) : l);
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiStackedBaseLineSeries3, context, pointF.Value, startPoint, l, num10, num8, stiAxisArea.AxisCore.GetArgumentLabel(stiAxisArea.XAxis.Info.StripLines[index], stiStackedBaseLineSeries3), stiStackedBaseLineSeries3.Core.GetTag(l), 0, 1, rect);
						if (stiSeriesLabelsGeom != null)
						{
							list.Add(stiSeriesLabelsGeom);
						}
					}
				}
			}
			num7++;
		}
		foreach (StiSeriesLabelsGeom item2 in list)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item2);
			item2.ClientRectangle = CheckLabelsRect(item2.SeriesLabels, geom, item2.ClientRectangle);
		}
	}

	private void CalculateTotalHeight(IStiSeries[] series, int pointIndex, out double totalPositiveHeight, out double totalNegativeHeight)
	{
		IStiStackedBaseLineSeries stiStackedBaseLineSeries = base.Series as IStiStackedBaseLineSeries;
		totalPositiveHeight = 0.0;
		totalNegativeHeight = 0.0;
		if (!((StiStackedBaseLineSeriesCoreXF)stiStackedBaseLineSeries.Core).IsFullStacked)
		{
			return;
		}
		foreach (IStiSeries stiSeries in series)
		{
			if (pointIndex < stiSeries.Values.Length)
			{
				double? num = (((IStiAxisArea)base.Series.Chart.Area).ReverseHor ? stiSeries.Values[stiSeries.Values.Length - pointIndex - 1] : stiSeries.Values[pointIndex]);
				if (!num.HasValue && stiStackedBaseLineSeries.ShowNulls)
				{
					num = 0.0;
				}
				if (num > 0.0)
				{
					totalPositiveHeight += num.GetValueOrDefault();
				}
				else
				{
					totalNegativeHeight += Math.Abs(num.GetValueOrDefault());
				}
			}
		}
	}

	private PointF CorrectPoint(PointF point, RectangleF rect)
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
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		IStiStackedBaseLineSeries stiStackedBaseLineSeries = base.Series as IStiStackedBaseLineSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return (StiBrush)new StiSolidBrush(StiColorUtils.Dark(stiStackedBaseLineSeries.LineColor, (byte)20));
		}
		return seriesBrush;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		IStiStackedBaseLineSeries stiStackedBaseLineSeries = base.Series as IStiStackedBaseLineSeries;
		object seriesBorderColor = base.GetSeriesBorderColor(colorIndex, colorCount);
		if (seriesBorderColor == null)
		{
			return stiStackedBaseLineSeries.LineColor;
		}
		return seriesBorderColor;
	}

	public StiStackedBaseLineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
