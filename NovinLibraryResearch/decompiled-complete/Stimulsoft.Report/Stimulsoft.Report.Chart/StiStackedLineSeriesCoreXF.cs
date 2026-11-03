using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedLineSeriesCoreXF : StiStackedBaseLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedLine");

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.Points != null)
		{
			StiStackedLineSeriesGeom stiStackedLineSeriesGeom = new StiStackedLineSeriesGeom(geom, pointsInfo, base.Series);
			if (stiStackedLineSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiStackedLineSeriesGeom);
			}
			RenderMarkers(context, geom, pointsInfo.Points);
		}
	}

	public StiStackedLineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
