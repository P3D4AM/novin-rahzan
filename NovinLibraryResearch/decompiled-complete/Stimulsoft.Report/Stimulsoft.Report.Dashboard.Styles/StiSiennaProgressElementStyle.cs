using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSiennaProgressElementStyle : StiProgressElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSiennaDashboardProgressStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Sienna;

	public override Color TrackColor { get; set; } = StiColor.Get("e5ddd0");


	public override Color BandColor { get; set; } = StiColor.Get("3d2211");


	public override Color[] SeriesColors { get; set; } = new Color[10]
	{
		StiColor.Get("794d26"),
		StiColor.Get("c7986a"),
		StiColor.Get("c4b49a"),
		StiColor.Get("894d29"),
		StiColor.Get("422515"),
		StiColor.Get("564438"),
		StiColor.Get("876c57"),
		StiColor.Get("d8814b"),
		StiColor.Get("532525"),
		StiColor.Get("59413f")
	};

}
