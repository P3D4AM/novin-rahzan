using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSlateGrayProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSlateGrayDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "SlateGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.SlateGray;

	public override Color TrackColor { get; set; } = StiColor.Get("3b4d5f");


	public override Color BandColor { get; set; } = StiColor.Get("0BAC45");


	public override Color[] SeriesColors { get; set; } = new Color[6]
	{
		StiColor.Get("0bac45"),
		StiColor.Get("ec334d"),
		StiColor.Get("a1ae94"),
		StiColor.Get("ed7d31"),
		StiColor.Get("5ab0ee"),
		StiColor.Get("3dde77")
	};


	public override Color BackColor { get; set; } = StiColor.Get("33475b");

}
