using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedBarAreaCoreXF : StiStackedBarAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedBar");

	public override int Position => 12;

	public StiFullStackedBarAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
