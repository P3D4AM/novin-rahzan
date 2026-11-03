using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedBarSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedBar");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiStackedBarSeries stiStackedBarSeries = base.Series as IStiStackedBarSeries;
		if (stiStackedBarSeries.AllowApplyStyle)
		{
			stiStackedBarSeries.Brush = style.Core.GetColumnBrush(color);
			StiBrush brush = stiStackedBarSeries.Brush;
			StiGradientBrush val = (StiGradientBrush)(object)((brush is StiGradientBrush) ? brush : null);
			if (val != null)
			{
				val.Angle += 90.0;
			}
			StiBrush brush2 = stiStackedBarSeries.Brush;
			StiGlareBrush val2 = (StiGlareBrush)(object)((brush2 is StiGlareBrush) ? brush2 : null);
			if (val2 != null)
			{
				val2.Angle += 90.0;
			}
			stiStackedBarSeries.BorderColor = style.Core.GetColumnBorder(color);
			stiStackedBarSeries.BorderThickness = style.Core.SeriesBorderThickness;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		List<StiStackedBarSeriesElementGeom> list2 = new List<StiStackedBarSeriesElementGeom>();
		IStiArea area = geom.Area;
		IStiAxisArea stiAxisArea = area as IStiAxisArea;
		IStiStackedBarSeries stiStackedBarSeries = base.Series as IStiStackedBarSeries;
		float dividerX = stiAxisArea.AxisCore.GetDividerX();
		float num = (float)stiAxisArea.XAxis.Info.Dpi;
		int valuesCount = stiAxisArea.AxisCore.ValuesCount;
		int?[,] array = new int?[stiAxisArea.AxisCore.ValuesCount, series.Length];
		int num2 = 0;
		for (int i = 0; i < series.Length; i++)
		{
			for (int j = 0; j < series[i].Values.Length; j++)
			{
				array[j, i] = num2;
				num2++;
			}
		}
		int colorCount = num2;
		for (int k = 0; k < valuesCount; k++)
		{
			double totalPositiveWidth = 0.0;
			double totalNegativeWidth = 0.0;
			CalculateTotalWidth(series, k, out totalPositiveWidth, out totalNegativeWidth);
			float num3 = stiAxisArea.YAxis.Info.StripPositions[stiAxisArea.YAxis.StartFromZero ? (k + 1) : k] - stiAxisArea.YAxis.Info.Step / 2f;
			float num4 = stiAxisArea.YAxis.Info.Step - stiAxisArea.YAxis.Info.Step * (1f - stiStackedBarSeries.Width);
			float num5 = num3 + (stiAxisArea.YAxis.Info.Step - num4) / 2f;
			float num6 = 0f;
			float num7 = 0f;
			if (series.Length == 0)
			{
				return;
			}
			if (((IStiStackedBarSeries)series[0]).ShowShadow)
			{
				for (int l = 0; l < series.Length; l++)
				{
					if (k < series[l].Values.Length)
					{
						IStiSeries stiSeries = series[l];
						double? num8 = (stiAxisArea.ReverseVert ? stiSeries.Values[k] : stiSeries.Values[stiSeries.Values.Length - k - 1]);
						if (stiAxisArea.ReverseHor && num8.HasValue)
						{
							num8 = 0.0 - num8;
						}
						if (num8 > 0.0)
						{
							num6 += (float)num8.GetValueOrDefault();
						}
						else
						{
							num7 += (float)num8.GetValueOrDefault();
						}
					}
				}
				if (base.Series is IStiFullStackedBarSeries)
				{
					double num9 = (stiAxisArea.ReverseHor ? totalNegativeWidth : totalPositiveWidth);
					double num10 = (stiAxisArea.ReverseHor ? totalPositiveWidth : totalNegativeWidth);
					if (num9 > 0.0 && num10 > 0.0)
					{
						StiStackedBarSeriesShadowElementGeom item = new StiStackedBarSeriesShadowElementGeom(clientRectangle: new RectangleF(-8f, num5, rect.Width + 8f, num4), series: series[0], isLeftShadow: false, isRightShadow: false);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item);
					}
					else if (num9 > 0.0)
					{
						StiStackedBarSeriesShadowElementGeom item2 = new StiStackedBarSeriesShadowElementGeom(clientRectangle: new RectangleF(stiAxisArea.AxisCore.GetDividerX() - 8f, num5, rect.Width - stiAxisArea.AxisCore.GetDividerX() + 8f, num4), series: series[0], isLeftShadow: false, isRightShadow: false);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item2);
					}
					else if (num10 > 0.0)
					{
						StiStackedBarSeriesShadowElementGeom item3 = new StiStackedBarSeriesShadowElementGeom(clientRectangle: new RectangleF(-8f, num5, stiAxisArea.AxisCore.GetDividerX() + 8f, num4), series: series[0], isLeftShadow: false, isRightShadow: false);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item3);
					}
				}
				else
				{
					if (num6 > 0f)
					{
						RectangleF clientRectangle4 = new RectangleF(dividerX, num5, num6 * num, num4);
						clientRectangle4.X -= 7f;
						clientRectangle4.Width += 7f;
						StiStackedBarSeriesShadowElementGeom item4 = new StiStackedBarSeriesShadowElementGeom(series[0], clientRectangle4, isLeftShadow: false, isRightShadow: true);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item4);
					}
					if (num7 < 0f)
					{
						StiStackedBarSeriesShadowElementGeom item5 = new StiStackedBarSeriesShadowElementGeom(clientRectangle: new RectangleF(dividerX + num7 * num, num5, (0f - num7) * num, num4), series: series[0], isLeftShadow: true, isRightShadow: false);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item5);
					}
				}
			}
			num6 = 0f;
			num7 = 0f;
			bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
			for (int m = 0; m < series.Length; m++)
			{
				IStiStackedBarSeries stiStackedBarSeries2 = series[m] as IStiStackedBarSeries;
				if (k >= stiStackedBarSeries2.Values.Length)
				{
					continue;
				}
				int valueOrDefault = array[k, m].GetValueOrDefault();
				double? num11 = (stiAxisArea.ReverseVert ? stiStackedBarSeries2.Values[k] : stiStackedBarSeries2.Values[stiStackedBarSeries2.Values.Length - k - 1]);
				double? num12 = ((!num11.HasValue) ? num11 : (0.0 - num11));
				double? num13 = 0.0;
				int num14 = (stiAxisArea.ReverseHor ? k : (stiStackedBarSeries2.Values.Length - k - 1));
				foreach (IStiSeries stiSeries2 in series)
				{
					if (stiSeries2.Values.Length > num14)
					{
						num13 += stiSeries2.Values[num14];
					}
				}
				if (base.Series is IStiFullStackedBarSeries && (totalPositiveWidth != 0.0 || totalNegativeWidth != 0.0) && num12.HasValue)
				{
					num = ((!(num12 > 0.0)) ? ((rect.Width - stiAxisArea.AxisCore.GetDividerX()) / (float)totalPositiveWidth) : (stiAxisArea.AxisCore.GetDividerX() / (float)totalNegativeWidth));
				}
				if (stiAxisArea.ReverseHor && num12.HasValue)
				{
					num12 = 0.0 - num12;
				}
				float num15 = (float)(num12.GetValueOrDefault() * (double)num);
				float num16 = 0f - num15 + dividerX;
				if (num12 > 0.0)
				{
					num16 -= num6;
				}
				else
				{
					num16 = num7 + dividerX;
					num15 = 0f - num15;
				}
				if (stiStackedBarSeries2.ShowZeros && (num12 == 0.0 || !num12.HasValue))
				{
					num15 = Math.Max(context.Options.Zoom, 1f);
					if (!stiAxisArea.ReverseHor)
					{
						num16 -= num15;
					}
				}
				RectangleF columnRect = new RectangleF(num16, num5, num15, num4);
				columnRect = CorrectRect(columnRect, rect);
				RectangleF columnRectStart = ((!(num12 < 0.0)) ? RectangleF.FromLTRB(columnRect.Right, columnRect.Top, columnRect.Right, columnRect.Bottom) : RectangleF.FromLTRB(columnRect.Left, columnRect.Top, columnRect.Left, columnRect.Bottom));
				RectangleF clientRectangle6 = ((StiAxisAreaGeom)geom).View.ClientRectangle;
				clientRectangle6.X = 0f;
				clientRectangle6.Y = 0f;
				RectangleF rectangleF = columnRect;
				rectangleF.X += geom.ClientRectangle.X;
				rectangleF.Y += geom.ClientRectangle.Y;
				if ((rectangleF.Bottom > clientRectangle6.Y && rectangleF.Y < clientRectangle6.Bottom) || ((IStiAxisArea)base.Series.Chart.Area).YAxis.Range.Auto)
				{
					StiBrush seriesBrush = stiStackedBarSeries2.Core.GetSeriesBrush(valueOrDefault, colorCount);
					if (stiStackedBarSeries2.AllowApplyBrushNegative && num12 > 0.0)
					{
						seriesBrush = stiStackedBarSeries2.BrushNegative;
					}
					seriesBrush = stiStackedBarSeries2.ProcessSeriesBrushes(valuesCount - 1 - k, seriesBrush);
					if (columnRect.Height != 0f && Math.Round((decimal)columnRect.Width, 2) > 0m && seriesBrush != null)
					{
						Color seriesBorderColor = (Color)stiStackedBarSeries2.Core.GetSeriesBorderColor(valueOrDefault, colorCount);
						StiStackedBarSeriesElementGeom stiStackedBarSeriesElementGeom = new StiStackedBarSeriesElementGeom(geom, num12.GetValueOrDefault(), k, seriesBrush, seriesBorderColor, stiStackedBarSeries2, columnRect, columnRectStart);
						if (stiStackedBarSeries2.Core.Interaction != null)
						{
							stiStackedBarSeriesElementGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, stiStackedBarSeries2, valuesCount - 1 - k);
						}
						list2.Add(stiStackedBarSeriesElementGeom);
					}
					IStiAxisSeriesLabels seriesLabels = stiStackedBarSeries2.Core.GetSeriesLabels();
					if (seriesLabels != null && seriesLabels.Visible && Math.Round((decimal)columnRect.Width, 2) > 0m)
					{
						PointF endPoint = new PointF(columnRect.X, columnRect.Y + columnRect.Height / 2f);
						PointF startPoint = new PointF(columnRect.Right, columnRect.Y + columnRect.Height / 2f);
						if (num12 < 0.0)
						{
							startPoint.X = columnRect.X;
							endPoint.X = columnRect.Right;
						}
						if (num12 < 0.0)
						{
							startPoint.X = columnRect.X;
							endPoint.X = columnRect.Right;
						}
						if (seriesLabels.Step == 0 || k % seriesLabels.Step == 0)
						{
							double? value = num12;
							if ((stiStackedBarSeries2 as StiSeries).IsTotalLabel)
							{
								value = (num11 = num13);
							}
							int num17 = (stiAxisArea.YAxis.StartFromZero ? (k + 1) : k);
							StiStripLineXF line = ((stiAxisArea.YAxis.Info.StripLines.Count > num17) ? stiAxisArea.YAxis.Info.StripLines[num17] : null);
							StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiStackedBarSeries2, context, endPoint, startPoint, k, value, num11, stiAxisArea.AxisCore.GetArgumentLabel(line, stiStackedBarSeries2), stiStackedBarSeries2.Core.GetTag(k), valueOrDefault, colorCount, rect);
							if (stiSeriesLabelsGeom != null)
							{
								list.Add(stiSeriesLabelsGeom);
							}
						}
					}
				}
				if (num12 > 0.0)
				{
					num6 += num15;
				}
				else
				{
					num7 += num15;
				}
			}
		}
		for (int num18 = list2.Count - 1; num18 >= 0; num18--)
		{
			StiStackedBarSeriesElementGeom item6 = list2[num18];
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item6);
		}
		foreach (StiSeriesLabelsGeom item7 in list)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item7);
			item7.ClientRectangle = CheckLabelsRect(item7.SeriesLabels, geom, item7.ClientRectangle);
		}
	}

	private void CalculateTotalWidth(IStiSeries[] series, int pointIndex, out double totalPositiveWidth, out double totalNegativeWidth)
	{
		totalPositiveWidth = 0.0;
		totalNegativeWidth = 0.0;
		if (!(base.Series is IStiFullStackedBarSeries))
		{
			return;
		}
		foreach (IStiSeries stiSeries in series)
		{
			if (pointIndex < stiSeries.Values.Length)
			{
				double? num = (((IStiAxisArea)base.Series.Chart.Area).ReverseVert ? stiSeries.Values[pointIndex] : stiSeries.Values[stiSeries.Values.Length - pointIndex - 1]);
				if (num > 0.0)
				{
					totalPositiveWidth += num.GetValueOrDefault();
				}
				else
				{
					totalNegativeWidth += Math.Abs(num.GetValueOrDefault());
				}
			}
		}
	}

	private RectangleF CorrectRect(RectangleF columnRect, RectangleF rect)
	{
		if (columnRect.X > rect.Width || columnRect.Right < 0f)
		{
			columnRect.Width = 0f;
			return columnRect;
		}
		if (columnRect.X < 0f)
		{
			float num = 0f - columnRect.X;
			columnRect.X += num;
			columnRect.Width -= num;
		}
		if (columnRect.Right > rect.Width)
		{
			float num2 = columnRect.Right - rect.Width;
			columnRect.Width -= num2;
		}
		return columnRect;
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		IStiStackedBarSeries stiStackedBarSeries = base.Series as IStiStackedBarSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return stiStackedBarSeries.Brush;
		}
		return seriesBrush;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		IStiStackedBarSeries stiStackedBarSeries = base.Series as IStiStackedBarSeries;
		object seriesBorderColor = base.GetSeriesBorderColor(colorIndex, colorCount);
		if (seriesBorderColor == null)
		{
			return stiStackedBarSeries.BorderColor;
		}
		return seriesBorderColor;
	}

	public StiStackedBarSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
