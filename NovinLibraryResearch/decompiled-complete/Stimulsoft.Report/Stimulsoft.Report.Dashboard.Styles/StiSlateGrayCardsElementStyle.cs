using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dashboard.Styles;

public class StiSlateGrayCardsElementStyle : StiCardsElementStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiSlateGrayDashboardCardsStyle;

	public override string LocalizedName => StiLocalization.Get("PropertyColor", "SlateGray");

	public override StiElementStyleIdent Ident => StiElementStyleIdent.SlateGray;

	public override Color CellForeColor { get; set; } = StiColor.Get("ddd");


	public override Color CellBackColor { get; set; } = StiColor.Get("475b6f");


	public override Color BackColor { get; set; } = StiColor.Get("33475b");


	public override Color LineColor { get; set; } = StiColor.Get("c0c0c0");

}
