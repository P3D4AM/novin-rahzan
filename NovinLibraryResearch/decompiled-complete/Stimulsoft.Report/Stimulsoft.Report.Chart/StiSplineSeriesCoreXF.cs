using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSplineSeriesCoreXF : StiBaseLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Spline");

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.Points != null)
		{
			StiSplineSeriesGeom stiSplineSeriesGeom = new StiSplineSeriesGeom(geom, pointsInfo, base.Series);
			if (stiSplineSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiSplineSeriesGeom);
			}
			if (base.Interaction != null)
			{
				stiSplineSeriesGeom.Interactions = GetInteractions(context, geom, pointsInfo.Points);
			}
			RenderMarkers(context, geom, pointsInfo.Points);
		}
	}

	public StiSplineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
