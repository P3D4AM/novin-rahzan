using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiAreaAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Area");

	public override int Position => 50;

	public StiAreaAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
