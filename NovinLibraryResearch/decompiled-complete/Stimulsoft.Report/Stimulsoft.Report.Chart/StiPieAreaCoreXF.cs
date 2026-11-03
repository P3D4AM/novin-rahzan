using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiPieAreaCoreXF : StiAreaCoreXF
{
	internal int ValuesCount;

	public override string LocalizedName => StiLocalization.Get("Chart", "Pie");

	public override int Position => 20;

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		PrepareInfo(rect);
		StiPieAreaGeom stiPieAreaGeom = new StiPieAreaGeom(base.Area, rect);
		List<IStiSeries> series = GetSeries();
		RenderSeries(context, rect, stiPieAreaGeom, series);
		return stiPieAreaGeom;
	}

	public void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, List<IStiSeries> seriesCollection)
	{
		List<List<IStiSeries>> list = new List<List<IStiSeries>>();
		Hashtable hashtable = new Hashtable();
		foreach (IStiSeries item in seriesCollection)
		{
			List<IStiSeries> list2 = hashtable[item.GetType()] as List<IStiSeries>;
			if (list2 == null)
			{
				list2 = new List<IStiSeries>();
				list.Add(list2);
				hashtable.Add(item.GetType(), list2);
			}
			list2.Add(item);
		}
		foreach (List<IStiSeries> item2 in list)
		{
			IStiSeries[] array = item2.ToArray();
			array[0].Core.RenderSeries(context, rect, geom, array);
		}
	}

	protected override void PrepareInfo(RectangleF rect)
	{
		ValuesCount = 0;
		List<IStiSeries> series = GetSeries();
		if (series.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < series.Count; i++)
		{
			double?[] values = series[i].Values;
			if (values != null)
			{
				ValuesCount = Math.Max(values.Length, ValuesCount);
			}
		}
	}

	public StiPieAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
