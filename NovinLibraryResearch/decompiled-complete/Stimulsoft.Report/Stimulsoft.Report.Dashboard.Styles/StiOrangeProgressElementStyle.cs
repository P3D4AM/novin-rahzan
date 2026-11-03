using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiOrangeProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiOrangeDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Orange;

	public override Color TrackColor { get; set; } = StiColor.Get("e6e6e6");


	public override Color BandColor { get; set; } = StiColor.Get("ed7d31");


	public override Color[] SeriesColors { get; set; } = new Color[6]
	{
		StiColor.Get("ed7d31"),
		StiColor.Get("ffc000"),
		StiColor.Get("70ad47"),
		StiColor.Get("9e480e"),
		StiColor.Get("997300"),
		StiColor.Get("43682b")
	};

}
