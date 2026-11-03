using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiLineSeriesCoreXF : StiBaseLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Line");

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		if (pointsInfo.Points != null && pointsInfo.Points.Length > 1)
		{
			StiLineSeriesGeom stiLineSeriesGeom = new StiLineSeriesGeom(geom, pointsInfo, base.Series);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(stiLineSeriesGeom);
			if (base.Interaction != null)
			{
				stiLineSeriesGeom.Interactions = GetInteractions(context, geom, pointsInfo.Points);
			}
		}
		RenderMarkers(context, geom, pointsInfo.Points);
	}

	public StiLineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
