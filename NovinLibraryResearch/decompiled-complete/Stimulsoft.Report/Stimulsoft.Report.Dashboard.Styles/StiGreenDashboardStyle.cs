using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiGreenDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiGreenDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Green");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Green;

	public override Color ForeColor { get; set; } = StiColor.Get("70ad47");


	public override Color BackColor { get; set; } = Color.White;


	public override Color TitleBackColor { get; set; } = StiColor.Get("70ad47");


	public override Color TitleForeColor { get; set; } = Color.White;

}
