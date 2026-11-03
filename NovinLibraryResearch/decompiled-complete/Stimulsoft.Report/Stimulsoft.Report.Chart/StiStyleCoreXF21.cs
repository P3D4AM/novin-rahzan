using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF21 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "21";

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush(StiColorUtils.Light(BasicStyleColor, (byte)100));

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush(BasicStyleColor);

	public override Color ChartAreaBorderColor => StiColor.Get("#6a6a6a");

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(StiColorUtils.Light(BasicStyleColor, (byte)100));

	public override Color SeriesLabelsColor => StiColor.Get("#6a6a6a");

	public override Color SeriesLabelsBorderColor => Color.White;

	public override StiBrush LegendBrush => (StiBrush)new StiSolidBrush(StiColorUtils.Light(BasicStyleColor, (byte)100));

	public override Color LegendLabelsColor => StiColor.Get("#6a6a6a");

	public override Color AxisTitleColor => StiColor.Get("#6a6a6a");

	public override Color AxisLineColor => StiColor.Get("#6a6a6a");

	public override Color AxisLabelsColor => StiColor.Get("#6a6a6a");

	public override Color[] StyleColors { get; } = new Color[2]
	{
		StiColor.Get("#239fd9"),
		StiColor.Get("#b2b2b2")
	};


	public override Color BasicStyleColor => StiColor.Get("#666666");

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle21;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(color);
	}

	public override Color GetColumnBorder(Color color)
	{
		return StiColorUtils.Light(color, byte.MaxValue);
	}
}
