using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF28 : StiStyleCoreXF26
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "28";

	public override Color[] StyleColors => new Color[5]
	{
		StiColor.Get("#165d9e"),
		StiColor.Get("#577eb6"),
		StiColor.Get("#569436"),
		StiColor.Get("#225056"),
		StiColor.Get("#d4dae0")
	};

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush("#0a325a");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("#0a325a");

	public override Color AxisTitleColor => StiColorUtils.Dark(BasicStyleColor, (byte)50);

	public override Color AxisLineColor => StiColorUtils.Dark(BasicStyleColor, (byte)50);

	public override Color AxisLabelsColor => StiColorUtils.Dark(BasicStyleColor, (byte)50);

	public override StiBrush LegendBrush => (StiBrush)new StiSolidBrush(Color.Transparent);

	public override Color LegendLabelsColor => Color.White;

	public override Color LegendBorderColor => Color.Transparent;

	public override Color LegendTitleColor => Color.White;

	public override bool LegendShowShadow => false;

	public override Font LegendFont => new Font("Arial", 9f);

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle28;
}
