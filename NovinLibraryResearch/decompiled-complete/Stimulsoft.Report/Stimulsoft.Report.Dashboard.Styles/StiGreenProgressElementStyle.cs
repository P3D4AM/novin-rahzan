using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiGreenProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiGreenDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Green;

	public override Color TrackColor { get; set; } = StiColor.Get("e6e6e6");


	public override Color BandColor { get; set; } = StiColor.Get("70ad47");


	public override Color[] SeriesColors { get; set; } = new Color[6]
	{
		StiColor.Get("70ad47"),
		StiColor.Get("4472c4"),
		StiColor.Get("ffc000"),
		StiColor.Get("43682b"),
		StiColor.Get("fd6a37"),
		StiColor.Get("997300")
	};

}
