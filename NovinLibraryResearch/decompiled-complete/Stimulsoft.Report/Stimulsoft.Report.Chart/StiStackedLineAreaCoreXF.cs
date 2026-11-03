using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedLineAreaCoreXF : StiStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedLine");

	public override int Position => 32;

	public StiStackedLineAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
