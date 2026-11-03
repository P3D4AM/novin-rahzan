using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiOrangeDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiOrangeDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Orange");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Orange;

	public override Color ForeColor { get; set; } = StiColor.Get("ed7d31");


	public override Color BackColor { get; set; } = Color.White;


	public override Color TitleBackColor { get; set; } = StiColor.Get("ed7d31");


	public override Color TitleForeColor { get; set; } = Color.White;

}
