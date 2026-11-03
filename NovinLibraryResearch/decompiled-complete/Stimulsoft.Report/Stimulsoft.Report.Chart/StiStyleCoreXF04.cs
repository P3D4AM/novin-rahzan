using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF04 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "04";

	public override Color BasicStyleColor => StiColor.Get("B5A6C8");

	public override Color[] StyleColors { get; } = new Color[4]
	{
		StiColor.Get("48395A"),
		StiColor.Get("6D5789"),
		StiColor.Get("927BAD"),
		StiColor.Get("B5A6C8")
	};


	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle04;
}
