using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiClusteredBarAreaCoreXF : StiClusteredColumnAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "ClusteredBar");

	public override StiChartSeriesOrientation SeriesOrientation => StiChartSeriesOrientation.Horizontal;

	public override int Position => 10;

	public StiClusteredBarAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
