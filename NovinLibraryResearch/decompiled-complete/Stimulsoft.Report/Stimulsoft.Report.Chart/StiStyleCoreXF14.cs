using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF14 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "14";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("#f0a22e"),
		StiColor.Get("#a5644e"),
		StiColor.Get("#b58b80"),
		StiColor.Get("#c3986d"),
		StiColor.Get("#a19574"),
		StiColor.Get("#c17529")
	};


	public override Color BasicStyleColor => StiColor.Get("#fbeec9");

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle14;
}
