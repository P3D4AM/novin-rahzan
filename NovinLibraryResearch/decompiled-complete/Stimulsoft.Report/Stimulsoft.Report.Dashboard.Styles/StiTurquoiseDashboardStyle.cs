using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiTurquoiseDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiTurquoiseDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Turquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Turquoise;

	public override Color ForeColor { get; set; } = StiColor.Get("2ec6c8");


	public override Color BackColor { get; set; } = Color.White;


	public override Color TitleBackColor { get; set; } = StiColor.Get("2ec6c8");


	public override Color TitleForeColor { get; set; } = Color.White;

}
