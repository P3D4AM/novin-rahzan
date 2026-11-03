using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiRadarLineAreaCoreXF : StiRadarAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "RadarLine");

	public override int Position => 83;

	public StiRadarLineAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
