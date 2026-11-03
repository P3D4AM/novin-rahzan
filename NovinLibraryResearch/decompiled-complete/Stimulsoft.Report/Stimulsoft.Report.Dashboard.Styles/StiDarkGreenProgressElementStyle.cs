using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGreenProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGreenDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkGreen");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGreen;

	public override Color TrackColor { get; set; } = StiColor.Get("144b45");


	public override Color BandColor { get; set; } = StiColor.Get("3f745e");


	public override Color[] SeriesColors { get; set; } = new Color[5]
	{
		StiColor.Get("ecb92f"),
		StiColor.Get("d66153"),
		StiColor.Get("319491"),
		StiColor.Get("f7f7f7"),
		StiColor.Get("dd7c21")
	};


	public override Color BackColor { get; set; } = StiColor.Get("cf822e");

}
