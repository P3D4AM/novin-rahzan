using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedLineAreaCoreXF : StiFullStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedLine");

	public override int Position => 33;

	public StiFullStackedLineAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
