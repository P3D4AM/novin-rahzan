using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF25 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "25";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("#70ad47"),
		StiColor.Get("#4472c4"),
		StiColor.Get("#ffc000"),
		StiColor.Get("#43682b"),
		StiColor.Get("#fd6a37"),
		StiColor.Get("#997300")
	};


	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle25;

	public override Color ChartAreaBorderColor => Color.Transparent;

	public override bool LegendShowShadow => false;

	public override Color LegendBorderColor => Color.Transparent;

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(119, 255, 255, 255));

	public override Color SeriesLabelsColor => StiColor.Get("#33475B");

	public override Font SeriesLabelsFont => new Font("Arial", 10f);

	public override bool SeriesLighting => false;

	public override bool SeriesShowShadow => false;
}
