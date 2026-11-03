using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedSplineAreaSeriesCoreXF : StiStackedSplineAreaSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedSplineArea");

	public StiFullStackedSplineAreaSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
