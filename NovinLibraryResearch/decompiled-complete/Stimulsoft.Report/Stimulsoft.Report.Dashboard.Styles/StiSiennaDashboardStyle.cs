using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSiennaDashboardStyle : StiDashboardStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSiennaDashboardPageStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "Sienna");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.Sienna;

	public override Color BorderColor { get; set; } = StiColor.Get("#9f9282");


	public override Color ForeColor { get; set; } = StiColor.Get("#422d1c");


	public override Color BackColor { get; set; } = StiColor.Get("#f0eadc");


	public override Color TitleBackColor { get; set; } = StiColor.Get("#5c4841");


	public override Color TitleForeColor { get; set; } = StiColor.Get("#eaeeef");

}
