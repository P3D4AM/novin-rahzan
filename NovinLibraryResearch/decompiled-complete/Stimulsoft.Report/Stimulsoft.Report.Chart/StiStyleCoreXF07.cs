using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF07 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "07";

	public override Color[] StyleColors { get; } = new Color[5]
	{
		StiColor.Get("D9FBA8"),
		StiColor.Get("C8B5E7"),
		StiColor.Get("9EE8FF"),
		StiColor.Get("FFBD86"),
		StiColor.Get("A3C3FE")
	};


	public override Color BasicStyleColor => Color.White;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle07;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		return (StiBrush)new StiGradientBrush(StiColorUtils.Dark(color, (byte)50), StiColorUtils.Light(color, (byte)50), -90.0);
	}
}
