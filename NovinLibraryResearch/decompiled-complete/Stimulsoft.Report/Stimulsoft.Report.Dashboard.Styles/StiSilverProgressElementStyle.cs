using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSilverProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSilverDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Silver");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Silver;

	public override Color TrackColor { get; set; } = StiColor.Get("3a5263");


	public override Color BandColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color[] SeriesColors { get; set; } = new Color[4]
	{
		StiColor.Get("73829a"),
		StiColor.Get("343c49"),
		StiColor.Get("4f737a"),
		StiColor.Get("cfd3dd")
	};

}
