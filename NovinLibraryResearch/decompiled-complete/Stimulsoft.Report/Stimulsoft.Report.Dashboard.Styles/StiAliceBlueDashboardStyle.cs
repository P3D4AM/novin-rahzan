using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiAliceBlueDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiAliceBlueDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "AliceBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.AliceBlue;

	public override Color BorderColor { get; set; } = StiColor.Get("40568d");


	public override Color ForeColor { get; set; } = StiColor.Get("3a3a3a");


	public override Color BackColor { get; set; } = StiColor.Get("f2f5fc");


	public override Color TitleBackColor { get; set; } = StiColor.Get("40568d");


	public override Color TitleForeColor { get; set; } = StiColor.Get("ffffff");

}
