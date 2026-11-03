using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiBlueProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiBlueDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;

	public override Color TrackColor { get; set; } = StiColor.Get("e7ebec");


	public override Color BandColor { get; set; } = StiColor.Get("3498db");


	public override Color[] SeriesColors { get; set; } = new Color[5]
	{
		StiColor.Get("3498db"),
		StiColor.Get("ef717a"),
		StiColor.Get("6dcbb3"),
		StiColor.Get("f28161"),
		StiColor.Get("fccd1b")
	};

}
