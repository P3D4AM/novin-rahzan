using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSunburstAreaCoreXF : StiAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Sunburst");

	public override int Position => 140;

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		StiSunburstAreaGeom stiSunburstAreaGeom = new StiSunburstAreaGeom(base.Area, rect);
		List<IStiSeries> series = GetSeries();
		if (series.Count > 0)
		{
			series[0].Core.RenderSeries(context, rect, stiSunburstAreaGeom, series.ToArray());
		}
		return stiSunburstAreaGeom;
	}

	protected override void PrepareInfo(RectangleF rect)
	{
	}

	public StiSunburstAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
