using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiLineAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Line");

	public override int Position => 30;

	public StiLineAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
