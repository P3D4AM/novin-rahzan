using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRadarAreaAreaCoreXF : StiRadarAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "RadarArea");

	public override int Position => 84;

	public StiRadarAreaAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
