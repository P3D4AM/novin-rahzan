using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiTreemapSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Treemap");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		base.ApplyStyle(style, color);
		IStiTreemapSeries stiTreemapSeries = base.Series as IStiTreemapSeries;
		if (stiTreemapSeries.AllowApplyStyle)
		{
			stiTreemapSeries.Brush = style.Core.GetColumnBrush(color);
			stiTreemapSeries.BorderColor = style.Core.GetColumnBorder(color);
			stiTreemapSeries.BorderThickness = style.Core.SeriesBorderThickness;
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		TimeSpan globalDurationElement = StiChartHelper.GlobalDurationElement;
		TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
		List<double> list = new List<double>();
		foreach (double? item in base.Series.Values.ToList())
		{
			list.Add(item.GetValueOrDefault());
		}
		List<double> data = ((StiTreemapAreaCoreXF)base.Series.Chart.Area.Core).NormalizeDataForArea(list, rect.Width * rect.Height);
		List<RectangleF> list2 = ((StiTreemapAreaCoreXF)base.Series.Chart.Area.Core).Squarify(data, new List<double?>(), rect, new List<RectangleF>());
		int num = 0;
		foreach (IStiSeries stiSeries in seriesArray)
		{
			if (stiSeries == base.Series)
			{
				break;
			}
			num += stiSeries.Values.Length;
		}
		int colorCount = seriesArray.Sum((IStiSeries x) => x.Values.Length);
		List<StiSeriesLabelsGeom> list3 = new List<StiSeriesLabelsGeom>();
		int num2 = -1;
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] == 0.0)
			{
				continue;
			}
			num2++;
			StiBrush seriesBrush = base.Series.Core.GetSeriesBrush(num, colorCount);
			seriesBrush = base.Series.ProcessSeriesBrushes(num, seriesBrush);
			Color seriesBorderColor = (Color)base.Series.Core.GetSeriesBorderColor(num, colorCount);
			if (seriesBorderColor.A == 0 && !base.Series.Chart.Area.ColorEach)
			{
				seriesBorderColor = Color.White;
			}
			TimeSpan beginTime = new TimeSpan(globalBeginTimeElement.Ticks / list2.Count * num2);
			StiOpacityAnimation animation = new StiOpacityAnimation(globalDurationElement, beginTime);
			StiTreemapSeriesElementGeom stiTreemapSeriesElementGeom = new StiTreemapSeriesElementGeom(geom, base.Series.Values[j].GetValueOrDefault(), num2, seriesBrush, seriesBorderColor, base.Series, list2[num2], animation);
			if (base.Series.Core.Interaction != null)
			{
				stiTreemapSeriesElementGeom.Interaction = new StiSeriesInteractionData(geom.Area, base.Series, num2);
			}
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(stiTreemapSeriesElementGeom);
			if (base.Series.Chart.SeriesLabels is StiCenterTreemapLabels { Visible: not false } stiCenterTreemapLabels)
			{
				StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiCenterTreemapLabelsCoreXF)stiCenterTreemapLabels.Core).RenderLabel(base.Series, context, num2, base.Series.Values[j].GetValueOrDefault(), GetArgumentText(base.Series, j), base.Series.Core.GetTag(j), num, colorCount, list2[num2]);
				if (stiSeriesLabelsGeom != null)
				{
					list3.Add(stiSeriesLabelsGeom);
				}
			}
			num++;
		}
		foreach (StiSeriesLabelsGeom item2 in list3)
		{
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item2);
		}
		if (geom.Area.Chart.SeriesLabels.PreventIntersection)
		{
			CheckIntersectionLabels(geom);
		}
	}

	private string GetArgumentText(IStiSeries series, int index)
	{
		if (series.Arguments.Length > index && series.Arguments[index] != null)
		{
			return series.Arguments[index].ToString();
		}
		return string.Empty;
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return ((IStiTreemapSeries)base.Series).Brush;
		}
		return seriesBrush;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		object seriesBorderColor = base.GetSeriesBorderColor(colorIndex, colorCount);
		if (seriesBorderColor == null)
		{
			return ((IStiTreemapSeries)base.Series).BorderColor;
		}
		return seriesBorderColor;
	}

	public StiTreemapSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
