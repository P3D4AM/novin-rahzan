using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF24 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "24";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("#ed7d31"),
		StiColor.Get("#ffc000"),
		StiColor.Get("#70ad47"),
		StiColor.Get("#9e480e"),
		StiColor.Get("#997300"),
		StiColor.Get("#43682b")
	};


	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle24;

	public override Color ChartAreaBorderColor => Color.Transparent;

	public override Color LegendBorderColor => Color.Transparent;

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(119, 255, 255, 255));
}
