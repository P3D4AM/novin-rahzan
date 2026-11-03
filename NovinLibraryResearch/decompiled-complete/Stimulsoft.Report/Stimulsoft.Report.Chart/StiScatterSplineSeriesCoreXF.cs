using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiScatterSplineSeriesCoreXF : StiScatterSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "ScatterSpline");

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.Points != null)
		{
			StiScatterSplineSeriesGeom stiScatterSplineSeriesGeom = new StiScatterSplineSeriesGeom(geom, pointsInfo, base.Series);
			if (stiScatterSplineSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiScatterSplineSeriesGeom);
			}
			RenderMarkers(context, geom, pointsInfo.Points);
		}
	}

	public StiScatterSplineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
