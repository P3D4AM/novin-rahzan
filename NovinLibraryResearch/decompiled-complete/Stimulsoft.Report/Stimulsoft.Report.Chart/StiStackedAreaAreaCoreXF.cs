using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedAreaAreaCoreXF : StiStackedColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedArea");

	public override int Position => 52;

	public StiStackedAreaAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
