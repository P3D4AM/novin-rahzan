using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF13 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "13";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("979EA8"),
		StiColor.Get("BDAE89"),
		StiColor.Get("CDB005"),
		StiColor.Get("AAA6C2"),
		StiColor.Get("84BFD2"),
		StiColor.Get("8B9F73")
	};


	public override Color BasicStyleColor => StiColor.Get("D4D2D0");

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle13;
}
