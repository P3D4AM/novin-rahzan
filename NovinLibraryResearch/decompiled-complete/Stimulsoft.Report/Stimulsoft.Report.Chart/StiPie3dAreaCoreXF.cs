using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiPie3dAreaCoreXF : StiPieAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Pie") + " 3D";

	public override int Position => 200;

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		PrepareInfo(rect);
		StiPieAreaGeom stiPieAreaGeom = new StiPieAreaGeom(base.Area, rect);
		List<IStiSeries> series = GetSeries();
		RenderSeries(context, rect, stiPieAreaGeom, series);
		return stiPieAreaGeom;
	}

	public StiPie3dAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
