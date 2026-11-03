using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart.Geoms.Series.Pie;

namespace Stimulsoft.Report.Chart;

public class StiPieSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Pie");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiPieSeries stiPieSeries = base.Series as IStiPieSeries;
		if (stiPieSeries.AllowApplyStyle)
		{
			stiPieSeries.Lighting = style.Core.SeriesLighting;
			stiPieSeries.BorderThickness = style.Core.SeriesBorderThickness;
			if (stiPieSeries.AllowApplyBrush)
			{
				stiPieSeries.Brush = style.Core.GetColumnBrush(color);
			}
			if (stiPieSeries.AllowApplyBorderColor)
			{
				stiPieSeries.BorderColor = style.Core.GetColumnBorder(color);
			}
		}
	}

	private StiBrush CorrectBrush(StiBrush brush)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (brush is StiGradientBrush)
		{
			object obj = brush.Clone();
			brush = (StiBrush)((obj is StiGradientBrush) ? obj : null);
			((StiGradientBrush)brush).Angle = -45.0;
		}
		if (brush is StiGlareBrush)
		{
			object obj2 = brush.Clone();
			brush = (StiBrush)((obj2 is StiGlareBrush) ? obj2 : null);
			((StiGlareBrush)brush).Angle = -45.0;
		}
		return brush;
	}

	private StiPieSeriesElementGeom RenderPieElement(StiContext context, PointF center, float radius, Color borderColor, StiBrush brush, float start, float angle, double? value, int index, IStiPieSeries currentSeries, float distance, StiAreaGeom geom, TimeSpan beginTime)
	{
		RectangleF rectPie = RectangleF.Empty;
		brush = CorrectBrush(brush);
		if (MeasurePieElementCore(context, center, radius, start, angle, currentSeries, distance, out var path, out var pathLight, out rectPie).IsEmpty)
		{
			return null;
		}
		return new StiPieSeriesElementGeom(geom, value.GetValueOrDefault(), index, currentSeries, rectPie, path, pathLight, borderColor, brush, start, start + angle, radius);
	}

	private void RenderPieElementShadow(StiContext context, PointF center, float radius, StiBrush brush, float start, float angle, IStiPieSeries currentSeries, float distance)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		if (((StiChart)base.Series.Chart).IsAnimation)
		{
			return;
		}
		RectangleF rectPie = RectangleF.Empty;
		brush = CorrectBrush(brush);
		if (!MeasurePieElementCore(context, center, radius, start, angle, currentSeries, distance, out var path, out var pathLight, out rectPie).IsEmpty)
		{
			context.PushSmoothingModeToAntiAlias();
			context.FillPath(brush, path, rectPie);
			if (pathLight != null)
			{
				StiGradientBrush brush2 = new StiGradientBrush(Color.FromArgb(100, Color.White), Color.FromArgb(50, Color.Black), 45.0);
				context.FillPath(brush2, pathLight, rectPie);
			}
			context.PopSmoothingMode();
		}
	}

	private RectangleF MeasurePieElement(StiContext context, PointF center, float radius, float start, float angle, IStiPieSeries currentSeries, float distance)
	{
		RectangleF rectPie = RectangleF.Empty;
		List<StiSegmentGeom> path;
		List<StiSegmentGeom> pathLight;
		return MeasurePieElementCore(context, center, radius, start, angle, currentSeries, distance, out path, out pathLight, out rectPie);
	}

	private RectangleF MeasurePieElementCore(StiContext context, PointF center, float radius, float start, float angle, IStiPieSeries currentSeries, float distance, out List<StiSegmentGeom> path, out List<StiSegmentGeom> pathLight, out RectangleF rectPie)
	{
		path = new List<StiSegmentGeom>();
		pathLight = null;
		rectPie = RectangleF.Empty;
		if (angle == 0f || float.IsNaN(angle))
		{
			return RectangleF.Empty;
		}
		if (distance > 0f)
		{
			center = GetPoint(center, distance * context.Options.Zoom, start + angle / 2f);
		}
		rectPie = new RectangleF(center.X - radius, center.Y - radius, radius * 2f, radius * 2f);
		if (rectPie.Width <= 0f && rectPie.Height <= 0f)
		{
			return RectangleF.Empty;
		}
		path.Add(new StiPieSegmentGeom(new RectangleF(rectPie.X, rectPie.Y, rectPie.Width, rectPie.Height), start, angle, null));
		if (currentSeries.Lighting && !Color.Transparent.Equals((object?)currentSeries.BorderColor))
		{
			float num = 0f;
			pathLight = new List<StiSegmentGeom>();
			num = radius * 0.02f;
			pathLight.Add(new StiArcSegmentGeom(rectPie, start, angle));
			pathLight.Add(new StiLineSegmentGeom(GetPoint(center, radius - num, start + angle), GetPoint(center, radius - num, start + angle)));
			pathLight.Add(new StiArcSegmentGeom(new RectangleF(rectPie.X + num, rectPie.Y + num, rectPie.Width - num * 2f, rectPie.Height - num * 2f), start + angle, 0f - angle));
			pathLight.Add(new StiLineSegmentGeom(GetPoint(center, radius - num, start), GetPoint(center, radius - num, start)));
		}
		return context.GetPathBounds(path);
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

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bec: Expected O, but got Unknown
		float radius = GetRadius(context, rect);
		PointF pointCenter = GetPointCenter(rect);
		if (seriesArray == null || seriesArray.Length == 0 || base.Series.Chart == null)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(new StiPieEmptySeriesElementGeom(new RectangleF(pointCenter.X - radius, pointCenter.Y - radius, 2f * radius, 2f * radius)));
			return;
		}
		TimeSpan globalDurationElement = StiChartHelper.GlobalDurationElement;
		TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
		StiChart stiChart = base.Series.Chart as StiChart;
		int colorCount = GetColorCount(seriesArray);
		int nonZeroValuesCount = 0;
		int firstNonZeroValueIndex = -1;
		IStiSeries firstNonZeroSeries = null;
		CheckNonZerovalue(seriesArray, out nonZeroValuesCount, out firstNonZeroValueIndex, out firstNonZeroSeries);
		float gradPerValue = GetGradPerValue(seriesArray);
		float percentPerValue = GetPercentPerValue(seriesArray);
		IStiPieSeries stiPieSeries = seriesArray[0] as IStiPieSeries;
		IStiPieSeriesLabels pieSeriesLabels = ((StiPieSeriesCoreXF)stiPieSeries.Core).GetPieSeriesLabels();
		bool isAnimationChangingValues = stiChart.IsAnimationChangingValues;
		float num = stiPieSeries.StartAngle;
		RectangleF a = new RectangleF(0f, 0f, rect.Width, rect.Height);
		foreach (IStiSeries stiSeries in seriesArray)
		{
			if (nonZeroValuesCount <= 1)
			{
				continue;
			}
			IStiPieSeries currentSeries = stiSeries as IStiPieSeries;
			int num2 = 0;
			double?[] values = stiSeries.Values;
			for (int j = 0; j < values.Length; j++)
			{
				double? num3 = values[j];
				float num4 = (float)((double)gradPerValue * Math.Abs(num3.GetValueOrDefault()));
				RectangleF b = MeasurePieElement(context, pointCenter, radius, num, num4, currentSeries, GetPieDistance(currentSeries, num2));
				if (num3 != 0.0)
				{
					a = RectangleF.Union(a, b);
				}
				num += num4;
				num2++;
			}
		}
		float val = Math.Max(0f - a.Left, a.Right - rect.Width);
		float val2 = Math.Max(0f - a.Top, a.Bottom - rect.Height);
		float num5 = Math.Max(val, val2);
		radius -= num5 * 1.2f;
		RectangleF[] array = new RectangleF[colorCount];
		float[] array2 = new float[colorCount];
		int[] array3 = new int[4];
		Array.Clear(array3, 0, array3.Length);
		num = stiPieSeries.StartAngle;
		a = new RectangleF(10f * context.Options.Zoom, 10f * context.Options.Zoom, rect.Width - 20f * context.Options.Zoom, rect.Height - 20f * context.Options.Zoom);
		RectangleF rect2 = a;
		int num6 = 0;
		foreach (IStiSeries stiSeries2 in seriesArray)
		{
			IStiPieSeries stiPieSeries2 = stiSeries2 as IStiPieSeries;
			int num7 = 0;
			double?[] values2 = stiSeries2.Values;
			for (int l = 0; l < values2.Length; l++)
			{
				double? num8 = values2[l];
				float num9 = (float)((double)gradPerValue * Math.Abs(num8.GetValueOrDefault()));
				IStiPieSeriesLabels pieSeriesLabels2 = ((StiPieSeriesCoreXF)stiPieSeries2.Core).GetPieSeriesLabels();
				if (pieSeriesLabels2 != null && pieSeriesLabels2.Visible)
				{
					float num10 = radius;
					if (GetPieDistance(stiPieSeries2, num7) > 0f)
					{
						num10 += stiPieSeries2.Distance * context.Options.Zoom;
					}
					float num11 = num + num9 / 2f;
					((StiPieSeriesLabelsCoreXF)pieSeriesLabels2.Core).RenderLabel(stiPieSeries2, context, pointCenter, num10, 0f, num11, num7, Math.Abs(num8.GetValueOrDefault()), num8, GetArgumentText(stiSeries2, num7), stiPieSeries2.Core.GetTag(num7), measure: true, num7, colorCount, percentPerValue, out var measureRect, drawValue: false, 0f);
					if (!measureRect.IsEmpty)
					{
						a = RectangleF.Union(a, measureRect);
					}
					if (num8 != 0.0 || pieSeriesLabels2.ShowZeros)
					{
						array[num6] = measureRect;
						array2[num6] = measureRect.Y;
						if (num11 >= 0f && num11 <= 90f)
						{
							array3[0]++;
						}
						if (num11 > 90f && num11 <= 180f)
						{
							array3[1]++;
						}
						if (num11 > 180f && num11 <= 270f)
						{
							array3[2]++;
						}
						if (num11 > 270f && num11 <= 360f)
						{
							array3[3]++;
						}
					}
				}
				num += num9;
				num7++;
				if (num8 != 0.0 || base.Series.Chart.SeriesLabels.ShowZeros)
				{
					num6++;
				}
			}
		}
		if (stiPieSeries.Diameter == 0f)
		{
			float val3 = 0f;
			val3 = Math.Min(val3, a.Left - rect2.Left);
			val3 = Math.Min(val3, rect2.Right - a.Right);
			val3 = Math.Min(val3, a.Top - rect2.Top);
			val3 = Math.Min(val3, rect2.Bottom - a.Bottom);
			radius += val3;
		}
		num6 = 0;
		foreach (IStiSeries stiSeries3 in seriesArray)
		{
			IStiPieSeries stiPieSeries3 = stiSeries3 as IStiPieSeries;
			int num12 = 0;
			double?[] values3 = stiSeries3.Values;
			for (int n = 0; n < values3.Length; n++)
			{
				double? num13 = values3[n];
				float num14 = (float)((double)gradPerValue * num13.GetValueOrDefault());
				IStiPieSeriesLabels pieSeriesLabels3 = ((StiPieSeriesCoreXF)stiPieSeries3.Core).GetPieSeriesLabels();
				if (pieSeriesLabels3 != null && pieSeriesLabels3.Visible)
				{
					float num15 = radius;
					if (GetPieDistance(stiPieSeries3, num12) > 0f)
					{
						num15 += stiPieSeries3.Distance * context.Options.Zoom;
					}
					float pieAngle = num + num14 / 2f;
					((StiPieSeriesLabelsCoreXF)pieSeriesLabels3.Core).RenderLabel(base.Series, context, pointCenter, num15, 0f, pieAngle, num12, num13, num13, GetArgumentText(stiSeries3, num12), stiPieSeries3.Core.GetTag(num12), measure: true, num12, colorCount, percentPerValue, out var measureRect2, drawValue: false, 0f);
					if (num13 != 0.0 || pieSeriesLabels3.ShowZeros)
					{
						array[num6] = measureRect2;
						array2[num6] = measureRect2.Y;
					}
				}
				num += num14;
				num12++;
				if (num13 != 0.0 || base.Series.Chart.SeriesLabels.ShowZeros)
				{
					num6++;
				}
			}
		}
		if (base.Series.Chart != null && base.Series.Chart.SeriesLabels != null && base.Series.Chart.SeriesLabels is IStiTwoColumnsPieLabels { PreventIntersection: not false })
		{
			float num16 = 0f;
			if (array3[0] > 0)
			{
				RectangleF[] array4 = new RectangleF[array3[0]];
				Array.Copy(array, 0, array4, 0, array3[0]);
				if (IsIntersectionLabels(array4))
				{
					num16 = a.Height / 2f / (float)array3[0];
					array[0].Y = a.Y + a.Height / 2f + num16 / 2f - array[0].Height / 2f;
					for (int num17 = 1; num17 < array3[0]; num17++)
					{
						array[num17].Y = array[num17 - 1].Y + num16;
					}
				}
			}
			int num18 = array3[0];
			if (array3[1] > 0)
			{
				RectangleF[] array5 = new RectangleF[array3[1]];
				Array.Copy(array, num18, array5, 0, array3[1]);
				if (IsIntersectionLabels(array5))
				{
					num16 = a.Height / 2f / (float)array3[1];
					array[num18].Y = a.Y + a.Height - num16 / 2f - array[num18].Height / 2f;
					for (int num19 = num18 + 1; num19 < num18 + array3[1]; num19++)
					{
						array[num19].Y = array[num19 - 1].Y - num16;
					}
				}
			}
			num18 += array3[1];
			if (array3[2] > 0)
			{
				RectangleF[] array6 = new RectangleF[array3[2]];
				Array.Copy(array, num18, array6, 0, array3[2]);
				if (IsIntersectionLabels(array6))
				{
					num16 = a.Height / 2f / (float)array3[2];
					array[num18].Y = a.Y + a.Height / 2f - num16 / 2f - array[num18].Height / 2f;
					for (int num20 = num18 + 1; num20 < num18 + array3[2]; num20++)
					{
						array[num20].Y = array[num20 - 1].Y - num16;
					}
				}
			}
			num18 += array3[2];
			if (array3[3] > 0)
			{
				RectangleF[] array7 = new RectangleF[array3[3]];
				Array.Copy(array, num18, array7, 0, array3[3]);
				if (IsIntersectionLabels(array7))
				{
					num16 = a.Height / 2f / (float)array3[3];
					array[num18].Y = a.Y + num16 / 2f - array[num18].Height / 2f;
					for (int num21 = num18 + 1; num21 < num18 + array3[3]; num21++)
					{
						array[num21].Y = array[num21 - 1].Y + num16;
					}
				}
			}
			for (int num22 = 0; num22 < num6; num22++)
			{
				array2[num22] = array[num22].Y - array2[num22];
			}
		}
		if (radius <= 5f)
		{
			return;
		}
		if (stiPieSeries.ShowShadow)
		{
			num = stiPieSeries.StartAngle;
			foreach (IStiSeries stiSeries4 in seriesArray)
			{
				if (nonZeroValuesCount == 1)
				{
					RectangleF clientRectangle = new RectangleF(pointCenter.X - radius, pointCenter.Y - radius, radius * 2f, radius * 2f);
					RectangleF rect3 = new RectangleF(0f, 0f, radius * 2f, radius * 2f);
					StiContext stiContext = context.CreateShadowGraphics();
					stiContext?.FillEllipse(Color.FromArgb(100, Color.Black), rect3);
					StiPieSeriesShadowElementGeom item = new StiPieSeriesShadowElementGeom(stiPieSeries, clientRectangle, radius * 0.01f + 2f * context.Options.Zoom, stiContext, globalDurationElement, globalBeginTimeElement);
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(item);
					break;
				}
				IStiPieSeries currentSeries2 = stiSeries4 as IStiPieSeries;
				int num24 = 0;
				double?[] values4 = stiSeries4.Values;
				foreach (double? num26 in values4)
				{
					float num27 = (float)((double)gradPerValue * Math.Abs(num26.GetValueOrDefault()));
					PointF center = pointCenter;
					StiSolidBrush brush = new StiSolidBrush(Color.FromArgb(100, Color.Black));
					StiContext stiContext2 = context.CreateShadowGraphics();
					RectangleF clientRectangle2 = rect;
					clientRectangle2.X = 0f;
					clientRectangle2.Y = 0f;
					if (stiChart.IsAnimation)
					{
						clientRectangle2.X = pointCenter.X - radius;
						clientRectangle2.Y = pointCenter.Y - radius;
						clientRectangle2.Height = radius * 2f;
						clientRectangle2.Width = radius * 2f;
					}
					RenderPieElementShadow(stiContext2, center, radius, (StiBrush)(object)brush, num, num27, currentSeries2, GetPieDistance(currentSeries2, num24));
					StiPieSeriesShadowElementGeom item2 = new StiPieSeriesShadowElementGeom(stiPieSeries, clientRectangle2, radius * 0.01f + 2f * context.Options.Zoom, stiContext2, globalDurationElement, globalBeginTimeElement);
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(item2);
					num += num27;
					num24++;
				}
			}
		}
		List<StiCellGeom> pieElementGeoms = GetPieElementGeoms(context, geom, radius, rect, seriesArray);
		if (pieElementGeoms.Count < 1)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(new StiPieEmptySeriesElementGeom(new RectangleF(pointCenter.X - radius, pointCenter.Y - radius, 2f * radius, 2f * radius)));
			return;
		}
		List<StiCellGeom> pieElementGeoms2 = GetPieElementGeoms(context, geom, radius, rect, seriesArray, isAnimationChangingValues);
		for (int num28 = 0; num28 < pieElementGeoms.Count; num28++)
		{
			StiCellGeom stiCellGeom = pieElementGeoms[num28];
			if (stiCellGeom is StiPieSeriesFullElementGeom stiPieSeriesFullElementGeom)
			{
				if (stiPieSeriesFullElementGeom.Series.Core.Interaction != null)
				{
					stiPieSeriesFullElementGeom.Interaction = new StiSeriesInteractionData(geom.Area, stiPieSeriesFullElementGeom.Series, stiPieSeriesFullElementGeom.Index);
				}
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiPieSeriesFullElementGeom);
			}
			else if (stiCellGeom is StiPieSeriesElementGeom stiPieSeriesElementGeom)
			{
				stiPieSeriesElementGeom.Count = pieElementGeoms.Count;
				if (stiPieSeriesElementGeom.Series.Core.Interaction != null)
				{
					stiPieSeriesElementGeom.Interaction = new StiSeriesInteractionData(geom.Area, stiPieSeriesElementGeom.Series, stiPieSeriesElementGeom.Index);
				}
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiPieSeriesElementGeom);
			}
		}
		num = stiPieSeries.StartAngle;
		List<StiGeom> pieLabelGeoms = GetPieLabelGeoms(context, radius, rect, seriesArray, array2);
		if (isAnimationChangingValues && pieSeriesLabels != null && pieSeriesLabels.ValueType == StiSeriesLabelsValueType.Value)
		{
			List<StiGeom> pieLabelGeoms2 = GetPieLabelGeoms(context, radius, rect, seriesArray, array2, isAnimationChangingValues);
			float num29 = radius / 1.75f;
			RectangleF rectangleF = default(RectangleF);
			rectangleF.X = pointCenter.X - num29;
			rectangleF.Y = pointCenter.Y - num29;
			rectangleF.Width = 2f * num29;
			rectangleF.Height = 2f * num29;
			RectangleF rectangleF2 = rectangleF;
			for (int num30 = 0; num30 < pieElementGeoms.Count; num30++)
			{
				StiCellGeom stiCellGeom2 = ((pieLabelGeoms.Count > num30) ? (pieLabelGeoms[num30] as StiCellGeom) : null);
				if (stiCellGeom2 != null)
				{
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiCellGeom2);
				}
			}
		}
		else
		{
			foreach (StiCellGeom item3 in pieLabelGeoms)
			{
				if (isAnimationChangingValues && item3 is StiCenterPieLabelsGeom)
				{
					((StiCenterPieLabelsGeom)item3).Animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, StiChartHelper.GlobalBeginTimeElement);
				}
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(item3);
			}
		}
		IStiPieSeriesLabels pieSeriesLabels4 = GetPieSeriesLabels();
		if (pieSeriesLabels4 != null && pieSeriesLabels4.PreventIntersection)
		{
			if (pieSeriesLabels4 is IStiTwoColumnsPieLabels)
			{
				CheckIntersectionTwoColumnsLabels(geom, rect2);
			}
			else if (pieSeriesLabels4 is IStiOutsidePieLabels)
			{
				CheckIntersectionOutLabels(geom);
			}
			else
			{
				CheckIntersectionLabels(geom);
			}
		}
	}

	protected int GetColorCount(IStiSeries[] seriesArray, bool isForValueFrom = false)
	{
		int num = 0;
		foreach (IStiSeries stiSeries in seriesArray)
		{
			if (isForValueFrom && stiSeries.ValuesStart != null)
			{
				num += stiSeries.ValuesStart.Length;
			}
			if (!isForValueFrom && stiSeries.Values != null)
			{
				num += stiSeries.Values.Length;
			}
		}
		return num;
	}

	private bool IsIntersectionLabels(RectangleF[] labels)
	{
		for (int i = 0; i < labels.Length; i++)
		{
			for (int j = 0; j < labels.Length; j++)
			{
				if (i != j && labels[i].IntersectsWith(labels[j]))
				{
					return true;
				}
			}
		}
		return false;
	}

	private List<StiGeom> GetPieLabelGeoms(StiContext context, float radius, RectangleF rect, IStiSeries[] seriesArray, float[] deltaY, bool isForValueFrom = false)
	{
		List<StiGeom> list = new List<StiGeom>();
		int num = 0;
		int num2 = 0;
		IStiPieSeries stiPieSeries = seriesArray[0] as IStiPieSeries;
		float gradPerValue = GetGradPerValue(seriesArray, isForValueFrom);
		float percentPerValue = GetPercentPerValue(seriesArray, isForValueFrom);
		PointF pointCenter = GetPointCenter(rect);
		float num3 = stiPieSeries.StartAngle;
		int colorCount = GetColorCount(seriesArray, isForValueFrom);
		foreach (IStiSeries stiSeries in seriesArray)
		{
			IStiPieSeries stiPieSeries2 = stiSeries as IStiPieSeries;
			int num4 = 0;
			double?[] array = (isForValueFrom ? stiSeries.ValuesStart : stiSeries.Values);
			double?[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				double? num5 = array2[j];
				float num6 = (float)((double)gradPerValue * Math.Abs(num5.GetValueOrDefault()));
				IStiPieSeriesLabels pieSeriesLabels = ((StiPieSeriesCoreXF)stiPieSeries2.Core).GetPieSeriesLabels();
				if (pieSeriesLabels != null && pieSeriesLabels.Visible)
				{
					IStiPieSeriesLabels stiPieSeriesLabels = pieSeriesLabels;
					IStiTwoColumnsPieLabels stiTwoColumnsPieLabels = pieSeriesLabels as IStiTwoColumnsPieLabels;
					if (stiPieSeriesLabels != null)
					{
						float num7 = radius;
						if (GetPieDistance(stiPieSeries2, num4) > 0f)
						{
							num7 += stiPieSeries2.Distance * context.Options.Zoom;
						}
						float pieAngle = num3 + num6 / 2f;
						float num8 = 0f;
						if (stiTwoColumnsPieLabels != null && stiTwoColumnsPieLabels.PreventIntersection)
						{
							num8 += deltaY[num2];
						}
						if (stiPieSeriesLabels.Step == 0 || num4 % stiPieSeriesLabels.Step == 0)
						{
							StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiPieSeriesLabelsCoreXF)stiPieSeriesLabels.Core).RenderLabel(stiPieSeries2, context, pointCenter, num7, 0f, pieAngle, num4, Math.Abs(num5.GetValueOrDefault()), num5, GetArgumentText(stiSeries, num4), stiPieSeries2.Core.GetTag(num4), measure: false, num, colorCount, percentPerValue, out var measureRect, drawValue: false, num8);
							if (stiSeriesLabelsGeom != null)
							{
								list.Add(stiSeriesLabelsGeom);
							}
							if (stiPieSeriesLabels is IStiOutsidePieLabels && ((IStiOutsidePieLabels)stiPieSeriesLabels).ShowValue)
							{
								stiSeriesLabelsGeom = ((StiOutsidePieLabelsCoreXF)stiPieSeriesLabels.Core).RenderLabel(stiPieSeries2, context, pointCenter, num7, 0f, pieAngle, num4, Math.Abs(num5.GetValueOrDefault()), num5, GetArgumentText(stiSeries, num4), stiPieSeries2.Core.GetTag(num4), measure: false, num, colorCount, percentPerValue, out measureRect, drawValue: true, 0f);
								if (stiSeriesLabelsGeom != null)
								{
									list.Add(stiSeriesLabelsGeom);
								}
							}
						}
					}
				}
				num++;
				num3 += num6;
				num4++;
				if (num5 != 0.0 || (pieSeriesLabels != null && pieSeriesLabels.ShowZeros))
				{
					num2++;
				}
			}
		}
		return list;
	}

	public IStiPieSeriesLabels GetPieSeriesLabels()
	{
		if (base.Series.ShowSeriesLabels == StiShowSeriesLabels.FromChart)
		{
			return base.Series.Chart.SeriesLabels as IStiPieSeriesLabels;
		}
		if (base.Series.ShowSeriesLabels == StiShowSeriesLabels.FromSeries)
		{
			return base.Series.SeriesLabels as IStiPieSeriesLabels;
		}
		return null;
	}

	private List<StiCellGeom> GetPieElementGeoms(StiContext context, StiAreaGeom geom, float radius, RectangleF rect, IStiSeries[] seriesArray, bool isForValueFrom = false)
	{
		List<StiCellGeom> list = new List<StiCellGeom>();
		IStiPieSeries stiPieSeries = seriesArray[0] as IStiPieSeries;
		float gradPerValue = GetGradPerValue(seriesArray, isForValueFrom);
		float percentPerValue = GetPercentPerValue(seriesArray, isForValueFrom);
		PointF pointCenter = GetPointCenter(rect);
		float num = stiPieSeries.StartAngle;
		int colorCount = GetColorCount(seriesArray, isForValueFrom);
		int nonZeroValuesCount = 0;
		int firstNonZeroValueIndex = -1;
		IStiSeries firstNonZeroSeries = null;
		double? num2 = CheckNonZerovalue(seriesArray, out nonZeroValuesCount, out firstNonZeroValueIndex, out firstNonZeroSeries, isForValueFrom);
		int num3 = 0;
		for (int i = 0; i < seriesArray.Length; i++)
		{
			IStiPieSeries stiPieSeries2 = (IStiPieSeries)seriesArray[i];
			if (nonZeroValuesCount == 0)
			{
				break;
			}
			double?[] array = (isForValueFrom ? stiPieSeries2.ValuesStart : stiPieSeries2.Values);
			for (int j = 0; j < array.Length; j++)
			{
				double? num4 = array[j];
				if (num4 != 0.0)
				{
					float num5 = (float)((double)gradPerValue * Math.Abs(num4.GetValueOrDefault()));
					StiBrush brush = stiPieSeries2.Brush;
					if (stiPieSeries2.AllowApplyBrush)
					{
						brush = stiPieSeries2.Core.GetSeriesBrush(num3, colorCount);
						brush = stiPieSeries2.ProcessSeriesBrushes(j, brush);
					}
					Color borderColor = stiPieSeries2.BorderColor;
					if (stiPieSeries2.AllowApplyBorderColor)
					{
						borderColor = (Color)stiPieSeries2.Core.GetSeriesBorderColor(num3, colorCount);
					}
					long ticks = ((stiPieSeries2.Values.Length == 0) ? 0 : (StiChartHelper.GlobalBeginTimeElement.Ticks / stiPieSeries2.Values.Length * j));
					StiPieSeriesElementGeom stiPieSeriesElementGeom = RenderPieElement(beginTime: new TimeSpan(ticks), context: context, center: pointCenter, radius: radius, borderColor: borderColor, brush: brush, start: num, angle: num5, value: Math.Abs(num4.GetValueOrDefault()), index: j, currentSeries: stiPieSeries2, distance: GetPieDistance(stiPieSeries2, j), geom: geom);
					if (stiPieSeriesElementGeom != null)
					{
						list.Add(stiPieSeriesElementGeom);
					}
					num += num5;
				}
				num3++;
			}
		}
		return list;
	}

	protected void CheckIntersectionOutLabels(StiAreaGeom geom)
	{
		List<StiCellGeom> childGeoms = geom.ChildGeoms;
		List<StiOutsidePieLabelsGeom> list = new List<StiOutsidePieLabelsGeom>();
		if (childGeoms != null)
		{
			foreach (StiCellGeom item in childGeoms)
			{
				if (item is StiOutsidePieLabelsGeom)
				{
					list.Add((StiOutsidePieLabelsGeom)item);
				}
			}
		}
		int count = list.Count;
		bool flag = true;
		int num = 0;
		while (flag && num < 29)
		{
			num++;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count; j++)
				{
					if (j == i || !list[i].LabelRect.IntersectsWith(list[j].LabelRect))
					{
						continue;
					}
					RectangleF labelRect = list[i].LabelRect;
					RectangleF labelRect2 = list[j].LabelRect;
					if (labelRect.IntersectsWith(labelRect2))
					{
						float num2 = labelRect.Height - Math.Abs(list[j].LabelRect.Y - list[i].LabelRect.Y) + 2f;
						if (labelRect.Y > labelRect2.Y)
						{
							labelRect.Y += num2 / 2f;
							labelRect2.Y -= num2 / 2f;
						}
						else
						{
							labelRect.Y -= num2 / 2f;
							labelRect2.Y += num2 / 2f;
						}
						if (labelRect.Y < 0f)
						{
							labelRect.Y = 0f;
						}
						if (labelRect2.Y < 0f)
						{
							labelRect2.Y = 0f;
						}
						if (labelRect.Y + labelRect.Height > geom.ClientRectangle.Height)
						{
							labelRect.Y = geom.ClientRectangle.Height - labelRect.Height;
						}
						if (labelRect2.Y + num2 / 2f + labelRect2.Height > geom.ClientRectangle.Height)
						{
							labelRect2.Y = geom.ClientRectangle.Height - labelRect2.Height;
						}
						list[i].LabelRect = labelRect;
						list[j].LabelRect = labelRect2;
					}
				}
			}
		}
	}

	private void CheckIntersectionTwoColumnsLabels(StiAreaGeom geom, RectangleF rect)
	{
		List<StiCellGeom> childGeoms = geom.ChildGeoms;
		if (childGeoms == null)
		{
			return;
		}
		PointF pointCenter = GetPointCenter(rect);
		List<StiTwoColumnsPieLabelsGeom> list = new List<StiTwoColumnsPieLabelsGeom>();
		List<StiTwoColumnsPieLabelsGeom> list2 = new List<StiTwoColumnsPieLabelsGeom>();
		foreach (StiCellGeom item in childGeoms)
		{
			if (item is StiTwoColumnsPieLabelsGeom)
			{
				if (item.ClientRectangle.X < pointCenter.X)
				{
					list.Add((StiTwoColumnsPieLabelsGeom)item);
				}
				else
				{
					list2.Add((StiTwoColumnsPieLabelsGeom)item);
				}
			}
		}
		if (IsIntersectionLabels(list.Select((StiTwoColumnsPieLabelsGeom x) => x.ClientRectangle).ToArray()))
		{
			CheckLabelPosition(list, rect);
		}
		if (IsIntersectionLabels(list2.Select((StiTwoColumnsPieLabelsGeom x) => x.ClientRectangle).ToArray()))
		{
			CheckLabelPosition(list2, rect);
		}
	}

	protected override void CheckIntersectionLabels(StiAreaGeom geom)
	{
		List<StiCellGeom> childGeoms = geom.ChildGeoms;
		List<StiCenterPieLabelsGeom> list = new List<StiCenterPieLabelsGeom>();
		if (childGeoms != null)
		{
			foreach (StiCellGeom item in childGeoms)
			{
				if (item is StiCenterPieLabelsGeom)
				{
					list.Add((StiCenterPieLabelsGeom)item);
				}
			}
		}
		int count = list.Count;
		bool flag = true;
		int num = 0;
		while (flag && num < 29)
		{
			num++;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count; j++)
				{
					if (j == i || !list[i].ClientRectangle.IntersectsWith(list[j].ClientRectangle))
					{
						continue;
					}
					RectangleF labelRect = list[i].LabelRect;
					RectangleF labelRect2 = list[j].LabelRect;
					if (labelRect.IntersectsWith(labelRect2))
					{
						float num2 = labelRect.Height - Math.Abs(list[j].ClientRectangle.Y - list[i].ClientRectangle.Y) + 2f;
						if (labelRect.Y > labelRect2.Y)
						{
							labelRect.Y += num2 / 2f;
							labelRect2.Y -= num2 / 2f;
						}
						else
						{
							labelRect.Y -= num2 / 2f;
							labelRect2.Y += num2 / 2f;
						}
						if (labelRect.Y < 0f)
						{
							labelRect.Y = 0f;
						}
						if (labelRect2.Y < 0f)
						{
							labelRect2.Y = 0f;
						}
						if (labelRect.Y + labelRect.Height > geom.ClientRectangle.Height)
						{
							labelRect.Y = geom.ClientRectangle.Height - labelRect.Height;
						}
						if (labelRect2.Y + num2 / 2f + labelRect2.Height > geom.ClientRectangle.Height)
						{
							labelRect2.Y = geom.ClientRectangle.Height - labelRect2.Height;
						}
						list[i].LabelRect = labelRect;
						list[j].LabelRect = labelRect2;
					}
				}
			}
		}
	}

	private void CheckLabelPosition(List<StiTwoColumnsPieLabelsGeom> labels, RectangleF rect)
	{
		float num = rect.Height;
		labels = labels.OrderBy((StiTwoColumnsPieLabelsGeom x) => x.ClientRectangle.Y).ToList();
		foreach (StiTwoColumnsPieLabelsGeom label in labels)
		{
			num -= label.ClientRectangle.Height;
		}
		float num2 = num / (float)(labels.Count + 1);
		float num3 = rect.Y;
		foreach (StiTwoColumnsPieLabelsGeom label2 in labels)
		{
			label2.ClientRectangle = new RectangleF(label2.ClientRectangle.X, num3 + num2, label2.ClientRectangle.Width, label2.ClientRectangle.Height);
			label2.EndPoint = new PointF(label2.EndPoint.X, label2.ClientRectangle.Y + label2.ClientRectangle.Height / 2f);
			num3 = label2.ClientRectangle.Y + label2.ClientRectangle.Height;
		}
	}

	protected float GetGradPerValue(IStiSeries[] series, bool isForValueFrom = false)
	{
		double num = 0.0;
		int num2 = 0;
		foreach (IStiSeries stiSeries in series)
		{
			double?[] array = (isForValueFrom ? stiSeries.ValuesStart : stiSeries.Values);
			double?[] array2 = array;
			foreach (double? num3 in array2)
			{
				num += Math.Abs(num3.GetValueOrDefault());
				num2++;
			}
		}
		if (num2 == 0)
		{
			num2 = 1;
		}
		if (num > 0.0)
		{
			return (float)(360.0 / num);
		}
		return 360f / (float)num2;
	}

	internal float GetPercentPerValue(IStiSeries[] series, bool isForValueFrom = false)
	{
		double num = 0.0;
		foreach (IStiSeries stiSeries in series)
		{
			double?[] array = (isForValueFrom ? stiSeries.ValuesStart : stiSeries.Values);
			double?[] array2 = array;
			foreach (double? num2 in array2)
			{
				num += Math.Abs(num2.GetValueOrDefault());
			}
		}
		return (float)(1.0 / num * 100.0);
	}

	protected PointF GetPointCenter(RectangleF rect)
	{
		return new PointF(rect.Width / 2f, rect.Height / 2f);
	}

	protected float GetRadius(StiContext context, RectangleF rect)
	{
		IStiPieSeries stiPieSeries = base.Series as IStiPieSeries;
		if (stiPieSeries.Diameter > 0f)
		{
			return stiPieSeries.Diameter / 2f * context.Options.Zoom;
		}
		return Math.Min(rect.Width / 2f, rect.Height / 2f) * 0.95f;
	}

	protected PointF GetPoint(PointF centerPie, float radius, float angle)
	{
		float num = (float)(Math.PI * (double)angle / 180.0);
		return new PointF(centerPie.X + (float)Math.Cos(num) * radius, centerPie.Y + (float)Math.Sin(num) * radius);
	}

	protected virtual string GetArgumentText(IStiSeries series, int index)
	{
		if (series.Arguments.Length > index && series.Arguments[index] != null)
		{
			return series.Arguments[index].ToString();
		}
		return string.Empty;
	}

	public float GetPieDistance(int pieIndex)
	{
		return GetPieDistance(base.Series as IStiPieSeries, pieIndex);
	}

	public float GetPieDistance(IStiPieSeries series, int pieIndex)
	{
		if (series.Distance == 0f)
		{
			return 0f;
		}
		if (series.CutPieListValues.Length == 0)
		{
			return series.Distance;
		}
		double[] cutPieListValues = series.CutPieListValues;
		foreach (double num in cutPieListValues)
		{
			if (num == (double)(pieIndex + 1))
			{
				return series.Distance;
			}
		}
		return 0f;
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return ((IStiPieSeries)base.Series).Brush;
		}
		return seriesBrush;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		object seriesBorderColor = base.GetSeriesBorderColor(colorIndex, colorCount);
		if (seriesBorderColor == null)
		{
			return ((IStiPieSeries)base.Series).BorderColor;
		}
		return seriesBorderColor;
	}

	public StiPieSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
