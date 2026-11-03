using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedColumnSeriesCoreXF : StiStackedColumnSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedColumn");

	public StiFullStackedColumnSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
