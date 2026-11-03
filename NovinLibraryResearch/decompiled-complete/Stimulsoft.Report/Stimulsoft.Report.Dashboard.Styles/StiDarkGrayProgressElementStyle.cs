using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGrayProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGrayDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGray;

	public override Color TrackColor { get; set; } = StiColor.Get("4a4c55");


	public override Color BandColor { get; set; } = StiColor.Get("6b6e75");


	public override Color[] SeriesColors { get; set; } = new Color[4]
	{
		StiColor.Get("db7b46"),
		StiColor.Get("d3d3d5"),
		StiColor.Get("6b6e75"),
		StiColor.Get("4a4c55")
	};


	public override Color BackColor { get; set; } = StiColor.Get("595b65");

}
