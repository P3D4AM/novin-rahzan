using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedSplineAreaAreaCoreXF : StiStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedSplineArea");

	public override int Position => 61;

	public StiStackedSplineAreaAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
