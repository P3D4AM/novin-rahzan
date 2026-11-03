using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSteppedLineAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SteppedLine");

	public override int Position => 31;

	public StiSteppedLineAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
