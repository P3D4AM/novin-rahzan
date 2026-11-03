using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedLineSeriesCoreXF : StiStackedLineSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedLine");

	public StiFullStackedLineSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
