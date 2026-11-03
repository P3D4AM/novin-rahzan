using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF11 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "11";

	public override Color[] StyleColors { get; } = new Color[4]
	{
		StiColor.Get("CC4D5C"),
		StiColor.Get("FF6F61"),
		StiColor.Get("FF953E"),
		StiColor.Get("F8BD52")
	};


	public override Color BasicStyleColor => StiColorUtils.Dark(Color.NavajoWhite, (byte)30);

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle11;
}
