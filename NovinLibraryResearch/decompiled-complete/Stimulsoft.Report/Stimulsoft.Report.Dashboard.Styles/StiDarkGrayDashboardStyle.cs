using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkGrayDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkGrayDashboardPageStyle;

	public override string LocalizedName => Loc.Get("PropertyColor", "DarkGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkGray;

	public override Color ForeColor { get; set; } = StiColor.Get("f1f1f1");


	public override Color BackColor { get; set; } = StiColor.Get("595b65");


	public override Color TitleForeColor { get; set; } = StiColor.Get("4a4c55");


	public override Color TitleBackColor { get; set; } = StiColor.Get("db7b46");


	public override Color BorderColor => Color.Transparent;
}
