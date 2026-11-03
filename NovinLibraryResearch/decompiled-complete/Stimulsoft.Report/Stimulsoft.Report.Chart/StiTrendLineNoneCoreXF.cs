using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineNoneCoreXF : StiTrendLineCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsNone");

	public StiTrendLineNoneCoreXF(IStiTrendLine trendLine)
		: base(trendLine)
	{
	}
}
