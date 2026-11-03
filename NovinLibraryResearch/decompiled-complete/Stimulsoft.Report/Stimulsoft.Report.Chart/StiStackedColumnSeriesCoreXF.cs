using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedColumnSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedColumn");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiStackedColumnSeries stiStackedColumnSeries = base.Series as IStiStackedColumnSeries;
		if (stiStackedColumnSeries.AllowApplyStyle)
		{
			stiStackedColumnSeries.Brush = style.Core.GetColumnBrush(color);
			stiStackedColumnSeries.BorderColor = style.Core.GetColumnBorder(color);
			stiStackedColumnSeries.BorderThickness = style.Core.SeriesBorderThickness;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		List<StiStackedColumnSeriesElementGeom> list2 = new List<StiStackedColumnSeriesElementGeom>();
		IStiArea area = geom.Area;
		IStiAxisArea stiAxisArea = area as IStiAxisArea;
		IStiStackedColumnSeries stiStackedColumnSeries = base.Series as IStiStackedColumnSeries;
		bool startFromZero = stiAxisArea.XAxis.Core.GetStartFromZero();
		float dividerY = stiAxisArea.AxisCore.GetDividerY();
		float num = (float)stiAxisArea.YAxis.Info.Dpi;
		if (base.Series.YAxis == StiSeriesYAxis.RightYAxis)
		{
			num = (float)stiAxisArea.YRightAxis.Info.Dpi;
		}
		int?[,] array = new int?[stiAxisArea.AxisCore.ValuesCount, series.Length];
		int num2 = 0;
		bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
		for (int i = 0; i < series.Length; i++)
		{
			for (int j = 0; j < series[i].Values.Length; j++)
			{
				array[j, i] = num2;
				num2++;
			}
		}
		int colorCount = num2;
		for (int pointIndex = 0; pointIndex < stiAxisArea.AxisCore.ValuesCount; pointIndex++)
		{
			double totalPositiveHeight = 0.0;
			double totalNegativeHeight = 0.0;
			CalculateTotalHeight(series, pointIndex, out totalPositiveHeight, out totalNegativeHeight);
			float num3 = (startFromZero ? stiAxisArea.XAxis.Info.StripPositions[pointIndex + 1] : stiAxisArea.XAxis.Info.StripPositions[pointIndex]);
			num3 -= stiAxisArea.XAxis.Info.Step / 2f;
			float num4 = stiAxisArea.XAxis.Info.Step - stiAxisArea.XAxis.Info.Step * (1f - stiStackedColumnSeries.Width);
			float x = num3 + (stiAxisArea.XAxis.Info.Step - num4) / 2f;
			float num5 = 0f;
			float num6 = 0f;
			if (series.Length == 0)
			{
				return;
			}
			if (series[0].ShowShadow)
			{
				for (int k = 0; k < series.Length; k++)
				{
					if (pointIndex >= series[k].Values.Length)
					{
						continue;
					}
					IStiSeries stiSeries = series[k];
					double? num7 = (stiAxisArea.ReverseHor ? stiSeries.Values[stiSeries.Values.Length - pointIndex - 1] : stiSeries.Values[pointIndex]);
					if (num7.HasValue)
					{
						if (stiAxisArea.ReverseVert)
						{
							num7 = 0.0 - num7;
						}
						if (num7 > 0.0)
						{
							num5 += (float)num7.Value;
						}
						else
						{
							num6 += (float)num7.Value;
						}
					}
				}
				if (this is StiFullStackedColumnSeriesCoreXF)
				{
					double num8 = (stiAxisArea.ReverseVert ? totalNegativeHeight : totalPositiveHeight);
					double num9 = (stiAxisArea.ReverseVert ? totalPositiveHeight : totalNegativeHeight);
					if (num8 > 0.0 && num9 > 0.0)
					{
						StiStackedColumnSeriesShadowElementGeom item = new StiStackedColumnSeriesShadowElementGeom(clientRectangle: new RectangleF(x, -8f, num4, rect.Height + 8f), series: series[0], isTopShadow: false, isBottomShadow: false);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item);
					}
					else if (num8 > 0.0)
					{
						StiStackedColumnSeriesShadowElementGeom item2 = new StiStackedColumnSeriesShadowElementGeom(clientRectangle: new RectangleF(x, -8f, num4, stiAxisArea.AxisCore.GetDividerY() + 8f), series: series[0], isTopShadow: false, isBottomShadow: false);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item2);
					}
					else if (num9 > 0.0)
					{
						StiStackedColumnSeriesShadowElementGeom item3 = new StiStackedColumnSeriesShadowElementGeom(clientRectangle: new RectangleF(x, stiAxisArea.AxisCore.GetDividerY() - 8f, num4, rect.Height - stiAxisArea.AxisCore.GetDividerY() + 8f), series: series[0], isTopShadow: false, isBottomShadow: false);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item3);
					}
				}
				else
				{
					if (num5 > 0f)
					{
						StiStackedColumnSeriesShadowElementGeom item4 = new StiStackedColumnSeriesShadowElementGeom(clientRectangle: new RectangleF(x, (0f - num5) * num + dividerY, num4, num5 * num), series: series[0], isTopShadow: true, isBottomShadow: false);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item4);
					}
					if (num6 < 0f)
					{
						RectangleF clientRectangle5 = new RectangleF(x, dividerY, num4, (0f - num6) * num);
						clientRectangle5.Y -= 7f;
						clientRectangle5.Height += 7f;
						StiStackedColumnSeriesShadowElementGeom item5 = new StiStackedColumnSeriesShadowElementGeom(series[0], clientRectangle5, isTopShadow: false, isBottomShadow: true);
						geom.CreateChildGeoms();
						geom.ChildGeoms.Add(item5);
					}
				}
			}
			num5 = 0f;
			num6 = 0f;
			TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
			for (int l = 0; l < series.Length; l++)
			{
				IStiStackedColumnSeries currentSeries = series[l] as IStiStackedColumnSeries;
				if (pointIndex >= currentSeries.Values.Length)
				{
					continue;
				}
				int valueOrDefault = array[pointIndex, l].GetValueOrDefault();
				double? num10 = (stiAxisArea.ReverseHor ? currentSeries.Values[currentSeries.Values.Length - pointIndex - 1] : currentSeries.Values[pointIndex]);
				double? num11 = (stiAxisArea.ReverseHor ? series.Cast<StiSeries>().Sum((StiSeries s) => GetValueFromArray(s.Values, currentSeries.Values.Length - pointIndex - 1)) : series.Cast<StiSeries>().Sum((StiSeries s) => GetValueFromArray(s.Values, pointIndex)));
				if (base.Series is IStiFullStackedColumnSeries && (totalPositiveHeight != 0.0 || totalNegativeHeight != 0.0) && num10.HasValue)
				{
					num = ((!(num10 >= 0.0)) ? ((rect.Height - stiAxisArea.AxisCore.GetDividerY()) / (float)totalNegativeHeight) : (stiAxisArea.AxisCore.GetDividerY() / (float)totalPositiveHeight));
				}
				if (stiAxisArea.ReverseVert && num10.HasValue)
				{
					num10 = 0.0 - num10;
				}
				float num12 = (float)num10.GetValueOrDefault() * num;
				float num13 = 0f - num12 + dividerY;
				if (num10 > 0.0)
				{
					num13 -= num5;
				}
				else
				{
					num13 = num6 + dividerY;
					num12 = 0f - num12;
				}
				if (stiStackedColumnSeries.ShowZeros && (num10 == 0.0 || !num10.HasValue))
				{
					num12 = Math.Max(context.Options.Zoom, 1f);
					if (!stiAxisArea.ReverseVert)
					{
						num13 -= num12;
					}
				}
				RectangleF columnRect = new RectangleF(x, num13, num4, num12);
				columnRect = CorrectRect(columnRect, rect);
				RectangleF columnRectStart = ((!(num10 < 0.0)) ? RectangleF.FromLTRB(columnRect.Left, columnRect.Bottom, columnRect.Right, columnRect.Bottom) : RectangleF.FromLTRB(columnRect.Left, columnRect.Top, columnRect.Right, columnRect.Top));
				RectangleF clientRectangle6 = ((StiAxisAreaGeom)geom).View.ClientRectangle;
				clientRectangle6.X = 0f;
				clientRectangle6.Y = 0f;
				RectangleF rectangleF = columnRect;
				rectangleF.X += geom.ClientRectangle.X;
				rectangleF.Y += geom.ClientRectangle.Y;
				if ((rectangleF.Right > clientRectangle6.X && rectangleF.X < clientRectangle6.Right) || ((IStiAxisArea)base.Series.Chart.Area).XAxis.Range.Auto)
				{
					StiBrush seriesBrush = currentSeries.Core.GetSeriesBrush(valueOrDefault, colorCount);
					if (currentSeries.AllowApplyBrushNegative && num10 < 0.0)
					{
						seriesBrush = currentSeries.BrushNegative;
					}
					seriesBrush = currentSeries.ProcessSeriesBrushes(pointIndex, seriesBrush);
					if (columnRect.Width != 0f && Math.Round((decimal)columnRect.Height, 2) > 0m && seriesBrush != null)
					{
						Color seriesBorderColor = (Color)currentSeries.Core.GetSeriesBorderColor(valueOrDefault, colorCount);
						StiStackedColumnSeriesElementGeom stiStackedColumnSeriesElementGeom = new StiStackedColumnSeriesElementGeom(geom, num10.GetValueOrDefault(), pointIndex, seriesBrush, seriesBorderColor, currentSeries, columnRect, columnRectStart);
						if (currentSeries.Core.Interaction != null)
						{
							stiStackedColumnSeriesElementGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, currentSeries, pointIndex);
						}
						list2.Add(stiStackedColumnSeriesElementGeom);
					}
					IStiAxisSeriesLabels seriesLabels = currentSeries.Core.GetSeriesLabels();
					if (seriesLabels != null && seriesLabels.Visible && (Math.Round((decimal)columnRect.Height, 2) > 0m || seriesLabels.ShowNulls))
					{
						PointF endPoint = new PointF(columnRect.X + columnRect.Width / 2f, columnRect.Y);
						PointF startPoint = new PointF(columnRect.X + columnRect.Width / 2f, columnRect.Bottom);
						double? labelValue = (stiAxisArea.ReverseHor ? currentSeries.Values[currentSeries.Values.Length - pointIndex - 1] : currentSeries.Values[pointIndex]);
						if (stiAxisArea.ReverseVert && labelValue.HasValue)
						{
							labelValue = 0.0 - num10;
						}
						if (num10 < 0.0)
						{
							startPoint.Y = columnRect.Y;
							endPoint.Y = columnRect.Bottom;
						}
						if (seriesLabels.Step == 0 || pointIndex % seriesLabels.Step == 0)
						{
							double? value = num10;
							if ((currentSeries as StiSeries).IsTotalLabel)
							{
								value = (labelValue = num11);
							}
							int index = (stiAxisArea.XAxis.StartFromZero ? (pointIndex + 1) : pointIndex);
							StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(currentSeries, context, endPoint, startPoint, pointIndex, value, labelValue, stiAxisArea.AxisCore.GetArgumentLabel(stiAxisArea.XAxis.Info.StripLines[index], currentSeries), currentSeries.Core.GetTag(pointIndex), valueOrDefault, colorCount, rect);
							if (stiSeriesLabelsGeom != null)
							{
								list.Add(stiSeriesLabelsGeom);
							}
						}
					}
				}
				if (num10 > 0.0)
				{
					num5 += num12;
				}
				else
				{
					num6 += num12;
				}
			}
		}
		for (int num14 = list2.Count - 1; num14 >= 0; num14--)
		{
			StiStackedColumnSeriesElementGeom item6 = list2[num14];
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

	private static double? GetValueFromArray(double?[] values, int pointIndex)
	{
		if (values == null || pointIndex < 0 || pointIndex >= values.Length)
		{
			return 0.0;
		}
		return values[pointIndex];
	}

	private void CalculateTotalHeight(IStiSeries[] series, int pointIndex, out double totalPositiveHeight, out double totalNegativeHeight)
	{
		totalPositiveHeight = 0.0;
		totalNegativeHeight = 0.0;
		if (!(base.Series is IStiFullStackedColumnSeries))
		{
			return;
		}
		foreach (IStiSeries stiSeries in series)
		{
			if (pointIndex < stiSeries.Values.Length)
			{
				double? num = (((IStiAxisArea)base.Series.Chart.Area).ReverseHor ? stiSeries.Values[stiSeries.Values.Length - pointIndex - 1] : stiSeries.Values[pointIndex]);
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

	private RectangleF CorrectRect(RectangleF columnRect, RectangleF rect)
	{
		if (columnRect.Y > rect.Height || columnRect.Bottom < 0f)
		{
			columnRect.Height = 0f;
			return columnRect;
		}
		if (columnRect.Top < 0f)
		{
			float num = 0f - columnRect.Top;
			columnRect.Y += num;
			columnRect.Height -= num;
		}
		if (columnRect.Bottom > rect.Height)
		{
			float num2 = columnRect.Bottom - rect.Height;
			columnRect.Height -= num2;
		}
		return columnRect;
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		IStiStackedColumnSeries stiStackedColumnSeries = base.Series as IStiStackedColumnSeries;
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return stiStackedColumnSeries.Brush;
		}
		return seriesBrush;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		IStiStackedColumnSeries stiStackedColumnSeries = base.Series as IStiStackedColumnSeries;
		object seriesBorderColor = base.GetSeriesBorderColor(colorIndex, colorCount);
		if (seriesBorderColor == null)
		{
			return stiStackedColumnSeries.BorderColor;
		}
		return seriesBorderColor;
	}

	public StiStackedColumnSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
