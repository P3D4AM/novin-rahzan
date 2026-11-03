using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSplineAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Spline");

	public override int Position => 40;

	public StiSplineAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
