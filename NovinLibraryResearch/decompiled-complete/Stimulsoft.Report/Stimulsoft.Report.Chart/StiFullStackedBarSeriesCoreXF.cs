using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedBarSeriesCoreXF : StiStackedBarSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedBar");

	public StiFullStackedBarSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
