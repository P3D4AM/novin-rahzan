using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF01 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "01";

	public override Color[] StyleColors { get; } = new Color[5]
	{
		StiColor.Get("C27535"),
		StiColor.Get("E78C41"),
		StiColor.Get("F8AA79"),
		StiColor.Get("FACBB4"),
		StiColor.Get("FDE6DC")
	};


	public override Color BasicStyleColor => Color.Wheat;

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle01;

	public override bool SeriesShowBorder { get; } = true;


	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		return (StiBrush)new StiGlareBrush(StiColorUtils.Dark(color, (byte)50), color, 0.0);
	}
}
