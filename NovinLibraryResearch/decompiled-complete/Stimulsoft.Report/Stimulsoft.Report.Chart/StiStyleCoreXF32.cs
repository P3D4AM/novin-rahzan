using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF32 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "32";

	public override Color[] StyleColors => new Color[3]
	{
		StiColor.Get("3a5263"),
		StiColor.Get("90a1ab"),
		StiColor.Get("c9d5dc")
	};

	public override Color BasicStyleColor => StiColor.Get("e9f4fc");

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush("6d7e8b");

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush("6d7e8b");

	public override StiBrush LegendBrush => (StiBrush)new StiSolidBrush(Color.Transparent);

	public override Color LegendLabelsColor => StiColor.Get("e9f4fc");

	public override Color LegendBorderColor => Color.Transparent;

	public override Color LegendTitleColor => StiColor.Get("e9f4fc");

	public override bool LegendShowShadow => false;

	public override Font LegendFont => new Font("Arial", 9f);

	public override Color SeriesLabelsBorderColor => Color.Transparent;

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(Color.FromArgb(119, 255, 255, 255));

	public override Color SeriesLabelsColor => StiColor.Get("8c8c8c");

	public override Color SeriesLabelsLineColor => StiColor.Get("e9f4fc");

	public override Font SeriesLabelsFont => new Font("Arial", 10f);

	public override Color AxisTitleColor => StiColor.Get("e9f4fc");

	public override Color AxisLineColor => StiColor.Get("e9f4fc");

	public override Color AxisLabelsColor => StiColor.Get("e9f4fc");

	public override Color GridLinesHorColor => StiColor.Get("e9f4fc");

	public override Color GridLinesVertColor => StiColor.Get("e9f4fc");

	public override bool SeriesLighting => false;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle32;

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
