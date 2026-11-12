using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiChartAtom : StiElementAtom
{
	public StiChart Chart { get; set; }

	protected StiChartElement ChartElement => base.Element as StiChartElement;

	public override void Paint(Graphics g)
	{
		if (Chart.Series.Count == 0)
		{
			return;
		}
		CultureInfo culture = StiCultureHelper.Set(Chart?.Report);
		try
		{
			StiChartGdiPainter stiChartGdiPainter = new StiChartGdiPainter();
			stiChartGdiPainter.AnimationEngine = base.AnimationEngine;
			float num = (float)StiScale.Factor;
			if (Chart?.Report != null && Chart.Report.IsDesigning)
			{
				num *= (float)Chart.Report.Info.Zoom;
			}
			ProcessSelectedElementGeoms(num);
			stiChartGdiPainter.DrawChart(Chart, g, base.Location.ToRectangleF(), useMargins: true, useBackground: false, num);
		}
		finally
		{
			StiCultureHelper.Restore(culture);
		}
	}

	private void ProcessSelectedElementGeoms(float zoom)
	{
		List<StiDataFilterRule> userFilters = ChartElement.UserFilters;
		if (userFilters == null || userFilters.Count <= 0)
		{
			return;
		}
		StiCellGeom chartGeom = StiChartElementInteractionHelper.GetChartGeom(Chart, ChartElement, base.Location, zoom);
		List<StiSeriesElementGeom> seriesElementGeoms = StiChartElementInteractionHelper.GetSeriesElementGeoms(chartGeom.ChildGeoms);
		foreach (StiSeriesElementGeom item in seriesElementGeoms)
		{
			List<StiDataFilterRule> elementGeomFilter = StiChartElementFilterHelper.GetElementGeomFilter(ChartElement, item, isDrillDown: true);
			elementGeomFilter.ForEach(delegate(StiDataFilterRule f)
			{
				f.ElementKey = ChartElement.Key;
			});
			int num = 0;
			foreach (StiDataFilterRule item2 in elementGeomFilter)
			{
				foreach (StiDataFilterRule userFilter in ChartElement.UserFilters)
				{
					if (userFilter.GetStringRepresentation().Equals(item2.GetStringRepresentation()))
					{
						num++;
						break;
					}
				}
			}
			item.IsSelected = num == elementGeomFilter.Count;
		}
	}

	public override void Render()
	{
		Chart = StiChartElementBuilder.Create(ChartElement).Render(ChartElement, DataTable);
	}

	public StiChartAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}

	public StiChartAtom(IStiElement element, RectangleD location, StiDataTable dataTable, StiAnimationEngine animation)
		: base(element, location, dataTable, animation)
	{
	}
}
