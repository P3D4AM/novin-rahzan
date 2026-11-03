using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiHistogramSeriesCoreXF : StiClusteredColumnSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Histogram");

	public StiHistogramSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
