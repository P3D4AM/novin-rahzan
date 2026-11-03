using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedAreaAreaCoreXF : StiFullStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedArea");

	public override int Position => 53;

	public StiFullStackedAreaAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
