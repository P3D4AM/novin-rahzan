using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF08 : StiStyleCoreXF03
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "08";

	public override Color[] StyleColors { get; } = new Color[5]
	{
		StiColor.Get("3C6494"),
		StiColor.Get("4978B1"),
		StiColor.Get("7E9BC8"),
		StiColor.Get("B6C3DC"),
		StiColor.Get("DDE3EE")
	};


	public override Color BasicStyleColor => StiColor.Get("EEB6C3DC");

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle08;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(color);
	}
}
