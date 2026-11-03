using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSplineAreaAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SplineArea");

	public override int Position => 60;

	public StiSplineAreaAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
