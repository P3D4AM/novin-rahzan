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

public class StiClusteredBarSeriesCoreXF : StiClusteredColumnSeriesCoreXF, IStiApplyStyleSeries
{
	public override string LocalizedName => StiLocalization.Get("Chart", "ClusteredBar");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		base.ApplyStyle(style, color);
		IStiClusteredBarSeries stiClusteredBarSeries = base.Series as IStiClusteredBarSeries;
		if (stiClusteredBarSeries.AllowApplyStyle)
		{
			stiClusteredBarSeries.Brush = style.Core.GetColumnBrush(color);
			if (stiClusteredBarSeries.Brush is StiGradientBrush)
			{
				StiGradientBrush val = (StiGradientBrush)stiClusteredBarSeries.Brush;
				val.Angle += 90.0;
			}
			if (stiClusteredBarSeries.Brush is StiGlareBrush)
			{
				StiGlareBrush val2 = (StiGlareBrush)stiClusteredBarSeries.Brush;
				val2.Angle += 90.0;
			}
			stiClusteredBarSeries.BorderColor = style.Core.GetColumnBorder(color);
			stiClusteredBarSeries.BorderThickness = style.Core.SeriesBorderThickness;
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
		bool startFromZero = stiAxisArea.YAxis.Core.GetStartFromZero();
		float dividerX = stiAxisArea.AxisCore.GetDividerX();
		float dividerTopX = stiAxisArea.AxisCore.GetDividerTopX();
		int num = stiAxisArea.AxisCore.ValuesCount * series.Length;
		int num2 = (stiAxisArea.ReverseVert ? (num - 1) : 0);
		int valuesCount = stiAxisArea.AxisCore.ValuesCount;
		List<List<PointF?>> list2 = new List<List<PointF?>>();
		for (int i = 0; i < series.Length; i++)
		{
			list2.Add(new List<PointF?>());
		}
		for (int j = 0; j < valuesCount; j++)
		{
			float num3 = (startFromZero ? (stiAxisArea.YAxis.Info.StripPositions[j + 1] - stiAxisArea.YAxis.Info.Step / 2f) : (stiAxisArea.YAxis.Info.StripPositions[j] - stiAxisArea.YAxis.Info.Step / 2f));
			float num4 = 0f;
			foreach (IStiSeries stiSeries in series)
			{
				num4 += stiAxisArea.YAxis.Info.Step / (float)series.Length * ((IStiClusteredColumnSeries)stiSeries).Width;
			}
			float num5 = num3 + (stiAxisArea.YAxis.Info.Step - num4) / 2f;
			int num6 = 0;
			if (stiAxisArea.ReverseVert)
			{
				num6 = series.Length - 1;
			}
			TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
			while ((num6 < series.Length && !stiAxisArea.ReverseVert) || (num6 >= 0 && stiAxisArea.ReverseVert))
			{
				IStiClusteredBarSeries stiClusteredBarSeries = series[num6] as IStiClusteredBarSeries;
				float num7 = stiAxisArea.YAxis.Info.Step / (float)series.Length * stiClusteredBarSeries.Width;
				if (j < stiClusteredBarSeries.Values.Length)
				{
					double? num8 = 0.0 - ((!stiAxisArea.ReverseVert) ? stiClusteredBarSeries.Values[stiClusteredBarSeries.Values.Length - j - 1] : stiClusteredBarSeries.Values[j]);
					double num9 = num8.GetValueOrDefault() * stiAxisArea.XAxis.Info.Dpi;
					RectangleF barRect = GetBarRect(context, stiClusteredBarSeries, num8, num5, num7);
					RectangleF columnRectStart = RectangleF.Empty;
					double? num10 = null;
					if (isAnimationChangingValues)
					{
						if (stiClusteredBarSeries.ValuesStart.Length > j)
						{
							num10 = 0.0 - ((!stiAxisArea.ReverseVert) ? stiClusteredBarSeries.ValuesStart[stiClusteredBarSeries.ValuesStart.Length - j - 1] : stiClusteredBarSeries.ValuesStart[j]);
						}
						columnRectStart = (stiAxisArea.ReverseHor ? ((!(num8 < 0.0)) ? RectangleF.FromLTRB(barRect.Left, barRect.Top, barRect.Left, barRect.Bottom) : RectangleF.FromLTRB(barRect.Right, barRect.Top, barRect.Right, barRect.Bottom)) : ((!(num8 >= 0.0)) ? RectangleF.FromLTRB(barRect.Left, barRect.Top, barRect.Left, barRect.Bottom) : RectangleF.FromLTRB(barRect.Right, barRect.Top, barRect.Right, barRect.Bottom)));
					}
					if (!((StiSeries)stiClusteredBarSeries).TrendLines.ToList().All((StiTrendLine l) => l is IStiTrendLineNone))
					{
						list2[num6].Add(new PointF((float)num9, num5 + num7 / 2f));
					}
					RectangleF clientRectangle = ((StiAxisAreaGeom)geom).View.ClientRectangle;
					clientRectangle.X = 0f;
					clientRectangle.Y = 0f;
					RectangleF rectangleF = barRect;
					rectangleF.X += geom.ClientRectangle.X;
					rectangleF.Y += geom.ClientRectangle.Y;
					if ((rectangleF.Bottom > clientRectangle.Y && rectangleF.Y < clientRectangle.Bottom) || ((IStiAxisArea)base.Series.Chart.Area).YAxis.Range.Auto)
					{
						if (base.Series.Chart != null && base.Series.Chart.Style != null && barRect.Width > 0f)
						{
							StiBrush seriesBrush = stiClusteredBarSeries.Core.GetSeriesBrush(num2, num);
							if (stiClusteredBarSeries.AllowApplyBrushNegative && num8 > 0.0)
							{
								seriesBrush = stiClusteredBarSeries.BrushNegative;
							}
							seriesBrush = stiClusteredBarSeries.ProcessSeriesBrushes(valuesCount - 1 - j, seriesBrush);
							Color seriesBorderColor = (Color)stiClusteredBarSeries.Core.GetSeriesBorderColor(num2, num);
							StiClusteredBarSeriesElementGeom stiClusteredBarSeriesElementGeom = new StiClusteredBarSeriesElementGeom(geom, num10.GetValueOrDefault(), stiAxisArea.ReverseHor ? (0.0 - num8.GetValueOrDefault()) : num8.GetValueOrDefault(), j, seriesBrush, seriesBorderColor, stiClusteredBarSeries, columnRectStart, barRect);
							if (stiClusteredBarSeries.Core.Interaction != null)
							{
								stiClusteredBarSeriesElementGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, stiClusteredBarSeries, valuesCount - 1 - j);
							}
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiClusteredBarSeriesElementGeom);
						}
						IStiAxisSeriesLabels seriesLabels = stiClusteredBarSeries.Core.GetSeriesLabels();
						if (seriesLabels != null && seriesLabels.Visible)
						{
							double? num11 = num8;
							if (num8.HasValue)
							{
								num11 = (stiAxisArea.ReverseHor ? num8 : (0.0 - num8));
							}
							float num12 = ((stiClusteredBarSeries.YAxis == StiSeriesYAxis.RightYAxis) ? dividerTopX : dividerX);
							double num13 = ((stiClusteredBarSeries.YAxis == StiSeriesYAxis.RightYAxis) ? stiAxisArea.XTopAxis.Info.Dpi : stiAxisArea.XAxis.Info.Dpi);
							PointF point = new PointF((float)(num11.GetValueOrDefault() * num13) + num12, num5 + num7 / 2f);
							StiAnimation animation = null;
							if (isAnimationChangingValues)
							{
								if (seriesLabels.ValueType == StiSeriesLabelsValueType.Value && string.IsNullOrEmpty(seriesLabels.Format) && seriesLabels.FormatService is StiGeneralFormatService)
								{
									PointF pointF = new PointF(0f - (float)(num10.GetValueOrDefault() * num13) + num12, num5 + num7 / 2f);
									double? valueFrom = null;
									double? value = null;
									if (num10.HasValue)
									{
										valueFrom = (stiAxisArea.ReverseHor ? num10 : (0.0 - num10));
									}
									if (num8.HasValue)
									{
										value = (stiAxisArea.ReverseHor ? num8 : (0.0 - num8));
									}
									animation = new StiLabelAnimation(valueFrom, value, new PointF(pointF.X, pointF.Y), new PointF(point.X, point.Y), globalBeginTimeElement, TimeSpan.Zero);
								}
								else
								{
									animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, TimeSpan.Zero);
								}
							}
							if (seriesLabels.Step == 0 || j % seriesLabels.Step == 0)
							{
								int index = (stiAxisArea.YAxis.StartFromZero ? (j + 1) : j);
								StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiClusteredBarSeries, context, CorrectPoint(point, rect), CorrectPoint(new PointF(num12, point.Y), rect), j, num8, (!stiAxisArea.ReverseHor) ? num11 : (0.0 - num11), stiAxisArea.AxisCore.GetArgumentLabel(stiAxisArea.YAxis.Info.StripLines[index], stiClusteredBarSeries), stiClusteredBarSeries.Core.GetTag(j), num2, num, rect, animation);
								if (stiSeriesLabelsGeom != null)
								{
									list.Add(stiSeriesLabelsGeom);
									stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, clientRectangle, stiSeriesLabelsGeom.ClientRectangle);
								}
							}
						}
					}
				}
				num5 += num7;
				num2 = (stiAxisArea.ReverseVert ? (num2 - 1) : (num2 + 1));
				num6 = ((!stiAxisArea.ReverseVert) ? (num6 + 1) : (num6 - 1));
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
	}

	private RectangleF GetBarRect(StiContext context, IStiClusteredBarSeries currentSeries, double? value, float seriesTopPos, float seriesHeight)
	{
		IStiAxisArea stiAxisArea = currentSeries.Chart.Area as IStiAxisArea;
		float dividerX = stiAxisArea.AxisCore.GetDividerX();
		float dividerTopX = stiAxisArea.AxisCore.GetDividerTopX();
		float num = ((currentSeries.YAxis == StiSeriesYAxis.RightYAxis) ? dividerTopX : dividerX);
		double num2 = ((currentSeries.YAxis == StiSeriesYAxis.RightYAxis) ? stiAxisArea.XTopAxis.Info.Dpi : stiAxisArea.XAxis.Info.Dpi);
		if (stiAxisArea.ReverseHor && value.HasValue)
		{
			value = 0.0 - value;
		}
		double num3 = value.GetValueOrDefault() * num2;
		double num4 = 0.0 - num3 + (double)num;
		if (((IStiClusteredBarSeries)base.Series).ShowZeros && (value == 0.0 || !value.HasValue))
		{
			num3 = Math.Max(context.Options.Zoom, 2f);
			if (stiAxisArea.ReverseHor)
			{
				num4 -= num3;
			}
		}
		if (value < 0.0)
		{
			num4 = num;
			num3 = 0.0 - num3;
		}
		return new RectangleF((float)num4, seriesTopPos, (float)num3, seriesHeight);
	}

	public StiClusteredBarSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
