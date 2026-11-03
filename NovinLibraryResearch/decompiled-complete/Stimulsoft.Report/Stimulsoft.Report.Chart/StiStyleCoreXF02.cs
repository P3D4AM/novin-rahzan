using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF02 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "02";

	public override Color BasicStyleColor => Color.WhiteSmoke;

	public override Color[] StyleColors { get; } = new Color[10]
	{
		StiColor.Get("8091A5"),
		StiColor.Get("aB7B7A"),
		StiColor.Get("9BA781"),
		StiColor.Get("90859D"),
		StiColor.Get("79A1AD"),
		StiColor.Get("BD987A"),
		StiColor.Get("8BA4C2"),
		StiColor.Get("CA8482"),
		StiColor.Get("B3C68D"),
		StiColor.Get("A292B6")
	};


	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle02;

	public override StiBrush GetColumnBrush(Color color)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		return (StiBrush)new StiGlareBrush(color, StiColorUtils.Light(color, (byte)50), 0.0);
	}
}
