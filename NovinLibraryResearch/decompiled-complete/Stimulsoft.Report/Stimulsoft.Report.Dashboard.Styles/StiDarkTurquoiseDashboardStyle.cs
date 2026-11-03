using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkTurquoiseDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkTurquoiseDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkTurquoise");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkTurquoise;

	public override Color ForeColor { get; set; } = StiColor.Get("f4fcff");


	public override Color BackColor { get; set; } = StiColor.Get("2c8590");


	public override Color TitleForeColor { get; set; } = StiColor.Get("f4fcff");


	public override Color TitleBackColor { get; set; } = StiColor.Get("216275");


	public override Color BorderColor { get; set; } = StiColor.Get("1c475c");

}
