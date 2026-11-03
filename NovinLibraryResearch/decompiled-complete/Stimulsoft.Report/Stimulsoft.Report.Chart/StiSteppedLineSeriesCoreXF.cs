using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSteppedLineSeriesCoreXF : StiBaseLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SteppedLine");

	protected override void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo)
	{
		IStiSteppedLineSeries stiSteppedLineSeries = base.Series as IStiSteppedLineSeries;
		if (pointsInfo.Points.Length > 1 || (stiSteppedLineSeries.PointAtCenter && pointsInfo.Points.Length != 0))
		{
			StiSteppedLineSeriesGeom stiSteppedLineSeriesGeom = new StiSteppedLineSeriesGeom(geom, pointsInfo, base.Series);
			if (stiSteppedLineSeriesGeom != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(stiSteppedLineSeriesGeom);
			}
			if (base.Interaction != null)
			{
				stiSteppedLineSeriesGeom.Interactions = GetInteractions(context, geom, pointsInfo.Points);
			}
		}
		RenderMarkers(context, geom, pointsInfo.Points);
	}

	public StiSteppedLineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
