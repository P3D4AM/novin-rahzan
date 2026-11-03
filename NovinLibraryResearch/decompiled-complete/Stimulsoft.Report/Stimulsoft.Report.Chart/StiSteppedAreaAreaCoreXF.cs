using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSteppedAreaAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "SteppedArea");

	public override int Position => 51;

	public StiSteppedAreaAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
