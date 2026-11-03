using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiFullStackedAreaSeriesCoreXF : StiStackedAreaSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "FullStackedArea");

	public StiFullStackedAreaSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
