using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF33 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "33";

	public override Color[] StyleColors => new Color[7]
	{
		StiColor.Get("40568d"),
		StiColor.Get("4569bb"),
		StiColor.Get("e47334"),
		StiColor.Get("9d9c9c"),
		StiColor.Get("f8b92d"),
		StiColor.Get("5e93cc"),
		StiColor.Get("6ea548")
	};

	public override Color BasicStyleColor => StiColor.Get("e9f4fc");

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush("f2f5fc");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("f2f5fc");

	public override StiBrush LegendBrush => (StiBrush)new StiSolidBrush(Color.Transparent);

	public override Color LegendLabelsColor => StiColor.Get("8c8c8c");

	public override Color LegendBorderColor => Color.Transparent;

	public override Color LegendTitleColor => StiColor.Get("e9f4fc");

	public override bool LegendShowShadow => false;

	public override Font LegendFont => new Font("Arial", 9f);

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(119, 255, 255, 255));

	public override Color SeriesLabelsLineColor => StiColor.Get("e9f4fc");

	public override Font SeriesLabelsFont => new Font("Arial", 10f);

	public override Color AxisTitleColor => StiColor.Get("8c8c8c");

	public override Color AxisLineColor => StiColor.Get("e9f4fc");

	public override Color AxisLabelsColor => StiColor.Get("8c8c8c");

	public override Color GridLinesHorColor => StiColor.Get("e9f4fc");

	public override Color GridLinesVertColor => StiColor.Get("e9f4fc");

	public override bool SeriesLighting => false;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle33;

	public override Color GetColumnBorder(Color color)
	{
		return Color.Transparent;
	}

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(color);
	}
}
