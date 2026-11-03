using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF20 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "20";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("#d9bb94"),
		StiColor.Get("#f5be77"),
		StiColor.Get("#e6bd7f"),
		StiColor.Get("#dcd9d4"),
		StiColor.Get("#ede5d8"),
		StiColor.Get("#92e9d4")
	};


	public override Color BasicStyleColor => Color.Bisque;

	public override Color AxisLineColor => StiColorUtils.Dark(BasicStyleColor, (byte)50);

	public override Color ChartAreaBorderColor => StiColorUtils.Dark(BasicStyleColor, (byte)50);

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle20;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(color);
	}
}
