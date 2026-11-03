using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF03 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "03";

	public override Color[] StyleColors { get; } = new Color[5]
	{
		StiColor.Get("963D3B"),
		StiColor.Get("B34A47"),
		StiColor.Get("C97E7D"),
		StiColor.Get("DDB6B5"),
		StiColor.Get("EEDDDD")
	};


	public override Color BasicStyleColor => Color.WhiteSmoke;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle03;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(color);
	}
}
