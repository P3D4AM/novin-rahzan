using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiScatterLineSeriesCoreXF : StiScatterSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "ScatterLine");

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.Points != null)
		{
			StiLineSeriesGeom stiLineSeriesGeom = new StiLineSeriesGeom(geom, pointsInfo, base.Series);
			if (stiLineSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiLineSeriesGeom);
			}
			RenderMarkers(context, geom, pointsInfo.Points);
		}
	}

	public StiScatterLineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
