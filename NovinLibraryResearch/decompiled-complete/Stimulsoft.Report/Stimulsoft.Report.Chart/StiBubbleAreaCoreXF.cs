using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiBubbleAreaCoreXF : StiScatterAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Bubble");

	public override int Position => 81;

	public StiBubbleAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
