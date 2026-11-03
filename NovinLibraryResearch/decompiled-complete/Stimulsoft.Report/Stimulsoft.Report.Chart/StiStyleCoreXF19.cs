using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF19 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "19";

	public override Color[] StyleColors { get; } = new Color[5]
	{
		StiColor.Get("#f2eadd"),
		StiColor.Get("#f0ede8"),
		StiColor.Get("#e8ddcb"),
		StiColor.Get("#e1c9ad"),
		StiColor.Get("#d8b48c")
	};


	public override Color BasicStyleColor => StiColor.Get("#f2eadd");

	public override StiBrush InterlacingHorBrush => (StiBrush)new StiSolidBrush("#f0ede8");

	public override StiBrush InterlacingVertBrush => (StiBrush)new StiSolidBrush("#f0ede8");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("#f0ede8");

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush(Color.White);

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle19;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(color);
	}
}
