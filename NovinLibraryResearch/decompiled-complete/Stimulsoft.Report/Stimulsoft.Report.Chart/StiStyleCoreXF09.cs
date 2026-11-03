using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF09 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "09";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("4F81BD"),
		StiColor.Get("C0504D"),
		StiColor.Get("9BBB59"),
		StiColor.Get("8064A2"),
		StiColor.Get("4BACC6"),
		StiColor.Get("F79646")
	};


	public override Color BasicStyleColor => StiColor.Get("EEEEECE1");

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle09;
}
