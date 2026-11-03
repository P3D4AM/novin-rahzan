using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRadarPointAreaCoreXF : StiRadarAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "RadarPoint");

	public override int Position => 82;

	public StiRadarPointAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
