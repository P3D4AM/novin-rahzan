using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkBlueProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkBlueDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkBlue;

	public override Color TrackColor { get; set; } = StiColor.Get("1f4265");


	public override Color BandColor { get; set; } = StiColor.Get("165d9e");


	public override Color[] SeriesColors { get; set; } = new Color[5]
	{
		StiColor.Get("165d9e"),
		StiColor.Get("577eb6"),
		StiColor.Get("569436"),
		StiColor.Get("225056"),
		StiColor.Get("d4dae0")
	};


	public override Color BackColor { get; set; } = StiColor.Get("0a325a");

}
