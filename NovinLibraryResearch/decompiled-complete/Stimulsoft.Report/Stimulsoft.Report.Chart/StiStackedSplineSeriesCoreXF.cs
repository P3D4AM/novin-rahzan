using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineSeriesCoreXF : StiStackedBaseLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedSpline");

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.Points != null)
		{
			StiStackedSplineSeriesGeom item = new StiStackedSplineSeriesGeom(geom, pointsInfo, base.Series);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
			RenderMarkers(context, geom, pointsInfo.Points);
		}
	}

	public StiStackedSplineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
