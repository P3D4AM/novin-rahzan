using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkTurquoiseProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkTurquoiseDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkTurquoise;

	public override Color TrackColor { get; set; } = StiColor.Get("1c445b");


	public override Color BandColor { get; set; } = StiColor.Get("f0621e");


	public override Color[] SeriesColors { get; set; } = new Color[3]
	{
		StiColor.Get("99a8d7e2"),
		StiColor.Get("995ea8bf"),
		StiColor.Get("992b7f9e")
	};


	public override Color BackColor { get; set; } = StiColor.Get("235e6d");

}
