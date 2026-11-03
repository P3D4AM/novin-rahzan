using System.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF12 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "12";

	public override Color[] StyleColors { get; } = new Color[2]
	{
		Color.Goldenrod,
		Color.Khaki
	};


	public override Color BasicStyleColor => Color.Moccasin;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle12;
}
