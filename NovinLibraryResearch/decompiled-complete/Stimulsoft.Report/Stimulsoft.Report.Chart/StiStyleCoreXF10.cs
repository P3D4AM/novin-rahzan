using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF10 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "10";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("79B5B0"),
		StiColor.Get("B4BC4C"),
		StiColor.Get("B77851"),
		StiColor.Get("776A5B"),
		StiColor.Get("B6AD76"),
		StiColor.Get("95AEB1")
	};


	public override Color BasicStyleColor => StiColor.Get("D1E1E3");

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle10;
}
