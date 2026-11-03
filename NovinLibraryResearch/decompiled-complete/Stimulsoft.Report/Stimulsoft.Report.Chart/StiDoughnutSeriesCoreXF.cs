using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart.Geoms.Series.Doughnut;

namespace Stimulsoft.Report.Chart;

public class StiDoughnutSeriesCoreXF : StiPieSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Doughnut");

	private StiDoughnutSeriesElementGeom RenderDoughnutElement(StiContext context, PointF center, float radius, float radiusDt, Color borderColor, StiBrush brush, float start, float angle, double? value, int index, IStiDoughnutSeries currentSeries, bool shadow, StiAreaGeom areaGeom, TimeSpan beginTime)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		if (radiusDt < 0f)
		{
			radiusDt = 1f;
		}
		if (angle == 0f || float.IsNaN(angle))
		{
			return null;
		}
		RectangleF rectangleF = new RectangleF(center.X - radius, center.Y - radius, radius * 2f, radius * 2f);
		RectangleF rectangleF2 = new RectangleF(center.X - radiusDt, center.Y - radiusDt, radiusDt * 2f, radiusDt * 2f);
		if (rectangleF.Width <= 0f && rectangleF.Height <= 0f)
		{
			return null;
		}
		if (!shadow)
		{
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
		}
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		StiBrush val = null;
		StiBrush brushDark = null;
		List<StiSegmentGeom> list2 = null;
		List<StiSegmentGeom> list3 = null;
		float num = 0f;
		if (currentSeries.Lighting && !shadow)
		{
			list2 = new List<StiSegmentGeom>();
			list3 = new List<StiSegmentGeom>();
			num = radius * 0.02f;
			val = (StiBrush)new StiGradientBrush(Color.FromArgb(100, Color.White), Color.FromArgb(50, Color.Black), 45.0);
			brushDark = (StiBrush)new StiGradientBrush(Color.FromArgb(50, Color.Black), Color.FromArgb(100, Color.White), 45.0);
		}
		StiChart stiChart = base.Series.Chart as StiChart;
		list.Add(new StiArcSegmentGeom(rectangleF, start, angle));
		list.Add(new StiLineSegmentGeom(GetPoint(center, radius, start + angle), GetPoint(center, radiusDt, start + angle)));
		list.Add(new StiArcSegmentGeom(rectangleF2, start + angle, 0f - angle));
		list.Add(new StiLineSegmentGeom(GetPoint(center, radiusDt, start), GetPoint(center, radius, start)));
		list.Add(new StiCloseFigureSegmentGeom());
		if (shadow)
		{
			if (stiChart.IsAnimation)
			{
				StiOpacityAnimation animation = new StiOpacityAnimation(TimeSpan.FromSeconds(1.0), beginTime);
				context.FillDrawAnimationPath(brush, null, list, rectangleF, null, animation);
			}
			else
			{
				context.FillPath(brush, list, rectangleF);
			}
		}
		if (val != null && !shadow)
		{
			list2.Add(new StiLineSegmentGeom(GetPoint(center, radius - num, start), GetPoint(center, radius, start)));
			list2.Add(new StiArcSegmentGeom(rectangleF, start, angle));
			list2.Add(new StiLineSegmentGeom(GetPoint(center, radius, start + angle), GetPoint(center, radius - num, start + angle)));
			list2.Add(new StiArcSegmentGeom(new RectangleF(rectangleF.X + num, rectangleF.Y + num, rectangleF.Width - num * 2f, rectangleF.Height - num * 2f), start + angle, 0f - angle));
			list3.Add(new StiLineSegmentGeom(GetPoint(center, radiusDt + num, start), GetPoint(center, radiusDt, start)));
			list3.Add(new StiArcSegmentGeom(rectangleF2, start, angle));
			list3.Add(new StiLineSegmentGeom(GetPoint(center, radiusDt, start + angle), GetPoint(center, radiusDt + num, start + angle)));
			list3.Add(new StiArcSegmentGeom(new RectangleF(rectangleF2.X - num, rectangleF2.Y - num, rectangleF2.Width + num * 2f, rectangleF2.Height + num * 2f), start + angle, 0f - angle));
		}
		if (!shadow)
		{
			return new StiDoughnutSeriesElementGeom(areaGeom, value.GetValueOrDefault(), index, currentSeries, rectangleF, rectangleF2, list, list2, list3, borderColor, brush, val, brushDark, start, start + angle, radius, radiusDt, beginTime);
		}
		return null;
	}

	private bool IsNotNullValues(IStiSeries[] seriesArray)
	{
		foreach (IStiSeries stiSeries in seriesArray)
		{
			double?[] values = stiSeries.Values;
			foreach (double? num in values)
			{
				if (num.HasValue)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Expected O, but got Unknown
		float radius = GetRadius(context, rect);
		PointF pointCenter = GetPointCenter(rect);
		if (seriesArray == null || seriesArray.Length == 0 || base.Series.Chart == null || (seriesArray != null && !IsNotNullValues(seriesArray)))
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(new StiDoughnutEmptySeriesElementGeom(new RectangleF(pointCenter.X - radius, pointCenter.Y - radius, 2f * radius, 2f * radius)));
			return;
		}
		TimeSpan globalDurationElement = StiChartHelper.GlobalDurationElement;
		TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
		float num = radius / (float)(seriesArray.Length + 1);
		float num2 = num;
		int num3 = 0;
		for (int i = 0; i < seriesArray.Length; i++)
		{
			IStiDoughnutSeries stiDoughnutSeries = (IStiDoughnutSeries)seriesArray[i];
			if (stiDoughnutSeries.Values != null)
			{
				num3 += stiDoughnutSeries.Values.Length;
			}
			num2 += ((stiDoughnutSeries.Width != 0f) ? stiDoughnutSeries.Width : num);
		}
		radius = num2;
		if (num3 == 0)
		{
			return;
		}
		IStiPieSeries stiPieSeries = seriesArray[0] as IStiPieSeries;
		int num4 = 0;
		int num5 = 0;
		RectangleF rectangleF = new RectangleF(10f * context.Options.Zoom, 10f * context.Options.Zoom, rect.Width - 20f * context.Options.Zoom, rect.Height - 20f * context.Options.Zoom);
		RectangleF rectangleF2 = rectangleF;
		float num6 = 0f;
		float num7 = radius;
		for (int j = 0; j < seriesArray.Length; j++)
		{
			IStiDoughnutSeries stiDoughnutSeries2 = (IStiDoughnutSeries)seriesArray[j];
			num6 = stiDoughnutSeries2.StartAngle;
			float gradPerValue = GetGradPerValue(stiDoughnutSeries2);
			double?[] values = stiDoughnutSeries2.Values;
			for (int k = 0; k < values.Length; k++)
			{
				double? labelValue = values[k];
				float percentPerValue = GetPercentPerValue(stiDoughnutSeries2);
				float num8 = (float)((double)gradPerValue * Math.Abs(labelValue.GetValueOrDefault()));
				if (base.Series.Chart != null && base.Series.Chart.SeriesLabels != null && base.Series.Chart.SeriesLabels.Visible && base.Series.Chart.SeriesLabels is IStiPieSeriesLabels { Visible: not false } stiPieSeriesLabels)
				{
					float radius2 = ((stiDoughnutSeries2.Width != 0f) ? (num7 - stiDoughnutSeries2.Width) : (num7 - num));
					((StiPieSeriesLabelsCoreXF)stiPieSeriesLabels.Core).RenderLabel(stiDoughnutSeries2, context, pointCenter, num7, radius2, num6 + num8 / 2f, num5, Math.Abs(labelValue.GetValueOrDefault()), labelValue, GetArgumentText(stiDoughnutSeries2, num5), stiPieSeries.Core.GetTag(num5), measure: true, num4, num3, percentPerValue, out var measureRect, drawValue: false, 0f);
					if (!measureRect.IsEmpty)
					{
						rectangleF = RectangleF.Union(rectangleF, measureRect);
					}
				}
				num4++;
				num6 += num8;
				num5++;
			}
			num7 -= ((stiDoughnutSeries2.Width != 0f) ? (stiDoughnutSeries2.Width + num * 0.2f) : (num * 1.2f));
		}
		float val = 0f;
		val = Math.Min(val, rectangleF.Left - rectangleF2.Left);
		val = Math.Min(val, rectangleF2.Right - rectangleF.Right);
		val = Math.Min(val, rectangleF.Top - rectangleF2.Top);
		val = Math.Min(val, rectangleF2.Bottom - rectangleF.Bottom);
		radius += val;
		StiChart stiChart = base.Series.Chart as StiChart;
		num5 = 0;
		if (stiPieSeries.ShowShadow && !stiChart.IsAnimation)
		{
			RectangleF clientRectangle = new RectangleF(pointCenter.X - radius, pointCenter.Y - radius, radius * 2f, radius * 2f);
			StiContext stiContext = context.CreateShadowGraphics();
			StiSolidBrush brush = new StiSolidBrush(Color.FromArgb(100, Color.Black));
			num = radius / (float)(seriesArray.Length + 1);
			num7 = radius;
			for (int l = 0; l < seriesArray.Length; l++)
			{
				IStiDoughnutSeries stiDoughnutSeries3 = (IStiDoughnutSeries)seriesArray[l];
				num6 = stiDoughnutSeries3.StartAngle;
				float gradPerValue2 = GetGradPerValue(stiDoughnutSeries3);
				double?[] values2 = stiDoughnutSeries3.Values;
				for (int m = 0; m < values2.Length; m++)
				{
					double? num9 = values2[m];
					float num10 = (float)((double)gradPerValue2 * Math.Abs(num9.GetValueOrDefault()));
					float radiusDt = ((stiDoughnutSeries3.Width != 0f) ? (num7 - stiDoughnutSeries3.Width) : (num7 - num));
					RenderDoughnutElement(stiContext, new PointF(clientRectangle.Width / 2f, clientRectangle.Height / 2f), num7, radiusDt, Color.Black, (StiBrush)(object)brush, num6, num10, Math.Abs(num9.GetValueOrDefault()), num5, stiDoughnutSeries3, shadow: true, geom, new TimeSpan(globalBeginTimeElement.Ticks));
					num6 += num10;
					num5++;
				}
				num7 -= ((stiDoughnutSeries3.Width != 0f) ? (stiDoughnutSeries3.Width + num * 0.2f) : (num * 1.2f));
			}
			StiPieSeriesShadowElementGeom item = new StiPieSeriesShadowElementGeom(stiPieSeries, clientRectangle, radius * 0.01f + 2f * context.Options.Zoom, stiContext, globalDurationElement, globalBeginTimeElement);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
		num = radius / (float)(seriesArray.Length + 1);
		num4 = 0;
		num5 = 0;
		num7 = radius;
		for (int n = 0; n < seriesArray.Length; n++)
		{
			IStiDoughnutSeries stiDoughnutSeries4 = (IStiDoughnutSeries)seriesArray[n];
			int num11 = 0;
			if (!stiDoughnutSeries4.Chart.Area.ColorEach)
			{
				num4 = 0;
			}
			num6 = stiDoughnutSeries4.StartAngle;
			float gradPerValue3 = GetGradPerValue(stiDoughnutSeries4);
			int nonZeroValuesCount = 0;
			int firstNonZeroValueIndex = -1;
			IStiSeries firstNonZeroSeries = null;
			double? num12 = CheckNonZerovalue(seriesArray, out nonZeroValuesCount, out firstNonZeroValueIndex, out firstNonZeroSeries);
			switch (nonZeroValuesCount)
			{
			case 1:
			{
				StiBrush brush3 = stiDoughnutSeries4.Brush;
				if (stiDoughnutSeries4.AllowApplyBrush)
				{
					brush3 = stiDoughnutSeries4.Core.GetSeriesBrush(firstNonZeroValueIndex, num3);
					brush3 = stiDoughnutSeries4.ProcessSeriesBrushes(firstNonZeroValueIndex, brush3);
				}
				Color borderColor2 = (Color)stiPieSeries.Core.GetSeriesBorderColor(num4, num3);
				if (firstNonZeroValueIndex >= stiDoughnutSeries4.Values.Length)
				{
					continue;
				}
				float radiusDt3 = ((stiDoughnutSeries4.Width != 0f) ? (num7 - stiDoughnutSeries4.Width) : (num7 - num));
				StiDoughnutSeriesElementGeom stiDoughnutSeriesElementGeom2 = RenderDoughnutElement(context, pointCenter, num7, radiusDt3, borderColor2, brush3, 0f, 360f, stiDoughnutSeries4.Values[firstNonZeroValueIndex], firstNonZeroValueIndex, stiDoughnutSeries4, shadow: false, geom, new TimeSpan(globalBeginTimeElement.Ticks));
				if (stiDoughnutSeriesElementGeom2 != null)
				{
					if (stiDoughnutSeries4.Core.Interaction != null)
					{
						stiDoughnutSeriesElementGeom2.Interaction = new StiSeriesInteractionData(geom.Area, stiDoughnutSeries4, 0);
					}
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiDoughnutSeriesElementGeom2);
				}
				num7 -= num * 1.2f;
				num11++;
				num4++;
				num5++;
				continue;
			}
			default:
			{
				int num13 = 0;
				double?[] values3 = stiDoughnutSeries4.Values;
				for (int num14 = 0; num14 < values3.Length; num14++)
				{
					double? num15 = values3[num14];
					float num16 = (float)((double)gradPerValue3 * Math.Abs(num15.GetValueOrDefault()));
					if (num15 != 0.0)
					{
						StiBrush brush2 = stiDoughnutSeries4.Brush;
						if (stiDoughnutSeries4.AllowApplyBrush)
						{
							brush2 = stiDoughnutSeries4.Core.GetSeriesBrush(num4, num3);
							brush2 = stiDoughnutSeries4.ProcessSeriesBrushes(num11, brush2);
						}
						Color borderColor = stiDoughnutSeries4.BorderColor;
						if (stiDoughnutSeries4.AllowApplyBorderColor)
						{
							borderColor = (Color)stiDoughnutSeries4.Core.GetSeriesBorderColor(num4, num3);
						}
						float radiusDt2 = ((stiDoughnutSeries4.Width != 0f) ? (num7 - stiDoughnutSeries4.Width) : (num7 - num));
						StiDoughnutSeriesElementGeom stiDoughnutSeriesElementGeom = RenderDoughnutElement(context, pointCenter, num7, radiusDt2, borderColor, brush2, num6, num16, Math.Abs(num15.GetValueOrDefault()), num13, stiDoughnutSeries4, shadow: false, geom, new TimeSpan(globalBeginTimeElement.Ticks / stiDoughnutSeries4.Values.Length * num4));
						if (stiDoughnutSeriesElementGeom != null)
						{
							if (stiDoughnutSeries4.Core.Interaction != null)
							{
								stiDoughnutSeriesElementGeom.Interaction = new StiSeriesInteractionData(geom.Area, stiDoughnutSeries4, num13);
							}
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiDoughnutSeriesElementGeom);
						}
						num6 += num16;
					}
					num11++;
					num4++;
					num5++;
					num13++;
				}
				break;
			}
			case 0:
				break;
			}
			num7 -= ((stiDoughnutSeries4.Width != 0f) ? (stiDoughnutSeries4.Width + num * 0.2f) : (num * 1.2f));
		}
		if (num5 == 0)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(new StiDoughnutEmptySeriesElementGeom(new RectangleF(pointCenter.X - radius, pointCenter.Y - radius, 2f * radius, 2f * radius)));
			return;
		}
		num4 = 0;
		num7 = radius;
		for (int num17 = 0; num17 < seriesArray.Length; num17++)
		{
			IStiDoughnutSeries stiDoughnutSeries5 = (IStiDoughnutSeries)seriesArray[num17];
			num5 = 0;
			num6 = stiDoughnutSeries5.StartAngle;
			float gradPerValue4 = GetGradPerValue(stiDoughnutSeries5);
			double?[] values4 = stiDoughnutSeries5.Values;
			for (int num18 = 0; num18 < values4.Length; num18++)
			{
				double? labelValue2 = values4[num18];
				float percentPerValue2 = GetPercentPerValue(stiDoughnutSeries5);
				float num19 = (float)((double)gradPerValue4 * Math.Abs(labelValue2.GetValueOrDefault()));
				IStiSeriesLabels stiSeriesLabels = null;
				if (stiDoughnutSeries5.ShowSeriesLabels == StiShowSeriesLabels.FromChart)
				{
					stiSeriesLabels = base.Series.Chart.SeriesLabels;
				}
				if (stiDoughnutSeries5.ShowSeriesLabels == StiShowSeriesLabels.FromSeries)
				{
					stiSeriesLabels = stiPieSeries.SeriesLabels;
				}
				if (base.Series.Chart != null && stiSeriesLabels != null && stiSeriesLabels.Visible && stiSeriesLabels is IStiPieSeriesLabels { Visible: not false } stiPieSeriesLabels2 && (stiPieSeriesLabels2.Step == 0 || num5 % stiPieSeriesLabels2.Step == 0))
				{
					float radius3 = ((stiDoughnutSeries5.Width != 0f) ? (num7 - stiDoughnutSeries5.Width) : (num7 - num));
					RectangleF measureRect2;
					StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiPieSeriesLabelsCoreXF)stiPieSeriesLabels2.Core).RenderLabel(stiDoughnutSeries5, context, pointCenter, num7, radius3, num6 + num19 / 2f, num5, Math.Abs(labelValue2.GetValueOrDefault()), labelValue2, GetArgumentText(stiDoughnutSeries5, num5), GetTag(num5), measure: false, num4, num3, percentPerValue2, out measureRect2, drawValue: false, 0f);
					if (stiSeriesLabelsGeom != null)
					{
						stiSeriesLabelsGeom.Duration = globalDurationElement;
						stiSeriesLabelsGeom.BeginTime = new TimeSpan(globalBeginTimeElement.Ticks / stiDoughnutSeries5.Values.Length * num4);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(stiSeriesLabelsGeom);
					}
				}
				num4++;
				num6 += num19;
				num5++;
			}
			num7 -= ((stiDoughnutSeries5.Width != 0f) ? (stiDoughnutSeries5.Width + num * 0.2f) : (num * 1.2f));
		}
		IStiPieSeriesLabels pieSeriesLabels = GetPieSeriesLabels();
		if (pieSeriesLabels != null && pieSeriesLabels.PreventIntersection)
		{
			CheckIntersectionLabels(geom);
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

	private float GetGradPerValue(IStiSeries series)
	{
		double num = 0.0;
		double?[] values = series.Values;
		foreach (double? num2 in values)
		{
			num += Math.Abs(num2.GetValueOrDefault());
		}
		return (float)(360.0 / num);
	}

	private float GetPercentPerValue(IStiSeries series)
	{
		double num = 0.0;
		double?[] values = series.Values;
		foreach (double? num2 in values)
		{
			num += Math.Abs(num2.GetValueOrDefault());
		}
		return (float)(1.0 / num * 100.0);
	}

	protected override string GetArgumentText(IStiSeries series, int index)
	{
		if (series.Arguments.Length > index)
		{
			return series.Arguments[index].ToString();
		}
		return string.Empty;
	}

	public StiDoughnutSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
