using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedColumnAreaCoreXF : StiStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedColumn");

	public override int Position => 2;

	public StiFullStackedColumnAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
