using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiDarkBlueDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiDarkBlueDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "DarkBlue");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.DarkBlue;

	public override Color ForeColor { get; set; } = StiColor.Get("ddd");


	public override Color BackColor { get; set; } = StiColor.Get("1f4265");


	public override Color TitleBackColor { get; set; } = StiColor.Get("0a325a");


	public override Color TitleForeColor { get; set; } = StiColor.Get("ddd");

}
