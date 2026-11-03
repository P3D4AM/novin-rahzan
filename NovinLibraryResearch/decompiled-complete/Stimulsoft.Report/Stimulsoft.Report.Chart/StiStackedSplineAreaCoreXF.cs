using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineAreaCoreXF : StiStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedSpline");

	public override int Position => 41;

	public StiStackedSplineAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
