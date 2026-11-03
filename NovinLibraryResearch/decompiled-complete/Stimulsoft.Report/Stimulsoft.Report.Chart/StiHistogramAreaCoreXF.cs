using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiHistogramAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Histogram");

	public override int Position => 5;

	public StiHistogramAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
