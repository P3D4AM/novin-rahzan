using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF26 : StiStyleCoreXF22
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "26";

	public override Color[] StyleColors => new Color[6]
	{
		StiColor.Get("#2ec6c8"),
		StiColor.Get("#b5a1dd"),
		StiColor.Get("#5ab0ee"),
		StiColor.Get("#f4984e"),
		StiColor.Get("#d77a80"),
		StiColor.Get("#d04456")
	};

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("#ffffff");

	public override Color ChartAreaBorderColor => Color.Transparent;

	public override bool LegendShowShadow => false;

	public override Color LegendBorderColor => Color.Transparent;

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(119, 255, 255, 255));

	public override Color SeriesLabelsColor => StiColor.Get("#33475B");

	public override Font SeriesLabelsFont => new Font("Arial", 10f);

	public override bool SeriesLighting => false;

	public override bool SeriesShowShadow => false;

	public override bool MarkerVisible => false;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle26;

	public override Color GetColumnBorder(Color color)
	{
		return Color.Transparent;
	}
}
