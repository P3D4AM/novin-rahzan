using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF27 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "27";

	public override Color[] StyleColors => new Color[6]
	{
		StiColor.Get("#0bac45"),
		StiColor.Get("#585257"),
		StiColor.Get("#ec334d"),
		StiColor.Get("#a1ae94"),
		StiColor.Get("#ed7d31"),
		StiColor.Get("#5ab0ee")
	};

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush("#33475b");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("#33475b");

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

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle27;

	public override Color GetColumnBorder(Color color)
	{
		return Color.Transparent;
	}
}
