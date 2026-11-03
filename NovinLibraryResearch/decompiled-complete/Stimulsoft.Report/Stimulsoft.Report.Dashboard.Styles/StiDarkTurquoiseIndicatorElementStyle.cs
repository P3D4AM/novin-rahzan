using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkTurquoiseIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkTurquoiseDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkTurquoise;

	public override Color GlyphColor { get; set; } = StiColor.Get("f0621e");


	public override Color BackColor { get; set; } = StiColor.Get("235e6d");


	public override Color PositiveColor { get; set; } = Color.LimeGreen;


	public override Color NegativeColor { get; set; } = StiColor.Get("dd5555");

}
