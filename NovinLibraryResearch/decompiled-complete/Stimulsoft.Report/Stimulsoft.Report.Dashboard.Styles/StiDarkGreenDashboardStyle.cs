using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGreenDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGreenDashboardPageStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "DarkGreen");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGreen;

	public override Color ForeColor { get; set; } = StiColor.Get("f3f7f7");


	public override Color BackColor { get; set; } = StiColor.Get("144b45");


	public override Color TitleForeColor { get; set; } = StiColor.Get("feedf6");


	public override Color TitleBackColor { get; set; } = StiColor.Get("26775d");


	public override Color BorderColor => Color.Transparent;
}
