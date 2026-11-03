using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF35 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "35";

	public override Color[] StyleColors => new Color[10]
	{
		StiColor.Get("#794d26"),
		StiColor.Get("#c7986a"),
		StiColor.Get("#c4b49a"),
		StiColor.Get("#894d29"),
		StiColor.Get("#422515"),
		StiColor.Get("#564438"),
		StiColor.Get("#876c57"),
		StiColor.Get("#d8814b"),
		StiColor.Get("#532525"),
		StiColor.Get("#59413f")
	};

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush("#fefefe");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("#fefefe");

	public override Color ChartAreaBorderColor => Color.Transparent;

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(StiColor.Get("#5a4941"));

	public override Color SeriesLabelsColor => StiColor.Get("#fefefe");

	public override Font SeriesLabelsFont => new Font("Arial", 10f);

	public override StiBrush LegendBrush => (StiBrush)new StiSolidBrush(Color.Transparent);

	public override Color LegendLabelsColor => StiColor.Get("#222222");

	public override Color LegendBorderColor => Color.Transparent;

	public override Color LegendTitleColor => Color.White;

	public override bool LegendShowShadow => false;

	public override Font LegendFont => new Font("Arial", 9f);

	public override bool SeriesLighting => false;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle35;

	public override Color GetColumnBorder(Color color)
	{
		return Color.Transparent;
	}
}
