using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF31 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "31";

	public override Color[] StyleColors => new Color[5]
	{
		StiColor.Get("#fefef9"),
		StiColor.Get("#a8d7e2"),
		StiColor.Get("#5ea8bf"),
		StiColor.Get("#2b7f9e"),
		StiColor.Get("#1c4458")
	};

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush("#235e6d");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("#235e6d");

	public override Color ChartAreaBorderColor => Color.Transparent;

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(119, 255, 255, 255));

	public override Font SeriesLabelsFont => new Font("Arial", 10f);

	public override StiBrush LegendBrush => (StiBrush)new StiSolidBrush(Color.Transparent);

	public override Color LegendLabelsColor => Color.White;

	public override Color LegendBorderColor => Color.Transparent;

	public override Color LegendTitleColor => Color.White;

	public override bool LegendShowShadow => false;

	public override Font LegendFont => new Font("Arial", 9f);

	public override bool SeriesLighting => false;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle31;

	public override Color GetColumnBorder(Color color)
	{
		return Color.Transparent;
	}
}
