using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF34 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "34";

	public override Color[] StyleColors => new Color[7]
	{
		StiColor.Get("144b45"),
		StiColor.Get("ecb92f"),
		StiColor.Get("d66153"),
		StiColor.Get("25775b"),
		StiColor.Get("319491"),
		StiColor.Get("f7f7f7"),
		StiColor.Get("dd7c21")
	};

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush("3f745e");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("3f745e");

	public override Color ChartAreaBorderColor => Color.Transparent;

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(119, 255, 255, 255));

	public override Color SeriesLabelsColor => Color.White;

	public override Font SeriesLabelsFont => new Font("Arial", 10f);

	public override StiBrush LegendBrush => (StiBrush)new StiSolidBrush(Color.Transparent);

	public override Color LegendLabelsColor => Color.White;

	public override Color LegendBorderColor => Color.Transparent;

	public override Color LegendTitleColor => Color.White;

	public override bool LegendShowShadow => false;

	public override Font LegendFont => new Font("Arial", 9f);

	public override bool SeriesLighting => false;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle34;

	public override Color GetColumnBorder(Color color)
	{
		return Color.Transparent;
	}
}
