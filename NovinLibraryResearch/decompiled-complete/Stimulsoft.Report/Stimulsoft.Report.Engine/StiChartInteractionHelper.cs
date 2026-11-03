using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Engine;

public static class StiChartInteractionHelper
{
	private static RectangleD cachedChartRect = RectangleD.Empty;

	private static StiChart cachedChart;

	private static StiCellGeom cachedChartGeom;

	public static void ProcessChart(StiChart masterChart, StiChart childChart)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		if (!StiOptions.Configuration.IsWeb || !StiOptions.Engine.AllowInteractionInChartWithComponents)
		{
			return;
		}
		bool flag = false;
		foreach (IStiSeries item3 in childChart.Series)
		{
			if ((item3.Hyperlinks != null && item3.Hyperlinks.Length != 0) || (item3.Tags != null && item3.Tags.Length != 0) || (item3.ToolTips != null && item3.ToolTips.Length != 0) || (item3.Core.Interaction != null && ((StiSeriesInteraction)item3.Core.Interaction).DrillDownEnabled))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		StiGdiContextPainter contextPainter = new StiGdiContextPainter(StiReport.GlobalMeasureGraphics);
		StiContext context = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, 1f);
		RectangleD val = masterChart.Report.Unit.ConvertToHInches(masterChart.ClientRectangle);
		StiCellGeom stiCellGeom = childChart.Core.Render(context, new RectangleF(0f, 0f, (float)((RectangleD)(ref val)).Width, (float)((RectangleD)(ref val)).Height), useMargins: true);
		List<StiCellGeom> seriesGeoms = stiCellGeom.GetSeriesGeoms();
		if (seriesGeoms.Count == 0)
		{
			return;
		}
		List<StiText> list = new List<StiText>();
		RectangleD rectD = default(RectangleD);
		RectangleD rectD2 = default(RectangleD);
		foreach (StiCellGeom item4 in seriesGeoms)
		{
			IStiSeries stiSeries2 = null;
			StiSeriesElementGeom stiSeriesElementGeom = item4 as StiSeriesElementGeom;
			if (stiSeriesElementGeom != null)
			{
				stiSeries2 = stiSeriesElementGeom.Series;
			}
			if (stiSeries2 != null && stiSeriesElementGeom.Interaction != null)
			{
				RectangleF rectangleF = stiCellGeom.GetRect(item4);
				if (stiSeriesElementGeom is StiPieSeriesElementGeom)
				{
					StiPieSeriesElementGeom stiPieSeriesElementGeom = stiSeriesElementGeom as StiPieSeriesElementGeom;
					float num = (float)(Math.PI * (double)stiPieSeriesElementGeom.EndAngle / 180.0);
					PointF pointF = new PointF(rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height / 2f);
					PointF pointF2 = new PointF(pointF.X + (float)Math.Cos(num) * stiPieSeriesElementGeom.Radius / 2f, pointF.Y + (float)Math.Sin(num) * stiPieSeriesElementGeom.Radius / 2f);
					rectangleF = new RectangleF(pointF2.X - stiPieSeriesElementGeom.Radius / 4f, pointF2.Y - stiPieSeriesElementGeom.Radius / 4f, stiPieSeriesElementGeom.Radius / 2f, stiPieSeriesElementGeom.Radius / 2f);
				}
				((RectangleD)(ref rectD))._002Ector((double)rectangleF.X, (double)rectangleF.Y, (double)rectangleF.Width, (double)rectangleF.Height);
				rectD = masterChart.Report.Unit.ConvertFromHInches(rectD);
				StiText item = CreateInteractionText(masterChart, childChart, stiSeries2.Core, stiSeriesElementGeom.Interaction, ref rectD);
				list.Add(item);
			}
			StiSeriesGeom stiSeriesGeom = item4 as StiSeriesGeom;
			if (stiSeriesGeom != null)
			{
				stiSeries2 = stiSeriesGeom.Series;
			}
			if (stiSeries2 == null || stiSeriesGeom == null || stiSeriesGeom.Interactions == null)
			{
				continue;
			}
			float val2 = stiSeriesGeom.Interactions[stiSeriesGeom.Interactions.Count - 1].Point.GetValueOrDefault().X - stiSeriesGeom.Interactions[0].Point.GetValueOrDefault().X;
			val2 = Math.Min(val2, 30f);
			PointF pointF3 = PointF.Empty;
			if (stiSeriesGeom.AreaGeom != null)
			{
				pointF3 = stiCellGeom.GetRect(stiSeriesGeom.AreaGeom).Location;
			}
			foreach (StiSeriesInteractionData interaction in stiSeriesGeom.Interactions)
			{
				((RectangleD)(ref rectD2))._002Ector((double)(pointF3.X + interaction.Point.GetValueOrDefault().X - val2 / 2f), (double)(pointF3.Y + interaction.Point.GetValueOrDefault().Y - val2 / 2f), (double)val2, (double)val2);
				rectD2 = masterChart.Report.Unit.ConvertFromHInches(rectD2);
				StiText item2 = CreateInteractionText(masterChart, childChart, stiSeries2.Core, interaction, ref rectD2);
				list.Add(item2);
			}
		}
		if (list.Count > 0)
		{
			childChart.ChartInfoV2.InteractiveComps = list;
		}
	}

	private static StiText CreateInteractionText(StiChart masterChart, StiChart childChart, StiSeriesCoreXF series, StiSeriesInteractionData interactionData, ref RectangleD rectD)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		StiText stiText = new StiText(rectD)
		{
			Name = childChart.Name + "Interaction#FX%",
			ToolTipValue = interactionData.Tooltip,
			HyperlinkValue = interactionData.Hyperlink,
			TagValue = interactionData.Tag
		};
		if (series.Interaction != null && ((StiSeriesInteraction)series.Interaction).DrillDownEnabled)
		{
			stiText.Page = masterChart.Page;
			stiText.Interaction = new StiInteraction
			{
				DrillDownEnabled = ((StiSeriesInteraction)series.Interaction).DrillDownEnabled,
				DrillDownPageGuid = ((StiSeriesInteraction)series.Interaction).DrillDownPageGuid,
				DrillDownReport = ((StiSeriesInteraction)series.Interaction).DrillDownReport
			};
			stiText.DrillDownParameters = new Dictionary<string, object>();
			stiText.DrillDownParameters.Add("Series", series);
			stiText.DrillDownParameters.Add("SeriesIndex", series.Series.Chart.Series.IndexOf(series.Series));
			stiText.DrillDownParameters.Add("SeriesArgument", interactionData.Argument);
			stiText.DrillDownParameters.Add("SeriesValue", interactionData.Value);
			stiText.DrillDownParameters.Add("SeriesEndValue", interactionData.EndValue);
			stiText.DrillDownParameters.Add("SeriesPointIndex", interactionData.PointIndex);
			stiText.DrillDownParameters.Add("SeriesTag", interactionData.Tag);
			stiText.DrillDownParameters.Add("SeriesHyperlink", interactionData.Hyperlink);
			stiText.DrillDownParameters.Add("SeriesTooltip", interactionData.Tooltip);
		}
		return stiText;
	}

	private static StiCellGeom GetGeomAt(StiChart chart, RectangleD chartRect, float x, float y, out StiCellGeom chartGeom, float zoom, Type typeNotUse = null)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		chartGeom = null;
		x -= (float)((RectangleD)(ref chartRect)).X;
		y -= (float)((RectangleD)(ref chartRect)).Y;
		if (cachedChart == chart && cachedChartGeom != null && ((object)(RectangleD)(ref cachedChartRect)).Equals((object?)chartRect) && typeNotUse == null)
		{
			chartGeom = cachedChartGeom;
		}
		else
		{
			StiGdiContextPainter contextPainter = new StiGdiContextPainter(StiReport.GlobalMeasureGraphics);
			StiContext context = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, zoom);
			chartGeom = chart.Core.Render(context, new RectangleF(0f, 0f, (float)((RectangleD)(ref chartRect)).Width, (float)((RectangleD)(ref chartRect)).Height), useMargins: true);
			cachedChart = chart;
			cachedChartGeom = chartGeom;
			cachedChartRect = chartRect;
		}
		return chartGeom.GetGeomAt(chartGeom, x, y, typeNotUse);
	}

	public static IStiGeomInteraction GetInteraction(StiChart chart, RectangleD chartRect, float x, float y, out StiCellGeom chartGeom, float zoom, Type typeNotUse = null)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetGeomAt(chart, chartRect, x, y, out chartGeom, zoom, typeNotUse);
	}

	public static void ResetChartGeomCache()
	{
		if (cachedChartGeom != null)
		{
			cachedChartGeom.Dispose();
			cachedChartGeom = null;
		}
		cachedChart = null;
	}
}
