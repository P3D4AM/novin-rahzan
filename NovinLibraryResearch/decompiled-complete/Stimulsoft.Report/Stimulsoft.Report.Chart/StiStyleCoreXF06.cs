using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF06 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "06";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("CEB966"),
		StiColor.Get("9CB084"),
		StiColor.Get("6BB1C9"),
		StiColor.Get("6585CF"),
		StiColor.Get("7E6BC9"),
		StiColor.Get("A379BB")
	};


	public override Color BasicStyleColor => StiColor.Get("CAC6CE");

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle06;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(color);
	}
}
