using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF22 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "22";

	public override StiBrush ChartBrush => (StiBrush)new StiSolidBrush(StiColorUtils.Light(BasicStyleColor, (byte)100));

	public override StiBrush ChartAreaBrush => (StiBrush)new StiSolidBrush(BasicStyleColor);

	public override Color ChartAreaBorderColor => StiColor.Get("#abacad");

	public override StiBrush SeriesLabelsBrush => (StiBrush)new StiSolidBrush(StiColorUtils.Light(BasicStyleColor, (byte)100));

	public override Color SeriesLabelsColor => StiColor.Get("#5a5a5a");

	public override Color SeriesLabelsBorderColor => StiColor.Get("#8c8c8c");

	public override StiBrush LegendBrush => (StiBrush)new StiSolidBrush(StiColorUtils.Light(BasicStyleColor, (byte)100));

	public override Color LegendLabelsColor => StiColor.Get("#8c8c8c");

	public override Color AxisTitleColor => StiColor.Get("#8c8c8c");

	public override Color AxisLineColor => StiColor.Get("#8c8c8c");

	public override Color AxisLabelsColor => StiColor.Get("#8c8c8c");

	public override Color GridLinesHorColor => Color.FromArgb(50, StiColorUtils.Dark(BasicStyleColor, (byte)150));

	public override Color GridLinesVertColor => Color.FromArgb(50, StiColorUtils.Dark(BasicStyleColor, (byte)150));

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("#5b9bd5"),
		StiColor.Get("#ed7d31"),
		StiColor.Get("#9f9f9f"),
		StiColor.Get("#ffc000"),
		StiColor.Get("#4472c4"),
		StiColor.Get("#70ad47")
	};


	public override Color BasicStyleColor => Color.White;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle22;

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
