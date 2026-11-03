using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGrayIndicatorElementStyle : StiIndicatorElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGrayDashboardIndicatorStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGray;

	public override Color GlyphColor { get; set; } = StiColor.Get("db7b46");


	public override Color BackColor { get; set; } = StiColor.Get("595b65");


	public override Color PositiveColor { get; set; } = Color.LimeGreen;


	public override Color NegativeColor { get; set; } = StiColor.Get("dd5555");

}
