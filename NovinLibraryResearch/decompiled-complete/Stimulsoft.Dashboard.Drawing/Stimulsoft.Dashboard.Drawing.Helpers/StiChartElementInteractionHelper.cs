using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Drawing.Helpers;

public static class StiChartElementInteractionHelper
{
	private static RectangleD cachedChartRect = RectangleD.Empty;

	private static StiChartElement cachedChart;

	private static StiCellGeom cachedChartGeom;

	public static List<StiSeriesElementGeom> GetSeriesElementGeomsIsSelected(StiChart chart, StiChartElement chartElement, RectangleD chartRect, float zoom)
	{
		StiCellGeom chartGeom = GetChartGeom(chart, chartElement, chartRect, zoom);
		return GetSeriesElementGeomsIsSelected(chartGeom.ChildGeoms);
	}

	private static List<StiSeriesElementGeom> GetSeriesElementGeomsIsSelected(List<StiCellGeom> geoms)
	{
		List<StiSeriesElementGeom> list = new List<StiSeriesElementGeom>();
		if (geoms == null)
		{
			return list;
		}
		foreach (StiCellGeom geom in geoms)
		{
			if (geom is StiSeriesElementGeom { IsSelected: not false } stiSeriesElementGeom)
			{
				list.Add(stiSeriesElementGeom);
			}
			List<StiSeriesElementGeom> seriesElementGeomsIsSelected = GetSeriesElementGeomsIsSelected(geom.ChildGeoms);
			list.AddRange(seriesElementGeomsIsSelected);
		}
		return list;
	}

	internal static List<StiSeriesElementGeom> GetSeriesElementGeoms(List<StiCellGeom> geoms)
	{
		List<StiSeriesElementGeom> list = new List<StiSeriesElementGeom>();
		if (geoms == null)
		{
			return list;
		}
		foreach (StiCellGeom geom in geoms)
		{
			if (geom is StiSeriesElementGeom item)
			{
				list.Add(item);
			}
			List<StiSeriesElementGeom> seriesElementGeoms = GetSeriesElementGeoms(geom.ChildGeoms);
			list.AddRange(seriesElementGeoms);
		}
		return list;
	}

	public static void ResetSeriesElementGeomsSelection(StiChart chart, StiChartElement chartElement, RectangleD chartRect, float zoom)
	{
		StiCellGeom chartGeom = GetChartGeom(chart, chartElement, chartRect, zoom);
		ResetSeriesElementGeomsSelection(chartGeom.ChildGeoms);
	}

	private static void ResetSeriesElementGeomsSelection(List<StiCellGeom> geoms)
	{
		if (geoms == null)
		{
			return;
		}
		foreach (StiCellGeom geom in geoms)
		{
			if (geom is StiSeriesElementGeom stiSeriesElementGeom)
			{
				stiSeriesElementGeom.IsSelected = false;
			}
			ResetSeriesElementGeomsSelection(geom.ChildGeoms);
		}
	}

	private static StiCellGeom GetGeomAt(StiChart chart, StiChartElement chartElement, RectangleD chartRect, float x, float y, out StiCellGeom chartGeom, float zoom, Type typeNotUse = null)
	{
		chartGeom = GetChartGeom(chart, chartElement, chartRect, zoom, typeNotUse);
		x -= (float)chartRect.X;
		y -= (float)chartRect.Y;
		return chartGeom.GetGeomAt(chartGeom, x, y, typeNotUse);
	}

	internal static StiCellGeom GetChartGeom(StiChart chart, StiChartElement chartElement, RectangleD chartRect, float zoom, Type typeNotUse = null)
	{
		StiCellGeom result;
		if (cachedChart == chartElement && cachedChartGeom != null && cachedChartRect.Equals(chartRect) && typeNotUse == null)
		{
			result = cachedChartGeom;
		}
		else
		{
			StiGdiContextPainter contextPainter = new StiGdiContextPainter(StiReport.GlobalMeasureGraphics);
			StiContext context = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, zoom);
			result = chart.Core.Render(context, new RectangleF(0f, 0f, (float)chartRect.Width, (float)chartRect.Height), useMargins: true);
			cachedChart = chartElement;
			cachedChartGeom = result;
			cachedChartRect = chartRect;
		}
		return result;
	}

	public static IStiGeomInteraction GetInteraction(StiChart chart, StiChartElement chartElement, RectangleD chartRect, float x, float y, out StiCellGeom chartGeom, float zoom, Type typeNotUse = null)
	{
		return GetGeomAt(chart, chartElement, chartRect, x, y, out chartGeom, zoom, typeNotUse);
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
