using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedSplineSeriesCoreXF : StiStackedSplineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedSpline");

	public StiFullStackedSplineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
