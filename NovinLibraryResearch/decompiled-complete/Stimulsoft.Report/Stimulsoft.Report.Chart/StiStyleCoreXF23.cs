using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF23 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "23";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("#5b9bd5"),
		StiColor.Get("#9f9f9f"),
		StiColor.Get("#4472c4"),
		StiColor.Get("#255e91"),
		StiColor.Get("#636363"),
		StiColor.Get("#264478")
	};


	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle23;
}
