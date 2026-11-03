using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiBoxAndWhiskerAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "BoxAndWhisker");

	public override int Position => 121;

	public StiBoxAndWhiskerAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
