using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiParetoAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Pareto");

	public override int Position => 3;

	public StiParetoAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
