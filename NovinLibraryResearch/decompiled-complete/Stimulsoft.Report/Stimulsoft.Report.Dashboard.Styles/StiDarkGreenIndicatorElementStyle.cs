using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGreenIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGreenDashboardIndicatorStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "DarkGreen");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGreen;

	public override Color GlyphColor { get; set; } = StiColor.Get("e1bb41");


	public override Color BackColor { get; set; } = StiColor.Get("3f745e");


	public override Color PositiveColor { get; set; } = StiColor.Get("e2ba41");


	public override Color NegativeColor { get; set; } = StiColor.Get("c76858");

}
