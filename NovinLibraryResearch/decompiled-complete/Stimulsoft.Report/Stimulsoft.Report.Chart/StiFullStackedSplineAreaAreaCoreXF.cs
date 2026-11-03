using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedSplineAreaAreaCoreXF : StiFullStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedSplineArea");

	public override int Position => 62;

	public StiFullStackedSplineAreaAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
