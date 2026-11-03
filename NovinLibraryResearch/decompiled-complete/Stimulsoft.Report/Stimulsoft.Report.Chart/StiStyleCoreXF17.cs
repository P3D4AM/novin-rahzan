using System.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF17 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "17";

	public override Color[] StyleColors { get; } = new Color[2]
	{
		Color.White,
		Color.Black
	};


	public override Color BasicStyleColor => Color.White;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle17;
}
