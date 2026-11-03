using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF29 : StiStyleCoreXF26
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "29";

	public override Color[] StyleColors => new Color[5]
	{
		StiColor.Get("#3498db"),
		StiColor.Get("#ef717a"),
		StiColor.Get("#6dcbb3"),
		StiColor.Get("#f28161"),
		StiColor.Get("#fccd1b")
	};

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle29;

	public override Color ChartAreaBorderColor => Color.Transparent;

	public override bool LegendShowShadow => false;

	public override Color LegendBorderColor => Color.Transparent;

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(119, 255, 255, 255));

	public override Color SeriesLabelsColor => StiColor.Get("#323a45");

	public override Font SeriesLabelsFont => new Font("Arial", 10f);

	public override bool SeriesLighting => false;

	public override bool SeriesShowShadow => false;
}
