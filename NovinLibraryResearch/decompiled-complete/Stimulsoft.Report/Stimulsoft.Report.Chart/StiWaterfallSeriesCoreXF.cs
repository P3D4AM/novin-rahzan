using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiWaterfallSeriesCoreXF : StiClusteredColumnSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Waterfall");

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series != null && series.Length != 0 && base.Series.Chart != null)
		{
			RenderColumns(context, rect, geom, series);
		}
	}

	private float GetSumSeriesWidth(IStiAxisArea axisArea, IStiSeries[] series)
	{
		float num = 0f;
		foreach (IStiSeries stiSeries in series)
		{
			num += axisArea.XAxis.Info.Step / (float)series.Length * ((IStiWaterfallSeries)stiSeries).Width;
		}
		return num;
	}

	private float[] GetDividerYSeries(IStiAxisArea axisArea, IStiSeries[] series)
	{
		float dividerY = axisArea.AxisCore.GetDividerY();
		float[] array = new float[series.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = dividerY;
		}
		return array;
	}

	private void RenderColumns(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		IStiArea area = geom.Area;
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		IStiAxisArea stiAxisArea = area as IStiAxisArea;
		float[] dividerYSeries = GetDividerYSeries(stiAxisArea, series);
		int num = 0;
		int colorCount = stiAxisArea.AxisCore.ValuesCount * series.Length;
		float sumSeriesWidth = GetSumSeriesWidth(stiAxisArea, series);
		for (int i = 0; i < stiAxisArea.AxisCore.ValuesCount; i++)
		{
			float num2 = (stiAxisArea.XAxis.Core.GetStartFromZero() ? stiAxisArea.XAxis.Info.StripPositions[i + 1] : stiAxisArea.XAxis.Info.StripPositions[i]);
			num2 -= stiAxisArea.XAxis.Info.Step / 2f;
			float num3 = num2 + (stiAxisArea.XAxis.Info.Step - sumSeriesWidth) / 2f;
			int num4 = (stiAxisArea.ReverseHor ? (series.Length - 1) : 0);
			while ((num4 < series.Length && !stiAxisArea.ReverseHor) || (num4 >= 0 && stiAxisArea.ReverseHor))
			{
				IStiWaterfallSeries stiWaterfallSeries = series[num4] as IStiWaterfallSeries;
				float num5 = stiAxisArea.XAxis.Info.Step / (float)series.Length * stiWaterfallSeries.Width;
				if (i < stiWaterfallSeries.Values.Length)
				{
					double? num6 = (stiAxisArea.ReverseHor ? stiWaterfallSeries.Values[stiWaterfallSeries.Values.Length - i - 1] : stiWaterfallSeries.Values[i]);
					if (!num6.HasValue)
					{
						num5 = 0f;
					}
					if (stiAxisArea.ReverseVert && num6.HasValue)
					{
						num6 = 0.0 - num6;
					}
					bool flag = (stiAxisArea.ReverseHor ? (i == 0) : (i == stiWaterfallSeries.Values.Length - 1));
					float posY = ((flag && stiWaterfallSeries.Total.Visible) ? stiAxisArea.AxisCore.GetDividerY() : dividerYSeries[num4]);
					RectangleF rectangleF = ((flag && stiWaterfallSeries.Total.Visible) ? GetColumnRect(context, stiWaterfallSeries, num6, num3, num5, ref posY) : GetColumnRect(context, stiWaterfallSeries, num6, num3, num5, ref dividerYSeries[num4]));
					RectangleF columnRectStart = ((!(num6 < 0.0)) ? RectangleF.FromLTRB(rectangleF.Left, rectangleF.Bottom, rectangleF.Right, rectangleF.Bottom) : RectangleF.FromLTRB(rectangleF.Left, rectangleF.Top, rectangleF.Right, rectangleF.Top));
					RectangleF clientRectangle = ((StiAxisAreaGeom)geom).View.ClientRectangle;
					clientRectangle.X = 0f;
					clientRectangle.Y = 0f;
					RectangleF rectangleF2 = rectangleF;
					rectangleF2.X += geom.ClientRectangle.X;
					rectangleF2.Y += geom.ClientRectangle.Y;
					if ((rectangleF2.Right > clientRectangle.X && rectangleF2.X < clientRectangle.Right) || ((IStiAxisArea)base.Series.Chart.Area).XAxis.Range.Auto)
					{
						Color seriesBorderColor;
						StiBrush seriesBrush;
						if (area.ColorEach)
						{
							seriesBrush = stiWaterfallSeries.Core.GetSeriesBrush(num, colorCount);
							if (stiWaterfallSeries.AllowApplyBrushNegative && num6 < 0.0)
							{
								seriesBrush = stiWaterfallSeries.BrushNegative;
							}
							seriesBrush = stiWaterfallSeries.ProcessSeriesBrushes(i, seriesBrush);
							seriesBorderColor = (Color)stiWaterfallSeries.Core.GetSeriesBorderColor(num, colorCount);
						}
						else
						{
							seriesBrush = ((stiWaterfallSeries.ConnectorLine.Visible && (!flag || !stiWaterfallSeries.Total.Visible)) ? stiWaterfallSeries.Core.GetSeriesBrush(0, 3) : stiWaterfallSeries.Core.GetSeriesBrush(2, 3));
							if (num6 < 0.0)
							{
								seriesBrush = (stiWaterfallSeries.AllowApplyBrushNegative ? stiWaterfallSeries.BrushNegative : stiWaterfallSeries.Core.GetSeriesBrush(1, 3));
							}
							seriesBrush = stiWaterfallSeries.ProcessSeriesBrushes(i, seriesBrush);
							seriesBrush = StiBrush.Light(seriesBrush, (byte)(num4 * 10));
							seriesBorderColor = (Color)stiWaterfallSeries.Core.GetSeriesBorderColor(0, 3);
						}
						if (base.Series.Chart != null && base.Series.Chart.Style != null)
						{
							StiClusteredColumnSeriesElementGeom stiClusteredColumnSeriesElementGeom = null;
							if (rectangleF.Height > 0f)
							{
								stiClusteredColumnSeriesElementGeom = new StiClusteredColumnSeriesElementGeom(geom, num6.GetValueOrDefault(), i, seriesBrush, seriesBorderColor, stiWaterfallSeries, rectangleF, columnRectStart);
								if (stiWaterfallSeries.Core.Interaction != null)
								{
									stiClusteredColumnSeriesElementGeom.Interaction = new StiSeriesInteractionData(stiAxisArea, stiWaterfallSeries, i);
								}
							}
							if (stiWaterfallSeries.ConnectorLine.Visible && !flag)
							{
								StiPenGeom stiPenGeom = new StiPenGeom(stiWaterfallSeries.ConnectorLine.LineColor, stiWaterfallSeries.ConnectorLine.LineWidth);
								stiPenGeom.PenStyle = stiWaterfallSeries.ConnectorLine.LineStyle;
								float num7 = ((num6 > 0.0) ? rectangleF.Top : rectangleF.Bottom);
								float num8 = rectangleF.Right + stiAxisArea.XAxis.Info.Step - num5;
								StiWaterfallLineGeom item = new StiWaterfallLineGeom(new PointF(rectangleF.Right, num7), new PointF(num8, num7), stiPenGeom, new RectangleF(rectangleF.Right, num7, num8 - rectangleF.Right, num7), ((StiChart)stiWaterfallSeries.Chart).IsAnimation);
								geom.CreateChildGeoms();
								geom.ChildGeoms.Add(item);
							}
							if (stiClusteredColumnSeriesElementGeom != null)
							{
								geom.CreateChildGeoms();
								geom.ChildGeoms.Add(stiClusteredColumnSeriesElementGeom);
							}
						}
						IStiAxisSeriesLabels seriesLabels = stiWaterfallSeries.Core.GetSeriesLabels();
						if (seriesLabels != null && seriesLabels.Visible && num6.HasValue)
						{
							float num9 = ((flag && stiWaterfallSeries.Total.Visible) ? stiAxisArea.AxisCore.GetDividerY() : posY);
							PointF pointEnd = GetPointEnd(stiWaterfallSeries, num6, num3, num5, num9);
							double? labelValue = ((!stiAxisArea.ReverseVert) ? num6 : (0.0 - num6));
							StiAnimation animation = null;
							if (seriesLabels.Step == 0 || i % seriesLabels.Step == 0)
							{
								int index = (stiAxisArea.XAxis.StartFromZero ? (i + 1) : i);
								StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiWaterfallSeries, context, CorrectPoint(pointEnd, rect), CorrectPoint(new PointF(pointEnd.X, num9), rect), i, num6, labelValue, stiAxisArea.AxisCore.GetArgumentLabel(stiAxisArea.XAxis.Info.StripLines[index], stiWaterfallSeries), stiWaterfallSeries.Core.GetTag(i), num, colorCount, rect, animation);
								if (stiSeriesLabelsGeom != null)
								{
									list.Add(stiSeriesLabelsGeom);
									stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom.ClientRectangle);
								}
							}
						}
					}
				}
				num3 += num5;
				num++;
				num4 = ((!stiAxisArea.ReverseHor) ? (num4 + 1) : (num4 - 1));
			}
		}
		foreach (StiSeriesLabelsGeom item2 in list)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item2);
		}
		if (geom.Area.Chart.SeriesLabels.PreventIntersection)
		{
			CheckIntersectionLabels(geom);
		}
	}

	protected PointF GetPointEnd(IStiClusteredColumnSeries currentSeries, double? value, float seriesLeftPos, float seriesWidth, float posY)
	{
		IStiAxisArea stiAxisArea = currentSeries.Chart.Area as IStiAxisArea;
		float dividerRightY = stiAxisArea.AxisCore.GetDividerRightY();
		return (currentSeries.YAxis != 0) ? new PointF(seriesLeftPos + seriesWidth / 2f, 0f - (float)(value.GetValueOrDefault() * stiAxisArea.YRightAxis.Info.Dpi) + dividerRightY) : new PointF(seriesLeftPos + seriesWidth / 2f, 0f - (float)(value.GetValueOrDefault() * stiAxisArea.YAxis.Info.Dpi) + posY);
	}

	protected RectangleF GetColumnRect(StiContext context, IStiClusteredColumnSeries currentSeries, double? value, float seriesLeftPos, float seriesWidth, ref float posY)
	{
		IStiAxisArea stiAxisArea = currentSeries.Chart.Area as IStiAxisArea;
		float num = 0f;
		float num2 = 0f;
		float dividerRightY = stiAxisArea.AxisCore.GetDividerRightY();
		if (currentSeries.YAxis == StiSeriesYAxis.LeftYAxis)
		{
			if (!stiAxisArea.ReverseVert)
			{
				num = (float)(value.GetValueOrDefault() * stiAxisArea.YAxis.Info.Dpi);
				num2 = 0f - num + posY;
			}
			else
			{
				num = (float)(value.GetValueOrDefault() * stiAxisArea.YAxis.Info.Dpi);
				num2 = 0f - num + posY;
			}
		}
		else
		{
			num = (float)(value.GetValueOrDefault() * stiAxisArea.YRightAxis.Info.Dpi);
			num2 = 0f - num + dividerRightY;
		}
		if (currentSeries.ShowZeros && (value == 0.0 || !value.HasValue))
		{
			num = Math.Max(context.Options.Zoom, 2f);
			if (!stiAxisArea.ReverseVert)
			{
				num2 -= num;
			}
		}
		if (value < 0.0)
		{
			num2 = ((currentSeries.YAxis != 0) ? dividerRightY : posY);
			num = 0f - num;
		}
		posY = ((value > 0.0) ? num2 : (num2 + num));
		return new RectangleF(seriesLeftPos, num2, seriesWidth, num);
	}

	public StiWaterfallSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
