using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiAliceBlueProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiAliceBlueDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "AliceBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.AliceBlue;

	public override Color TrackColor { get; set; } = StiColor.Get("3a5263");


	public override Color BandColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color[] SeriesColors { get; set; } = new Color[6]
	{
		StiColor.Get("4569bb"),
		StiColor.Get("e47334"),
		StiColor.Get("9d9c9c"),
		StiColor.Get("f8b92d"),
		StiColor.Get("5e93cc"),
		StiColor.Get("6ea548")
	};

}
