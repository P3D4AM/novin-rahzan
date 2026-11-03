using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF30 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "30";

	public override Color[] StyleColors => new Color[4]
	{
		StiColor.Get("db7b46"),
		StiColor.Get("d3d3d5"),
		StiColor.Get("6b6e75"),
		StiColor.Get("4a4c55")
	};

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush("595b65");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("595b65");

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

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle30;

	public override Color GetColumnBorder(Color color)
	{
		return Color.Transparent;
	}
}
