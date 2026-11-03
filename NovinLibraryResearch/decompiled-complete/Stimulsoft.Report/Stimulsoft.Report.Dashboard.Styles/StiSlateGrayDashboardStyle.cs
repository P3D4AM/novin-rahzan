using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSlateGrayDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSlateGrayDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "SlateGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.SlateGray;

	public override Color ForeColor { get; set; } = Color.White;


	public override Color BackColor { get; set; } = StiColor.Get("708090");


	public override Color TitleBackColor { get; set; } = StiColor.Get("475b6f");


	public override Color TitleForeColor { get; set; } = Color.White;

}
