using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRangeBarSeriesCoreXF : StiClusteredColumnSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "RangeBar");

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		if (series == null || series.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		IStiRangeBarArea stiRangeBarArea = geom.Area as IStiRangeBarArea;
		float dividerX = stiRangeBarArea.AxisCore.GetDividerX();
		float dividerY = stiRangeBarArea.AxisCore.GetDividerY();
		for (int i = 0; i < series.Length; i++)
		{
			IStiRangeBarSeries stiRangeBarSeries = (IStiRangeBarSeries)series[i];
			int num = stiRangeBarSeries.Values.Length;
			if (stiRangeBarSeries.ValuesEnd.Length < num)
			{
				num = stiRangeBarSeries.ValuesEnd.Length;
			}
			int val = stiRangeBarSeries.Arguments.Length;
			int num2 = Math.Min(num, val);
			int num3 = (stiRangeBarArea.ReverseHor ? (num2 - 1) : 0);
			for (int j = 0; j < num2; j++)
			{
				double? num4 = stiRangeBarSeries.Values[j];
				double? num5 = stiRangeBarSeries.ValuesEnd[j];
				if (!num4.HasValue)
				{
					num4 = 0.0;
				}
				if (!num5.HasValue)
				{
					num5 = 0.0;
				}
				double num6 = Math.Abs(num4.Value - num5.Value) * stiRangeBarArea.YAxis.Info.Dpi;
				double num7 = dividerY;
				double num8 = Math.Max(num4.Value, num5.Value);
				num7 = ((!stiRangeBarArea.ReverseVert) ? (num7 - num8 * stiRangeBarArea.YAxis.Info.Dpi) : (num7 + Math.Min(num4.Value, num5.Value) * stiRangeBarArea.YAxis.Info.Dpi));
				int num9 = stiRangeBarArea.XAxis.Info.StripLines.Count - 1;
				foreach (StiStripLineXF stripLine in stiRangeBarArea.XAxis.Info.StripLines)
				{
					if (stiRangeBarSeries.Arguments[j].ToString() == stripLine.ValueObject?.ToString())
					{
						break;
					}
					num9--;
				}
				num9 = stiRangeBarArea.XAxis.Info.StripLines.Count - 1 - num9;
				float num10 = stiRangeBarArea.XAxis.Info.Step - stiRangeBarArea.XAxis.Info.Step * (1f - stiRangeBarSeries.Width);
				float num11 = dividerX - num10 / 2f;
				num11 += stiRangeBarArea.XAxis.Info.Step * (float)num9;
				RectangleF clientRectangle = new RectangleF(num11, (float)num7, num10, (float)num6);
				RectangleF rectFrom = RectangleF.FromLTRB(clientRectangle.Left, clientRectangle.Bottom, clientRectangle.Right, clientRectangle.Bottom);
				StiBrush seriesBrush = stiRangeBarSeries.Core.GetSeriesBrush(num3, num2);
				StiBrush brush = stiRangeBarSeries.ProcessSeriesBrushes(num3, seriesBrush);
				StiRangeBarElementGeom stiRangeBarElementGeom = new StiRangeBarElementGeom(geom, num8, j, stiRangeBarSeries, brush, clientRectangle, rectFrom);
				if (stiRangeBarSeries.Core.Interaction != null)
				{
					stiRangeBarElementGeom.Interaction = new StiSeriesInteractionData(stiRangeBarArea, stiRangeBarSeries, j);
				}
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiRangeBarElementGeom);
				IStiAxisSeriesLabels seriesLabels = stiRangeBarSeries.Core.GetSeriesLabels();
				if (seriesLabels != null && seriesLabels.Visible)
				{
					PointF pointF = new PointF(num11 + num10 / 2f, (float)num7);
					PointF pointF2 = new PointF(num11 + num10 / 2f, (float)num7 + (float)num6);
					if (seriesLabels.Step == 0 || j % seriesLabels.Step == 0)
					{
						if (num5 < num4)
						{
							double? num12 = num4;
							num4 = num5;
							num5 = num12;
						}
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiRangeBarSeries, context, pointF2, pointF2, j, num4, num4, stiRangeBarSeries.Arguments[j].ToString(), stiRangeBarSeries.Core.GetTag(j), 0, 1, rect);
						StiSeriesLabelsGeom stiSeriesLabelsGeom2 = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(stiRangeBarSeries, context, pointF, pointF, j, num5, num5, stiRangeBarSeries.Arguments[j].ToString(), stiRangeBarSeries.Core.GetTag(j), 0, 1, rect);
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
				num3 = (stiRangeBarArea.ReverseHor ? (num3 - 1) : (num3 + 1));
			}
		}
		foreach (StiSeriesLabelsGeom item in list)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
	}

	public StiRangeBarSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
