using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSilverDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSilverDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Silver");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Silver;

	public override Color BorderColor { get; set; } = StiColor.Get("3a5263");


	public override Color ForeColor { get; set; } = StiColor.Get("3a5263");


	public override Color BackColor { get; set; } = StiColor.Get("e9f5fc");


	public override Color TitleBackColor { get; set; } = StiColor.Get("3a5263");


	public override Color TitleForeColor { get; set; } = StiColor.Get("e9f5fc");

}
