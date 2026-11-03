using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF05 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "05";

	public override Color[] StyleColors { get; } = new Color[8]
	{
		StiColor.Get("E0A93D"),
		StiColor.Get("B4B500"),
		StiColor.Get("0E980D"),
		StiColor.Get("AA4824"),
		StiColor.Get("85DD4D"),
		StiColor.Get("E6C745"),
		StiColor.Get("DB6434"),
		StiColor.Get("83BE2F")
	};


	public override Color BasicStyleColor => Color.LemonChiffon;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle05;
}
