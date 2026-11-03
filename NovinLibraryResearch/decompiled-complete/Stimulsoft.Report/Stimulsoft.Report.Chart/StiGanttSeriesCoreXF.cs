using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiGanttSeriesCoreXF : StiClusteredBarSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Gantt");

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiGanttArea stiGanttArea = geom.Area as IStiGanttArea;
		double dividerXD = stiGanttArea.AxisCore.GetDividerXD();
		float dividerY = stiGanttArea.AxisCore.GetDividerY();
		bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
		for (int i = 0; i < series.Length; i++)
		{
			IStiGanttSeries stiGanttSeries = (IStiGanttSeries)series[i];
			int num = stiGanttSeries.Values.Length;
			if (stiGanttSeries.ValuesEnd.Length < num)
			{
				num = stiGanttSeries.ValuesEnd.Length;
			}
			int num2 = stiGanttSeries.Arguments.Length;
			int num3 = Math.Min(num, num2);
			int num4 = num2 - 1;
			for (int j = 0; j < num3; j++)
			{
				double? num5 = stiGanttSeries.Values[j];
				double? num6 = stiGanttSeries.ValuesEnd[j];
				if (!num5.HasValue)
				{
					num5 = 0.0;
				}
				if (!num6.HasValue)
				{
					num6 = 0.0;
				}
				double num7 = Math.Abs(num5.Value - num6.Value) * stiGanttArea.XAxis.Info.Dpi;
				double num8 = dividerXD;
				double num9 = Math.Min(num5.Value, num6.Value);
				num8 = ((!stiGanttArea.ReverseHor) ? (num8 + num9 * stiGanttArea.XAxis.Info.Dpi) : (num8 - (num9 * stiGanttArea.XAxis.Info.Dpi + num7)));
				int num10 = stiGanttArea.YAxis.Info.StripLines.Count - 1;
				foreach (StiStripLineXF stripLine in stiGanttArea.YAxis.Info.StripLines)
				{
					if (stripLine.ValueObject != null && stiGanttSeries.Arguments[j].ToString() == stripLine.ValueObject.ToString())
					{
						break;
					}
					num10--;
				}
				if (stiGanttArea.ReverseVert)
				{
					num10 = stiGanttArea.YAxis.Info.StripLines.Count - 1 - num10;
				}
				float num11 = stiGanttArea.YAxis.Info.Step / 2f;
				float num12 = dividerY - num11 / 2f;
				num12 = ((!stiGanttArea.ReverseVert) ? (num12 - stiGanttArea.YAxis.Info.Step * (float)num10) : (num12 + stiGanttArea.YAxis.Info.Step * (float)num10));
				RectangleF clientRectangle = new RectangleF((float)num8, num12, (float)num7, num11);
				RectangleF rectFrom = RectangleF.FromLTRB(clientRectangle.Left, clientRectangle.Top, clientRectangle.Left, clientRectangle.Bottom);
				StiBrush seriesBrush = stiGanttSeries.Core.GetSeriesBrush(num4, num3);
				StiBrush brush = stiGanttSeries.ProcessSeriesBrushes(num4, seriesBrush);
				StiGanttSeriesElementGeom stiGanttSeriesElementGeom = new StiGanttSeriesElementGeom(geom, num9, j, stiGanttSeries, clientRectangle, rectFrom, brush);
				if (stiGanttSeries.Core.Interaction != null)
				{
					stiGanttSeriesElementGeom.Interaction = new StiSeriesInteractionData(stiGanttArea, stiGanttSeries, j);
				}
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiGanttSeriesElementGeom);
				IStiAxisSeriesLabels seriesLabels = stiGanttSeries.Core.GetSeriesLabels();
				if (seriesLabels != null && seriesLabels.Visible)
				{
					if (seriesLabels is StiValueAxisLabels)
					{
						PointF pointF = new PointF((float)num8, num12 + num11 / 2f);
						PointF pointF2 = new PointF((float)num8 + (float)num7, num12 + num11 / 2f);
						if (seriesLabels.Step == 0 || j % seriesLabels.Step == 0)
						{
							if (num6 < num5)
							{
								double? num13 = num5;
								num5 = num6;
								num6 = num13;
							}
							StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiGanttSeries, context, pointF2, pointF2, j, num6, num6, stiGanttSeries.Arguments[j].ToString(), stiGanttSeries.Core.GetTag(j), 0, 1, rect);
							StiSeriesLabelsGeom stiSeriesLabelsGeom2 = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiGanttSeries, context, pointF, pointF, j, num5, num5, stiGanttSeries.Arguments[j].ToString(), stiGanttSeries.Core.GetTag(j), 0, 1, rect);
							if (stiSeriesLabelsGeom2 != null)
							{
								list.Add(stiSeriesLabelsGeom2);
								stiSeriesLabelsGeom2.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom2.ClientRectangle);
							}
							if (stiSeriesLabelsGeom != null)
							{
								list.Add(stiSeriesLabelsGeom);
								stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom.ClientRectangle);
							}
						}
					}
					else if (!(seriesLabels is StiNoneLabels))
					{
						PointF startPoint;
						PointF endPoint;
						if (seriesLabels is StiCenterAxisLabels)
						{
							startPoint = new PointF((float)num8, num12 + num11 / 2f);
							endPoint = new PointF((float)num8 + (float)num7, num12 + num11 / 2f);
						}
						else
						{
							startPoint = new PointF((float)num8 + (float)num7 / 2f, num12);
							endPoint = new PointF((float)num8 + (float)num7 / 2f, num12);
						}
						double? labelValue = num9;
						if (seriesLabels.Step == 0 || j % seriesLabels.Step == 0)
						{
							StiSeriesLabelsGeom stiSeriesLabelsGeom3 = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiGanttSeries, context, endPoint, startPoint, j, num9, labelValue, stiGanttSeries.Arguments[j]?.ToString(), stiGanttSeries.Core.GetTag(j), 0, 1, rect);
							if (stiSeriesLabelsGeom3 != null)
							{
								list.Add(stiSeriesLabelsGeom3);
								stiSeriesLabelsGeom3.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom3.ClientRectangle);
							}
						}
					}
				}
				num4--;
			}
		}
		foreach (StiSeriesLabelsGeom item in list)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
	}

	public StiGanttSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
