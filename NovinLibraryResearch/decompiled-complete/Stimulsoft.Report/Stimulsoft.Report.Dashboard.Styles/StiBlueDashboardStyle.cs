using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiBlueDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiBlueDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Blue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Blue;

	public override Color ForeColor { get; set; } = StiColor.Get("3498db");


	public override Color BackColor { get; set; } = Color.White;


	public override Color TitleBackColor { get; set; } = StiColor.Get("3498db");


	public override Color TitleForeColor { get; set; } = Color.White;

}
