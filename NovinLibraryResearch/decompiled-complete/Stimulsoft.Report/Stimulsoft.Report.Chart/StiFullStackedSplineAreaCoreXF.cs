using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedSplineAreaCoreXF : StiFullStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedSpline");

	public override int Position => 42;

	public StiFullStackedSplineAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
