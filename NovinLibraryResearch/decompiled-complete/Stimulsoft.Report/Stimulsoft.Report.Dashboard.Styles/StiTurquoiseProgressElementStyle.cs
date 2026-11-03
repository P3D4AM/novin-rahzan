using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiTurquoiseProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiTurquoiseDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Turquoise;

	public override Color TrackColor { get; set; } = StiColor.Get("e6e6e6");


	public override Color BandColor { get; set; } = StiColor.Get("2ec6c8");


	public override Color[] SeriesColors { get; set; } = new Color[6]
	{
		StiColor.Get("2ec6c8"),
		StiColor.Get("b5a1dd"),
		StiColor.Get("5ab0ee"),
		StiColor.Get("f4984e"),
		StiColor.Get("d77a80"),
		StiColor.Get("d04456")
	};

}
