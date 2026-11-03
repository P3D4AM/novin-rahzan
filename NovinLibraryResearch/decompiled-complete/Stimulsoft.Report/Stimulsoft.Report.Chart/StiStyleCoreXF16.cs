using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF16 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "16";

	public override Color[] StyleColors { get; } = new Color[4]
	{
		StiColor.Get("#ff8000"),
		Color.Black,
		Color.Silver,
		Color.Red
	};


	public override Color BasicStyleColor => Color.White;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle16;
}
