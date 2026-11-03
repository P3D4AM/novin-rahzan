using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiDoughnutAreaCoreXF : StiPieAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Doughnut");

	public override int Position => 21;

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		PrepareInfo(rect);
		StiDoughnutAreaGeom stiDoughnutAreaGeom = new StiDoughnutAreaGeom(base.Area, rect);
		List<IStiSeries> series = GetSeries();
		RenderSeries(context, rect, stiDoughnutAreaGeom, series);
		return stiDoughnutAreaGeom;
	}

	public StiDoughnutAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
